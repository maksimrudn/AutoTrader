using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface IStockAccountData
    {
        bool Connected { get; }

        string? FortsClientId { get; }

        Models.TransaqConnector.Ingoing.positions? Positions { get; }
        // todo: use dictionary instead list
        List<forts_position> FortsPositions { get; }

        string? Union { get; }

        double? Money { get; }

        List<Models.TransaqConnector.Ingoing.securities_ns.stock_security>? Securities { get; }

        List<Models.TransaqConnector.Ingoing.trades_ns.trade>? Trades { get; }

        List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? Orders { get; }

        List<Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesBuy { get; }

        List<Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesSell { get; }
    }
}
