using FluentValidation;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Validators;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.DentistId).NotEmpty();
        RuleFor(x => x.ClinicId).NotEmpty();
        RuleFor(x => x.ServiceId).NotEmpty();
        RuleFor(x => x.StartAt).LessThan(x => x.EndAt);
    }
}

public class RescheduleAppointmentRequestValidator : AbstractValidator<RescheduleAppointmentRequest>
{
    public RescheduleAppointmentRequestValidator()
    {
        RuleFor(x => x.NewStartAt).LessThan(x => x.NewEndAt);
    }
}
