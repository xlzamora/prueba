using FluentValidation;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Validators;

public class TriageRequestValidator : AbstractValidator<TriageRequest>
{
    public TriageRequestValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.MainComplaint).NotEmpty();
        RuleFor(x => x.PainLevel).NotEmpty();
    }
}
