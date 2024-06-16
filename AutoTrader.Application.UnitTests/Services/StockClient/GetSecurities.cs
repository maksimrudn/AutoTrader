using AutoTrader.Infrastructure.Stock.Dummy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using AutoTrader.Application.Exceptions;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using AutoTrader.Infrastructure.Stock;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class GetSecurities
    {
        TransaqConnectorFactory _factory;


        public GetSecurities()
        {
            _factory = new TransaqConnectorFactory(true);
        }

        [Fact]
        public async Task Success()
        {
            var stockClient = new StockClientMaster(_factory);

            await stockClient.Login(DummyTransaqConnectorRequestHandler.CorrectUsername, DummyTransaqConnectorRequestHandler.CorrectPassword, Domain.Models.Types.ConnectionType.Prod);

            var securities = await stockClient.GetSecurities();
            securities.ShouldNotBeNull();
            securities.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task UnAuthorizedException()
        {
            var stockClient = new StockClientMaster(_factory);

            var exception = await Should.ThrowAsync<StockClientException>(() =>
                                stockClient.GetSecurities());

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.UnAuthorizedCode  );
        }
    }
}
