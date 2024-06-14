using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface ITransaqConnectorRequestHandler: IDisposable
    {
        result ConnectorSendCommand(command commandInfo);
    }
}
