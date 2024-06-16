using AutoTrader.Application.Common;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public server_status? ServerStatus = null;
        public client? Forts_client = null;
        public List<client> Clients = new();       // клиенты-счета различных площадок forts: market=4
        public positions Positions = new();
        public mc_portfolio mc_portfolio = new();
        public ConcurrentDictionary<string, candle> CurrentCandle = new();
        public bool PositionsAreActual = false;

        private readonly ReaderWriterLockSlim _quotesLock = new();
        private readonly ConcurrentDictionary<string, Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> _quotes = new();
        public List<Application.Models.TransaqConnector.Ingoing.quotes_ns.quote> Quotes
        {
            get
            {
                try
                {
                    _quotesLock.EnterReadLock();
                    return _quotes.Values.ToList();
                }
                finally
                {
                    _quotesLock.ExitReadLock();
                }

            }
        }

        private readonly ReaderWriterLockSlim _ordersLock = new();
        private readonly ConcurrentDictionary<Int64, Application.Models.TransaqConnector.Ingoing.orders_ns.order> _orders = new();
        public List<Application.Models.TransaqConnector.Ingoing.orders_ns.order> Orders
        {
            get
            {
                try
                {
                    _ordersLock.EnterReadLock();
                    return _orders.Values.ToList();
                }
                finally
                {
                    _ordersLock.ExitReadLock();
                }
            }
        }

        private readonly ReaderWriterLockSlim _tradesLock = new();
        private readonly ConcurrentDictionary<long, Application.Models.TransaqConnector.Ingoing.trades_ns.trade> _trades = new();
        public List<Application.Models.TransaqConnector.Ingoing.trades_ns.trade> Trades
        {
            get
            {
                try
                {
                    _tradesLock.EnterReadLock();
                    return _trades.Values.ToList();
                }
                finally
                {
                    _tradesLock.ExitReadLock();
                }
            }
        }

        private readonly ReaderWriterLockSlim _securitiesLock = new();
        private readonly ConcurrentDictionary<string, Application.Models.TransaqConnector.Ingoing.securities_ns.security> _securities = new();
        public List<Application.Models.TransaqConnector.Ingoing.securities_ns.security> Securities
        {
            get
            {
                try
                {
                    _securitiesLock.EnterReadLock();
                    return _securities.Values.ToList();
                }
                finally
                {
                    _securitiesLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// for history data
        /// </summary>
        public ConcurrentDictionary<string, candles> Candles { get; set; } = new ConcurrentDictionary<string, candles>();
        public candlekinds Candlekinds { get; set; }
        public event EventHandler<TransaqEventArgs<List<Application.Models.TransaqConnector.Ingoing.securities_ns.security>>> SecuritiesUpdated;
        public event EventHandler<TransaqEventArgs<mc_portfolio>> MCPositionsUpdated;

        public void HandleData(string result)
        {
            string nodeName = _getNodeName(result);
            Debug.WriteLine(nodeName);
            //File.AppendAllText("stream.csv", result+"\n");
            switch (nodeName)
            {
                case "server_status":
                    ServerStatus = (server_status)XMLHelper.Deserialize(result, typeof(server_status));

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
                    var securities = (securities)XMLHelper.Deserialize(result, typeof(securities));

                    _securitiesHandle(securities.security);
                    SecuritiesUpdated?.Invoke(this, new TransaqEventArgs<List<Application.Models.TransaqConnector.Ingoing.securities_ns.security>>(Securities));
                    SecuritiesLoaded.Set();
                    break;

                case "pits":
                    break;

                case "sec_info_upd":
                    break;

                case "client":
                    var clientInfo = (client)XMLHelper.Deserialize(result, typeof(client));

                    // todo реализовать проверку на присутствие и удаление элемента перед добавлением
                    Clients.Add(clientInfo);

                    //ServerStatusUpdated.Set();

                    if (clientInfo.forts_acc != null)
                        Forts_client = clientInfo;

                    break;

                case "positions":
                    var positions = (positions)XMLHelper.Deserialize(result, typeof(positions));

                    if (positions.forts_collaterals != null) Positions.forts_collaterals = positions.forts_collaterals;
                    if (positions.forts_money != null) Positions.forts_money = positions.forts_money;
                    if (positions.forts_position != null) Positions.forts_position = positions.forts_position;
                    if (positions.money_position != null) Positions.money_position = positions.money_position;
                    if (positions.sec_position != null) Positions.sec_position = positions.sec_position;
                    if (positions.spot_limit != null) Positions.spot_limit = positions.spot_limit;


                    PositionsAreActual = true;

                    PositionsLoaded.Set();
                    break;

                case "mc_portfolio":
                    mc_portfolio = (mc_portfolio)XMLHelper.Deserialize(result, typeof(mc_portfolio));

                    MC_portfolioLoaded.Set();
                    MCPositionsUpdated?.Invoke(this, new TransaqEventArgs<mc_portfolio>(mc_portfolio));

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

        private void _securitiesHandle(List<Application.Models.TransaqConnector.Ingoing.securities_ns.security> security)
        {
            try
            {
                _securitiesLock.EnterWriteLock();
                foreach (var sec in security)
                {
                    // filter only unique values
                    _securities.AddOrUpdate(sec.GetKey(), 
                                            sec, 
                                            (key, existingSecurity) => sec);
                }
            }
            finally
            {
                _securitiesLock.ExitWriteLock();
            }
        }

        protected void _tradesHandle(trades trades)
        {
            try
            {
                _tradesLock.EnterWriteLock();
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
            finally
            {
                _tradesLock.ExitWriteLock();
            }
        }

        protected void _ordersHandle(orders orders)
        {
            try
            {
                _ordersLock.EnterWriteLock();
                foreach (var order in orders.order)
                {
                    var temp = _orders.GetValueOrDefault(order.orderno);

                    if (temp != null)
                    {
                        temp.result = order.result;
                        temp.status = order.status;
                    }
                    else _orders.AddOrUpdate(order.orderno, order, (key, curValue) => order);
                }
            }
            finally
            {
                _ordersLock?.ExitWriteLock();
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

                try
                {                    
                    
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
                        _quotes.AddOrUpdate(quote.GetKey(), quote, (key, existingValue)=> quote);
                    }
                }
                finally
                {
                    _quotesLock.ExitWriteLock();
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
