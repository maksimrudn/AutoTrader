using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Stock.TransaqConnector;

namespace AutoTrader.Infrastructure.Contracts.Transaq
{
    public interface ITransaqConnectorRequestHandler: IDisposable
    {
        result ConnectorSendCommand(command commandInfo);
        TransaqConnectorInputStreamHandler InputStreamHandler { get; }
    }
}
