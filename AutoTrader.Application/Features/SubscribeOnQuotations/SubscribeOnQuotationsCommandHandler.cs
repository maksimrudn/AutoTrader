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
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using System.Diagnostics;
using AutoTrader.Application.Models;

namespace AutoTrader.Application.Features.SubscribeOnQuotations
{
    public class SubscribeOnQuotationsCommandHandler : IRequestHandler<SubscribeOnQuotationsCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public SubscribeOnQuotationsCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(SubscribeOnQuotationsCommand request, CancellationToken cancellationToken)
        {
            var settings = _settingsService.GetSettings();
            settings.Seccode = request.Seccode;
            _settingsService.UpdateSettings(settings);
            _stockClients.Master.SubscribeQuotations(TradingMode.Futures, request.Seccode);

            return Unit.Value;
        }
    }
}
