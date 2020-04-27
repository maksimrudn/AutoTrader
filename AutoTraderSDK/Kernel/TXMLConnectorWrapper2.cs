using AutoTraderSDK.Domain.InputXML;
using AutoTraderSDK.Domain.OutputXML;
using System;
using System.Collections.Concurrent;
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
    //обертка над коннектором
    public class TXMLConnectorWrapper2: IDisposable //, AutoTraderSDK.Kernel.ITXMLConnectorWrapper
    {
        #region CONSTRUCTOR
        string _logpath = Globals.GetWorkFolder()+"\0";
        int _loglevel = 3;

        protected static TXMLConnectorWrapper2 _txmlconnector1 = null;
        protected static TXMLConnectorWrapper2 _txmlconnector2 = null;



        protected static string _serverStatus = null;
        protected static client _client = null;
        protected static positions _positions = null;
        protected static candle _currentCandle = null;
        protected static HashSet<quote> _quotes { get; set; }
        protected static HashSet<order> _orders { get; set; }
        protected static HashSet<trade> _trades { get; set; }

        
        protected static double _ask = 0;

        protected static double _bid = 0;
        protected static double _bid1 = 0;


        protected static event EventHandler<InputStreamEventArgs> _newInputData;

        public bool IsConnected { get; private set; }
        public string ClientId { get { return (_client != null) ? _client.forts_acc : null; } }
        public List<quote> QuotesBuy
        {
            get
            {

                List<quote> quotes1;

                lock (_quotes)
                {
                    quotes1 = new List<quote>( _quotes);
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
                    quotes1 = new List<quote>( _quotes);                }

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
                    if (_quotes.Count>0)
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
                        res = _quotes.Where(x => x.sell > 0 && x.buy == 0).Min(x=>x.price);
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


        delegate bool CallBackDelegate(IntPtr pData);
        static readonly CallBackDelegate _callBackDelegate = new CallBackDelegate(_inputStreamHandler);
        static readonly GCHandle _callbackHandle = GCHandle.Alloc(_callBackDelegate);







        protected TXMLConnectorWrapper2()
        {
            _quotes = new HashSet<quote>();
            _orders = new HashSet<order>();
            _trades = new HashSet<trade>();

            _newInputData+=_handleData;
            
            //шаг 1
            IntPtr pPath = Kernel.MarshalUTF8.StringToHGlobalUTF8(_logpath);
            IntPtr pResult = Initialize(pPath, _loglevel);

            if (!pResult.Equals(IntPtr.Zero))
            {
                String result = Kernel.MarshalUTF8.PtrToStringUTF8(pResult);
                Marshal.FreeHGlobal(pPath);
                FreeMemory(pResult);
                log.WriteLog(result);

                throw new Exception();
            }
            else
            {
                Marshal.FreeHGlobal(pPath);
                FreeMemory(pResult);
                log.WriteLog("Initialize() OK");
            }



            //шаг 2
            bool callbackres = SetCallback(_callBackDelegate);
            if (callbackres == false)
            {
                throw new Exception("Не удалось установить обработчик обратного вызова");
            }
        }

        public static TXMLConnectorWrapper2 GetInstance()
        {
            TXMLConnectorWrapper2 res = null;

            if (_txmlconnector1 == null)
            {
                _txmlconnector1 = new TXMLConnectorWrapper2();
                res = _txmlconnector1;
            }
            else
            {
                res = _txmlconnector1;
            }

            return res;
        }

        #endregion

        #region INPUT STREAM HANDLER



        protected static bool _inputStreamHandler(IntPtr pData)
        {
            bool res = true;
            String result = Kernel.MarshalUTF8.PtrToStringUTF8(pData);
            FreeMemory(pData);

            //_handleData(result);

            _newInputData.Invoke(null, new InputStreamEventArgs() { Data = result });

            return res;
        }

        protected static void _handleData(object sender, InputStreamEventArgs arg)
        {
            string result = arg.Data;

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
                    break;

                case "pits":
                    break;

                case "sec_info_upd":
                    break;





                case "client":
                    var clientInfo = (client)_deserialize(result, typeof(client));

                    if (clientInfo.forts_acc != null)
                        _client = clientInfo;

                    break;

                case "positions":
                    _positions = (positions)_deserialize(result, typeof(positions));
                    break;

                case "overnight":
                    break;




                case "trades":
                    var trades = (trades)_deserialize(result, typeof(trades));
                    _tradesHandle(trades);
                    break;

                case "orders":
                    var orders = (orders)_deserialize(result, typeof(orders));
                    _ordersHandle(orders);
                    break;

                case "candles":
                    var candles = (candles)_deserialize(result, typeof(candles));
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
                    var q = (AutoTraderSDK.Domain.InputXML.quotes)_deserialize(result, typeof(AutoTraderSDK.Domain.InputXML.quotes));
                    _quotesHandle(q);
                    break;


                
            }
        }

        protected static void _tradesHandle(trades trades)
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

        protected static void _ordersHandle(orders orders)
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
        
        protected static void _quotesHandle(Domain.InputXML.quotes quotes)
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
        
        protected static string _getNodeName(string data)
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

        #endregion


        #region PUBLIC

        //public Trader CreateTrader(boardsCode board, string seccode)
        //{
        //    return new Trader(this, board, seccode);
        //}

        public void GetFortsPositions(boardsCode board, string seccode)
        {
            var com = new command();
            com.id = command_id.get_forts_positions;
            com.client = ClientId;


            var res = ConnectorSendCommand(com, com.GetType());

            if (res.success == false)
            {
                throw new Exception(res.message);
            }
        }


        public void Login(string username, string password, string server = "213.247.141.133", int port = 3900)
        {
            _serverStatus = null;

            var com = command.CreateConnectionCommand(username, password, server, port);

            var res = ConnectorSendCommand(com, com.GetType());

            if (res.success == false)
            {
                throw new Exception(res.message);
            }
            else
            {
                while (true)
                {
                    if (_serverStatus != null && _client!=null)
                    {
                        var data = (server_status)_deserialize(_serverStatus, typeof(server_status));

                        if (data.connected == "error")
                        {
                            throw new Exception(data.InnerText);
                        }
                        else break;
                    }

                    Application.DoEvents();
                }

            }
        }

        public void Logout()
        {
            var com = command.CreateDisconnectCommand();

            var res = ConnectorSendCommand(com, com.GetType());

            UnInitialize();

        }

        public void Dispose()
        {
            //UnInitialize();
            Logout();

            _txmlconnector1 = null;
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

        public result ConnectorSendCommand(object commandInfo, Type type)
        {
            string cmd;

            XmlSerializer xser = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            xser.Serialize(writer, commandInfo, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);

            cmd = sr.ReadToEnd();


            string res = ConnectorSendCommand(cmd);

            if (res.Contains("<error>"))
            {
                var er = (error)_deserialize(res,typeof(error));

                throw new Exception(er.Text);
            }

            return (result)_deserialize(res, typeof(result));

        }

        public static String ConnectorSendCommand(String command)
        {

            IntPtr pData = Kernel.MarshalUTF8.StringToHGlobalUTF8(command);
            IntPtr pResult = SendCommand(pData);

            String result = Kernel.MarshalUTF8.PtrToStringUTF8(pResult);



            Marshal.FreeHGlobal(pData);
            FreeMemory(pResult);

            return result;

        }

        public void CloseLimitOrder(int transactionid)
        {
            int res = 0;

            command com = command.CreateMoveOrderCommand(transactionid,0,2,0);


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
            int res = 0;

            command com = command.CreateNewOrder(this.ClientId, board, seccode, buysell, bymarket, price, volume);


            result sendResult = ConnectorSendCommand(com, typeof(command));

            if (sendResult.success == true)
            {
                res = sendResult.transactionid;

                //while (_orders.FirstOrDefault(x => x.transactionid == sendResult.transactionid) == null) Application.DoEvents();
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
            if (correction!=0) com.takeprofit.correction = correction;
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

                //while (_orders.FirstOrDefault(x => x.transactionid == sendResult.transactionid) == null) Application.DoEvents();
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
            double slPrice =        (buysell == buysell.B) ? tr.price - slDistance : tr.price + slDistance;
            double tpPrice =        (buysell == buysell.B) ? tr.price + tpDistance : tr.price - tpDistance;
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
        public void NewMarketOrderWithCondOrder(boardsCode board, string seccode, buysell buysell,  int volume, int slDistance)
        {
            bymarket bymarket = Domain.OutputXML.bymarket.yes;

            int tid = NewOrder(board, seccode, buysell, bymarket, 0, volume);

            order ord = null;

            while (ord == null) { Application.DoEvents(); ord = GetOrderByTransactionId(tid); }

            trade tr = null;

            while (tr == null) { Application.DoEvents(); tr = GetTradeByOrderNo(ord.orderno); }

            var condOrderBysell = (buysell==buysell.B)?buysell.S:buysell.B;

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

        #endregion

        #region PRIVATE

        


        protected static string _serializeToString(object commandInfo, Type type)
        {
            string res;

            XmlSerializer xser = new XmlSerializer(type);
            MemoryStream ms = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            xser.Serialize(writer, commandInfo, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);

            res = sr.ReadToEnd();

            return res;
        }

        protected static IntPtr _serializeToIntPtr(object commandInfo, Type type)
        {
            string cmd = _serializeToString(commandInfo, type);

            IntPtr pData = Kernel.MarshalUTF8.StringToHGlobalUTF8(cmd);

            return pData;
        }

        protected static object _deserialize(string data, Type type)
        {
            object res = null;

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = type.Name;
            xRoot.IsNullable = true;

            XmlSerializer xser = new XmlSerializer(type, xRoot);
            StringReader sr = new StringReader(data);
            res = xser.Deserialize(sr);

            return res;
        }

        #endregion


        

        #region TXmlConnector.dll
        //--------------------------------------------------------------------------------
        // файл библиотеки TXmlConnector.dll должен находиться в одной папке с программой

        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.StdCall)]
        static extern bool SetCallback(CallBackDelegate pCallback);
                //для передачи делегата обработчика события обратного вызова, используется в начале работы

                ////Обработчик обратных вызовов
                //delegate bool CallBackDelegate(IntPtr pData);

                //IntPtr - данные обратного вызова
                ////IntPtr -> String
                //String data = Kernel.MarshalUTF8.PtrToStringUTF8(pData);
                ////string -> IntPtr
                //IntPtr pData = Kernel.MarshalUTF8.StringToHGlobalUTF8(command);


                //после получения данных, их нужно очистить с помощью FreeMemory(pData);




        //[DllImport("txmlconnector.dll", CallingConvention = CallingConvention.StdCall)]
        //private static extern bool SetCallbackEx(CallBackExDelegate pCallbackEx, IntPtr userData);

        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr SendCommand(IntPtr pData);
                    //отправка комманды
                    //IntPtr pResult = SendCommand(IntPtr pData);

                    //после отправки комманды необходимо выполнить очистку
                    //Marshal.FreeHGlobal(pData); //очистка памяти от отправляемой команды
                    //FreeMemory(pResult);		//очистка памяти от полученного результата

                    //Возвращаемые значени
                    //        <result success=”true”/>
		
                    //        <result success=”false”>
                    //            <message>error message</message>
                    //        </result>

                    //        <error> Текст сообщения об ошибке</error>




        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.StdCall)]
        static extern bool FreeMemory(IntPtr pData);

        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr Initialize(IntPtr pPath, Int32 logLevel);
                //Выполняет инициализацию библиотеки: запускает поток обработки очереди
                //обратных вызовов, инициализирует систему логирования библиотеки.
                //logPath Путь к директории, в которую будут сохраняться файлы отчетов
                //logLevel Глубина логирования
                //Предусмотрено три уровня логирования, в соответствии с
                //детализацией и размером лог-файла:
                //1 – минимальный
                //2 – стандартный (рекомендуемый);
                //3 – максимальный
                //Данная функция в качестве аргументов принимает путь к папке (const BYTE*
                //logPath), в которой будут созданы лог-файлы (XDF*.log, DSP*.txt, TS*.log), и
                //уровень логирования (int logLevel).
                //logPath должен включать в себя завершающий символ "\" и заканчиваться на
                //терминальный символ «\0». Пример:
                //logPath = "D:\\Logs\\\0";
                //Функция Initialize может быть вызвана в процессе работы с Коннектором
                //повторно для изменения директории и уровня логирования, но только в
                //случае, когда библиотека остановлена, то есть была выполнена команда
                //disconnect или соединение еще не было установлено.


        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr UnInitialize();
                //    Выполняет остановку внутренних потоков библиотеки, в том числе завершает
                //поток обработки очереди обратных вызовов. Останавливает систему
                //логирования библиотеки и закрывает файлы отчетов.


        [DllImport("txmlconnector2.dll", CallingConvention = CallingConvention.Winapi)]
        static extern IntPtr SetLogLevel(Int32 logLevel);
        //-------------------------------------------------------------------------------
        #endregion







        
    }
}
