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
using FluentValidation;

namespace AutoTrader.Application.Features.Login
{
    public class Login2CommandHandler : IRequestHandler<Login2Command, LoginResponse>
    {
        private readonly ISettingsService _settingsService;
        private readonly Settings _settings;
        private readonly IDoubleStockClient _stockClients;

        public Login2CommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients)
        {
            this._settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            this._stockClients = stockClients;
        }

        public async Task<LoginResponse> Handle(Login2Command request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            var validator = new Login2CommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _settings.ConnectionType);

            await _stockClients.Slave.Login(_settings.GetUsername(), _settings.GetPassword(), connType);

            var resp = new LoginResponse()
            {
                SelectedSeccode = _settings.Seccode,
                ClientId = _stockClients.Slave.FortsClientId,
                Union = _stockClients.Slave.Union,
                FreeMoney = _stockClients.Slave.Money.ToString("N"),
            };

            return resp;
        }
    }
}
