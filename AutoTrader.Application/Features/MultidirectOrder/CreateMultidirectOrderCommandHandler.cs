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
        private readonly IDualStockClient _stockClients;

        public CreateMultidirectOrderCommandHandler(ISettingsService settingsService, IDualStockClient stockClients)
        {
            this._settingsService = settingsService;
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(CreateMultidirectOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateMultidirectOrderCommandValidator();
            var validationResult = await validator.ValidateAsync(request.Settings).ConfigureAwait(false);

            if (validationResult.Errors.Count > 0)
                throw new Exceptions.ValidationException(validationResult);

            _settingsService.UpdateSettings(request.Settings);

            if (!_stockClients.Master.Connected || !_stockClients.Slave.Connected)
            {
                throw new Exception("Не все клиенты авторизованы!");
            }

            int sl = request.Settings.SL;
            int tp = request.Settings.TP;
            int price = request.Settings.Price;
            int vol = request.Settings.Volume;
            bool bymarket = request.Settings.ByMarket;
            string seccode = request.Settings.Seccode;

            await StockOperationHelper.MakeMultidirect(_stockClients, price, vol, sl, tp, bymarket, seccode).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
