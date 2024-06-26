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

        List<forts_position> FortsPositions { get; }

        string? Union { get; }

        double? Money { get; }
    }
}
