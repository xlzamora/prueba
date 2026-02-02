using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Identity;

namespace TelemedicinaOdonto.Infrastructure.Data;

public static class SeedData
{
    public static async Task EnsureSeedAsync(AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await dbContext.Database.MigrateAsync();

        var roles = new[] { "Patient", "Dentist", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@telemed.com";
        var dentistEmail = "dentist@telemed.com";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "Admin" };
            await userManager.CreateAsync(admin, "Admin123$");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var dentistUser = await userManager.FindByEmailAsync(dentistEmail);
        if (dentistUser is null)
        {
            dentistUser = new ApplicationUser { UserName = dentistEmail, Email = dentistEmail, FullName = "Dra. Lara" };
            await userManager.CreateAsync(dentistUser, "Dentist123$");
            await userManager.AddToRoleAsync(dentistUser, "Dentist");
        }

        if (!await dbContext.Clinics.AnyAsync())
        {
            var clinic = new Clinic
            {
                ClinicId = Guid.NewGuid(),
                Name = "Clínica Central",
                Address = "Av. Principal 123",
                City = "Lima",
                IsActive = true
            };
            dbContext.Clinics.Add(clinic);

            var dentist = new Dentist
            {
                DentistId = Guid.NewGuid(),
                UserId = dentistUser.Id,
                FullName = "Dra. Lara",
                Specialty = "Odontología General",
                ClinicId = clinic.ClinicId,
                IsActive = true
            };
            dbContext.Dentists.Add(dentist);

            var service = new Service
            {
                ServiceId = Guid.NewGuid(),
                Name = "Consulta general",
                DurationMin = 30,
                Description = "Consulta odontológica inicial.",
                IsActive = true
            };
            dbContext.Services.Add(service);

            dbContext.DentistSchedules.Add(new DentistSchedule
            {
                ScheduleId = Guid.NewGuid(),
                DentistId = dentist.DentistId,
                DayOfWeek = (int)DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                SlotMinutes = 30,
                IsActive = true
            });
        }

        if (!await dbContext.KnowledgeBaseItems.AnyAsync())
        {
            dbContext.KnowledgeBaseItems.AddRange(
                new KnowledgeBaseItem
                {
                    KbId = Guid.NewGuid(),
                    Category = "FAQ",
                    Title = "Dolor de muelas",
                    Content = "Puedes enjuagar con agua tibia y evitar alimentos duros. Si el dolor es fuerte, agenda una cita.",
                    TagsJson = "[\"dolor\",\"muelas\"]",
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow
                },
                new KnowledgeBaseItem
                {
                    KbId = Guid.NewGuid(),
                    Category = "Servicio",
                    Title = "Limpieza dental",
                    Content = "Ofrecemos limpieza dental con evaluación básica.",
                    TagsJson = "[\"limpieza\",\"profilaxis\"]",
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }

        await dbContext.SaveChangesAsync();
    }
}
