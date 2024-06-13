using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Exceptions;
using AutoTrader.Application.Features.Login;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Models;
using AutoTrader.Application.UnitTests.Mocks;
using AutoTrader.Infrastructure.Stock;
using Shouldly;
namespace AutoTrader.Application.UnitTests
{
    public class Login1Tests
    {
        Login1Command wrongReq = new Login1Command()
        {
            Settings = new Settings()
            {
                
            },
        };

        Login1Command successReq = new Login1Command()
        {
            Settings = new Settings()
            {
                ConnectionType = "Prod",
                Username = "test",
                Password = "test",
            },
        };

        Login1CommandHandler login1CommandHandler;

        public Login1Tests()
        {
            Settings settings = new Models.Settings();

            login1CommandHandler = new Login1CommandHandler(ServiceMocks.GetSettingsService(settings).Object, ServiceMocks.GetDoubleStockClient().Object);
        }

        [Fact]
        public async Task Exceptions()
        {
            var exception = await Should.ThrowAsync<ValidationException>(() => login1CommandHandler.Handle(wrongReq, CancellationToken.None));
        }

        [Fact]
        public async Task Success()
        {
            var resp = await login1CommandHandler.Handle(successReq, CancellationToken.None);
            resp.ShouldBeOfType<LoginResponse>();
        }
    }
}