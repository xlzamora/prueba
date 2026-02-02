namespace TelemedicinaOdonto.Domain.Entities;

public class ChatSession
{
    public Guid SessionId { get; set; }
    public Guid? PatientId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Outcome { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
