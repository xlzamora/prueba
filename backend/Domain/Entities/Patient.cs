namespace TelemedicinaOdonto.Domain.Entities;

public class Patient
{
    public Guid PatientId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DocumentId { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public PatientMedicalProfile? MedicalProfile { get; set; }
    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
    public ICollection<TriageResult> TriageResults { get; set; } = new List<TriageResult>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
