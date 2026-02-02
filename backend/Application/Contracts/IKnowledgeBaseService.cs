using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IKnowledgeBaseService
{
    Task<ChatBotReply> FindAnswerAsync(string message);
    Task<IReadOnlyList<KnowledgeBaseItemResponse>> GetAllAsync();
    Task<KnowledgeBaseItemResponse> CreateAsync(KnowledgeBaseItemRequest request);
    Task<KnowledgeBaseItemResponse> UpdateAsync(Guid id, KnowledgeBaseItemRequest request);
    Task DeleteAsync(Guid id);
}
