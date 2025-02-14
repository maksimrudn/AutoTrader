﻿using AutoTrader.Application.Exceptions;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using Shouldly;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class LoginTests
    {
        ITransaqConnectorFactory _factory;

        public LoginTests()
        {
            _factory = new TransaqConnectorEmulatorFactory();
        }

        [Fact]
        public async Task Login_RightData_Success()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.LoginAsync(TxmlServerEmulator.TestUsername, 
                                    TxmlServerEmulator.TestPassword);

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();

            // todo: implement check of securities
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
