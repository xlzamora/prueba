using TelemedicinaOdonto.Domain.Enums;

namespace TelemedicinaOdonto.Application.DTOs;

public record AvailableSlotsRequest(Guid DentistId, DateTime Date);

public record AvailableSlotResponse(DateTime StartAt, DateTime EndAt);

public record CreateAppointmentRequest(
    Guid PatientId,
    Guid DentistId,
    Guid ClinicId,
    Guid ServiceId,
    DateTime StartAt,
    DateTime EndAt,
    PriorityLevel Priority,
    Guid? TriageId,
    string SummaryText
);

public record AppointmentResponse(
    Guid AppointmentId,
    Guid PatientId,
    Guid DentistId,
    DateTime StartAt,
    DateTime EndAt,
    AppointmentStatus Status,
    PriorityLevel Priority,
    string SummaryText
);

public record RescheduleAppointmentRequest(DateTime NewStartAt, DateTime NewEndAt);
