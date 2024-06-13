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

namespace AutoTrader.Application.Features.Login
{
    public class Login1CommandHandler : IRequestHandler<Login1Command, LoginResponse>
    {
        private readonly ISettingsService _settingsService;
        private readonly Settings _settings;
        private readonly IDoubleStockClient _stockClients;

        public Login1CommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients)
        {
            this._settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            this._stockClients = stockClients;
        }

        public async Task<LoginResponse> Handle(Login1Command request, CancellationToken cancellationToken)
        {
            int connectorNumber = 0;
            _settingsService.UpdateSettings(request.Settings);

            var validator = new Login1CommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _settings.ConnectionType);

            await _stockClients.Master.Login(_settings.GetUsername(), _settings.GetPassword(), connType);

            var _seccodeList = (await _stockClients.Master.GetSecurities())
                                        .Where(x => x.board == boardsCode.FUT.ToString())
                                        .Select(x => x.seccode)
                                        .OrderBy(x => x)
                                        .ToList();

            var resp = new LoginResponse()
            {
                SeccodeList = _seccodeList,
                SelectedSeccode = _settings.Seccode,
                ClientId = _stockClients.Master.FortsClientId,
                Union = _stockClients.Master.Union,
                FreeMoney = _stockClients.Master.Money.ToString("N"),
            };        

            return resp;
        }
    }
}
