using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;

namespace AutoTrader.Infrastructure.Stock
{
    public class StockClientSlave : BaseStockClient
    {
        public StockClientSlave(ITransaqConnectorFactory transaqConnectorFactory) : base(transaqConnectorFactory)
        {

        }

        protected override ITransaqConnectorRequestHandler CreateRequestHandler()
        {
            return _transaqConnectorFactory.GetSlave();
        }
    }
}
