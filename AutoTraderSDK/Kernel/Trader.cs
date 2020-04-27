using AutoTraderSDK.Domain.InputXML;
using AutoTraderSDK.Domain.OutputXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraderSDK.Kernel
{
    public class Trader:IDisposable
    {
        private TXMLConnectorWrapper _cl = null;

        public Trader(TXMLConnectorWrapper cl, boardsCode board, string seccode)
        {
            Board = board;
            Seccode = seccode;
            _cl = cl;

            _cancelSource = new CancellationTokenSource();
            _cancelToken = _cancelSource.Token;
            _positionsWatcherTask = Task.Factory.StartNew(_positionsWatcherFunc);
        }

        CancellationTokenSource _cancelSource;
        CancellationToken _cancelToken;
        Task _positionsWatcherTask = null;
        void _positionsWatcherFunc()
        {
            while (true)
            {
                if (_cancelToken.IsCancellationRequested) break;

                _cl.GetFortsPositions(Board, Seccode);

                Thread.Sleep(100);

                Application.DoEvents();
            }
        }


        public void Dispose()
        {
            _cancelSource.Cancel();
        }


        public candle CurrentCandle { get { return _cl.GetCurrentCandle(Seccode); } }

        public string ClientId { get { return _cl.ClientId; } }

        public boardsCode Board { get; private set; }

        public string Seccode { get; private set; }

        public int? OpenPositions { 
            get {
                int? res = null;

                try
                {
                    res = _cl.Positions.forts_position.Where(x => x.client == ClientId &&
                                                                x.seccode == Seccode).ToList()[0].totalnet;
                }
                catch { }

                return res;
            } 
        }

        public double? VarMargin
        {
            get
            {
                double? res = null;

                try
                {
                    res = _cl.Positions.forts_position.Where(x => x.client == ClientId &&
                                                                x.seccode == Seccode).ToList()[0].varmargin;
                }
                catch { }

                return res;
            }
        }

        public List<order> OpenLimitOrdersBuy;

        public List<order> OpenLimitOrdersSell;

        public List<quote> QuotesSell { get { return _cl.QuotesSell; } }

        public List<quote> QuotesBuy { get { return _cl.QuotesBuy; } }


        public int NewOrder( buysell buysell, bymarket bymarket, double price, int volume)
        {
            return _cl.NewOrder(Board, Seccode, buysell, bymarket, price, volume);
        }

        public void NewMarketOrderWithCondOrder(buysell buysell, int volume, int slDistance)
        {
            _cl.NewMarketOrderWithCondOrder(Board, Seccode, buysell,  volume, slDistance);
        }

        //public abstract void NewLimitOrder();

        //public abstract void NewStopOrder();

        //public abstract void NewCondOrder();

        //public abstract void CancelOrder();

        //public abstract void CancelStopOrder();

        //public abstract void MoveOrder();



        public void NewComboOrder(buysell buysell, int volume, int slDistance, int tpDistance)
        {
            _cl.NewComboOrder(Board, Seccode, buysell, volume, slDistance, tpDistance);
        }
    }
}
