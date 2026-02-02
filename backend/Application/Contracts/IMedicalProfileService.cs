using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IMedicalProfileService
{
    Task<MedicalProfileResponse> UpsertAsync(Guid patientId, MedicalProfileRequest request);
    Task<MedicalProfileResponse> GetAsync(Guid patientId, string actorUserId, string actorRole);
}
