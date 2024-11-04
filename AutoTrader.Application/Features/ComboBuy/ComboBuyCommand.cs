using AutoTrader.Application.Models;
using MediatR;

namespace AutoTrader.Application.Features.ComboBuy
{
    public class ComboBuyCommand: IRequest
    {
        public Settings Settings { get; set; }
    }
}
