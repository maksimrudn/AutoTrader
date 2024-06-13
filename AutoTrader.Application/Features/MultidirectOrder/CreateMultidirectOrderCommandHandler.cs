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
using AutoTrader.Application.Models;
using FluentValidation;

namespace AutoTrader.Application.Features.MultidirectOrder
{
    public class CreateMultidirectOrderCommandHandler : IRequestHandler<CreateMultidirectOrderCommand>
    {
        private readonly ISettingsService _settingsService;
        private readonly IDoubleStockClient _stockClients;
        private readonly Settings _settings;
        private readonly IMainFormView _view;

        public CreateMultidirectOrderCommandHandler(ISettingsService settingsService, IDoubleStockClient stockClients, IMainFormView view)
        {
            this._settingsService = settingsService;
            _settings = settingsService.GetSettings();
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(CreateMultidirectOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMultidirectOrderCommandValidator();
            var validationResult = await validator.ValidateAsync(_settings);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            _settingsService.UpdateSettings(_settings);

            if (!_stockClients.Master.Connected || !_stockClients.Slave.Connected)
            {
                throw new Exception("Не все клиенты авторизованы!");
            }

            int sl = _settings.SL;
            int tp = _settings.TP;
            int price = _settings.Price;
            int vol = _settings.Volume;
            bool bymarket = _settings.ByMarket;
            string seccode = _settings.Seccode;

            await StockOperationHelper.MakeMultidirect(_stockClients, price, vol, sl, tp, bymarket, seccode);

            return Unit.Value;
        }
    }
}
