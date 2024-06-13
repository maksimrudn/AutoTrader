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

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword1CommandHandler : IRequestHandler<ChangePassword1Command>
    {
        private readonly ISettingsService _settingsService;
        private readonly Settings _settings;
        private readonly IDoubleStockClient _stockClients;

        public ChangePassword1CommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients)
        {
            this._settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(ChangePassword1Command request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(_settings);
            _stockClients.Master.ChangePassword(_settings.Username, _settings.Password);

            return Unit.Value;
        }
    }
}
