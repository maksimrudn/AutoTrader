using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Models;
using Moq;

namespace AutoTrader.Application.UnitTests.Mocks
{
    public class ServiceMocks
    {
        public static Mock<ISettingsService> GetSettingsService(Settings settings)
        {
            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(srv => srv.GetSettings()).Returns(settings);

            return settingsService;
        }

        internal static Mock<IDualStockClient> GetDoubleStockClient()
        {
            var doubleStockClientService = new Mock<IDualStockClient>();
            //doubleStockClientService.Setup(srv => srv.Master).Returns(new DummyStockClient());
            //doubleStockClientService.Setup(srv => srv.Slave).Returns(new DummyStockClient());

            return doubleStockClientService;
        }
    }
}
