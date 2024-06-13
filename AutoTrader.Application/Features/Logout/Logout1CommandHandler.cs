﻿using AutoMapper;
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

namespace AutoTrader.Application.Features.Logout
{
    public class Logout1CommandHandler : IRequestHandler<Logout1Command>
    {
        private readonly IDoubleStockClient _stockClients;

        public Logout1CommandHandler(IDoubleStockClient stockClients)
        {
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(Logout1Command request, CancellationToken cancellationToken)
        {
            _stockClients.Master.Logout();

            return Unit.Value;
        }
    }
}
