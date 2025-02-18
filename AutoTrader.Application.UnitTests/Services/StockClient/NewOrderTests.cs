using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock.TransaqConnector;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class NewOrderTests
    {
        ITransaqConnectorFactory _factory;
        private BaseStockClient stockClient;

        public NewOrderTests()
        {
            _factory = new TransaqConnectorEmulatorFactory();
            stockClient = new StockClientMaster(_factory);
        }

        [Fact]
        public async Task CreateNewOrder_MarketOrderMatched_Success()
        {
            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                                    TxmlServerEmulator.TestPassword);


            var tid = stockClient.CreateNewOrder(TradingMode.Futures, "SiH5", OrderDirection.Buy, true, 0, 1);
            Assert.True(tid>0);

            // Check order
            var order = stockClient.Orders.FirstOrDefault();
            Assert.NotNull(order);
            Assert.Equal(status.matched, order.status);
            
            // Check trades
            
            // Check positions
            
            // Check money
        }
        
        // Open limit order placement
        // pay attention to orders table
        
        [Fact]
        public async Task CreateNewOrder_LimitOrder_Success()
        {
            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                TxmlServerEmulator.TestPassword);


            var tid = stockClient.CreateNewOrder(TradingMode.Futures, "SiH5", OrderDirection.Buy, false, TxmlServerEmulator.LimitOrderPrice, 1);
            Assert.True(tid>0);

            // Check order
            var order = stockClient.Orders.FirstOrDefault();
            Assert.NotNull(order);
            Assert.Equal(status.active, order.status);
        }
        
        // limit order delayed execution
        // pay attention to
        // - moment of placement (and state of orders table)
        // - moment of execution
        
        [Fact]
        public async Task CreateNewOrder_NotEnoughtMoney_Fail()
        {
            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                TxmlServerEmulator.TestPassword);

            Assert.ThrowsAny<Exception>(()=>stockClient.CreateNewOrder(TradingMode.Futures, "SiH5", OrderDirection.Buy, true, 0, TxmlServerEmulator.MaxOrderVolume+1));
        }
        
        // Conditional order
    }
}
