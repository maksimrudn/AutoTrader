using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Domain.Models;
using MediatR;

namespace AutoTrader.Application.Features.ComboSell
{
    public class ComboSellCommandHandler : IRequestHandler<ComboSellCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ComboSellCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(ComboSellCommand request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            ComboOrder co = new ComboOrder();
            co.SL = request.Settings.SL;
            co.TP = request.Settings.TP;
            co.Price = request.Settings.Price;
            co.Vol = request.Settings.Volume;
            co.ByMarket = request.Settings.ByMarket;
            co.Seccode = request.Settings.Seccode;
            co.OrderDirection = OrderDirection.Sell;

            await _stockClients.Master.CreateNewComboOrder(co).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
