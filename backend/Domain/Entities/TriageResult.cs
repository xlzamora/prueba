using TelemedicinaOdonto.Domain.Enums;

namespace TelemedicinaOdonto.Domain.Entities;

public class TriageResult
{
    public Guid TriageId { get; set; }
    public Guid SessionId { get; set; }
    public Guid PatientId { get; set; }
    public string MainComplaint { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; }
    public string RedFlagsJson { get; set; } = "{}";
    public string AnswersJson { get; set; } = "{}";
    public string SummaryText { get; set; } = string.Empty;
    public string RecommendedNextStep { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ChatSession Session { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
}
