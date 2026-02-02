using TelemedicinaOdonto.Domain.Enums;

namespace TelemedicinaOdonto.Application.DTOs;

public record TriageRequest(
    Guid SessionId,
    Guid PatientId,
    string MainComplaint,
    string PainLevel,
    bool Swelling,
    Dictionary<string, string> Answers
);

public record TriageResponse(
    Guid TriageId,
    PriorityLevel Priority,
    string SummaryText,
    string RecommendedNextStep
);
