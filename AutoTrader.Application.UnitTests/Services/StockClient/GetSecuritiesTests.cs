using AutoTrader.Infrastructure.Stock.Dummy;
using Shouldly;
using AutoTrader.Application.Exceptions;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using AutoTrader.Infrastructure.Stock;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class GetSecurities
    {
        ITransaqConnectorFactory _factory;

        public GetSecurities()
        {
            _factory = new TransaqConnectorEmulatorFactory();
        }

        [Fact]
        public async Task GetSecurities_DefaulScenario_Success()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.LoginAsync(Constants.TestUsername, 
                Constants.TestPassword);

            var securities = await stockClient.GetSecurities();
            securities.ShouldNotBeNull();
            securities.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetSecurities_Unauthorized_UnauthorizedException()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(async () =>
                                await stockClient.GetSecurities());

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.UnAuthorizedCode  );
        }
    }
}
