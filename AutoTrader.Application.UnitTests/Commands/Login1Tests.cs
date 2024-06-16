using AutoTrader.Application.Features.Login;
using AutoTrader.Application.Models;
namespace AutoTrader.Application.UnitTests.Commands
{
    public class Login1Tests
    {
        LoginMasterCommand wrongReq = new LoginMasterCommand()
        {
            Settings = new Settings()
            {

            },
        };

        LoginMasterCommand successReq = new LoginMasterCommand()
        {
            Settings = new Settings()
            {
                ConnectionType = "Prod",
                Username = "test",
                Password = "test",
            },
        };

        LoginMasterCommandHandler login1CommandHandler;

        //public Login1Tests()
        //{
        //    //Settings settings = new Settings();

        //    //login1CommandHandler = new LoginMasterCommandHandler(ServiceMocks.GetSettingsService(settings).Object, ServiceMocks.GetDoubleStockClient().Object);
        //}

        //[Fact]
        //public async Task Exceptions()
        //{
        //    var exception = await Should.ThrowAsync<ValidationException>(() => login1CommandHandler.Handle(wrongReq, CancellationToken.None));
        //}

        //[Fact]
        //public async Task Success()
        //{
        //    var resp = await login1CommandHandler.Handle(successReq, CancellationToken.None);
        //    resp.ShouldBeOfType<LoginResponse>();
        //}
    }
}