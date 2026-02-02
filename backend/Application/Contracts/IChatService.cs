using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IChatService
{
    Task<ChatSessionResponse> CreateSessionAsync(CreateChatSessionRequest request);
    Task<ChatMessageResponse> AddMessageAsync(Guid sessionId, SendChatMessageRequest request);
    Task<IReadOnlyList<ChatMessageResponse>> GetMessagesAsync(Guid sessionId);
}
