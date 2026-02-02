namespace TelemedicinaOdonto.Domain.Entities;

public class PatientMedicalProfile
{
    public Guid PatientId { get; set; }
    public string BloodType { get; set; } = string.Empty;
    public string AllergiesText { get; set; } = string.Empty;
    public string ConditionsText { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
}
