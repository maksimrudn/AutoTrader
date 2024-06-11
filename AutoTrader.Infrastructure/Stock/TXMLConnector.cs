using AutoTrader.Application.Common;
using AutoTrader.Application.Contracts.Infrastructure.TXMLConnector;
using AutoTrader.Application.Helpers;
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
    public class TXMLConnector: TXMLConnectorCallbackableBase, ITXMLConnector
    {
        
        public bool PositionsIsActual
        {
            get { return _positionsIsActual; }
            set { _positionsIsActual = value; }
        }

        
        public string FortsClientId { get { return (_forts_client != null) ? _forts_client.forts_acc : null; } }
        public double Money
        {
            get
            {
                double res = 0;

                try
                {
                    res = _mc_portfolio.moneys.First(x=>x.currency == "RUB").balance;
                }
                catch { }

                return res;
            }
        }
        public string Union
        {
            get
            {
                return _forts_client?.union;
            }
        }

        public candlekinds Candlekinds
        {
            get
            {
                return _candlekinds;
            }
        }

        public List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> QuotesBuy
        {
            get
            {

                List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote> quotes1;

                lock (_quotes)
                {
                    quotes1 = new List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote>(_quotes);
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

                lock (_quotes)
                {
                    quotes1 = new List<Application.Models.TXMLConnector.Ingoing.quotes_ns.quote>(_quotes);
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

                lock (_quotes)
                {
                    if (_quotes.Count > 0)
                        res = _quotes.Where(x => x.buy > 0 && x.sell == 0).Max(x => x.price);


                }

                return res;
            }
        }

        public double Ask
        {
            get
            {
                double res = 0;

                lock (_quotes)
                {
                    if (_quotes.Count > 0)
                        res = _quotes.Where(x => x.sell > 0 && x.buy == 0).Min(x => x.price);
                }

                return res;
            }
        }


        public positions Positions { get { return _positions; } }

        public mc_portfolio MCPortfolio { get { return _mc_portfolio; } }

        public List<Application.Models.TXMLConnector.Ingoing.orders_ns.order> OpenOrders
        {
            get
            {
                List<Application.Models.TXMLConnector.Ingoing.orders_ns.order> res = null;

                lock (_orders)
                {
                    res = _orders.Where(x => x.status == status.active || x.status == status.watching).ToList();
                }

                return res;
            }
        }


        public TXMLConnector(string tconFile = "txmlconnector1.dll"):base(tconFile)
        {
            
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
            var sendCommandResult = ConnectorSendCommand(com, com.GetType());

            
            if (sendCommandResult.success == false)
            {
                throw new Exception(sendCommandResult.message);
            }
            else
            {
                // ждём ассинхронный ответ о результате подключения
                await _serverStatusUpdated.WaitOne();

                if (_serverStatus.connected == "error")
                {
                    throw new Exception(_serverStatus.InnerText);
                }

                await _positionsLoaded.WaitOne();

                _getMCPortfolioPositions();
                await _mc_portfolioLoaded.WaitOne();
            }
        }

        

        public void Logout()
        {
            var com = command.CreateDisconnectCommand();

            _serverStatusUpdated.Reset();
            var res = ConnectorSendCommand(com, com.GetType());
        }

        public void ChangePassword(string oldpass, string newpass)
        {
            var com = command.CreateChangePasswordCommand(oldpass, newpass);

            var res = ConnectorSendCommand(com, com.GetType());

            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        protected void _getMCPortfolioPositions()
        {
            var com = command.CreateGetMCPortfolioCommand(_positions.money_position.client);

            var res = ConnectorSendCommand(com, com.GetType());

            if (!res.success)
            {
                throw new Exception(res.message);
            }
        }

        public async Task UnInitialize()
        {
            if (Connected)
            {
                Logout();
                await _serverStatusUpdated.WaitOne(25 * 1000);
            }

            IntPtr pResult = _unInitialize();

            if (!pResult.Equals(IntPtr.Zero))
            {
                String result = MarshalUTF8.PtrToStringUTF8(pResult);
                _freeUpMemory(pResult);
                log.WriteLog(result);
            }
            else
            {
                log.WriteLog("UnInitialize() OK");
            }
        }

        public new async ValueTask DisposeAsync()
        {
            await UnInitialize();
            _serverStatusUpdated.Dispose();
            await base.DisposeAsync();
        }

        public void SubscribeQuotations(boardsCode board, string seccode)
        {
            if (board == null)
            {
                throw new Exception("board is not identified");
            }
            else if (string.IsNullOrEmpty(seccode))
            {
                throw new Exception("seccode is not identified");
            }


            var com = command.CreateSubscribeQuotationsCommand(board, seccode);

            var res = ConnectorSendCommand(com, com.GetType());


        }
        public void SubscribeQuotes(boardsCode board, string seccode)
        {
            var com = command.CreateSubscribeQuotesCommand(board, seccode);

            var res = ConnectorSendCommand(com, com.GetType());


        }


        public async Task<List<candle>> GetHistoryData(string seccode, boardsCode board = boardsCode.FUT, SecurityPeriods periodId =  SecurityPeriods.M1, int candlesCount = 1)
        {
            candle res = null;

            command com = command.CreateGetHistoryDataCommand(board, seccode, periodId, candlesCount);

            // initialization must be before call of txml connector because response can be come fast
            if (!_candlesLoaded.ContainsKey(seccode))
                _candlesLoaded[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _candlesLoaded[seccode].WaitOne();
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return _candles[seccode].candlesValue;
        }

        public async Task<candle> GetCurrentCandle(string seccode, boardsCode board = boardsCode.FUT)
        {
            candle res = null;

            command com = new command();
            com.id = command_id.gethistorydata;
            com.security.board = board.ToString();
            com.security.seccode = seccode;
            com.periodValue = 1;
            com.countValue = 1;
            com.resetValue = true;

            _currentCandle = null;
            if (!_waitForCurrentCandle.ContainsKey(seccode))
                _waitForCurrentCandle[seccode] = new AsyncAutoResetEvent(false);

            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _waitForCurrentCandle[seccode].WaitOne();

                res = _currentCandle[seccode];
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


            result sendResult = ConnectorSendCommand(com, typeof(command));

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


            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            //return res;
        }

        public int NewOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, double price, int volume)
        {
            if (!Connected) throw new Exception("Соединение не установлено. Операция не может быть выполнена");

            int res = 0;

            command com = command.CreateNewOrder(this.FortsClientId, 
                                                        board, 
                                                        seccode, 
                                                        buysell, 
                                                        bymarket? AutoTrader.Domain.Models.bymarket.yes: AutoTrader.Domain.Models.bymarket.no, 
                                                        price, 
                                                        volume);


            result sendResult = ConnectorSendCommand(com, typeof(command));

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

        public int NewStopOrderWithDistance(boardsCode board, string seccode, buysell buysell, double price, double SLDistance, double TPDistance, int volume, Int64 orderno = 0)
        {
            return NewStopOrder(board,
                                seccode,
                                buysell,
                                (buysell == buysell.B) ? (price + SLDistance) : (price - SLDistance),
                                (buysell == buysell.B) ? (price - TPDistance) : (price + TPDistance),
                                volume,
                                orderno);
        }

        public int NewStopOrder(boardsCode board, 
                                    string seccode, 
                                    buysell buysell, 
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
            com.security.board = board.ToString();
            com.security.seccode = seccode;

            com.buysellValue = buysell;
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


            result sendResult = ConnectorSendCommand(com, typeof(command));


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

        public int NewConditionOrder(boardsCode board, 
                                    string seccode,
                                    AutoTrader.Domain.Models.buysell buysell, 
                                    bool bymarket, 
                                    cond_type condtype, 
                                    double condvalue, 
                                    int volume)
        {
            int res = 0;

            command com = command.CreateNewCondOrderCommand(); ;
            com.id = command_id.newcondorder;
            com.security.board = board.ToString();
            com.security.seccode = seccode;
            com.buysellValue = buysell;
            com.client = FortsClientId;
            // параметры, которые говорят переносить заявку на следующий день
            com.validafter = "0";
            com.validbefore = "till_canceled";
            /////////////////////////////////
            com.cond_typeValue = condtype;
            com.cond_valueValue = condvalue;
            com.quantityValue = volume;
            com.bymarketValue = bymarket? AutoTrader.Domain.Models.bymarket.yes: AutoTrader.Domain.Models.bymarket.no;

            result sendResult = ConnectorSendCommand(com, typeof(command));

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

        public async Task NewComboOrder(ComboOrder co)
        {
            if (co.BuySell == null) throw new Exception("Не установлен параметр BuySell");

            await NewComboOrder(co.Board, co.Seccode, co.BuySell.Value, co.ByMarket, co.Price, co.Vol, co.SL, co.TP);
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
        public async Task NewComboOrder(boardsCode board, 
                                        string seccode, 
                                        buysell buysell,
                                        bool bymarket,
                                        int price,
                                        int volume, 
                                        int slDistance, 
                                        int tpDistance,
                                        int comboType = 1)
        {
            
            int tid = NewOrder(board, 
                                seccode, 
                                buysell, 
                                bymarket, 
                                price, 
                                volume);

            // todo перейти на AutoResetEvent
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

            var closeOrderBysell = (buysell == buysell.B) ? buysell.S : buysell.B;

            if (comboType == 1)
            {
                // открытие sl заявки
                if (slDistance > 0)
                {
                    double slPrice = (buysell == buysell.B) ? tr.price - slDistance : tr.price + slDistance;
                    var closeCondition = (buysell == buysell.B) ? cond_type.LastDown : cond_type.LastUp;

                    NewConditionOrder(board,
                                    seccode,
                                    closeOrderBysell,
                                    bymarket,
                                    closeCondition,
                                    slPrice,
                                    volume);
                }

                // открытие tp заявки
                if (tpDistance > 0)
                {

                    double tpPrice = (buysell == buysell.B) ? tr.price + tpDistance : tr.price - tpDistance;
                    NewOrder(board, seccode, closeOrderBysell, bymarket, tpPrice, volume);
                }
            }
            else
            {
                // todo дописать
                //NewStopOrder(board, seccode, buysell, )
            }
              

        }

        

        public Application.Models.TXMLConnector.Ingoing.orders_ns.order GetOrderByTransactionId(int transactionid)
        {
            Application.Models.TXMLConnector.Ingoing.orders_ns.order res = null;

            lock (_orders)
            {
                res = _orders.FirstOrDefault(x => x.transactionid == transactionid);
            }

            return res;
        }

        public Application.Models.TXMLConnector.Ingoing.trades_ns.trade GetTradeByOrderNo(Int64 orderno)
        {
            Application.Models.TXMLConnector.Ingoing.trades_ns.trade res = null;

            lock (_trades)
            {
                res = _trades.FirstOrDefault(x => x.orderno == orderno);
            }

            return res;
        }

        public async Task<List<Application.Models.TXMLConnector.Ingoing.securities_ns.security>> GetSecurities()
        {
            List<Application.Models.TXMLConnector.Ingoing.securities_ns.security> res = new List<Application.Models.TXMLConnector.Ingoing.securities_ns.security>();
            command com = command.CreateGetSecurities();

            _securitiesLoaded.Reset();
            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                await _securitiesLoaded.WaitOne();

                res = _securities.ToList();
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
                var positions = _positions.forts_position.FirstOrDefault(x => x.seccode == seccode);

                res = positions.totalnet;
            }
            catch
            {
            }


            return res;
        }
    }
}
