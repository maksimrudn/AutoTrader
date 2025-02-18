using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.mc_portfolio_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.orders_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.trades_ns;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;
using System.Collections;
using System.Collections.Specialized;
using System.Text.Json;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public partial class TxmlServerEmulator : ITransaqConnectorRequestHandler
    {
        private Dictionary<command_id, List<Func<List<string>>>> _responseStreamGenerators;

        public TxmlServerEmulator()
        {
            InputStreamHandler = new TransaqConnectorInputStreamHandler();
            
            _responseStreamGenerators = new()
            {
                {
                    command_id.connect, new List<Func<List<string>>>()
                    {
                        () => _clients.Select(x=> x.ToXml()).ToList(),
                        
                        // Todo: implement later. Now it's not used
                        // markets, boards, candlekinds
                        // {  "initial_data", "initial_data.csv" },
                        
                        // Todo: implement later. Now it's not used.
                        // sec_info_upd, messages, securities
                        // {  "periodical_data", "periodical_data.csv" },
                        
                        () => new List<string>() { _stockSecurities.ToXml()},
                        () => new List<string>() { _positions.ToXml()},
                        
                        // Todo: implement. It's not implemented yet. There is no such entity 
                        // {  "union", "union.csv" },
                        
                        () => new List<string>() { _overnight.ToXml()},
                        () => new List<string>() { _serverStatus.ToXml() },
                        () => new List<string>() { _orders.ToXml()},
                        () => new List<string>() { _trades.ToXml()},
                    }
                },
                {
                    command_id.get_mc_portfolio, new List<Func<List<string>>>()
                    {
                        ()=> new List<string>() { _mcPortfolio.ToXml()},
                    }
                },
                {
                    command_id.disconnect, new List<Func<List<string>>>()
                    {
                        ()=> new List<string>() { _serverStatus.ToXml()}
                    }
                },
                {
                    command_id.get_securities, new List<Func<List<string>>>()
                    {
                        ()=> new List<string>() { _stockSecurities.ToXml()}
                    }
                },
                {
                    command_id.neworder, new List<Func<List<string>>>()
                    {
                        ()=> new List<string>() { _orders.ToXml()},
                        ()=> new List<string>() { _trades.ToXml()},
                        () => _positions.forts_position
                            .Select(x =>
                            {
                                var p = new positions
                                {
                                    forts_position = new List<forts_position> { x },
                                    forts_money = _positions.forts_money
                                };
                                return p.ToXml();
                            })
                            .ToList()
                    }
                }
            };
            
        }
        
        public TransaqConnectorInputStreamHandler InputStreamHandler { get; private set; }

        public result ConnectorSendCommand(command commandInfo)
        {
            result res = new result();

            List<string> streamSequence = null;
            List<Func<List<string>>> streamDelegates = null;

            switch (commandInfo.id)
            {
                case command_id.connect:
                    if (commandInfo.login == TestUsername && commandInfo.password == TestPassword)
                    {
                        res.success = true;
                        _serverStatus.connected = "true";
                        SendResponseStream(commandInfo.id);
                    }
                    else
                    {
                        res.success = true;
                        _serverStatus.connected = "error";
                        _serverStatus.InnerText = "Ошибка подключения";
                        SendResponseStream(commandInfo.id);
                    }
                    break;

                case command_id.get_mc_portfolio:
                    res.success = true;
                    SendResponseStream(commandInfo.id);
                    break;

                case command_id.disconnect:
                    res.success = true;
                    _serverStatus.connected = "false";
                    SendResponseStream(commandInfo.id);
                    break;

                case command_id.get_securities:
                    res.success = true;
                    SendResponseStream(commandInfo.id);
                    break;

                case command_id.neworder:
                    if (commandInfo.quantityValue > MaxOrderVolume)
                    {
                        res.success = false;
                        res.message = "[145]Недостаток обеспечения в сумме";
                    }
                    // limit order
                    else if (commandInfo.bymarket == null && commandInfo.buysell == buysell.B.ToString() && commandInfo.priceValue == LimitOrderPrice)
                    {
                        res.success = true;
                        res.transactionid = 1;
                        
                        var order = new order()
                        {
                            transactionid = 1,
                            orderno = 1,
                            board = commandInfo.security.board,
                            union = TestUnion,
                            seccode = commandInfo.security.seccode,
                            client = commandInfo.client,
                            quantity = commandInfo.quantityValue.Value,
                            status = status.active,
                            buysell = commandInfo.buysell
                        };
                        _orders.order.Add(order);
                        
                        var position = _positions.forts_position.FirstOrDefault(x => x.seccode == commandInfo.security.seccode);
                        if (position == null)
                        {
                            position = new forts_position();
                            position.client = commandInfo.client;
                            position.seccode = commandInfo.security.seccode;
                        }
                        if (commandInfo.buysell == buysell.B.ToString())
                        {
                            position.openbuys += commandInfo.quantityValue.Value;
                        }
                        else
                        {
                            position.opensells += commandInfo.quantityValue.Value;
                        }
                        
                        SendResponseStream(commandInfo.id);
                    }
                    // order execution
                    else
                    {
                        res.success = true;
                        res.transactionid = 1;

                        // Orders handling
                        var order = new order()
                        {
                            transactionid = 1,
                            orderno = 1,
                            board = commandInfo.security.board,
                            union = TestUnion,
                            seccode = commandInfo.security.seccode,
                            client = commandInfo.client,
                            quantity = commandInfo.quantityValue.Value,
                            status = status.matched,
                            buysell = commandInfo.buysell
                        };
                        _orders.order.Add(order);
                        
                        // Trades handling
                        _trades.trade.Add(new trade()
                        {
                            orderno = order.orderno,
                            tradeno = 1,
                            board = order.board,
                            seccode = order.seccode,
                            client = order.client,
                            buysell = order.buysell,
                            quantity = order.quantity,
                        });

                        // Positions handling
                        var position = _positions.forts_position.FirstOrDefault(x => x.seccode == commandInfo.security.seccode);
                        if (position == null)
                        {
                            position = new forts_position();
                            position.client = commandInfo.client;
                            position.seccode = commandInfo.security.seccode;
                            _positions.forts_position.Add(position);
                        }
                        
                        if (commandInfo.buysell == buysell.B.ToString())
                        {
                            position.totalnet += commandInfo.quantityValue.Value;
                            position.todaybuy += commandInfo.quantityValue.Value;
                        }
                        else
                        {
                            position.totalnet -= commandInfo.quantityValue.Value;
                            position.todaysell += commandInfo.quantityValue.Value;
                        }

                        if (_positions.forts_money == null)
                        {
                            _positions.forts_money = new forts_money()
                            {
                                client = TestFortsClientId
                            };
                        }
                        
                        // MC Portfolio handling
                        var portfolio_security = _mcPortfolio.securities.FirstOrDefault(x => x.seccode == commandInfo.security.seccode);
                        if (portfolio_security == null)
                        {
                            portfolio_security = new security();
                            _mcPortfolio.securities.Add(new security()
                            {
                                market = 4,
                                seccode = commandInfo.security.seccode,
                            });
                        }
                        if (commandInfo.buysell == buysell.B.ToString())
                        {
                            portfolio_security.bought += commandInfo.quantityValue.Value;
                            portfolio_security.balance += commandInfo.quantityValue.Value;
                        }
                        else
                        {
                            portfolio_security.sold += commandInfo.quantityValue.Value;
                            portfolio_security.balance -= commandInfo.quantityValue.Value;
                        }
                        
                        SendResponseStream(commandInfo.id);
                    }
                    break;
                default:
                    break;
            }

            return res;
        }

        private void SendResponseStream(command_id cmd)
        {
            List<Func<List<string>>> streamDelegates = _responseStreamGenerators[cmd];
            foreach (Func<List<string>> stream in streamDelegates)
            {
                Task.Run(async () =>
                {
                    stream().ForEach(x=>InputStreamHandler.HandleData(x));
                }).Wait();
            }                     
        }

        public void Dispose()
        {

        }
    }
}
