using AutoTrader.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.ComboSell
{
    public class ComboSellCommand: IRequest
    {
        public Settings Settings { get; set; }
    }
}
