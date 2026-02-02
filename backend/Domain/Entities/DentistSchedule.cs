namespace TelemedicinaOdonto.Domain.Entities;

public class DentistSchedule
{
    public Guid ScheduleId { get; set; }
    public Guid DentistId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int SlotMinutes { get; set; }
    public bool IsActive { get; set; }

    public Dentist Dentist { get; set; } = null!;
}
