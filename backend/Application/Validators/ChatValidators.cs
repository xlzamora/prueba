using FluentValidation;
using TelemedicinaOdonto.Application.DTOs;

namespace TelemedicinaOdonto.Application.Validators;

public class SendChatMessageRequestValidator : AbstractValidator<SendChatMessageRequest>
{
    public SendChatMessageRequestValidator()
    {
        RuleFor(x => x.Sender).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(4000);
    }
}
