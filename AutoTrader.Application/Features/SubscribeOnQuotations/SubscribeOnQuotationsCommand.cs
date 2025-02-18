using MediatR;

namespace AutoTrader.Application.Features.SubscribeOnQuotations
{
    public class SubscribeOnQuotationsCommand: IRequest
    {
        public string Seccode { get; set; }
    }
}
