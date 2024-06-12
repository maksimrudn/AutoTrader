using AutoTrader.Domain.Models;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Application.Models.TXMLConnector.Ingoing.quotes_ns;
using AutoTrader.Application.Models.TXMLConnector.Ingoing.securities_ns;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Models;

namespace AutoTrader.Infrastructure.Stock
{
    public class TXMLDummyConnector : IStockClient
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

        public async new ValueTask DisposeAsync()
        {
            
        }

        public async Task<List<candle>> GetHistoryData(string seccode, boardsCode board = boardsCode.FUT, SecurityPeriods periodId = SecurityPeriods.M1 , int candlesCount = 1)
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
                    close = 96130
                }
            };

            return new Random().Next(0,10) == 0? signal:nonsignal;
        }

        public async Task<List<security>> GetSecurities()
        {
            return new List<security>()
            {
                new security() { seccode = "siz1" , board = boardsCode.FUT.ToString()}
            };
        }

        public async Task Login(string username, string password, ConnectionType connectionType)
        {
            Connected = true;
            FortsClientId = "1111";
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task CreateNewComboOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, int price, int volume, int slDistance, int tpDistance, int comboType = 1)
        {
            throw new NotImplementedException();
        }

        public async Task CreateNewComboOrder(ComboOrder co)
        {
            throw new NotImplementedException();
        }

        public int CreateNewConditionOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, cond_type condtype, double condvalue, int volume)
        {
            throw new NotImplementedException();
        }

        public int CreateNewOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, double price, int volume)
        {
            throw new NotImplementedException();
        }

        public int CreateNewStopOrder(boardsCode board, string seccode, buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0)
        {
            throw new NotImplementedException();
        }

        public int CreateNewStopOrderWithDistance(boardsCode board, string seccode, buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0)
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
