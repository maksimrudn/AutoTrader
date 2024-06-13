using AutoTrader.Application.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
