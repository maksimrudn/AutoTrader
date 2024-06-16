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

namespace AutoTrader.Application.Features.Logout
{
    public class Logout2CommandHandler : IRequestHandler<Logout2Command>
    {
        private readonly IDualStockClient _stockClients;

        public Logout2CommandHandler(IDualStockClient stockClients)
        {
            this._stockClients = stockClients;
        }

        public async Task<Unit> Handle(Logout2Command request, CancellationToken cancellationToken)
        {
            await _stockClients.Slave.Logout();

            return Unit.Value;
        }
    }
}
