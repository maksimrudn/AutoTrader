using AutoMapper;
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Contracts.UI;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using System.Diagnostics;
using AutoTrader.Application.Models;

namespace AutoTrader.Application.Features.ComboBuy
{
    public class ComboBuyCommandHandler : IRequestHandler<ComboBuyCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ComboBuyCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
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

            await StockOperationHelper.HandleComboOperation(_stockClients.Master, co);

            return Unit.Value;
        }
    }
}
