using AutoTrader.Application.Common;
using AutoTrader.Application.Extensions;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Xml;

namespace AutoTrader.Infrastructure.Stock.TransaqConnector
{
    public class TransaqConnectorInputStreamHandler
    {
        public TransaqConnectorInputStreamHandler()
        {
        }

        public AsyncAutoResetEvent ServerStatusUpdated = new(false);
        public AsyncAutoResetEvent PositionsLoaded = new(false);
        public AsyncAutoResetEvent SecuritiesLoaded = new(false);
        public AsyncAutoResetEvent TradesLoaded = new(false);
        public ConcurrentDictionary<string, AsyncAutoResetEvent> WaitForCurrentCandle = new();
        public AsyncAutoResetEvent MC_portfolioLoaded = new(false);
        public ConcurrentDictionary<string, AsyncAutoResetEvent> CandlesLoaded = new();

        /// <summary>
        /// Результат подключения к серверу
        /// Заполняется с помощью обработчика _handleData
        /// </summary>
        private readonly ReaderWriterLockSlim _serverStatusLock = new();
        private server_status? _serverStatus = null;
        public server_status? ServerStatus
        {
            get {
                try
                {
                    _serverStatusLock.EnterReadLock();
                    return _serverStatus;
                }
                finally
                {
                    _serverStatusLock.ExitReadLock();
                }
            }
        }

        private readonly ReaderWriterLockSlim _forts_clientLock = new();
        private client? _forts_client = null;
        public client? Forts_client
        {
            get
            {
                try
                {
                    _forts_clientLock.EnterReadLock();
                    return _forts_client;
                }
                finally
                {
                    _forts_clientLock.ExitReadLock();
                }
            }
        }

        private readonly ReaderWriterLockSlim _mc_portfolioLock = new();
        private mc_portfolio _mc_portfolio = new();
        public mc_portfolio mc_portfolio
        {
            get
            {
                try
                {
                    _mc_portfolioLock.EnterReadLock();
                    return _mc_portfolio;
                }
                finally
                {
                    _mc_portfolioLock.ExitReadLock();
                }
            }
        }


        public ConcurrentDictionary<string, candle> CurrentCandle = new();
        public bool PositionsAreActual = false;

        private readonly ReaderWriterLockSlim _positionsLock = new();
        private readonly positions _positions = new();
        public positions Positions {
            get
            {
                try
                {
                    _positionsLock.EnterReadLock();
                    return _positions.DeepClone();
                }
                finally
                {
                    _positionsLock.ExitReadLock();
                }
            }
        }

        private readonly ConcurrentDictionary<string, forts_position> _fortsPositions = new();
        public List<forts_position> FortsPositions => _fortsPositions.Values.ToList();


        private readonly ConcurrentDictionary<string, Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> _quotes = new();
        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> Quotes => _quotes.Values.ToList();


        private readonly ConcurrentDictionary<Int64, Application.Models.TransaqConnector.Ingoing.orders_ns.order> _orders = new();
        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order> Orders => _orders.Values.ToList();


        private readonly ConcurrentDictionary<long, Application.Models.TransaqConnector.Ingoing.trades_ns.trade> _trades = new();
        public List<Application.Models.TransaqConnector.Ingoing.trades_ns.trade> Trades => _trades.Values.ToList();

        private readonly ConcurrentDictionary<string, Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security> _securities = new();
        public List<Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security> Securities => _securities.Values.ToList();
        
        private readonly ConcurrentDictionary<string, sec_info_upd> _secInfoUpds = new();
        public List<sec_info_upd> SecInfoUpds => _secInfoUpds.Values.ToList();

        /// <summary>
        /// for history data
        /// </summary>
        public ConcurrentDictionary<string, candles> Candles { get; set; } = new ConcurrentDictionary<string, candles>();
        public candlekinds Candlekinds { get; set; }

        public void HandleData(string result)
        {
            string nodeName = _getNodeName(result);
            Debug.WriteLine(nodeName);
            //File.AppendAllText("stream.csv", result+"\n");
            switch (nodeName)
            {
                case "server_status":
                    _serverStatusLock.EnterWriteLock();
                    _serverStatus = (server_status)XMLHelper.Deserialize(result, typeof(server_status));
                    _serverStatusLock.ExitWriteLock();

                    ServerStatusUpdated.Set();

                    break;

                case "markets":
                    break;

                case "boards":
                    break;

                case "candlekinds":
                    Candlekinds = (candlekinds)XMLHelper.Deserialize(result, typeof(candlekinds));

                    break;

                case "securities":
                    var securities = (stock_securities)XMLHelper.Deserialize(result, typeof(stock_securities));
                    _securitiesHandle(securities.security);
                    SecuritiesLoaded.Set();
                    break;

                case "pits":
                    break;

                case "sec_info_upd":
                    var secInfoUpds = (sec_info_upd)XMLHelper.Deserialize(result, typeof(sec_info_upd));
                    _secInfoUpdsHandle(secInfoUpds);
                    break;

                case "client":
                    var clientInfo = (client)XMLHelper.Deserialize(result, typeof(client));

                    if (clientInfo.forts_acc != null)
                    {
                        _forts_clientLock.EnterWriteLock();
                        _forts_client = clientInfo;
                        _forts_clientLock.ExitWriteLock();
                    }

                    break;

                case "positions":
                    var positions = (positions)XMLHelper.Deserialize(result, typeof(positions));

                    _positionsLock.EnterWriteLock();
                    if (positions.forts_collaterals != null) _positions.forts_collaterals = positions.forts_collaterals;
                    if (positions.forts_money != null) _positions.forts_money = positions.forts_money;
                    if (positions.money_position != null) _positions.money_position = positions.money_position;
                    if (positions.sec_position != null) _positions.sec_position = positions.sec_position;
                    if (positions.spot_limit != null) _positions.spot_limit = positions.spot_limit;
                    if (positions.united_limits != null) _positions.united_limits = positions.united_limits;
                    _positionsLock.ExitWriteLock();

                    if (positions.forts_position != null)
                    {
                        foreach (var position in positions.forts_position)
                        {
                            _fortsPositions.AddOrUpdate(position.GetKey(), position, (key, curValue) => position);
                        }
                    }
                    PositionsAreActual = true;

                    // Fix syncronization. Positions Loaded must be set when all necessary positions have been received
                    PositionsLoaded.Set();
                    break;

                case "mc_portfolio":
                    _mc_portfolioLock.EnterWriteLock();
                    _mc_portfolio = (mc_portfolio)XMLHelper.Deserialize(result, typeof(mc_portfolio));
                    _mc_portfolioLock.ExitWriteLock();

                    MC_portfolioLoaded.Set();

                    break;

                case "overnight":
                    break;

                case "trades":
                    var trades = (trades)XMLHelper.Deserialize(result, typeof(trades));
                    _tradesHandle(trades);
                    break;

                case "orders":
                    var orders = (orders)XMLHelper.Deserialize(result, typeof(orders));
                    _ordersHandle(orders);
                    break;

                case "candles":
                    var candles = (candles)XMLHelper.Deserialize(result, typeof(candles));

                    CurrentCandle[candles.seccode] = candles.candlesValue[0];
                    WaitForCurrentCandle[candles.seccode].Set();

                    Candles[candles.seccode] = candles;
                    CandlesLoaded[candles.seccode].Set();
                    break;

                case "ticks":
                    break;

                ///ДАННЫЕ СТАКАНА///
                case "alltrades":

                    break;
                case "quotations":
                    break;
                case "quotes":
                    var q = (quotes)XMLHelper.Deserialize(result, typeof(quotes));
                    _quotesHandle(q);
                    break;
            }
        }

        private void _secInfoUpdsHandle(sec_info_upd secInfoUpds)
        {
            _secInfoUpds.AddOrUpdate(secInfoUpds.GetKey(),
                secInfoUpds,
                (key, existingSecurity) => secInfoUpds);
        }

        private void _securitiesHandle(List<Application.Models.TransaqConnector.Ingoing.securities_ns.stock_security> security)
        {
            foreach (var sec in security)
            {
                // filter only unique values
                _securities.AddOrUpdate(sec.GetKey(),
                                        sec,
                                        (key, existingSecurity) => sec);
            }
        }

        protected void _tradesHandle(trades trades)
        {
            foreach (var trade in trades.trade)
            {
                var temp = _trades.GetValueOrDefault(trade.tradeno);

                if (temp != null)
                {
                    //temp = trade; // Globals.Trades[Globals.Trades.IndexOf(temp)] = trade;

                    continue;
                }
                else _trades.AddOrUpdate(trade.tradeno, trade, (key, curValue) => trade);
            }
        }

        protected void _ordersHandle(orders orders)
        {
            foreach (var order in orders.order)
            {
                var temp = _orders.GetValueOrDefault(order.transactionid);

                if (temp != null)
                {
                    temp.result = order.result;
                    temp.status = order.status;
                }
                else _orders.AddOrUpdate(order.transactionid, order, (key, curValue) => order);
            }
        }

        protected void _quotesHandle(quotes quotes)
        {
            foreach (var quote in quotes.quote)
            {
                //if (quote.buy > 0 && quote.sell == 0)
                //{
                //    if (_bid < quote.price)
                //    {

                //    }
                //}

                var q = _quotes.GetValueOrDefault(quote.GetKey());

                if (q != null)
                {
                    if (quote.buy == -1 || quote.sell == -1)
                    {
                        _quotes.Remove(q.GetKey(), out var value);
                    }
                    else
                    {
                        q.buy = quote.buy;
                        q.sell = quote.sell;
                    }
                }
                else if (quote.buy != -1 && quote.sell != -1)
                {
                    _quotes.AddOrUpdate(quote.GetKey(), quote, (key, existingValue) => quote);
                }
            }
        }

        protected string _getNodeName(string data)
        {
            log.WriteLog("ServerData: " + data);

            XmlReaderSettings xs = new XmlReaderSettings();
            xs.IgnoreWhitespace = true;
            xs.ConformanceLevel = ConformanceLevel.Fragment;
            xs.ProhibitDtd = false;
            XmlReader xr = XmlReader.Create(new StringReader(data), xs);

            xr.Read();
            return xr.Name;
        }
    }
}
