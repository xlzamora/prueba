using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Domain.Enums;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class TriageService : ITriageService
{
    private readonly AppDbContext _dbContext;

    public TriageService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TriageResponse> EvaluateAsync(TriageRequest request)
    {
        var priority = PriorityLevel.Media;
        if (request.MainComplaint.Equals("Dolor", StringComparison.OrdinalIgnoreCase)
            && request.PainLevel.Equals("fuerte", StringComparison.OrdinalIgnoreCase)
            && request.Swelling)
        {
            priority = PriorityLevel.Emergencia;
        }
        else if (request.PainLevel.Equals("moderado", StringComparison.OrdinalIgnoreCase))
        {
            priority = PriorityLevel.Alta;
        }

        var summary = $"Motivo: {request.MainComplaint}. Dolor: {request.PainLevel}. Hinchazón: {(request.Swelling ? "Sí" : "No")}.";
        var recommended = priority is PriorityLevel.Emergencia or PriorityLevel.Alta
            ? "Recomendado agendar cita prioritaria."
            : "Puede continuar con orientación por chat.";

        var triage = new TriageResult
        {
            TriageId = Guid.NewGuid(),
            SessionId = request.SessionId,
            PatientId = request.PatientId,
            MainComplaint = request.MainComplaint,
            Priority = priority,
            RedFlagsJson = JsonSerializer.Serialize(new { request.Swelling }),
            AnswersJson = JsonSerializer.Serialize(request.Answers),
            SummaryText = summary,
            RecommendedNextStep = recommended,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.TriageResults.Add(triage);
        await _dbContext.SaveChangesAsync();

        return new TriageResponse(triage.TriageId, triage.Priority, triage.SummaryText, triage.RecommendedNextStep);
    }
}
