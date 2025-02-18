using AutoTrader.Application.Exceptions;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Domain.Models;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using Shouldly;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class ComboOrderTests
    {
        ITransaqConnectorFactory _factory;
        private BaseStockClient stockClient;

        public ComboOrderTests()
        {
            _factory = new TransaqConnectorEmulatorFactory();
            stockClient = new StockClientMaster(_factory);
        }

        [Fact]
        public async Task Create_MarketOrderMatched_Success()
        {
            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                                    TxmlServerEmulator.TestPassword);

            var co = new ComboOrder()
            {
                TradingMode = TradingMode.Futures,
                OrderDirection = OrderDirection.Buy,
                Price = 0,
                ByMarket = true,
                Vol = 1,
                StopLoseOrderType = StopLoseOrderType.ConditionalOrder
            };
            
            
            await stockClient.CreateNewComboOrder(co);

            // Check order
            var order = stockClient.Orders.FirstOrDefault();
            Assert.NotNull(order);
            Assert.Equal(status.matched, order.status);
            
            // Check trades
            
            // Check positions
            
            // Check money
        }
        
        [Fact]
        public async Task Create_LimitOrder_Fail()
        {
            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                TxmlServerEmulator.TestPassword);

            var co = new ComboOrder()
            {
                TradingMode = TradingMode.Futures,
                OrderDirection = OrderDirection.Buy,
                Price = TxmlServerEmulator.LimitOrderPrice,
                ByMarket = false,
                Vol = 1,
                StopLoseOrderType = StopLoseOrderType.ConditionalOrder
            };
            
            await stockClient.CreateNewComboOrder(co);

            // Check order
            var order = stockClient.Orders.FirstOrDefault();
            Assert.NotNull(order);
            Assert.Equal(status.active, order.status);
            
            // Check trades
            
            // Check positions
            
            // Check money
        }
    }
}
