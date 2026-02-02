namespace TelemedicinaOdonto.Domain.Entities;

public class AuditLog
{
    public Guid AuditId { get; set; }
    public string ActorUserId { get; set; } = string.Empty;
    public string ActorRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string MetadataJson { get; set; } = "{}";
}
