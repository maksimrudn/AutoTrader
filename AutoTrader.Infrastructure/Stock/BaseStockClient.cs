using AutoTrader.Application.Common;
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

        protected readonly ITransaqConnectorFactory _transaqConnectorFactory;
        protected ITransaqConnectorRequestHandler? _requestHandler;

        Dictionary<TradingMode, boardsCode> _tradingModeMapping = new Dictionary<TradingMode, boardsCode>()
        {
            { TradingMode.Futures, boardsCode.FUT }
        };

        Dictionary<OrderDirection, buysell> _orderDirectionMapping = new Dictionary<OrderDirection, buysell>()
        {
            { OrderDirection.Buy, buysell.B},
            { OrderDirection.Sell, buysell.S}
        };

        public BaseStockClient(ITransaqConnectorFactory transaqConnectorFactory)
        {
            _transaqConnectorFactory = transaqConnectorFactory;
        }

        protected abstract ITransaqConnectorRequestHandler CreateRequestHandler();

        public bool Connected => _requestHandler?.InputStreamHandler.ServerStatus?.connected == "true";
       
        public List<Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security> Securities => _requestHandler?.InputStreamHandler.Securities ?? new ();

        [Obsolete]
        // todo: update this field inside StockClient on making order (in NewOrder method)
        /// <summary>
        /// Field is used in order to understand when positions are actual
        /// For example when we just have made order, positions are not actual and we know about it before. It become actual after updation of info from tconnector        /// 
        /// </summary>
        public bool? PositionsAreActual
        {
            get => _requestHandler?.InputStreamHandler?.PositionsAreActual;
            set
            {
                if (_requestHandler?.InputStreamHandler != null && value != null)
                {
                    _requestHandler.InputStreamHandler.PositionsAreActual = value.Value;
                }
            }
        }
        
        public string? FortsClientId => _requestHandler?.InputStreamHandler.Forts_client?.forts_acc;

        public double? Money 
        {
            get
            {
                if (_requestHandler?.InputStreamHandler.mc_portfolio?.moneys.Count == 0)
                    throw new Exception("No information about money have been loaded");

                return _requestHandler?.InputStreamHandler.mc_portfolio?.moneys.First(x => x.currency == "RUB").balance;
            }
        }

        public string? Union=> _requestHandler?.InputStreamHandler.Forts_client?.union;

        public candlekinds? Candlekinds => _requestHandler?.InputStreamHandler.Candlekinds;


        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesBuy
        {
            get
            {
                if (_requestHandler == null) return null;

                List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> quotes1;

                quotes1 = new List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>(_requestHandler.InputStreamHandler.Quotes);

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

        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> QuotesSell
        {
            get
            {
                if (_requestHandler == null) return null;

                List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> quotes1;

                quotes1 = new List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote>(_requestHandler.InputStreamHandler.Quotes);

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
                    if (_requestHandler.InputStreamHandler.Quotes.Count > 0)
                        res = _requestHandler.InputStreamHandler.Quotes.Where(x => x.buy > 0 && x.sell == 0).Max(x => x.price);
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
                    if (_requestHandler.InputStreamHandler.Quotes.Count > 0)
                        res = _requestHandler.InputStreamHandler.Quotes.Where(x => x.sell > 0 && x.buy == 0).Min(x => x.price);
                }

                return res;
            }
        }

        public positions Positions => _requestHandler?.InputStreamHandler.Positions;

        public List<forts_position> FortsPositions => _requestHandler?.InputStreamHandler.FortsPositions ?? new List<forts_position>();

        public List<Application.Models.TransaqConnector.Ingoing.trades_ns.trade>? Trades => _requestHandler?.InputStreamHandler.Trades;

        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? Orders => _requestHandler?.InputStreamHandler.Orders;

        public mc_portfolio? MCPortfolio => _requestHandler?.InputStreamHandler.mc_portfolio;

        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? OpenOrders => _requestHandler?.InputStreamHandler
                                                                                                                .Orders
                                                                                                                .Where(x => x.status == status.active || x.status == status.watching)
                                                                                                                .ToList();

        public async Task LoginAsync(string username, string password, ConnectionType connectionType = ConnectionType.Prod)
        {
            if (Connected)
                throw new StockClientException(CommonErrors.AlreadyLoggedIn);

            _requestHandler = CreateRequestHandler();

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

            var cmd = command.CreateConnectionCommand(username, password, server, port);

            // отправляем команду подключения
            var sendCommandResult = _requestHandler.ConnectorSendCommand(cmd);

            
            if (sendCommandResult.success == false)
            {
                ResetRequestHandler();
                throw new Exception(sendCommandResult.message);
            }
            else
            {
                // Waiting for server_status information
                // Or for receiving of clients (that is inderect means connection is successfull)
                var waitServerStatus = _requestHandler.InputStreamHandler.ServerStatusUpdated.WaitOne(2*60*1000);
                var waitPositions = _requestHandler.InputStreamHandler.PositionsLoaded.WaitOne(2*60*1000);
                try
                {
                    await waitServerStatus.ConfigureAwait(false);

                    if (!Connected)
                    {
                        var ex = new StockClientException(CommonErrors.ServerConnectionError, _requestHandler.InputStreamHandler.ServerStatus?.InnerText);
                        ResetRequestHandler();
                        throw ex;
                    }
                }
                catch (TimeoutException ex)
                {
                    ResetRequestHandler();
                    throw new Exception("Waiting of server_status exception");
                }
                catch (Exception ex)
                {
                    ResetRequestHandler();
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
                    //await waitPositions.ConfigureAwait(false);
                }
                catch (TimeoutException ex)
                {
                    ResetRequestHandler();
                    throw new StockClientException(CommonErrors.PositionsWaitingTimeout);
                }
                catch (Exception ex)
                {
                    ResetRequestHandler();
                    throw;
                }

                if (_requestHandler.InputStreamHandler.ServerStatus?.connected == "error")
                {
                    ResetRequestHandler();
                    throw new StockClientException(CommonErrors.ServerConnectionError,
                                                    _requestHandler.InputStreamHandler.ServerStatus.InnerText);
                }

                try
                {
                    GetMCPortfolioPositions();
                    await _requestHandler.InputStreamHandler.MC_portfolioLoaded.WaitOne().ConfigureAwait(false);
                }
                catch
                {
                    ResetRequestHandler();
                    throw;
                }
            }
        }

        public async Task Logout()
        {
            if (!Connected) return;

            var cmd = command.CreateDisconnectCommand();
            var res = _requestHandler.ConnectorSendCommand(cmd);

            if (!res.success)
            {
                throw new StockClientException(res.message);
            }

            await _requestHandler.InputStreamHandler.ServerStatusUpdated.WaitOne().ConfigureAwait(false);
            
            ResetRequestHandler();
        }

        private void ResetRequestHandler()
        {
            _requestHandler?.Dispose();
            _requestHandler = null;
        }

        public void ChangePassword(string oldpass, string newpass)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.Unauthorized);

            var cmd = command.CreateChangePasswordCommand(oldpass, newpass);

            var res = _requestHandler.ConnectorSendCommand(cmd);

            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        protected void GetMCPortfolioPositions()
        {
            var cmd = command.CreateGetMCPortfolioCommand(_requestHandler.InputStreamHandler.Forts_client.id);

            var res = _requestHandler.ConnectorSendCommand(cmd);

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
                _requestHandler?.Dispose();

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
                throw new StockClientException(CommonErrors.Unauthorized);

            if (string.IsNullOrEmpty(seccode))
            {
                throw new Exception("seccode is not identified");
            }

            var cmd = command.CreateSubscribeQuotationsCommand(_tradingModeMapping[tradingMode], seccode);

            var res = _requestHandler.ConnectorSendCommand(cmd);
        }
        public void SubscribeQuotes(TradingMode tradingMode, string seccode)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.Unauthorized);

            var cmd = command.CreateSubscribeQuotesCommand(_tradingModeMapping[tradingMode], seccode);

            var res = _requestHandler.ConnectorSendCommand(cmd);
        }


        public async Task<List<candle>> GetHistoryData(string seccode, 
                                                        TradingMode tradingMode = TradingMode.Futures, 
                                                        SecurityPeriods periodId =  SecurityPeriods.M1, 
                                                        int candlesCount = 1)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.Unauthorized);

            candle res = null;

            command cmd = command.CreateGetHistoryDataCommand(_tradingModeMapping[tradingMode], seccode, periodId, candlesCount);

            // initialization must be before call of txml connector because response can be come fast
            if (!_requestHandler.InputStreamHandler.CandlesLoaded.ContainsKey(seccode))
                _requestHandler.InputStreamHandler.CandlesLoaded[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

            if (sendResult.success)
            {
                await _requestHandler.InputStreamHandler.CandlesLoaded[seccode].WaitOne().ConfigureAwait(false);
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return _requestHandler.InputStreamHandler.Candles[seccode].candlesValue;
        }

        public async Task<candle> GetCurrentCandle(string seccode, TradingMode tradingMode = TradingMode.Futures)
        {
            if (!Connected)
                throw new StockClientException(CommonErrors.Unauthorized);

            candle res = null;

            command cmd = new command();
            cmd.id = command_id.gethistorydata;
            cmd.security.board = _tradingModeMapping[tradingMode].ToString();
            cmd.security.seccode = seccode;
            cmd.periodValue = 1;
            cmd.countValue = 1;
            cmd.resetValue = true;

            _requestHandler.InputStreamHandler.CurrentCandle = null;
            if (!_requestHandler.InputStreamHandler.WaitForCurrentCandle.ContainsKey(seccode))
                _requestHandler.InputStreamHandler.WaitForCurrentCandle[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

            if (sendResult.success)
            {
                await _requestHandler.InputStreamHandler.WaitForCurrentCandle[seccode].WaitOne().ConfigureAwait(false);

                res = _requestHandler.InputStreamHandler.CurrentCandle[seccode];
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
                throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;
            command cmd = command.CreateMoveOrderCommand(transactionid, 0, 2, 0);
            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

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
                throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;

            command cmd = command.CreateCancelOrderCommand(transactionid);

            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

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
                throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;

            boardsCode board;
            if (tradingMode == TradingMode.Futures)
                board = boardsCode.FUT;
            else
                throw new Exception($"Tradning mode {tradingMode} is not supported");

            command cmd = command.CreateNewOrder(this.FortsClientId, 
                                                        board, 
                                                        seccode,
                                                        _orderDirectionMapping[orderDirection],
                                                        bymarket? AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.no, 
                                                        price, 
                                                        volume);


            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

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
                throw new StockClientException(CommonErrors.Unauthorized);

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
                throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;

            command cmd = new command();
            cmd.id = command_id.newstoporder;
            cmd.security = new Application.Models.TransaqConnector.Outgoing.command_ns.security();
            cmd.security.board = _tradingModeMapping[tradingMode].ToString();
            cmd.security.seccode = seccode;

            cmd.buysellValue = _orderDirectionMapping[orderDirection];
            cmd.client = FortsClientId;
            if (orderno != 0) cmd.linkedordernoValue = orderno;

            cmd.validforValue = validbefore.till_canceled;

            //для стоп заявок все должно быть наоборот
            cmd.stoploss = new stoploss();
            cmd.stoploss.activationprice = SLPrice;
            cmd.stoploss.bymarket = bymarket.yes;
            cmd.stoploss.quantity = volume;

            cmd.takeprofit = new takeprofit();
            cmd.takeprofit.activationprice = TPPrice;
            if (correction != 0) cmd.takeprofit.correction = correction;
            cmd.takeprofit.bymarket = bymarket.yes;
            //com.takeprofit.orderprice = price - profitlimit;
            cmd.takeprofit.quantity = volume;

            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

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
                throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;

            command cmd = command.CreateNewCondOrderCommand(); ;
            cmd.id = command_id.newcondorder;
            cmd.security.board = _tradingModeMapping[tradingMode].ToString();
            cmd.security.seccode = seccode;
            cmd.buysellValue = _orderDirectionMapping[orderDirection];
            cmd.client = FortsClientId;
            // параметры, которые говорят переносить заявку на следующий день
            cmd.validafter = "0";
            cmd.validbefore = "till_canceled";
            /////////////////////////////////
            cmd.cond_typeValue = condtype;
            cmd.cond_valueValue = condvalue;
            cmd.quantityValue = volume;
            cmd.bymarketValue = bymarket ? AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.yes : AutoTrader.Application.Models.TransaqConnector.Outgoing.bymarket.no;

            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

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
                throw new StockClientException(CommonErrors.Unauthorized);

            if (co.OrderDirection == null) throw new Exception("Не установлен параметр BuySell");


            int tid = CreateNewOrder(co.TradingMode,
                                co.Seccode,
                                co.OrderDirection,
                                co.ByMarket,
                                co.Price,
                                co.Vol);

            Application.Models.TransaqConnector.Ingoing.orders_ns.order? order = null;

            await Task.Run(async () =>
            {
                while (order == null) { 
                    await Task.Delay(10).ConfigureAwait(false);
                    order = GetOrderByTransactionId(tid); 
                }

                if (order.status != status.matched)
                {
                    throw new Exception($"Order {order.orderno} is not matched. Further processing is not possible");
                }
            }).ConfigureAwait(false);

            // получение номера выполненного поручения
            Application.Models.TransaqConnector.Ingoing.trades_ns.trade? trade = null;

            await Task.Run(async () =>
            {
                while (trade == null)
                {
                    await Task.Delay(10).ConfigureAwait(false);
                    trade = GetTradeByOrderNo(order.orderno);
                }
            }).ConfigureAwait(false);

            var closingOrderDirection = (co.OrderDirection == OrderDirection.Buy) ? OrderDirection.Sell : OrderDirection.Buy;

            if (co.StopLoseOrderType == StopLoseOrderType.ConditionalOrder)
            {
                // открытие sl заявки
                if (co.SL > 0)
                {
                    double slPrice = (co.OrderDirection == OrderDirection.Buy) ? trade.price - co.SL : trade.price + co.SL;
                    var closeCondition = (co.OrderDirection == OrderDirection.Buy) ? cond_type.LastDown : cond_type.LastUp;

                    CreateNewConditionOrder(co.TradingMode,
                                    co.Seccode,
                                    closingOrderDirection,
                                    co.ByMarket,
                                    closeCondition,
                                    slPrice,
                                    co.Vol);
                }

                // открытие tp заявки
                if (co.TP > 0)
                {
                    double tpPrice = (co.OrderDirection == OrderDirection.Buy) ? trade.price + co.TP : trade.price - co.TP;
                    CreateNewOrder(co.TradingMode, co.Seccode, closingOrderDirection, co.ByMarket, tpPrice, co.Vol);
                }
            }
            else
            {
                // todo дописать
                //NewStopOrder(board, seccode, buysell, )
            }
        }

        private Application.Models.TransaqConnector.Ingoing.orders_ns.order? GetOrderByTransactionId(int transactionid) => _requestHandler?.InputStreamHandler
                                                                                                                                            .Orders
                                                                                                                                            .FirstOrDefault(x => x.transactionid == transactionid);

        private Application.Models.TransaqConnector.Ingoing.trades_ns.trade? GetTradeByOrderNo(Int64 orderno) => _requestHandler?.InputStreamHandler
                                                                                                                                .Trades
                                                                                                                                .FirstOrDefault(x => x.orderno == orderno);
        
        public async Task<List<Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security>> GetSecurities()
        {
            if (!Connected) throw new StockClientException(CommonErrors.Unauthorized);

            List<Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security> res = new ();
            command cmd = command.CreateGetSecurities();

            _requestHandler.InputStreamHandler.SecuritiesLoaded.Reset();
            result sendResult = _requestHandler.ConnectorSendCommand(cmd);

            if (sendResult.success)
            {
                await _requestHandler.InputStreamHandler.SecuritiesLoaded.WaitOne().ConfigureAwait(false);

                res = _requestHandler.InputStreamHandler.Securities.ToList();
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
            if (!Connected) throw new StockClientException(CommonErrors.Unauthorized);

            int res = 0;

            try
            {
                var positions = _requestHandler.InputStreamHandler.Positions.forts_position.FirstOrDefault(x => x.seccode == seccode);

                res = positions.totalnet;
            }
            catch
            {
            }

            return res;
        }
    }
}
