using AutoTraderSDK.Domain.InputXML;
using AutoTraderSDK.Domain.OutputXML;
using AutoTraderSDK.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraderSDK
{
    class Program
    {
        static string _seccode = "SiU6";

        static void Main(string[] args)
        {
            

            
            //string server = "78.41.194.72";
            //int port = 3939;

            //using (var cl2 = TXMLConnectorWrapper2.GetInstance())
            //{
            using (var cl = TXMLConnectorWrapper.GetInstance())
                {

                //try
                //{

                string demoserver = "tr1-demo5.finam.ru";


                    //cl.NewOrder(boardsCode.FUT, "SiM0", buysell.B, bymarket.yes, 66350, 1);

                    //cl.NewCondOrder(boardsCode.FUT, "SiM6", buysell.S, bymarket.yes, cond_type.Ask, 66400, 1);

                    //cl.CloseOrder(cl.OpenOrders.FirstOrDefault(x => x.status == status.watching).transactionid);

                    //cl.NewComboOrder(boardsCode.FUT, "SiM6", buysell.B, 1, 30, 90);
                    //cl.NewComboOrder(boardsCode.FUT, "SiM6", buysell.S, 1, 30, 90);

                    //while (true)
                    //{
                    //    var position = cl.Positions.forts_position.FirstOrDefault(x => x.seccode == "SiM6");

                    //    if (position != null && position.totalnet == 0)
                    //    {
                    //        List<order> lst = cl.OpenOrders;

                    //        foreach (var order in lst)
                    //        {
                    //            if (order.status == status.watching)
                    //                cl.CloseOrder(order.transactionid);
                    //            else
                    //                cl.CloseOrder(order.transactionid);  //cl.CloseLimitOrder(order.transactionid);
                    //        }
                    //    }
                    //    Application.DoEvents();
                    //}





                    //_testQuotesPair(cl);
                    //_testPositions(cl);

                    //_testComboOrder(cl);


                    //_testStopOrder(cl);

                    //_testCurrentCandle(cl);

                    //_testNewOrderWithCondOrder(cl);

                    //_testPositions(cl);

                    //_newComboOrder(cl);

                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show(e.Message);
                    //}
                }

            //}


        }

       

        private static void _newComboOrder(TXMLConnectorWrapper cl)
        {
            using (Trader tr = cl.CreateTrader(boardsCode.FUT, "SiH6"))
            {

                //tr.NewOrder(buysell.S, bymarket.yes, 0, 1);
                //Console.WriteLine("Open");

                //while (true)
                //{
                //    Console.WriteLine("{0}", tr.OpenPositions);


                //}
                tr.NewComboOrder(buysell.B, 1, 50, 100);
                
                Console.WriteLine("Open");

                while (true)
                {
                    Console.WriteLine("{0}", tr.OpenPositions);


                }

            }
        }

        private static void _testPositions(TXMLConnectorWrapper cl)
        {
            while (true)
            {
                Console.WriteLine(cl.GetOpenPositions("SiU6"));
            }
        }

        //private static void _testPositions(TXMLConnectorWrapper cl)
        //{
        //    using (Trader tr = cl.CreateTrader(boardsCode.FUT, "SiH6"))
        //    {

                

        //        while (true)
        //        {
        //            Application.DoEvents();


        //        }

        //    }
        //}

        private static void _testNewOrderWithCondOrder(TXMLConnectorWrapper cl)
        {
            using (Trader tr = cl.CreateTrader(boardsCode.FUT, "SiH6"))
            {

                //tr.NewOrder(buysell.S, bymarket.yes, 0, 1);
                //Console.WriteLine("Open");

                //while (true)
                //{
                //    Console.WriteLine("{0}", tr.OpenPositions);


                //}

                tr.NewMarketOrderWithCondOrder(buysell.S,  1, 500);
                Console.WriteLine("Open");

                while (true)
                {
                    Console.WriteLine("{0}", tr.OpenPositions);


                }

            }
        }

        private static void _testCurrentCandle(TXMLConnectorWrapper cl)
        {
            for (int i = 0; i < 50; i++)
            {
                var c = cl.GetCurrentCandle("SiH6");

                Console.WriteLine(string.Format("Open: {0}, Close: {1}, High: {2}, Low: {3}", c.open, c.close, c.high, c.low));

                Thread.Sleep(1);
            }
        }

        private static void _testStopOrder(TXMLConnectorWrapper cl)
        {
            cl.NewStopOrder(boardsCode.FUT, "SiH6", AutoTraderSDK.Domain.OutputXML.buysell.S, 74400, 73400, 1);
        }

        private static void _testComboOrder(TXMLConnectorWrapper cl)
        {
            //int tid = cl.NewOrder("FUT", "SiH6", AutoTraderSDK.Domain.OutputXML.buysell.B, Domain.OutputXML.bymarket.yes, 73400, 1);

            //order ord = null;

            //while (ord == null) ord = cl.GetOrderByTransactionId(tid);

            //trade tr = null;

            //while (tr == null) tr = cl.GetTradeByOrderNo(ord.orderno);

            //cl.NewCondOrder("FUT", "SiH6", buysell.S, bymarket.yes, cond_type.BidOrLast, ord.price - 200, 1);

            cl.NewMarketOrderWithCondOrder(boardsCode.FUT, "SiM6", buysell.S,   1, 5);
        }

        public enum Status
        {
            onstart,
            open,
            progress,
            close
        }

        private static void _testQuotesPair(TXMLConnectorWrapper cl)
        {
            cl.Subscribe(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, _seccode);

            double last = 0;
            int pos = 0;
            DateTime dt1 = new DateTime();

            Status status = Status.onstart;

            while (true)
            {
                Application.DoEvents();

                

                //if (cl.QuotesBuy.Count > 0 && cl.QuotesSell.Count > 0)
                //{
                    //var qS = cl.QuotesSell[0];
                    //var qB = cl.QuotesBuy[0];

                    int positions = cl.GetOpenPositions(_seccode);

                    //if (positions == 0 && cl.OpenOrders.Count == 2) status = Status.open;
                    //else if (positions == 0 && cl.OpenOrders.Count == 0 && status == Status.open || status == Status.progress) status = Status.close;
                    //else if (positions != 0 && cl.OpenOrders.Count != 0 && status == Status.open) status = Status.progress;
                    //else if (positions == 0 && cl.OpenOrders.Count == 0 && status == Status.progress) status = Status.close;

                    if (positions == 0 && cl.OpenOrders.Count == 0 && status == Status.open
                        && (DateTime.Now - dt1).Seconds >3 )
                            status = Status.close;



                    double bid = cl.Bid;
                    double ask = cl.Ask;
                
                    if (ask - bid>= 4 && (status == Status.onstart || status == Status.close) )
                    {
                        double sPrice = ask - 1;
                        double bPrice = bid + 1;

                        //cl.NewOrder("FUT", "SiH6", AutoTraderSDK.Domain.OutputXML.buysell.B, Domain.OutputXML.bymarket.no, bPrice, 1);
                        //cl.NewOrder("FUT", "SiH6", AutoTraderSDK.Domain.OutputXML.buysell.S, Domain.OutputXML.bymarket.no, sPrice, 1);

                        Task t1 = Task.Factory.StartNew(() =>
                            {
                                cl.NewOrder(boardsCode.FUT, _seccode, AutoTraderSDK.Domain.OutputXML.buysell.B, Domain.OutputXML.bymarket.no, bPrice, 1);
                            });

                        Task t2 = Task.Factory.StartNew(() =>
                            {
                                cl.NewOrder(boardsCode.FUT, _seccode, AutoTraderSDK.Domain.OutputXML.buysell.S, Domain.OutputXML.bymarket.no, sPrice, 1);
                            });

                        cl.PositionsIsActual = false;
                        pos = 1;
                        status = Status.open;
                        

                        while (t1.IsCompleted == false || t2.IsCompleted == false) Application.DoEvents();

                        Console.WriteLine("BUY: {0}, SELL: {1}", bPrice, sPrice);
                        //Console.WriteLine("{0}", ask - bid);

                        Thread.Sleep(1000);
                        //break;
                    }
                    else if (ask - bid != last)
                    {
                        Console.WriteLine("{0}", ask-bid);
                        last = ask - bid;
                    }
                //}

                
            } 
        }
    }
}
