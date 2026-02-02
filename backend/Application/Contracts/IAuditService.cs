using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IAuditService
{
    Task RecordAsync(string actorUserId, string actorRole, string action, string entity, string entityId, object metadata);
    Task<IReadOnlyList<AuditLogResponse>> GetAllAsync();
}
