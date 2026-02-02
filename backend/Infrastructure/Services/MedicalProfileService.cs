using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class MedicalProfileService : IMedicalProfileService
{
    private readonly AppDbContext _dbContext;
    private readonly IAuditService _auditService;

    public MedicalProfileService(AppDbContext dbContext, IAuditService auditService)
    {
        _dbContext = dbContext;
        _auditService = auditService;
    }

    public async Task<MedicalProfileResponse> UpsertAsync(Guid patientId, MedicalProfileRequest request)
    {
        var profile = await _dbContext.PatientMedicalProfiles.FirstOrDefaultAsync(p => p.PatientId == patientId);
        if (profile is null)
        {
            profile = new PatientMedicalProfile
            {
                PatientId = patientId,
                BloodType = request.BloodType,
                AllergiesText = request.AllergiesText,
                ConditionsText = request.ConditionsText,
                UpdatedAt = DateTime.UtcNow
            };
            _dbContext.PatientMedicalProfiles.Add(profile);
        }
        else
        {
            profile.BloodType = request.BloodType;
            profile.AllergiesText = request.AllergiesText;
            profile.ConditionsText = request.ConditionsText;
            profile.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
        return new MedicalProfileResponse(profile.PatientId, profile.BloodType, profile.AllergiesText, profile.ConditionsText, profile.UpdatedAt);
    }

    public async Task<MedicalProfileResponse> GetAsync(Guid patientId, string actorUserId, string actorRole)
    {
        var profile = await _dbContext.PatientMedicalProfiles.FirstOrDefaultAsync(p => p.PatientId == patientId)
                      ?? throw new InvalidOperationException("Ficha médica no encontrada");

        await _auditService.RecordAsync(actorUserId, actorRole, "View", "PatientMedicalProfile", patientId.ToString(), new { patientId });

        return new MedicalProfileResponse(profile.PatientId, profile.BloodType, profile.AllergiesText, profile.ConditionsText, profile.UpdatedAt);
    }
}
