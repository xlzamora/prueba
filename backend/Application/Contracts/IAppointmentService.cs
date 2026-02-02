using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Contracts;

public interface IAppointmentService
{
    Task<IReadOnlyList<AvailableSlotResponse>> GetAvailableSlotsAsync(AvailableSlotsRequest request);
    Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request);
    Task<AppointmentResponse> RescheduleAsync(Guid appointmentId, RescheduleAppointmentRequest request);
    Task CancelAsync(Guid appointmentId);
    Task<IReadOnlyList<AppointmentResponse>> GetDentistAppointmentsAsync(Guid dentistId);
    Task<AppointmentResponse> GetDentistAppointmentAsync(Guid dentistId, Guid appointmentId);
}
