using FluentValidation;

namespace AutoTrader.Application.Features.Login
{
    public class LoginSlaveCommandValidator : AbstractValidator<LoginSlaveCommand>
    {
        public LoginSlaveCommandValidator()
        {
            RuleFor(p => p.Settings.Username2)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Settings.Password2)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Settings.ConnectionType)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
