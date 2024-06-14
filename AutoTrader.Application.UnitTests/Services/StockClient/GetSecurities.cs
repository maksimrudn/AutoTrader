using AutoTrader.Infrastructure.Stock.Dummy;
using AutoTrader.Infrastructure.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using AutoTrader.Application.Exceptions;

namespace AutoTrader.Application.UnitTests.Services.StockClient
{
    public class GetSecurities
    {
        StockFactory _factory;


        public GetSecurities()
        {
            _factory = new StockFactory();
        }

        [Fact]
        public async Task Success()
        {
            var stockClient = _factory.GetMaster(true);

            await stockClient.Login(DummyRequestHandler.CorrectUsername, DummyRequestHandler.CorrectPassword, Domain.Models.Types.ConnectionType.Prod);

            var securities = await stockClient.GetSecurities();
            securities.ShouldNotBeNull();
            securities.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task UnAuthorizedException()
        {
            var stockClient = _factory.GetMaster(true);

            var exception = await Should.ThrowAsync<StockClientException>(() =>
                                stockClient.GetSecurities());

            exception.ErrorCode.ShouldBeEquivalentTo(CommonErrors.UnAuthorizedCode  );
        }
    }
}
