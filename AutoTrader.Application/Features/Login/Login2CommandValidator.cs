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
    public class Login2CommandValidator : AbstractValidator<Login2Command>
    {
        public Login2CommandValidator()
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
