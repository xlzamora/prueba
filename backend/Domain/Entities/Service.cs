namespace TelemedicinaOdonto.Domain.Entities;

public class Service
{
    public Guid ServiceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMin { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
