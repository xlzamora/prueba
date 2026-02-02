using FluentValidation;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Validators;

public class MedicalProfileRequestValidator : AbstractValidator<MedicalProfileRequest>
{
    public MedicalProfileRequestValidator()
    {
        RuleFor(x => x.BloodType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.AllergiesText).MaximumLength(4000);
        RuleFor(x => x.ConditionsText).MaximumLength(4000);
    }
}
