using AutoTrader.Application.Common;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using AutoTrader.Application.UnManaged;
using AutoTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTrader.Infrastructure.Stock
{
    public class StockClient: IStockClient
    {
        TXMLConnectorRequestHandler _requestHandler;
        TXMLConnectorInputStreamHandler _inputStreamHandler;

        Dictionary<TradingMode, boardsCode> _tradingModeMapping = new Dictionary<TradingMode, boardsCode>()
        {
            { TradingMode.Futures, boardsCode.FUT }
        };

        Dictionary<OrderDirection, buysell> _orderDirectionMapping = new Dictionary<OrderDirection, buysell>()
        {
            { OrderDirection.Buy, buysell.B},
            { OrderDirection.Sell, buysell.S}
        };

        public event EventHandler<OnMCPositionsUpdatedEventArgs> OnMCPositionsUpdated;

        public bool Connected
        {
            get
            {
                bool res = false;
                if (_inputStreamHandler.ServerStatus != null && _inputStreamHandler.ServerStatus.connected == "true") res = true;

                return res;
            }
        }

        public bool PositionsIsActual
        {
            get { return _inputStreamHandler.PositionsIsActual; }
            set { _inputStreamHandler.PositionsIsActual = value; }
        }
        
        public string FortsClientId 
        { 
            get 
            { 
                return (_inputStreamHandler.Forts_client != null) ? _inputStreamHandler.Forts_client.forts_acc : null; 
            } 
        }

        public double Money
        {
            get
            {
                double res = 0;

                try
                {
                    res = _inputStreamHandler.mc_portfolio.moneys.First(x=>x.currency == "RUB").balance;
                }
                catch { }

                return res;
            }
        }

        public string Union
        {
            get
            {
                return _inputStreamHandler.Forts_client?.union;
            }
        }

        public candlekinds Candlekinds
        {
            get
            {
                return _inputStreamHandler.Candlekinds;
            }
        }

        public List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> QuotesBuy
        {
            get
            {
                List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> quotes1;

                lock (_inputStreamHandler.Quotes)
                {
                    quotes1 = new List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote>(_inputStreamHandler.Quotes);
                }

                var q = quotes1.Where(x => x.buy > 0 && x.sell == 0).OrderByDescending(x => x.price).ToList();

                return q;

                //lock (_quotes)
                //{
                //    //var q = (from quotes in _quotes
                //    //         where quotes.buy > 0
                //    //         orderby quotes.price descending
                //    //         select quotes).ToList();
                //    var q = _quotes.Where(x => x.buy > 0 && x.sell == 0).OrderByDescending(x => x.price).ToList();

                //    return q;
                //}
            }
        }

        public List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> QuotesSell
        {
            get
            {
                List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> quotes1;

                lock (_inputStreamHandler.Quotes)
                {
                    quotes1 = new List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote>(_inputStreamHandler.Quotes);
                }

                var q = quotes1.Where(x => x.sell > 0 && x.buy == 0).OrderBy(x => x.price).ToList();

                return q;

                ////var q = _quotes.Where(x => x.buy > 0 && x.sell == 0).OrderBy(x => x.price).ToList();
                //lock (_quotes)
                //{
                //    //var q = (from quotes in _quotes
                //    //         where quotes.sell > 0
                //    //         orderby quotes.price
                //    //         select quotes).ToList();
                //    var q = _quotes.Where(x=> x.sell > 0 && x.buy == 0).OrderBy(x => x.price).ToList();

                //    return q;
                //}
            }
        }

        public double Bid
        {
            get
            {
                double res = 0;

                lock (_inputStreamHandler.Quotes)
                {
                    if (_inputStreamHandler.Quotes.Count > 0)
                        res = _inputStreamHandler.Quotes.Where(x => x.buy > 0 && x.sell == 0).Max(x => x.price);
                }

                return res;
            }
        }

        public double Ask
        {
            get
            {
                double res = 0;

                lock (_inputStreamHandler.Quotes)
                {
                    if (_inputStreamHandler.Quotes.Count > 0)
                        res = _inputStreamHandler.Quotes.Where(x => x.sell > 0 && x.buy == 0).Min(x => x.price);
                }

                return res;
            }
        }

        public positions Positions { get { return _inputStreamHandler.Positions; } }

        public mc_portfolio MCPortfolio { get { return _inputStreamHandler.mc_portfolio; } }

        public List<Application.Models.TXMLConnector.Ingoing.orders_ns.order> OpenOrders
        {
            get
            {
                List<Application.Models.TXMLConnector.Ingoing.orders_ns.order> res = null;

                lock (_inputStreamHandler.Orders)
                {
                    res = _inputStreamHandler.Orders.Where(x => x.status == status.active || x.status == status.watching).ToList();
                }

                return res;
            }
        }

        public StockClient(string tconFile)
        {
            _inputStreamHandler = new TXMLConnectorInputStreamHandler();
            _requestHandler = new TXMLConnectorRequestHandler(tconFile, _inputStreamHandler.HandleData);

            _inputStreamHandler.OnMCPositionsUpdated += OnMCPositionsUpdatedAction;
        }

        private void OnMCPositionsUpdatedAction(object? sender, OnMCPositionsUpdatedEventArgs e)
        {
            OnMCPositionsUpdated?.Invoke(sender, e);
        }

        public async Task Login(string username, string password, ConnectionType connectionType)
        {
            if (Connected)
                throw new Exception("Клиент уже авторизовался");
            
            string server;
            int port;
            if (connectionType == ConnectionType.Prod)
            {
                server = "tr1.finam.ru";
                port = 3900;
            }
            else
            {
                server = "tr1-demo5.finam.ru";
                port = 3939;
            }

            var com = command.CreateConnectionCommand(username, password, server, port);

            // отправляем команду подключения
            var sendCommandResult = _requestHandler.ConnectorSendCommand(com, com.GetType());

            
            if (sendCommandResult.success == false)
            {
                throw new Exception(sendCommandResult.message);
            }
            else
            {
                // ждём ассинхронный ответ о результате подключения
                await _inputStreamHandler.ServerStatusUpdated.WaitOne();

                if (_inputStreamHandler.ServerStatus.connected == "error")
                {
                    throw new Exception(_inputStreamHandler.ServerStatus.InnerText);
                }

                await _inputStreamHandler.PositionsLoaded.WaitOne();

                _getMCPortfolioPositions();
                await _inputStreamHandler.MC_portfolioLoaded.WaitOne();
            }
        }

        public void Logout()
        {
            var com = command.CreateDisconnectCommand();

            _inputStreamHandler.ServerStatusUpdated.Reset();
            var res = _requestHandler.ConnectorSendCommand(com, com.GetType());
        }

        public void ChangePassword(string oldpass, string newpass)
        {
            var com = command.CreateChangePasswordCommand(oldpass, newpass);

            var res = _requestHandler.ConnectorSendCommand(com, com.GetType());

            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        protected void _getMCPortfolioPositions()
        {
            var com = command.CreateGetMCPortfolioCommand(_inputStreamHandler.Positions.money_position.client);

            var res = _requestHandler.ConnectorSendCommand(com, com.GetType());

            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }


        bool _disposed = false;

        public async ValueTask DisposeAsync()
        {
            DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (Connected)
                {
                    Logout();
                    await _inputStreamHandler.ServerStatusUpdated.WaitOne(25 * 1000);
                }

                _requestHandler.Dispose();
                _inputStreamHandler.ServerStatusUpdated.Dispose();

                _disposed = true;
            }
        }

        public void SubscribeQuotations(TradingMode tradingMode, string seccode)
        {
            if (string.IsNullOrEmpty(seccode))
            {
                throw new Exception("seccode is not identified");
            }

            var com = command.CreateSubscribeQuotationsCommand(_tradingModeMapping[tradingMode], seccode);

            var res = _requestHandler.ConnectorSendCommand(com, com.GetType());
        }
        public void SubscribeQuotes(TradingMode tradingMode, string seccode)
        {
            var com = command.CreateSubscribeQuotesCommand(_tradingModeMapping[tradingMode], seccode);

            var res = _requestHandler.ConnectorSendCommand(com, com.GetType());
        }


        public async Task<List<candle>> GetHistoryData(string seccode, 
                                                        TradingMode tradingMode = TradingMode.Futures, 
                                                        SecurityPeriods periodId =  SecurityPeriods.M1, 
                                                        int candlesCount = 1)
        {
            candle res = null;

            command com = command.CreateGetHistoryDataCommand(_tradingModeMapping[tradingMode], seccode, periodId, candlesCount);

            // initialization must be before call of txml connector because response can be come fast
            if (!_inputStreamHandler.CandlesLoaded.ContainsKey(seccode))
                _inputStreamHandler.CandlesLoaded[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _inputStreamHandler.CandlesLoaded[seccode].WaitOne();
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return _inputStreamHandler.Candles[seccode].candlesValue;
        }

        public async Task<candle> GetCurrentCandle(string seccode, TradingMode tradingMode = TradingMode.Futures)
        {
            candle res = null;

            command com = new command();
            com.id = command_id.gethistorydata;
            com.security.board = _tradingModeMapping[tradingMode].ToString();
            com.security.seccode = seccode;
            com.periodValue = 1;
            com.countValue = 1;
            com.resetValue = true;

            _inputStreamHandler.CurrentCandle = null;
            if (!_inputStreamHandler.WaitForCurrentCandle.ContainsKey(seccode))
                _inputStreamHandler.WaitForCurrentCandle[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _inputStreamHandler.WaitForCurrentCandle[seccode].WaitOne();

                res = _inputStreamHandler.CurrentCandle[seccode];
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }
        
        public void CloseLimitOrder(int transactionid)
        {
            int res = 0;

            command com = command.CreateMoveOrderCommand(transactionid, 0, 2, 0);


            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }
        }

        public void CloseOrder(int transactionid)
        {
            int res = 0;

            command com = command.CreateCancelOrderCommand(transactionid);

            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }
        }

        public int CreateNewOrder(TradingMode tradingMode, string seccode, OrderDirection orderDirection, bool bymarket, double price, int volume)
        {
            if (!Connected) throw new Exception("Соединение не установлено. Операция не может быть выполнена");

            int res = 0;

            boardsCode board;
            if (tradingMode == TradingMode.Futures)
                board = boardsCode.FUT;
            else
                throw new Exception($"Tradning mode {tradingMode} is not supported");

            command com = command.CreateNewOrder(this.FortsClientId, 
                                                        board, 
                                                        seccode,
                                                        _orderDirectionMapping[orderDirection],
                                                        bymarket? AutoTrader.Application.Models.TXMLConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TXMLConnector.Outgoing.bymarket.no, 
                                                        price, 
                                                        volume);


            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        public int CreateNewStopOrderWithDistance(TradingMode tradingMode, 
                                                    string seccode, 
                                                    OrderDirection orderDirection, 
                                                    double price, 
                                                    double SLDistance, 
                                                    double TPDistance, 
                                                    int volume, 
                                                    Int64 orderno = 0)
        {
            buysell buysell = _orderDirectionMapping[orderDirection];

            return CreateNewStopOrder(tradingMode,
                                seccode,
                                orderDirection,
                                (buysell == buysell.B) ? (price + SLDistance) : (price - SLDistance),
                                (buysell == buysell.B) ? (price - TPDistance) : (price + TPDistance),
                                volume,
                                orderno);
        }

        public int CreateNewStopOrder(TradingMode tradingMode, 
                                    string seccode, 
                                    OrderDirection orderDirection, 
                                    double SLPrice, 
                                    double TPPrice, 
                                    int volume, 
                                    Int64 orderno = 0, 
                                    double correction = 0)
        {
            int res = 0;

            command com = new command();
            com.id = command_id.newstoporder;
            com.security = new Application.Models.TXMLConnector.Outgoing.command_ns.security();
            com.security.board = _tradingModeMapping[tradingMode].ToString();
            com.security.seccode = seccode;

            com.buysellValue = _orderDirectionMapping[orderDirection];
            com.client = FortsClientId;
            if (orderno != 0) com.linkedordernoValue = orderno;

            com.validforValue = validbefore.till_canceled;

            //для стоп заявок все должно быть наоборот
            com.stoploss = new stoploss();
            com.stoploss.activationprice = SLPrice;
            com.stoploss.bymarket = bymarket.yes;
            com.stoploss.quantity = volume;

            com.takeprofit = new takeprofit();
            com.takeprofit.activationprice = TPPrice;
            if (correction != 0) com.takeprofit.correction = correction;
            com.takeprofit.bymarket = bymarket.yes;
            //com.takeprofit.orderprice = price - profitlimit;
            com.takeprofit.quantity = volume;

            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        public int CreateNewConditionOrder(TradingMode tradingMode, 
                                    string seccode,
                                    OrderDirection orderDirection, 
                                    bool bymarket, 
                                    cond_type condtype, 
                                    double condvalue, 
                                    int volume)
        {
            int res = 0;

            command com = command.CreateNewCondOrderCommand(); ;
            com.id = command_id.newcondorder;
            com.security.board = _tradingModeMapping[tradingMode].ToString();
            com.security.seccode = seccode;
            com.buysellValue = _orderDirectionMapping[orderDirection];
            com.client = FortsClientId;
            // параметры, которые говорят переносить заявку на следующий день
            com.validafter = "0";
            com.validbefore = "till_canceled";
            /////////////////////////////////
            com.cond_typeValue = condtype;
            com.cond_valueValue = condvalue;
            com.quantityValue = volume;
            com.bymarketValue = bymarket ? AutoTrader.Application.Models.TXMLConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TXMLConnector.Outgoing.bymarket.no;

            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        public async Task CreateNewComboOrder(ComboOrder co)
        {
            if (co.OrderDirection == null) throw new Exception("Не установлен параметр BuySell");

            await CreateNewComboOrder(co.TradingMode, co.Seccode, co.OrderDirection, co.ByMarket, co.Price, co.Vol, co.SL, co.TP);
        }

        /// <summary>
        /// Выставляет открывающую заявку и при наличии условий по sl, tp закрывающие заявки
        /// </summary>
        /// <param name="board"></param>
        /// <param name="seccode"></param>
        /// <param name="buysell"></param>
        /// <param name="bymarket"></param>
        /// <param name="price"></param>
        /// <param name="volume"></param>
        /// <param name="slDistance"></param>
        /// <param name="tpDistance"></param>
        public async Task CreateNewComboOrder(TradingMode tradingMode, 
                                        string seccode, 
                                        OrderDirection orderDirection,
                                        bool bymarket,
                                        int price,
                                        int volume, 
                                        int slDistance, 
                                        int tpDistance,
                                        int comboType = 1)
        {
            
            int tid = CreateNewOrder(tradingMode, 
                                seccode,
                                orderDirection, 
                                bymarket, 
                                price, 
                                volume);

            Application.Models.TXMLConnector.Ingoing.orders_ns.order ord = null;

            await Task.Run(async () =>
            {
                while (ord == null) { 
                    await Task.Delay(10);
                    ord = GetOrderByTransactionId(tid); 
                }
            });

            // получение номера выполненного поручения
            Application.Models.TXMLConnector.Ingoing.trades_ns.trade tr = null;

            await Task.Run(async () =>
            {
                while (ord == null)
                {
                    await Task.Delay(10);
                    tr = GetTradeByOrderNo(ord.orderno);
                }
            });

            var closeOrderDirection = (orderDirection == OrderDirection.Buy) ? OrderDirection.Sell : OrderDirection.Buy;

            if (comboType == 1)
            {
                // открытие sl заявки
                if (slDistance > 0)
                {
                    double slPrice = (orderDirection == OrderDirection.Buy) ? tr.price - slDistance : tr.price + slDistance;
                    var closeCondition = (orderDirection == OrderDirection.Buy) ? cond_type.LastDown : cond_type.LastUp;

                    CreateNewConditionOrder(tradingMode,
                                    seccode,
                                    closeOrderDirection,
                                    bymarket,
                                    closeCondition,
                                    slPrice,
                                    volume);
                }

                // открытие tp заявки
                if (tpDistance > 0)
                {
                    double tpPrice = (orderDirection == OrderDirection.Buy) ? tr.price + tpDistance : tr.price - tpDistance;
                    CreateNewOrder(tradingMode, seccode, closeOrderDirection, bymarket, tpPrice, volume);
                }
            }
            else
            {
                // todo дописать
                //NewStopOrder(board, seccode, buysell, )
            }
        }

        private Application.Models.TXMLConnector.Ingoing.orders_ns.order GetOrderByTransactionId(int transactionid)
        {
            Application.Models.TXMLConnector.Ingoing.orders_ns.order res = null;

            lock (_inputStreamHandler.Orders)
            {
                res = _inputStreamHandler.Orders.FirstOrDefault(x => x.transactionid == transactionid);
            }

            return res;
        }

        private Application.Models.TXMLConnector.Ingoing.trades_ns.trade GetTradeByOrderNo(Int64 orderno)
        {
            Application.Models.TXMLConnector.Ingoing.trades_ns.trade res = null;

            lock (_inputStreamHandler.Trades)
            {
                res = _inputStreamHandler.Trades.FirstOrDefault(x => x.orderno == orderno);
            }

            return res;
        }

        public async Task<List<Application.Models.TXMLConnector.Ingoing.securities_ns.security>> GetSecurities()
        {
            List<Application.Models.TXMLConnector.Ingoing.securities_ns.security> res = new List<Application.Models.TXMLConnector.Ingoing.securities_ns.security>();
            command com = command.CreateGetSecurities();

            _inputStreamHandler.SecuritiesLoaded.Reset();
            result sendResult = _requestHandler.ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _inputStreamHandler.SecuritiesLoaded.WaitOne();

                res = _inputStreamHandler.Securities.ToList();
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        public int GetOpenPositions(string seccode)
        {
            int res = 0;

            try
            {
                var positions = _inputStreamHandler.Positions.forts_position.FirstOrDefault(x => x.seccode == seccode);

                res = positions.totalnet;
            }
            catch
            {
            }

            return res;
        }
    }
}
