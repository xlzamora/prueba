using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly AppDbContext _dbContext;
    private readonly IKnowledgeBaseService _kbService;

    public ChatService(AppDbContext dbContext, IKnowledgeBaseService kbService)
    {
        _dbContext = dbContext;
        _kbService = kbService;
    }

    public async Task<ChatSessionResponse> CreateSessionAsync(CreateChatSessionRequest request)
    {
        var session = new ChatSession
        {
            SessionId = Guid.NewGuid(),
            PatientId = request.PatientId,
            StartedAt = DateTime.UtcNow,
            Outcome = "InProgress"
        };

        _dbContext.ChatSessions.Add(session);
        await _dbContext.SaveChangesAsync();

        return new ChatSessionResponse(session.SessionId, session.PatientId, session.StartedAt, session.Outcome);
    }

    public async Task<ChatMessageResponse> AddMessageAsync(Guid sessionId, SendChatMessageRequest request)
    {
        var session = await _dbContext.ChatSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId)
                      ?? throw new InvalidOperationException("Sesión no encontrada");

        var userMessage = new ChatMessage
        {
            MessageId = Guid.NewGuid(),
            SessionId = sessionId,
            Sender = request.Sender,
            Text = request.Text,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.ChatMessages.Add(userMessage);

        var reply = await _kbService.FindAnswerAsync(request.Text);
        var botMessage = new ChatMessage
        {
            MessageId = Guid.NewGuid(),
            SessionId = sessionId,
            Sender = "Bot",
            Text = reply.ReplyText,
            CreatedAt = DateTime.UtcNow,
            Intent = reply.Intent,
            Confidence = reply.Confidence
        };

        _dbContext.ChatMessages.Add(botMessage);
        if (reply.EscalateToAppointment)
        {
            session.Outcome = "Escalated";
        }

        await _dbContext.SaveChangesAsync();

        return new ChatMessageResponse(botMessage.MessageId, sessionId, botMessage.Sender, botMessage.Text, botMessage.CreatedAt, botMessage.Intent, botMessage.Confidence);
    }

    public async Task<IReadOnlyList<ChatMessageResponse>> GetMessagesAsync(Guid sessionId)
    {
        var messages = await _dbContext.ChatMessages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new ChatMessageResponse(m.MessageId, m.SessionId, m.Sender, m.Text, m.CreatedAt, m.Intent, m.Confidence))
            .ToListAsync();

        return messages;
    }
}
