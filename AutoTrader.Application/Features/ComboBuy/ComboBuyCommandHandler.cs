using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Domain.Models;
using MediatR;

namespace AutoTrader.Application.Features.ComboBuy
{
    public class ComboBuyCommandHandler : IRequestHandler<ComboBuyCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ComboBuyCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            _settingsService = settingsService;
            _stockClients = stockClients;
        }

        public async Task<Unit> Handle(ComboBuyCommand request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            ComboOrder co = new ComboOrder();
            co.SL = request.Settings.SL;
            co.TP = request.Settings.TP;
            co.Price = request.Settings.Price;
            co.Vol = request.Settings.Volume;
            co.ByMarket = request.Settings.ByMarket;
            co.Seccode = request.Settings.Seccode;
            co.OrderDirection = OrderDirection.Buy;

            await _stockClients.Master.CreateNewComboOrder(co).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
