using AutoTraderSDK.Model;
using AutoTraderSDK.Model.Ingoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AutoTraderSDK.Core
{
    public abstract class TXMLConnectorCallbackableBase: TXMLConnectorBase
    {
        public AutoResetEvent serverStatusUpdated = new AutoResetEvent(false);
        public AutoResetEvent positionsLoaded = new AutoResetEvent(false);
        public AutoResetEvent securitiesLoaded = new AutoResetEvent(false);
        public AutoResetEvent tradesLoaded = new AutoResetEvent(false);
        protected AutoResetEvent mc_portfolioLoaded = new AutoResetEvent(false);


        public bool Connected
        {
            get {
                bool res = false;
                if (_serverStatus != null && _serverStatus.connected == "true") res = true;

                return res;
            }
        }

        public event EventHandler<OnMCPositionsUpdatedEventArgs> OnMCPositionsUpdated;


        /// <summary>
        /// Результат подключения к серверу
        /// Заполняется с помощью обработчика _handleData
        /// </summary>
        protected server_status _serverStatus = null;

        protected client _forts_client = null;

        protected List<client> _clients = new List<client>();       // клиенты-счета различных площадок forts: market=4


        protected positions _positions = new positions();
        protected mc_portfolio _mc_portfolio = new mc_portfolio();
        protected candle _currentCandle = null;

        protected bool _positionsIsActual = false;

        protected HashSet<Model.Ingoing.quotes_ns.quote> _quotes { get; set; }
        protected HashSet<Model.Ingoing.orders_ns.order> _orders { get; set; }
        protected HashSet<Model.Ingoing.trades_ns.trade> _trades { get; set; }

        protected HashSet<Model.Ingoing.securities_ns.security> _securities { get; set; }

        public TXMLConnectorCallbackableBase(string tConnFile) : base(tConnFile)
        {
            _quotes = new HashSet<Model.Ingoing.quotes_ns.quote>();
            _orders = new HashSet<Model.Ingoing.orders_ns.order>();
            _trades = new HashSet<Model.Ingoing.trades_ns.trade>();
            _securities = new HashSet<Model.Ingoing.securities_ns.security>();
        }

        protected override void _handleData(String result)
        {
            string nodeName = _getNodeName(result);

            switch (nodeName)
            {
                case "server_status":
                    _serverStatus = (server_status)XMLHelper.Deserialize(result, typeof(server_status));

                    serverStatusUpdated.Set();

                    break;

                case "markets":
                    break;

                case "boards":
                    break;

                case "candlekinds":
                    break;

                case "securities":
                    var securities = (securities)XMLHelper.Deserialize(result, typeof(securities));

                    _securitiesHandle(securities.security);
                    securitiesLoaded.Set();
                    break;

                case "pits":
                    break;

                case "sec_info_upd":
                    break;





                case "client":
                    var clientInfo = (client)XMLHelper.Deserialize(result, typeof(client));

                    // todo реализовать проверку на присутствие и удаление элемента перед добавлением
                    _clients.Add(clientInfo);

                    if (clientInfo.forts_acc != null)
                        _forts_client = clientInfo;

                    break;

                case "positions":
                    var positions = (positions)XMLHelper.Deserialize(result, typeof(positions));

                    if (positions.forts_collaterals != null) _positions.forts_collaterals = positions.forts_collaterals;
                    if (positions.forts_money != null) _positions.forts_money = positions.forts_money;
                    if (positions.forts_position != null) _positions.forts_position = positions.forts_position;
                    if (positions.money_position != null) _positions.money_position = positions.money_position;
                    if (positions.sec_position != null) _positions.sec_position = positions.sec_position;
                    if (positions.spot_limit != null) _positions.spot_limit = positions.spot_limit;


                    _positionsIsActual = true;

                    positionsLoaded.Set();
                    break;

                case "mc_portfolio":
                    _mc_portfolio = (mc_portfolio)XMLHelper.Deserialize(result, typeof(mc_portfolio));

                   
                    mc_portfolioLoaded.Set();
                    OnMCPositionsUpdated.Invoke(this, new OnMCPositionsUpdatedEventArgs(_mc_portfolio));


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
                    _currentCandle = candles.candle[0];
                    break;

                case "ticks":
                    break;


                ///ДАННЫЕ СТАКАНА///
                case "alltrades":

                    break;
                case "quotations":
                    break;
                case "quotes":
                    var q = (AutoTraderSDK.Model.Ingoing.quotes)XMLHelper.Deserialize(result, typeof(AutoTraderSDK.Model.Ingoing.quotes));
                    _quotesHandle(q);
                    break;



            }
        }



        private void _securitiesHandle(List<Model.Ingoing.securities_ns.security> security)
        {
            lock(_securities)
            {
                foreach (var sec in security)
                {    
                    if (_securities.Where(x=>x.seccode==sec.seccode).FirstOrDefault() == null)
                        _securities.Add(sec);
                }
            }
        }

        protected void _tradesHandle(trades trades)
        {
            lock (_trades)
            {
                foreach (var trade in trades.trade)
                {
                    var temp = _trades.FirstOrDefault(x => x.tradeno == trade.tradeno);

                    if (temp != null)
                    {
                        //temp = trade; // Globals.Trades[Globals.Trades.IndexOf(temp)] = trade;

                        continue;

                    }
                    else _trades.Add(trade);
                }
            }
        }

        protected void _ordersHandle(orders orders)
        {
            lock (_orders)
            {
                foreach (var order in orders.order)
                {
                    var temp = _orders.FirstOrDefault(x => x.orderno == order.orderno);

                    if (temp != null)
                    {
                        //temp = order; //Globals.Orders[ Globals.Orders.IndexOf( temp ) ] = order;

                        temp.result = order.result;
                        temp.status = order.status;

                    }
                    else _orders.Add(order);
                }
            }
        }

        protected void _quotesHandle(Model.Ingoing.quotes quotes)
        {

            foreach (var quote in quotes.quote)
            {
                //if (quote.buy > 0 && quote.sell == 0)
                //{
                //    if (_bid < quote.price)
                //    {

                //    }
                //}


                lock (_quotes)
                {
                    var q = _quotes.FirstOrDefault(x => x.price == quote.price);

                    if (q != null)
                    {
                        if (quote.buy == -1 || quote.sell == -1)
                        {
                            _quotes.Remove(q);
                        }
                        else
                        {
                            q.buy = quote.buy;
                            q.sell = quote.sell;
                        }
                    }
                    else if (quote.buy != -1 && quote.sell != -1)
                    {
                        _quotes.Add(quote);
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
