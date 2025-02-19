using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using System.Collections.Specialized;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    // Todo: remove after refactoring of StockClient
    [Obsolete]
    public class DummyTransaqConnectorRequestHandler : ITransaqConnectorRequestHandler
    {
        Dictionary<command_id, OrderedDictionary> _dataSequence = new()
        {
            { 
                command_id.connect, new OrderedDictionary()
                {
                    {  "clients", "clients.csv" },
                    {  "initial_data", "initial_data.csv" },
                    {  "periodical_data", "periodical_data.csv" },
                    {  "positions", "positions.csv" },
                    {  "union", "union.csv" },
                    {  "overnight", "overnight.csv" },
                    {  "server_status", "server_status_connected.csv" }
                }
            },
            {
                command_id.get_mc_portfolio, new OrderedDictionary()
                {
                    {  "mc_portfolio", "mc_portfolio.csv" },
                }
            }
        };

        Dictionary<command_id, OrderedDictionary> _dataSequenceWithData = new()
        {
            {
                command_id.connect, new OrderedDictionary()
                {
                    {  "clients", "clients.csv" },
                    {  "initial_data", "initial_data.csv" },
                    {  "periodical_data", "periodical_data.csv" },
                    {  "positions", "LoginWithInitialData\\positions_initial_closed.csv" },
                    {  "union", "union.csv" },
                    {  "overnight", "overnight.csv" },
                    {  "server_status", "server_status_connected.csv" },
                    {  "orders", "LoginWithInitialData\\orders.csv" },
                    {  "trades", "LoginWithInitialData\\trades.csv" },
                }
            },
            {
                command_id.get_mc_portfolio, new OrderedDictionary()
                {
                    {  "mc_portfolio", "LoginWithInitialData\\mc_portfolio.csv" },
                }
            }
        };

        public TransaqConnectorInputStreamHandler InputStreamHandler { get; private set; }
        decimal _freeMoney = 30_000;

        public DummyTransaqConnectorRequestHandler()
        {
            InputStreamHandler = new TransaqConnectorInputStreamHandler();
        }

        public result ConnectorSendCommand(command commandInfo)
        {
            result res = new result();

            List<string> streamSequence = null;

            switch (commandInfo.id)
            {
                case command_id.connect:
                    if (commandInfo.login == Constants.TestUsername && commandInfo.password == Constants.TestPassword)
                    {
                        res.success = true;

                        streamSequence = _dataSequenceWithData[command_id.connect].Values.Cast<string>().ToList();

                        foreach (string filename in streamSequence)
                        {
                            Task.Run(async () =>
                            {
                                await DummyStreamGenerator.Generate(filename, InputStreamHandler.HandleData);

                            }).Wait();
                        }                                              
                    }
                    else
                    {
                        res.success = true;

                        Task.Run(async () =>
                        {
                            await DummyStreamGenerator.Generate("stream-login-wrong-user-pass.csv", InputStreamHandler.HandleData);

                        });
                    }
                    break;

                case command_id.get_mc_portfolio:
                    res.success = true;


                    streamSequence = _dataSequenceWithData[command_id.get_mc_portfolio].Values.Cast<string>().ToList();

                    foreach (string filename in streamSequence)
                    {
                        Task.Run(async () =>
                        {
                            await DummyStreamGenerator.Generate(filename, InputStreamHandler.HandleData);

                        }).Wait();
                    }


                    //Task.Run(async () => {
                    //    await DummyStreamGenerator.Generate("stream-portfolio.csv", InputStreamHandler.HandleData);
                    //});
                    break;

                case command_id.disconnect:
                    res.success = true;
                    Task.Run(async () => {
                        await DummyStreamGenerator.Generate("stream-logout.csv", InputStreamHandler.HandleData);
                    });
                    break;

                case command_id.get_securities:
                    res.success = true;
                    Task.Run(async () => {
                        await DummyStreamGenerator.Generate("stream-getsecurities.csv", InputStreamHandler.HandleData);
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

        
        public void Dispose()
        {

        }
    }
}
