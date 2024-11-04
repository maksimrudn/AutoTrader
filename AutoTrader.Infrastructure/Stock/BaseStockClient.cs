﻿using AutoTrader.Application.Common;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Exceptions;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Domain.Models;
using AutoTrader.Domain.Models.Types;
using AutoTrader.Infrastructure.Contracts.Transaq;
using AutoTrader.Infrastructure.Stock.TransaqConnector;

namespace AutoTrader.Infrastructure.Stock
{
    public abstract class BaseStockClient: IStockClient
    {
        protected readonly ITransaqConnectorFactory TransaqConnectorFactory;
        protected ITransaqConnectorRequestHandler? RequestHandler;

        private readonly Dictionary<TradingMode, boardsCode> _tradingModeMapping = new()
        {
            { TradingMode.Futures, boardsCode.FUT }
        };

        private readonly Dictionary<OrderDirection, buysell> _orderDirectionMapping = new()
        {
            { OrderDirection.Buy, buysell.B},
            { OrderDirection.Sell, buysell.S}
        };

        protected BaseStockClient(ITransaqConnectorFactory transaqConnectorFactory)
        {
            TransaqConnectorFactory = transaqConnectorFactory;
        }

        protected abstract ITransaqConnectorRequestHandler CreateRequestHandler();

        public bool Connected => RequestHandler?.InputStreamHandler.ServerStatus?.connected == "true";
       
        public List<Application.Models.TransaqConnector.Ingoing.securities_ns.security> Securities => RequestHandler?.InputStreamHandler.Securities ?? new ();

        [Obsolete]
        // todo: update this field inside StockClient on making order (in NewOrder method)
        /// <summary>
        /// Field is used in order to understand when positions are actual
        /// For example when we just have made order, positions are not actual and we know about it before. It become actual after updation of info from tconnector        /// 
        /// </summary>
        public bool? PositionsAreActual
        {
            get => RequestHandler?.InputStreamHandler?.PositionsAreActual;
            set
            {
                if (RequestHandler?.InputStreamHandler != null && value != null)
                {
                    RequestHandler.InputStreamHandler.PositionsAreActual = value.Value;
                }
            }
        }
        
        public string? FortsClientId => RequestHandler?.InputStreamHandler.Forts_client?.forts_acc;

        public double? Money => RequestHandler?.InputStreamHandler.mc_portfolio?.moneys.First(x => x.currency == "RUB").balance;        

        public string? Union=> RequestHandler?.InputStreamHandler.Forts_client?.union;

        public candlekinds? Candlekinds => RequestHandler?.InputStreamHandler.Candlekinds;

        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesBuy
        {
            get
            {
                if (RequestHandler == null) return null;

                List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> quotes1;
                quotes1 = new List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>(RequestHandler.InputStreamHandler.Quotes);
                var q = quotes1.Where(x => x.buy > 0 && x.sell == 0).OrderByDescending(x => x.price).ToList();

                return q;

                // todo: old implementation, remove after tests
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

        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> QuotesSell
        {
            get
            {
                if (RequestHandler == null) return null;

                List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> quotes1;
                quotes1 = new List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>(RequestHandler.InputStreamHandler.Quotes);
                var q = quotes1.Where(x => x.sell > 0 && x.buy == 0).OrderBy(x => x.price).ToList();

                return q;

                // todo: old implementation, remove after tests
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

        /// <summary>
        /// Related to only that security which had been subscribed to beforehand
        /// </summary>
        object _bidLock = new object();
        public double Bid
        {
            get
            {
                double res = 0;

                lock (_bidLock)
                {
                    if (RequestHandler.InputStreamHandler.Quotes.Count > 0)
                        res = RequestHandler.InputStreamHandler.Quotes.Where(x => x.buy > 0 && x.sell == 0).Max(x => x.price);
                }

                return res;
            }
        }

        object _askObject = new object();
        public double Ask
        {
            get
            {
                double res = 0;

                lock (_askObject)
                {
                    if (RequestHandler.InputStreamHandler.Quotes.Count > 0)
                        res = RequestHandler.InputStreamHandler.Quotes.Where(x => x.sell > 0 && x.buy == 0).Min(x => x.price);
                }

                return res;
            }
        }

        public positions Positions => RequestHandler?.InputStreamHandler.Positions;

        public List<forts_position> FortsPositions => RequestHandler?.InputStreamHandler.FortsPositions ?? new List<forts_position>();

        public List<Application.Models.TransaqConnector.Ingoing.trades_ns.trade>? Trades => RequestHandler?.InputStreamHandler.Trades;

        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? Orders => RequestHandler?.InputStreamHandler.Orders;

        public mc_portfolio? MCPortfolio => RequestHandler?.InputStreamHandler.mc_portfolio;

        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? OpenOrders => RequestHandler?.InputStreamHandler
                                                                                                                .Orders
                                                                                                                .Where(x => x.status == status.active || x.status == status.watching)
                                                                                                                .ToList();

        public async Task Login(string username, string password, ConnectionType connectionType = ConnectionType.Prod)
        {
            if (Connected)
                throw new StockClientException(CommonErrors.AlreadyLoggedIn);

            RequestHandler = CreateRequestHandler();

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
            var sendCommandResult = RequestHandler.ConnectorSendCommand(com);

            
            if (sendCommandResult.success == false)
            {
                throw new Exception(sendCommandResult.message);
            }
            else
            {
                // Waiting for server_status information
                // Or for receiving of clients (that is inderect means connection is successfull)
                var waitServerStatus = RequestHandler.InputStreamHandler.ServerStatusUpdated.WaitOne(2*60*1000);
                var waitPositions = RequestHandler.InputStreamHandler.PositionsLoaded.WaitOne(2*60*1000);
                try
                {
                    await waitServerStatus.ConfigureAwait(false);

                    if (!Connected)
                    {
                        throw new StockClientException(CommonErrors.ServerConnectionError, RequestHandler.InputStreamHandler.ServerStatus?.InnerText);
                    }
                }
                catch (TimeoutException ex)
                {
                    throw new Exception("Waiting of server_status exception");
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    // todo: solve problem on exception while authorization. Library can't be disposed on bad authorization at this point and suspends closing of application
                    //_requestHandler.Dispose();
                    //_requestHandler = null;
                }

                try
                {
                    await waitPositions.ConfigureAwait(false);
                }
                catch (TimeoutException ex)
                {
                    throw new StockClientException(CommonErrors.PositionsWaitingTimeout);
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (RequestHandler.InputStreamHandler.ServerStatus?.connected == "error")
                {
                    throw new StockClientException(CommonErrors.ServerConnectionError,
                                                    RequestHandler.InputStreamHandler.ServerStatus.InnerText);
                }

                GetMCPortfolioPositions();
                await RequestHandler.InputStreamHandler.MC_portfolioLoaded.WaitOne().ConfigureAwait(false);
            }
        }

        public async Task Logout()
        {
            if (!Connected) return;

            var com = command.CreateDisconnectCommand();
            var res = RequestHandler.ConnectorSendCommand(com);
            if (!res.success)
            {
                throw new StockClientException(res.message);
            }
            await RequestHandler.InputStreamHandler.ServerStatusUpdated.WaitOne().ConfigureAwait(false);
            // cleaning up of resources
            RequestHandler.Dispose();
            RequestHandler = null;
        }

        public void ChangePassword(string oldpass, string newpass)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            var com = command.CreateChangePasswordCommand(oldpass, newpass);
            var res = RequestHandler.ConnectorSendCommand(com);
            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        protected void GetMCPortfolioPositions()
        {
            var com = command.CreateGetMCPortfolioCommand(RequestHandler.InputStreamHandler.Positions.money_position.client);
            var res = RequestHandler.ConnectorSendCommand(com);
            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        volatile bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                RequestHandler?.Dispose();
                _disposed = true;
            }
        }

        ~BaseStockClient()
        {
            Dispose(false);
        }

        public void SubscribeQuotations(TradingMode tradingMode, string seccode)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            if (string.IsNullOrEmpty(seccode))
            {
                throw new Exception("seccode is not identified");
            }

            var com = command.CreateSubscribeQuotationsCommand(_tradingModeMapping[tradingMode], seccode);
            var res = RequestHandler.ConnectorSendCommand(com);
            // todo: check result of res
        }
        public void SubscribeQuotes(TradingMode tradingMode, string seccode)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            var com = command.CreateSubscribeQuotesCommand(_tradingModeMapping[tradingMode], seccode);
            var res = RequestHandler.ConnectorSendCommand(com);
        }


        public async Task<List<candle>> GetHistoryData(string seccode, 
                                                        TradingMode tradingMode = TradingMode.Futures, 
                                                        SecurityPeriods periodId =  SecurityPeriods.M1, 
                                                        int candlesCount = 1)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            candle res = null;

            command com = command.CreateGetHistoryDataCommand(_tradingModeMapping[tradingMode], seccode, periodId, candlesCount);
            // initialization must be before call of txml connector because response can be come fast
            if (!RequestHandler.InputStreamHandler.CandlesLoaded.ContainsKey(seccode))
                RequestHandler.InputStreamHandler.CandlesLoaded[seccode] = new AsyncAutoResetEvent(false);
            result sendResult = RequestHandler.ConnectorSendCommand(com);
            if (sendResult.success)
            {
                await RequestHandler.InputStreamHandler.CandlesLoaded[seccode].WaitOne().ConfigureAwait(false);
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return RequestHandler.InputStreamHandler.Candles[seccode].candlesValue;
        }

        public async Task<candle> GetCurrentCandle(string seccode, TradingMode tradingMode = TradingMode.Futures)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            candle res = null;

            command com = new command();
            com.id = command_id.gethistorydata;
            com.security.board = _tradingModeMapping[tradingMode].ToString();
            com.security.seccode = seccode;
            com.periodValue = 1;
            com.countValue = 1;
            com.resetValue = true;

            RequestHandler.InputStreamHandler.CurrentCandle = null;
            if (!RequestHandler.InputStreamHandler.WaitForCurrentCandle.ContainsKey(seccode))
                RequestHandler.InputStreamHandler.WaitForCurrentCandle[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = RequestHandler.ConnectorSendCommand(com);

            if (sendResult.success)
            {
                await RequestHandler.InputStreamHandler.WaitForCurrentCandle[seccode].WaitOne().ConfigureAwait(false);

                res = RequestHandler.InputStreamHandler.CurrentCandle[seccode];
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }
        
        public void CloseLimitOrder(int transactionid)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            int res = 0;
            command com = command.CreateMoveOrderCommand(transactionid, 0, 2, 0);
            result sendResult = RequestHandler.ConnectorSendCommand(com);
            if (sendResult.success)
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
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            int res = 0;
            command com = command.CreateCancelOrderCommand(transactionid);
            result sendResult = RequestHandler.ConnectorSendCommand(com);
            if (sendResult.success)
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
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            int res = 0;

            boardsCode board;
            if (tradingMode == TradingMode.Futures)
                board = boardsCode.FUT;
            else
                throw new Exception($"Tradning mode {tradingMode} is not supported");

            command com = command.CreateNewOrder(FortsClientId, 
                                                        board, 
                                                        seccode,
                                                        _orderDirectionMapping[orderDirection],
                                                        bymarket? AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.no, 
                                                        price, 
                                                        volume);
            result sendResult = RequestHandler.ConnectorSendCommand(com);
            if (sendResult.success)
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
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

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
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

            int res = 0;

            command com = new command();
            com.id = command_id.newstoporder;
            com.security = new Application.Models.TransaqConnector.Outgoing.command_ns.security();
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

            result sendResult = RequestHandler.ConnectorSendCommand(com);

            if (sendResult.success)
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
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);

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
            com.bymarketValue = bymarket ? AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.no;

            result sendResult = RequestHandler.ConnectorSendCommand(com);

            if (sendResult.success)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        /// <summary>
        /// Выставляет открывающую заявку и при наличии условий по sl, tp закрывающие заявки
        /// </summary>
        public async Task CreateNewComboOrder(ComboOrder co)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.UnAuthorized);
            if (co.OrderDirection == null) throw new Exception("Не установлен параметр BuySell");
            
            int tid = CreateNewOrder(co.TradingMode,
                                co.Seccode,
                                co.OrderDirection,
                                co.ByMarket,
                                co.Price,
                                co.Vol);

            Application.Models.TransaqConnector.Ingoing.orders_ns.order? ord = null;
            await Task.Run(async () =>
            {
                while (ord == null) { 
                    await Task.Delay(10).ConfigureAwait(false);
                    ord = GetOrderByTransactionId(tid); 
                }

                if (ord.status != status.matched)
                {
                    throw new Exception($"Order {ord.orderno} is not matched. Further processing is not possible");
                }
            }).ConfigureAwait(false);

            // получение номера выполненного поручения
            Application.Models.TransaqConnector.Ingoing.trades_ns.trade? tr = null;

            await Task.Run(async () =>
            {
                while (tr == null)
                {
                    await Task.Delay(10).ConfigureAwait(false);
                    tr = GetTradeByOrderNo(ord.orderno);
                }
            }).ConfigureAwait(false);
            var closeOrderDirection = (co.OrderDirection == OrderDirection.Buy) 
                                                    ? OrderDirection.Sell : 
                                                    OrderDirection.Buy;
            if (co.StopLoseOrderType == StopLoseOrderType.ConditionalOrder)
            {
                // открытие sl заявки
                if (co.SL > 0)
                {
                    double slPrice = (co.OrderDirection == OrderDirection.Buy) ? tr.price - co.SL : tr.price + co.SL;
                    var closeCondition = (co.OrderDirection == OrderDirection.Buy) ? cond_type.LastDown : cond_type.LastUp;

                    CreateNewConditionOrder(co.TradingMode,
                                    co.Seccode,
                                    closeOrderDirection,
                                    co.ByMarket,
                                    closeCondition,
                                    slPrice,
                                    co.Vol);
                }

                // открытие tp заявки
                if (co.TP > 0)
                {
                    double tpPrice = (co.OrderDirection == OrderDirection.Buy) ? tr.price + co.TP : tr.price - co.TP;
                    CreateNewOrder(co.TradingMode, co.Seccode, closeOrderDirection, co.ByMarket, tpPrice, co.Vol);
                }
            }
            else
            {
                // todo дописать
                //NewStopOrder(board, seccode, buysell, )
            }
        }

        private Application.Models.TransaqConnector.Ingoing.orders_ns.order? GetOrderByTransactionId(int transactionid) => RequestHandler?.InputStreamHandler
                                                                                                                                            .Orders
                                                                                                                                            .FirstOrDefault(x => x.transactionid == transactionid);

        private Application.Models.TransaqConnector.Ingoing.trades_ns.trade? GetTradeByOrderNo(Int64 orderno) => RequestHandler?.InputStreamHandler
                                                                                                                                .Trades
                                                                                                                                .FirstOrDefault(x => x.orderno == orderno);
        
        public async Task<List<Application.Models.TransaqConnector.Ingoing.securities_ns.security>> GetSecurities()
        {
            if (!Connected) throw new StockClientException(CommonErrors.UnAuthorized);

            List<Application.Models.TransaqConnector.Ingoing.securities_ns.security> res = new ();
            command com = command.CreateGetSecurities();

            RequestHandler.InputStreamHandler.SecuritiesLoaded.Reset();
            result sendResult = RequestHandler.ConnectorSendCommand(com);

            if (sendResult.success)
            {
                await RequestHandler.InputStreamHandler.SecuritiesLoaded.WaitOne().ConfigureAwait(false);

                res = RequestHandler.InputStreamHandler.Securities.ToList();
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }

        // todo: finish implementation  of this method
        public int GetOpenPositions(string seccode)
        {
            if (!Connected) throw new StockClientException(CommonErrors.UnAuthorized);

            int res = 0;

            try
            {
                var positions = RequestHandler.InputStreamHandler.Positions.forts_position.FirstOrDefault(x => x.seccode == seccode);

                res = positions.totalnet;
            }
            catch
            {
            }

            return res;
        }
    }
}
