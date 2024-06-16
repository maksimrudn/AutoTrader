using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public class DummyTransaqConnectorRequestHandler : ITransaqConnectorRequestHandler
    {
        public TransaqConnectorInputStreamHandler InputStreamHandler { get; private set; }
        decimal _freeMoney = 30_000;

        public DummyTransaqConnectorRequestHandler()
        {
            InputStreamHandler = new TransaqConnectorInputStreamHandler();
        }

        public result ConnectorSendCommand(command commandInfo)
        {
            result res = new result();

            switch (commandInfo.id)
            {
                case command_id.connect:
                    if (commandInfo.login != CorrectUsername || commandInfo.password != CorrectPassword)
                    {
                        res.success = true;
                        Task.Run(() =>
                        {
                            DummyStreamGenerator.Generate("stream-login-wrong-user-pass.csv", InputStreamHandler.HandleData);
                        });
                    }
                    else
                    {
                        res.success = true;
                        // send stream
                        Task.Run(() =>
                        {
                            DummyStreamGenerator.Generate("stream-login.csv", InputStreamHandler.HandleData);
                        });
                    }
                    break;

                case command_id.get_mc_portfolio:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-portfolio.csv", InputStreamHandler.HandleData);
                    });
                    break;

                case command_id.disconnect:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-logout.csv", InputStreamHandler.HandleData);
                    });
                    break;

                case command_id.get_securities:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-getsecurities.csv", InputStreamHandler.HandleData);
                    });
                    break;

                case command_id.neworder:
                    if (commandInfo.quantityValue > 1)
                    {
                        res.success = false;
                        res.message = "[145]Недостаток обеспечения в сумме";
                    }
                    else
                    {
                        res.success = true;

                        throw new NotImplementedException();
                        //Task.Run(() => {
                        //    DummyStreamGenerator.Generate("stream-getsecurities.csv", _inputStreamHandler.HandleData);
                        //});
                    }
                    break; 

                default:
                    break;
            }

            return res;
        }

        public static string CorrectUsername { get; } = "TEST";
        public static string CorrectPassword { get; } = "TEST";

        public void Dispose()
        {

        }
    }
}
