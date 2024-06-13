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

namespace AutoTrader.Application.Features.SubscribeOnQuotations
{
    public class SubscribeOnQuotationsCommandHandler : IRequestHandler<SubscribeOnQuotationsCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly Settings _settings;
        private readonly IDoubleStockClient _stockClients;
        private readonly IMainFormView _view;

        public SubscribeOnQuotationsCommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients, IMainFormView view)
        {
            this._settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            this._stockClients = stockClients;
            this._view = view;
        }

        public async Task<Unit> Handle(SubscribeOnQuotationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _stockClients.Master.SubscribeQuotations(TradingMode.Futures, request.Seccode);
            }
            catch (Exception e)
            {
                _view.ShowMessage(e.Message);
            }

            return Unit.Value;
        }
    }
}
