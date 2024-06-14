using AutoTrader.Application.Common;
using AutoTrader.Application.Helpers;
using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
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

namespace AutoTrader.Infrastructure.Stock
{
    public class TXMLConnectorInputStreamHandler
    {
        public AsyncAutoResetEvent ServerStatusUpdated = new AsyncAutoResetEvent(false);
        public AsyncAutoResetEvent PositionsLoaded = new AsyncAutoResetEvent(false);
        public AsyncAutoResetEvent SecuritiesLoaded = new AsyncAutoResetEvent(false);
        public AsyncAutoResetEvent TradesLoaded = new AsyncAutoResetEvent(false);
        public ConcurrentDictionary<string, AsyncAutoResetEvent> WaitForCurrentCandle = new ConcurrentDictionary<string, AsyncAutoResetEvent>();

        public AsyncAutoResetEvent MC_portfolioLoaded = new AsyncAutoResetEvent(false);

        public ConcurrentDictionary<string, AsyncAutoResetEvent> CandlesLoaded = new ConcurrentDictionary<string, AsyncAutoResetEvent>();

        /// <summary>
        /// Результат подключения к серверу
        /// Заполняется с помощью обработчика _handleData
        /// </summary>
        public server_status ServerStatus = null;

        public client Forts_client = null;

        public List<client> Clients = new List<client>();       // клиенты-счета различных площадок forts: market=4

        public positions Positions = new positions();
        public mc_portfolio mc_portfolio = new mc_portfolio();
        public ConcurrentDictionary<string, Application.Models.TXMLConnector.Ingoing.candle> CurrentCandle = new ConcurrentDictionary<string, candle>();

        public bool PositionsIsActual = false;

        public HashSet<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> Quotes { get; private set; }
        public HashSet<Application.Models.TXMLConnector.Ingoing.orders_ns.order> Orders { get; private set; }
        public HashSet<Application.Models.TXMLConnector.Ingoing.trades_ns.trade> Trades { get; private set; }
        public HashSet<Application.Models.TXMLConnector.Ingoing.securities_ns.security> Securities { get; private set; }

        /// <summary>
        /// for history data
        /// </summary>
        public ConcurrentDictionary<string, Application.Models.TXMLConnector.Ingoing.candles> Candles { get; set; } = new ConcurrentDictionary<string, candles>();

        public Application.Models.TXMLConnector.Ingoing.candlekinds Candlekinds { get; set; }

        public event EventHandler<TXMLEventArgs<HashSet<Application.Models.TXMLConnector.Ingoing.securities_ns.security>>> SecuritiesUpdated;

        public event EventHandler<TXMLEventArgs<mc_portfolio>> MCPositionsUpdated;

        public TXMLConnectorInputStreamHandler()
        {
            Quotes = new HashSet<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote>();
            Orders = new HashSet<Application.Models.TXMLConnector.Ingoing.orders_ns.order>();
            Trades = new HashSet<Application.Models.TXMLConnector.Ingoing.trades_ns.trade>();
            Securities = new HashSet<Application.Models.TXMLConnector.Ingoing.securities_ns.security>();
        }

        public void HandleData(String result)
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
                    SecuritiesUpdated?.Invoke(this, new TXMLEventArgs<HashSet<Application.Models.TXMLConnector.Ingoing.securities_ns.security>>(Securities));
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

                    ServerStatusUpdated.Set();

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


                    PositionsIsActual = true;

                    PositionsLoaded.Set();
                    break;

                case "mc_portfolio":
                    mc_portfolio = (mc_portfolio)XMLHelper.Deserialize(result, typeof(mc_portfolio));
                   
                    MC_portfolioLoaded.Set();
                    MCPositionsUpdated?.Invoke(this, new TXMLEventArgs<mc_portfolio>(mc_portfolio));

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
                    var q = (Application.Models.TXMLConnector.Ingoing.quotes)XMLHelper.Deserialize(result, typeof(Application.Models.TXMLConnector.Ingoing.quotes));
                    _quotesHandle(q);
                    break;
            }
        }

        private void _securitiesHandle(List<Application.Models.TXMLConnector.Ingoing.securities_ns.security> security)
        {
            lock(Securities)
            {
                foreach (var sec in security)
                {    
                    // filter only unique values
                    if (Securities.Where(x=>x.seccode==sec.seccode).FirstOrDefault() == null)
                        Securities.Add(sec);
                }
            }
        }

        protected void _tradesHandle(trades trades)
        {
            lock (Trades)
            {
                foreach (var trade in trades.trade)
                {
                    var temp = Trades.FirstOrDefault(x => x.tradeno == trade.tradeno);

                    if (temp != null)
                    {
                        //temp = trade; // Globals.Trades[Globals.Trades.IndexOf(temp)] = trade;

                        continue;

                    }
                    else Trades.Add(trade);
                }
            }
        }

        protected void _ordersHandle(orders orders)
        {
            lock (Orders)
            {
                foreach (var order in orders.order)
                {
                    var temp = Orders.FirstOrDefault(x => x.orderno == order.orderno);

                    if (temp != null)
                    {
                        //temp = order; //Globals.Orders[ Globals.Orders.IndexOf( temp ) ] = order;

                        temp.result = order.result;
                        temp.status = order.status;

                    }
                    else Orders.Add(order);
                }
            }
        }

        protected void _quotesHandle(Application.Models.TXMLConnector.Ingoing.quotes quotes)
        {
            foreach (var quote in quotes.quote)
            {
                //if (quote.buy > 0 && quote.sell == 0)
                //{
                //    if (_bid < quote.price)
                //    {

                //    }
                //}

                lock (Quotes)
                {
                    var q = Quotes.FirstOrDefault(x => x.price == quote.price);

                    if (q != null)
                    {
                        if (quote.buy == -1 || quote.sell == -1)
                        {
                            Quotes.Remove(q);
                        }
                        else
                        {
                            q.buy = quote.buy;
                            q.sell = quote.sell;
                        }
                    }
                    else if (quote.buy != -1 && quote.sell != -1)
                    {
                        Quotes.Add(quote);
                    }
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
            XmlReader xr = XmlReader.Create(new System.IO.StringReader(data), xs);

            xr.Read();
            return xr.Name;
        }
    }
}
