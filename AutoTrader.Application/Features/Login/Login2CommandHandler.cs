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
        private readonly IDualStockClient _stockClients;

        public Login2CommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<LoginResponse> Handle(Login2Command request, CancellationToken cancellationToken)
        {
            _settingsService.UpdateSettings(request.Settings);

            var validator = new Login2CommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), request.Settings.ConnectionType);

            await _stockClients.Slave.Login(request.Settings.GetUsername(), request.Settings.GetPassword(), connType);

            var resp = new LoginResponse()
            {
                SelectedSeccode = request.Settings.Seccode,
                ClientId = _stockClients.Slave.FortsClientId,
                Union = _stockClients.Slave.Union,
                FreeMoney = _stockClients.Slave.Money.ToString("N"),
            };

            return resp;
        }
    }
}
