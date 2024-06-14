using AutoTrader.Application.Exceptions;
using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class LoginTests
    {
        StockFactory _factory;

        public LoginTests()
        {
            _factory = new StockFactory();
        }

        [Fact]
        public async Task Login()
        {
            var stockClient = _factory.GetMaster(true);

            await stockClient.Login(DummyRequestHandler.CorrectUsername, DummyRequestHandler.CorrectPassword, Domain.Models.Types.ConnectionType.Prod);

            stockClient.Connected.ShouldBeTrue();
            stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }

        [Fact]
        public async Task AlreadyConnected()
        {
            var stockClient = _factory.GetMaster(true);

            await stockClient.Login(DummyRequestHandler.CorrectUsername,
                                        DummyRequestHandler.CorrectPassword,
                                        Domain.Models.Types.ConnectionType.Prod);

            var exception = await Should.ThrowAsync<StockClientException>(() =>
                                stockClient.Login(DummyRequestHandler.CorrectUsername,
                                DummyRequestHandler.CorrectPassword,
                                Domain.Models.Types.ConnectionType.Prod));

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.AlreadyLoggedInCode);
        }

        [Fact]
        public async Task WrongUsername()
        {
            var stockClient = _factory.GetMaster(true);

            var exception = await Should.ThrowAsync<StockClientException>(() =>
                                stockClient.Login("wrong username",
                                                    DummyRequestHandler.CorrectPassword,
                                                    Domain.Models.Types.ConnectionType.Prod));

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.ServerConnectionErrorCode);
        }

        [Fact]
        public async Task Logout()
        {
            var stockClient = _factory.GetMaster(true);

            stockClient.Logout();

            stockClient.Connected.ShouldBeFalse();
        }
    }
}
