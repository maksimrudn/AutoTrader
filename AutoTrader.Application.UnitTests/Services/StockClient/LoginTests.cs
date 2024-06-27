using AutoTrader.Application.Exceptions;
using AutoTrader.Infrastructure.Stock.Dummy;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using AutoTrader.Infrastructure.Stock;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class LoginTests
    {
        TransaqConnectorFactory _factory;

        public LoginTests()
        {
            _factory = new TransaqConnectorFactory(true);
        }

        [Fact]
        public async Task Login()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername, 
                                    DummyTransaqConnectorRequestHandler.CorrectPassword, 
                                    Domain.Models.Types.ConnectionType.Prod);

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();

            // todo: implement check of securities
        }

        [Fact]
        public async Task LoginWithPositions()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                    DummyTransaqConnectorRequestHandler.CorrectPassword,
                                    Domain.Models.Types.ConnectionType.Prod);

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReLoginAfterConnectionError()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.Login("wrong username",
                                                    DummyTransaqConnectorRequestHandler.CorrectPassword,
                                                    Domain.Models.Types.ConnectionType.Prod));

            stockClient.Connected.ShouldBeFalse();
                        
            await Should.NotThrowAsync(async () =>
                                await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                                        DummyTransaqConnectorRequestHandler.CorrectPassword,
                                                        Domain.Models.Types.ConnectionType.Prod));

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }


        [Fact]
        public async Task AlreadyConnected()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                        DummyTransaqConnectorRequestHandler.CorrectPassword,
                                        Domain.Models.Types.ConnectionType.Prod);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername,
                                                        DummyTransaqConnectorRequestHandler.CorrectPassword,
                                                        Domain.Models.Types.ConnectionType.Prod));

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.AlreadyLoggedInCode);
        }

        [Fact]
        public async Task WrongUsername()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.Login("wrong username",
                                                    DummyTransaqConnectorRequestHandler.CorrectPassword,
                                                    Domain.Models.Types.ConnectionType.Prod));
            stockClient.Connected.ShouldBeFalse();

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.ServerConnectionErrorCode);
        }

        [Fact]
        public async Task Logout()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername, 
                                    DummyTransaqConnectorRequestHandler.CorrectPassword, 
                                    Domain.Models.Types.ConnectionType.Prod);

            stockClient.FortsClientId.ShouldNotBeNullOrEmpty();
            stockClient.Connected.ShouldBeTrue();
            await stockClient.Logout();
            stockClient.Connected.ShouldBeFalse();
        }
    }
}
