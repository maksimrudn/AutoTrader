using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.Dummy;

namespace AutoTrader.Infrastructure.Stock.TransaqConnector
{
    public class TransaqConnectorFactory : ITransaqConnectorFactory
    {
        private readonly bool dummyMode;

        public TransaqConnectorFactory(bool dummyMode = false)
        {
            this.dummyMode = dummyMode;
        }

        public ITransaqConnectorRequestHandler GetMaster()
        {
            return dummyMode ?
                        new TxmlServerEmulator() :
                        new TransaqConnectorRequestHandler("txmlconnector1.dll");
        }

        public ITransaqConnectorRequestHandler GetSlave()
        {
            return dummyMode ?
                        new TxmlServerEmulator() :
                        new TransaqConnectorRequestHandler("txmlconnector2.dll");
        }
    }
}
