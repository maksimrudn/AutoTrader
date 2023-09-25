using AutoTraderSDK.Model;
using AutoTraderSDK.Model.Ingoing;
using AutoTraderSDK.Model.Ingoing.quotes_ns;
using AutoTraderSDK.Model.Ingoing.securities_ns;
using AutoTraderSDK.Model.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderSDK.Core
{
    public class TXMLDummyConnector : ITXMLConnector
    {
        public bool Connected { get; private set; } = false;

        public string FortsClientId { get; private set; }

        public positions Positions { get; private set; }

        public List<quote> QuotesBuy { get; private set; }

        public List<quote> QuotesSell { get; private set; }

        public string Union { get; private set; }

        public double Money { get; private set; }

        public event EventHandler<OnMCPositionsUpdatedEventArgs> OnMCPositionsUpdated;

        public void ChangePassword(string oldpass, string newpass)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public List<candle> GetHistoryData(string seccode, boardsCode board = boardsCode.FUT, int periodId = 1, int candlesCount = 1)
        {
            var signal = new List<candle>()
            {
                new candle()
                {
                    close = 96001
                },
                new candle()
                {
                    close = 96005
                }
            };

            var nonsignal =
                new List<candle>()
            {
                new candle()
                {
                    close = 96001
                },
                new candle()
                {
                    close = 96030
                }
            };



            return new Random().Next(0,10) == 0? signal:nonsignal;
        }

        public List<security> GetSecurities()
        {
            return new List<security>()
            {
                new security() { seccode = "siz1" , board = boardsCode.FUT.ToString()}
            };
        }

        public void Login(string username, string password, ConnectionType connectionType)
        {
            Connected = true;
            FortsClientId = "1111";
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void NewComboOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, int price, int volume, int slDistance, int tpDistance, int comboType = 1)
        {
            throw new NotImplementedException();
        }

        public void NewComboOrder(ComboOrder co)
        {
            throw new NotImplementedException();
        }

        public int NewConditionOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, cond_type condtype, double condvalue, int volume)
        {
            throw new NotImplementedException();
        }

        public int NewOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, double price, int volume)
        {
            throw new NotImplementedException();
        }

        public int NewStopOrder(boardsCode board, string seccode, buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0)
        {
            throw new NotImplementedException();
        }

        public int NewStopOrderWithDistance(boardsCode board, string seccode, buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0)
        {
            throw new NotImplementedException();
        }

        public void SubscribeQuotations(boardsCode board, string seccode)
        {
            throw new NotImplementedException();
        }

        public void SubscribeQuotes(boardsCode board, string seccode)
        {
            throw new NotImplementedException();
        }
    }
}
