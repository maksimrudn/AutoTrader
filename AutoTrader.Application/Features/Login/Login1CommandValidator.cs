using AutoTrader.Application.Models;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.Login
{
    public class Login1CommandValidator : AbstractValidator<Login1Command>
    {
        public Login1CommandValidator()
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
