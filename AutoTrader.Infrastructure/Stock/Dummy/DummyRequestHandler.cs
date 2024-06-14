using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public class DummyRequestHandler : ITXMLConnectorRequestHandler
    {
        private readonly TXMLConnectorInputStreamHandler _inputStreamHandler;
        string _username = "TEST";
        string _password = "TEST";
        decimal _freeMoney = 30_000;
        public DummyRequestHandler(TXMLConnectorInputStreamHandler inputStreamHandler)
        {
            _inputStreamHandler = inputStreamHandler;
        }

        public result ConnectorSendCommand(command commandInfo)
        {
            result res = new result();

            switch (commandInfo.id)
            {
                case command_id.connect:
                    //if (commandInfo.login != _username || commandInfo.password != _password)
                    //    throw new NotImplementedException();

                    res.success = true;
                    // send stream
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-login.csv", _inputStreamHandler.HandleData);
                    });
                    break;

                case command_id.get_mc_portfolio:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-portfolio.csv", _inputStreamHandler.HandleData);
                    });
                    break;

                case command_id.disconnect:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-logout.csv", _inputStreamHandler.HandleData);
                    });
                    break;

                case command_id.get_securities:
                    res.success = true;
                    Task.Run(() => {
                        DummyStreamGenerator.Generate("stream-getsecurities.csv", _inputStreamHandler.HandleData);
                    });
                    break;

                default:
                    break;
            }

            return res;
        }

        public void Dispose()
        {

        }
    }
}
