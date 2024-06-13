using AutoTrader.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.Login
{
    public class Login2Command: IRequest<LoginResponse>
    {
        public Settings Settings { get; set; }
    }
}
