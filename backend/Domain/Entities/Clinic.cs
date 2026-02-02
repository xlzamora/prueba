namespace TelemedicinaOdonto.Domain.Entities;

public class Clinic
{
    public Guid ClinicId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<Dentist> Dentists { get; set; } = new List<Dentist>();
}
