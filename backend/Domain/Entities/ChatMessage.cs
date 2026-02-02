namespace TelemedicinaOdonto.Domain.Entities;

public class ChatMessage
{
    public Guid MessageId { get; set; }
    public Guid SessionId { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Intent { get; set; }
    public decimal? Confidence { get; set; }

    public ChatSession Session { get; set; } = null!;
}
