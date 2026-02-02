using TelemedicinaOdonto.Domain.Enums;

namespace TelemedicinaOdonto.Domain.Entities;

public class Appointment
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public Guid DentistId { get; set; }
    public Guid ClinicId { get; set; }
    public Guid ServiceId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public AppointmentStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public Guid? TriageId { get; set; }
    public string SummaryText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public Dentist Dentist { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public TriageResult? Triage { get; set; }
}
