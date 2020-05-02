using AutoTraderSDK.Domain.InputXML;
using AutoTraderSDK.Domain.OutputXML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace AutoTraderSDK.Kernel
{
    public class TXMLConnector: TXMLConnectorCallbackableBase, ITXMLConnector
    {
        
        

        public bool PositionsIsActual
        {
            get { return _positionsIsActual; }
            set { _positionsIsActual = value; }
        }

        public bool IsConnected { get; private set; }
        public string ClientId { get { return (_client != null) ? _client.forts_acc : null; } }
        public List<quote> QuotesBuy
        {
            get
            {

                List<quote> quotes1;

                lock (_quotes)
                {
                    quotes1 = new List<quote>(_quotes);
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

        public List<quote> QuotesSell
        {
            get
            {
                List<quote> quotes1;

                lock (_quotes)
                {
                    quotes1 = new List<quote>(_quotes);
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

        public List<order> OpenOrders
        {
            get
            {
                List<order> res = null;

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

        ~TXMLConnector()
        {
            Console.WriteLine("Finalizing object");

            Logout();
        }




        public void Login(string username, string password, string server = "tr1.finam.ru", int port = 3900)
        {
            _serverStatus = null;

            var com = command.CreateConnectionCommand(username, password, server, port);

            // отправляем команду подключения
            var res = ConnectorSendCommand(com, com.GetType());

            // ждём ассинхронный ответ о результате подключения
            if (res.success == false)
            {
                throw new Exception(res.message);
            }
            else
            {
                while (true)
                {
                    server_status serverStatus = null;

                    if (_serverStatus == null) continue;

                    try
                    {
                        serverStatus = (server_status)XMLHelper.Deserialize(_serverStatus, typeof(server_status));
                    }
                    catch { }



                    if (serverStatus.connected == "error")
                    {
                        throw new Exception(serverStatus.InnerText);
                    }
                    else if (serverStatus != null && _client != null)
                    {
                        if (_client != null)
                        {
                            IsConnected = true;
                            break;
                        }

                    }
                    //else if (serverStatus.)

                    Application.DoEvents();
                }
            }
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

        public void Logout()
        {
            var com = command.CreateDisconnectCommand();

            var res = ConnectorSendCommand(com, com.GetType());

            _unInitialize();

        }

        public void Dispose()
        {
            Logout();
            base.Dispose();
        }

        public void Subscribe(AutoTraderSDK.Domain.OutputXML.boardsCode board, string seccode)
        {
            var com = command.CreateSubscribeCommand(board, seccode);

            var res = ConnectorSendCommand(com, com.GetType());


        }

        public candle GetCurrentCandle(string seccode, boardsCode board = boardsCode.FUT)
        {
            candle res = null;

            command com = new command();
            com.id = command_id.gethistorydata;
            com.security.board = board;
            com.security.seccode = seccode;
            com.periodValue = 1;
            com.countValue = 1;
            com.resetValue = true;

            _currentCandle = null;
            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                while (true)
                {
                    Application.DoEvents();

                    if (_currentCandle != null)
                    {
                        res = _currentCandle;
                        break;
                    }
                }
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

        public int NewOrder(boardsCode board, string seccode, buysell buysell, bymarket bymarket, double price, int volume)
        {
            if (!IsConnected) throw new Exception("Соединение не установлено. Операция не может быть выполнена");

            int res = 0;

            command com = command.CreateNewOrder(this.ClientId, board, seccode, buysell, bymarket, price, volume);


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

        public int NewStopOrder(boardsCode board, string seccode, buysell buysell, double SLPrice, double TPPrice, int volume, Int64 orderno = 0, double correction = 0)
        {
            throw new Exception("не работает");

            int res = 0;

            command com = new command();
            com.id = command_id.newstoporder;
            com.security.board = board;
            com.security.seccode = seccode;

            com.buysellValue = buysell;
            com.client = ClientId;
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

        public int NewCondOrder(boardsCode board, string seccode, Domain.OutputXML.buysell buysell, Domain.OutputXML.bymarket bymarket, cond_type condtype, double condvalue, int volume)
        {
            int res = 0;

            command com = command.CreateNewCondOrderCommand(); ;
            com.id = command_id.newcondorder;
            com.security.board = board;
            com.security.seccode = seccode;

            com.buysellValue = buysell;
            com.client = ClientId;


            com.cond_typeValue = condtype;
            com.cond_valueValue = condvalue;

            com.quantityValue = volume;

            //com.bymarket = bymarket;

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

        public void NewComboOrder(boardsCode board, string seccode, buysell buysell, int volume, int slDistance, int tpDistance)
        {
            bymarket bymarket = Domain.OutputXML.bymarket.yes;

            int tid = NewOrder(board, seccode, buysell, bymarket, 0, volume);

            order ord = null;

            while (ord == null) { Application.DoEvents(); ord = GetOrderByTransactionId(tid); }

            trade tr = null;

            while (tr == null) { Application.DoEvents(); tr = GetTradeByOrderNo(ord.orderno); }

            var closeOrderBysell = (buysell == buysell.B) ? buysell.S : buysell.B;
            double slPrice = (buysell == buysell.B) ? tr.price - slDistance : tr.price + slDistance;
            double tpPrice = (buysell == buysell.B) ? tr.price + tpDistance : tr.price - tpDistance;
            var closeCondition = (buysell == buysell.B) ? cond_type.AskOrLast : cond_type.BidOrLast;

            NewCondOrder(board,
                        seccode,
                        closeOrderBysell,
                        bymarket,
                        closeCondition,
                        slPrice,
                        volume);


            if (tpDistance != 0)
                NewOrder(board, seccode, closeOrderBysell, bymarket.no, tpPrice, volume);
        }

        //Выставляет рыночную заявку и условную на закрытие при невыгодной цене 
        public void NewMarketOrderWithCondOrder(boardsCode board, string seccode, buysell buysell, int volume, int slDistance)
        {
            bymarket bymarket = Domain.OutputXML.bymarket.yes;

            int tid = NewOrder(board, seccode, buysell, bymarket, 0, volume);

            order ord = null;

            while (ord == null) { Application.DoEvents(); ord = GetOrderByTransactionId(tid); }

            trade tr = null;

            while (tr == null) { Application.DoEvents(); tr = GetTradeByOrderNo(ord.orderno); }

            var condOrderBysell = (buysell == buysell.B) ? buysell.S : buysell.B;

            NewCondOrder(board,
                        seccode,
                        condOrderBysell,
                        bymarket,
                        (condOrderBysell == Domain.OutputXML.buysell.B) ? cond_type.AskOrLast : cond_type.BidOrLast,
                        (condOrderBysell == Domain.OutputXML.buysell.B) ? ord.price + slDistance : ord.price - slDistance,
                        volume);
        }

        public order GetOrderByTransactionId(int transactionid)
        {
            order res = null;

            lock (_orders)
            {
                res = _orders.FirstOrDefault(x => x.transactionid == transactionid);
            }

            return res;
        }

        public trade GetTradeByOrderNo(Int64 orderno)
        {
            trade res = null;

            lock (_trades)
            {
                res = _trades.FirstOrDefault(x => x.orderno == orderno);
            }

            return res;
        }

        public List<security> GetSecurities()
        {
            List<security> res = new List<security>();
            command com = command.CreateGetSecurities();

            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                while (true)
                {
                    Application.DoEvents();

                    if (_securities.Count > 0)
                    {
                        res = _securities.ToList();
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(sendResult.message);
            }

            return res;
        }
    }
}
