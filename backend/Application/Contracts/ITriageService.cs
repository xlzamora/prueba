using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface ITriageService
{
    Task<TriageResponse> EvaluateAsync(TriageRequest request);
}
