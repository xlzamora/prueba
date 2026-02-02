namespace TelemedicinaOdonto.Application.DTOs;

public record MedicalProfileRequest(string BloodType, string AllergiesText, string ConditionsText);

public record MedicalProfileResponse(Guid PatientId, string BloodType, string AllergiesText, string ConditionsText, DateTime UpdatedAt);
