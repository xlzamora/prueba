namespace TelemedicinaOdonto.Application.DTOs;

public record CreateChatSessionRequest(Guid? PatientId);

public record ChatSessionResponse(Guid SessionId, Guid? PatientId, DateTime StartedAt, string Outcome);

public record SendChatMessageRequest(string Sender, string Text);

public record ChatMessageResponse(Guid MessageId, Guid SessionId, string Sender, string Text, DateTime CreatedAt, string? Intent, decimal? Confidence);

public record ChatBotReply(string ReplyText, string? Intent, decimal? Confidence, bool EscalateToAppointment);
