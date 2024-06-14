using FluentValidation;

namespace AutoTrader.Application.Features.Login
{
    public class LoginMasterCommandValidator : AbstractValidator<LoginMasterCommand>
    {
        public LoginMasterCommandValidator()
        {
            RuleFor(p => p.Settings)
                .NotNull()
                .WithMessage("Settings can't be null");

            RuleFor(p => p.Settings.Username)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Settings.Password)
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
