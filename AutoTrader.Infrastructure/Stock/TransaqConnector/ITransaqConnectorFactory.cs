using AutoTrader.Infrastructure.Contracts.Transaq;

namespace AutoTrader.Infrastructure.Stock.TransaqConnector
{
    public interface ITransaqConnectorFactory
    {
        ITransaqConnectorRequestHandler GetMaster();

        ITransaqConnectorRequestHandler GetSlave();
    }
}
