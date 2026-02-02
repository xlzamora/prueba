using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Domain.Enums;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _dbContext;

    public AppointmentService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AvailableSlotResponse>> GetAvailableSlotsAsync(AvailableSlotsRequest request)
    {
        var dayOfWeek = (int)request.Date.DayOfWeek;
        var schedules = await _dbContext.DentistSchedules
            .Where(s => s.DentistId == request.DentistId && s.DayOfWeek == dayOfWeek && s.IsActive)
            .ToListAsync();

        var appointments = await _dbContext.Appointments
            .Where(a => a.DentistId == request.DentistId && a.StartAt.Date == request.Date.Date && a.Status != AppointmentStatus.Canceled)
            .ToListAsync();

        var slots = new List<AvailableSlotResponse>();
        foreach (var schedule in schedules)
        {
            var start = request.Date.Date + schedule.StartTime;
            var end = request.Date.Date + schedule.EndTime;
            while (start.AddMinutes(schedule.SlotMinutes) <= end)
            {
                var slotEnd = start.AddMinutes(schedule.SlotMinutes);
                var hasConflict = appointments.Any(a => start < a.EndAt && slotEnd > a.StartAt);
                if (!hasConflict)
                {
                    slots.Add(new AvailableSlotResponse(start, slotEnd));
                }

                if (slots.Count == 3)
                {
                    return slots;
                }

                start = slotEnd;
            }
        }

        return slots.Take(3).ToList();
    }

    public async Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        var conflict = await _dbContext.Appointments.AnyAsync(a => a.DentistId == request.DentistId
                                                                  && a.Status != AppointmentStatus.Canceled
                                                                  && request.StartAt < a.EndAt
                                                                  && request.EndAt > a.StartAt);
        if (conflict)
        {
            throw new InvalidOperationException("Horario no disponible");
        }

        var appointment = new Appointment
        {
            AppointmentId = Guid.NewGuid(),
            PatientId = request.PatientId,
            DentistId = request.DentistId,
            ClinicId = request.ClinicId,
            ServiceId = request.ServiceId,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Status = AppointmentStatus.Scheduled,
            Priority = request.Priority,
            TriageId = request.TriageId,
            SummaryText = request.SummaryText,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();

        return MapAppointment(appointment);
    }

    public async Task<AppointmentResponse> RescheduleAsync(Guid appointmentId, RescheduleAppointmentRequest request)
    {
        var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId)
                          ?? throw new InvalidOperationException("Cita no encontrada");

        var conflict = await _dbContext.Appointments.AnyAsync(a => a.DentistId == appointment.DentistId
                                                                  && a.AppointmentId != appointmentId
                                                                  && a.Status != AppointmentStatus.Canceled
                                                                  && request.NewStartAt < a.EndAt
                                                                  && request.NewEndAt > a.StartAt);
        if (conflict)
        {
            throw new InvalidOperationException("Horario no disponible");
        }

        appointment.StartAt = request.NewStartAt;
        appointment.EndAt = request.NewEndAt;
        appointment.Status = AppointmentStatus.Rescheduled;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return MapAppointment(appointment);
    }

    public async Task CancelAsync(Guid appointmentId)
    {
        var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId)
                          ?? throw new InvalidOperationException("Cita no encontrada");
        appointment.Status = AppointmentStatus.Canceled;
        appointment.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetDentistAppointmentsAsync(Guid dentistId)
    {
        return await _dbContext.Appointments
            .Where(a => a.DentistId == dentistId)
            .OrderByDescending(a => a.StartAt)
            .Select(a => MapAppointment(a))
            .ToListAsync();
    }

    public async Task<AppointmentResponse> GetDentistAppointmentAsync(Guid dentistId, Guid appointmentId)
    {
        var appointment = await _dbContext.Appointments
            .Where(a => a.DentistId == dentistId && a.AppointmentId == appointmentId)
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Cita no encontrada");
        return MapAppointment(appointment);
    }

    private static AppointmentResponse MapAppointment(Appointment appointment)
    {
        return new AppointmentResponse(
            appointment.AppointmentId,
            appointment.PatientId,
            appointment.DentistId,
            appointment.StartAt,
            appointment.EndAt,
            appointment.Status,
            appointment.Priority,
            appointment.SummaryText);
    }
}
