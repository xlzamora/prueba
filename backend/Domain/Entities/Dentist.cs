namespace TelemedicinaOdonto.Domain.Entities;

public class Dentist
{
    public Guid DentistId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public Guid ClinicId { get; set; }
    public bool IsActive { get; set; }

    public Clinic Clinic { get; set; } = null!;
    public ICollection<DentistSchedule> Schedules { get; set; } = new List<DentistSchedule>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
