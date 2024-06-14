using AutoTrader.Domain.Models;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.quotes_ns;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.securities_ns;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Models;
using AutoTrader.Domain.Models.Types;

namespace AutoTrader.Infrastructure.Stock.Dummy
{
    public class DummyStockClient : IStockClient
    {
        public bool Connected { get; private set; } = false;

        public string FortsClientId { get; private set; }

        public positions Positions { get; private set; }

        public List<quote> QuotesBuy { get; private set; }

        public List<quote> QuotesSell { get; private set; }

        public string Union { get; private set; }

        public double Money { get; private set; }

        public HashSet<security> Securities => throw new NotImplementedException();

        public event EventHandler<TransaqEventArgs<HashSet<security>>> SecuritiesUpdated;

        public event EventHandler<TransaqEventArgs<mc_portfolio>> MCPositionsUpdated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void ChangePassword(string oldpass, string newpass)
        {
            throw new NotImplementedException();
        }

        public async new ValueTask DisposeAsync()
        {

        }

        public async Task<List<candle>> GetHistoryData(string seccode, TradingMode tradingMode = TradingMode.Futures, SecurityPeriods periodId = SecurityPeriods.M1, int candlesCount = 1)
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

            return new Random().Next(0, 10) == 0 ? signal : nonsignal;
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

        public async Task CreateNewComboOrder(TradingMode tradingMode, string seccode, OrderDirection orderDirection, bool bymarket, int price, int volume, int slDistance, int tpDistance, int comboType = 1)
        {
            throw new NotImplementedException();
        }

        public async Task CreateNewComboOrder(ComboOrder co)
        {
            throw new NotImplementedException();
        }

        public int CreateNewConditionOrder(TradingMode tradingMode, string seccode, OrderDirection orderDirection, bool bymarket, cond_type condtype, double condvalue, int volume)
        {
            throw new NotImplementedException();
        }

        public int CreateNewOrder(TradingMode tradingMode, string seccode, OrderDirection orderDirection, bool bymarket, double price, int volume)
        {
            throw new NotImplementedException();
        }

        public int CreateNewStopOrder(TradingMode tradingModes, string seccode, OrderDirection orderDirection, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0)
        {
            throw new NotImplementedException();
        }

        public int CreateNewStopOrderWithDistance(TradingMode tradingMode, string seccode, OrderDirection orderDirection, double price, double SLDistance, double TPDistance, int volume, long orderno = 0)
        {
            throw new NotImplementedException();
        }

        public void SubscribeQuotations(TradingMode tradingMode, string seccode)
        {
            throw new NotImplementedException();
        }

        public void SubscribeQuotes(TradingMode tradingMode, string seccode)
        {
            throw new NotImplementedException();
        }
    }
}
