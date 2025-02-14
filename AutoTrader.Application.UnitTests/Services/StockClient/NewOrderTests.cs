using AutoTrader.Application.Exceptions;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using Shouldly;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class NewOrderTests
    {
        ITransaqConnectorFactory _factory;

        public NewOrderTests()
        {
            _factory = new TransaqConnectorEmulatorFactory();
        }

        [Fact]
        public async Task CreateNewOrder_MarketOrderMatched_Success()
        {
            var stockClient = new StockClientMaster(_factory);

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

        [Fact]
        public async Task Login_WithPositions_Success()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.LoginAsync(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                    DummyTransaqConnectorRequestHandler.CorrectPassword);

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReLoginAfterConnectionError()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.LoginAsync("wrong username",
                                                    DummyTransaqConnectorRequestHandler.CorrectPassword));

            stockClient.Connected.ShouldBeFalse();
                        
            await Should.NotThrowAsync(async () =>
                                await stockClient.LoginAsync(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                                        DummyTransaqConnectorRequestHandler.CorrectPassword));

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }


        [Fact]
        public async Task AlreadyConnected()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.LoginAsync(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                        DummyTransaqConnectorRequestHandler.CorrectPassword);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.LoginAsync(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                                        DummyTransaqConnectorRequestHandler.CorrectPassword));

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.AlreadyLoggedInCode);
        }

        [Fact]
        public async Task WrongUsername()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.LoginAsync("wrong username",
                                                    DummyTransaqConnectorRequestHandler.CorrectPassword));
            stockClient.Connected.ShouldBeFalse();

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.ServerConnectionErrorCode);
        }

        [Fact]
        public async Task Logout()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.LoginAsync(DummyTransaqConnectorRequestHandler.CorrectUsername, 
                                    DummyTransaqConnectorRequestHandler.CorrectPassword);

            stockClient.FortsClientId.ShouldNotBeNullOrEmpty();
            stockClient.Connected.ShouldBeTrue();
            await stockClient.Logout();
            stockClient.Connected.ShouldBeFalse();
        }
    }
}
