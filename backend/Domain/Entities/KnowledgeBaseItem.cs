namespace TelemedicinaOdonto.Domain.Entities;

public class KnowledgeBaseItem
{
    public Guid KbId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string TagsJson { get; set; } = "[]";
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}
