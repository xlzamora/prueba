namespace TelemedicinaOdonto.Application.DTOs;

public record KnowledgeBaseItemRequest(string Category, string Title, string Content, string TagsJson, bool IsActive);

public record KnowledgeBaseItemResponse(Guid KbId, string Category, string Title, string Content, string TagsJson, bool IsActive, DateTime UpdatedAt);

public record AuditLogResponse(Guid AuditId, string ActorUserId, string ActorRole, string Action, string Entity, string EntityId, DateTime Timestamp, string MetadataJson);
