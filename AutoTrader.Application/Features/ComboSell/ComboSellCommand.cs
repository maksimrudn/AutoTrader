using AutoTrader.Application.Models;
using MediatR;

namespace AutoTrader.Application.Features.ComboSell
{
    public class ComboSellCommand: IRequest
    {
        public Settings Settings { get; set; }
    }
}
