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

namespace AutoTrader.Application.Features.ChangePassword
{
    public class ChangePassword2CommandHandler : IRequestHandler<ChangePassword1Command>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _stockClients;

        public ChangePassword2CommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(ChangePassword1Command request, CancellationToken cancellationToken)
        {
            _stockClients.Slave.ChangePassword(request.Settings.Username2, request.Settings.Password2);
            _settingsService.UpdateSettings(request.Settings);

            return Unit.Value;
        }
    }
}
