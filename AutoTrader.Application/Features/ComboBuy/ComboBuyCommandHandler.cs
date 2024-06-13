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
        private readonly Settings _settings;
        private readonly IDoubleStockClient _stockClients;

        public ComboBuyCommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients)
        {
            this._settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(ComboBuyCommand request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(_settings);

            ComboOrder co = new ComboOrder();
            co.SL = _settings.SL;
            co.TP = _settings.TP;
            co.Price = _settings.Price;
            co.Vol = _settings.Volume;
            co.ByMarket = _settings.ByMarket;
            co.Seccode = _settings.Seccode;
            co.OrderDirection = OrderDirection.Buy;

            await StockOperationHelper.HandleComboOperation(_stockClients.Master, co);

            return Unit.Value;
        }
    }
}
