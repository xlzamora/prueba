using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class KnowledgeBaseService : IKnowledgeBaseService
{
    private readonly AppDbContext _dbContext;

    public KnowledgeBaseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChatBotReply> FindAnswerAsync(string message)
    {
        var normalized = message.ToLowerInvariant();
        var items = await _dbContext.KnowledgeBaseItems.Where(x => x.IsActive).ToListAsync();
        var match = items.FirstOrDefault(item =>
        {
            var tags = item.TagsJson.Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("\"", string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return item.Title.ToLowerInvariant().Contains(normalized)
                   || item.Content.ToLowerInvariant().Contains(normalized)
                   || tags.Any(tag => normalized.Contains(tag.ToLowerInvariant()));
        });

        if (match is null)
        {
            return new ChatBotReply(
                "No tengo una respuesta clara. Te puedo ofrecer horarios con un odontólogo.",
                "Escalate",
                0.2m,
                true);
        }

        return new ChatBotReply(match.Content, match.Category, 0.85m, false);
    }

    public async Task<IReadOnlyList<KnowledgeBaseItemResponse>> GetAllAsync()
    {
        return await _dbContext.KnowledgeBaseItems
            .OrderByDescending(k => k.UpdatedAt)
            .Select(k => new KnowledgeBaseItemResponse(k.KbId, k.Category, k.Title, k.Content, k.TagsJson, k.IsActive, k.UpdatedAt))
            .ToListAsync();
    }

    public async Task<KnowledgeBaseItemResponse> CreateAsync(KnowledgeBaseItemRequest request)
    {
        var item = new KnowledgeBaseItem
        {
            KbId = Guid.NewGuid(),
            Category = request.Category,
            Title = request.Title,
            Content = request.Content,
            TagsJson = request.TagsJson,
            IsActive = request.IsActive,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.KnowledgeBaseItems.Add(item);
        await _dbContext.SaveChangesAsync();

        return new KnowledgeBaseItemResponse(item.KbId, item.Category, item.Title, item.Content, item.TagsJson, item.IsActive, item.UpdatedAt);
    }

    public async Task<KnowledgeBaseItemResponse> UpdateAsync(Guid id, KnowledgeBaseItemRequest request)
    {
        var item = await _dbContext.KnowledgeBaseItems.FirstOrDefaultAsync(k => k.KbId == id)
                   ?? throw new InvalidOperationException("KB no encontrado");

        item.Category = request.Category;
        item.Title = request.Title;
        item.Content = request.Content;
        item.TagsJson = request.TagsJson;
        item.IsActive = request.IsActive;
        item.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return new KnowledgeBaseItemResponse(item.KbId, item.Category, item.Title, item.Content, item.TagsJson, item.IsActive, item.UpdatedAt);
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _dbContext.KnowledgeBaseItems.FirstOrDefaultAsync(k => k.KbId == id)
                   ?? throw new InvalidOperationException("KB no encontrado");
        _dbContext.KnowledgeBaseItems.Remove(item);
        await _dbContext.SaveChangesAsync();
    }
}
