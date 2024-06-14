using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface ITXMLConnectorRequestHandler: IDisposable
    {
        result ConnectorSendCommand(command commandInfo);
    }
}
