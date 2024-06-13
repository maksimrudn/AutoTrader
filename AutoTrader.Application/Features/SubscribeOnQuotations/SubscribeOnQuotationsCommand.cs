using AutoTrader.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.SubscribeOnQuotations
{
    public class SubscribeOnQuotationsCommand: IRequest
    {
        public string Seccode { get; set; }
    }
}
