using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.Dummy;

namespace AutoTrader.Infrastructure.Stock.TransaqConnector
{
    public class TransaqConnectorEmulatorFactory : ITransaqConnectorFactory
    {
        public ITransaqConnectorRequestHandler GetMaster() => new TxmlServerEmulator();

        public ITransaqConnectorRequestHandler GetSlave() => new TxmlServerEmulator();
    }
}
