using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
