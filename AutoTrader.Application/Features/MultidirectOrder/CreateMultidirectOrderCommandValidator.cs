using AutoTrader.Application.Models;
using FluentValidation;

namespace AutoTrader.Application.Features.MultidirectOrder
{
    public class CreateMultidirectOrderCommandValidator : AbstractValidator<Settings>
    {
        public CreateMultidirectOrderCommandValidator()
        {
            RuleFor(p => p.Seccode)
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.");
        }
    }
}
