using AutoTraderSDK.Domain.InputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoTraderSDK.Kernel
{
    public abstract class TXMLConnectorCallbackableBase: TXMLConnectorBase
    {
        /// <summary>
        /// Результат подключения к серверу
        /// Заполняется с помощью обработчика _handleData
        /// </summary>
        protected string _serverStatus = null;

        protected static client _client = null;
        protected static positions _positions = null;
        protected static candle _currentCandle = null;

        protected static bool _positionsIsActual = false;

        protected static HashSet<quote> _quotes { get; set; }
        protected static HashSet<order> _orders { get; set; }
        protected static HashSet<trade> _trades { get; set; }

        protected static HashSet<security> _securities { get; set; }

        public TXMLConnectorCallbackableBase(string tConnFile) : base(tConnFile)
        {
            _quotes = new HashSet<quote>();
            _orders = new HashSet<order>();
            _trades = new HashSet<trade>();
            _securities = new HashSet<security>();
        }

        protected override void _handleData(String result)
        {
            string nodeName = _getNodeName(result);

            switch (nodeName)
            {
                case "server_status":
                    _serverStatus = result;

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
                    break;

                case "pits":
                    break;

                case "sec_info_upd":
                    break;





                case "client":
                    var clientInfo = (client)XMLHelper.Deserialize(result, typeof(client));

                    if (clientInfo.forts_acc != null)
                        _client = clientInfo;
                    break;

                case "positions":
                    _positions = (positions)XMLHelper.Deserialize(result, typeof(positions));

                    _positionsIsActual = true;
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
                    var q = (AutoTraderSDK.Domain.InputXML.quotes)XMLHelper.Deserialize(result, typeof(AutoTraderSDK.Domain.InputXML.quotes));
                    _quotesHandle(q);
                    break;



            }
        }

        private void _securitiesHandle(List<security> security)
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

        protected void _quotesHandle(Domain.InputXML.quotes quotes)
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
