using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Identity;

namespace TelemedicinaOdonto.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PatientMedicalProfile> PatientMedicalProfiles => Set<PatientMedicalProfile>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<TriageResult> TriageResults => Set<TriageResult>();
    public DbSet<KnowledgeBaseItem> KnowledgeBaseItems => Set<KnowledgeBaseItem>();
    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<Dentist> Dentists => Set<Dentist>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<DentistSchedule> DentistSchedules => Set<DentistSchedule>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Patient>(entity =>
        {
            entity.HasKey(x => x.PatientId);
            entity.HasOne(x => x.MedicalProfile)
                .WithOne(x => x.Patient)
                .HasForeignKey<PatientMedicalProfile>(x => x.PatientId);
        });

        builder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(x => x.SessionId);
            entity.HasMany(x => x.Messages)
                .WithOne(x => x.Session)
                .HasForeignKey(x => x.SessionId);
        });

        builder.Entity<TriageResult>(entity =>
        {
            entity.HasKey(x => x.TriageId);
            entity.HasOne(x => x.Session)
                .WithMany()
                .HasForeignKey(x => x.SessionId);
        });

        builder.Entity<Appointment>(entity =>
        {
            entity.HasKey(x => x.AppointmentId);
            entity.HasOne(x => x.Triage)
                .WithMany()
                .HasForeignKey(x => x.TriageId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<DentistSchedule>(entity =>
        {
            entity.HasKey(x => x.ScheduleId);
        });
    }
}
