using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Exceptions;
using AutoTrader.Application.Features.Login;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Models;
using AutoTrader.Application.UnitTests.Mocks;
using AutoTrader.Infrastructure.Stock;
using Shouldly;
namespace AutoTrader.Application.UnitTests.Services
{
    public class StockClientTests
    {
        IStockClient _stockClient;


        public StockClientTests()
        {
            var factory = new StockFactory();

            _stockClient = factory.GetMaster(true);
        }

        [Fact]
        public async Task Exceptions()
        {

        }

        [Fact]
        public async Task Login()
        {
            await _stockClient.Login("test", "test", Domain.Models.Types.ConnectionType.Prod);

            _stockClient.Connected.ShouldBeTrue();
            _stockClient.FortsClientId.ShouldNotBeNull()
                                        .ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetSecurities()
        {
            var res = await _stockClient.GetSecurities();

            res.ShouldNotBeNull();
        }

        [Fact]
        public async Task Logout()
        {
            _stockClient.Logout();

            _stockClient.Connected.ShouldBeFalse();
        }
    }
}