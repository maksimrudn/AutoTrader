using AutoTrader.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword2Command: IRequest
    {
        public Settings Settings { get; set; }
    }
}
