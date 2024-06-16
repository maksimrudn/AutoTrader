using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Contracts.Transaq
{
    public interface ITransaqConnectorRequestHandler: IDisposable
    {
        result ConnectorSendCommand(command commandInfo);

        TransaqConnectorInputStreamHandler InputStreamHandler { get; }
    }
}
