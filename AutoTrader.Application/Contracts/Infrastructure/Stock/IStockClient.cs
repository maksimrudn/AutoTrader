using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Domain.Models;
using AutoTrader.Domain.Models.Types;
using System;
using System.Collections.Generic;

namespace AutoTrader.Application.Contracts.Infrastructure.Stock
{
    public interface IStockClient : IAsyncDisposable
    {
        bool Connected { get; }

        string? FortsClientId { get; }

        Models.TransaqConnector.Ingoing.positions? Positions { get; }

        List<Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesBuy { get; }

        List<Models.TransaqConnector.Ingoing.quotes_ns.quote>? QuotesSell { get; }

        string? Union { get; }

        double? Money { get; }

        event EventHandler<TransaqEventArgs<mc_portfolio>> MCPositionsUpdated;

        event EventHandler<TransaqEventArgs<HashSet<Models.TransaqConnector.Ingoing.securities_ns.security>?>> SecuritiesUpdated;

        HashSet<Models.TransaqConnector.Ingoing.securities_ns.security>? Securities { get; }


        Task Login(string username, 
                    string password, 
                    ConnectionType connectionType);

        Task Logout();

        void ChangePassword(string oldpass, string newpass);

        Task CreateNewComboOrder(TradingMode tradingMode,
                                        string seccode,
                                        OrderDirection orderDirection,
                                        bool bymarket,
                                        int price,
                                        int volume,
                                        int slDistance,
                                        int tpDistance,
                                        int comboType = 1);

        Task CreateNewComboOrder(ComboOrder co);

        int CreateNewConditionOrder(TradingMode tradingMode,
                                    string seccode,
                                    OrderDirection orderDirection,
                                    bool bymarket,
                                    Models.TransaqConnector.Outgoing.cond_type condtype,
                                    double condvalue,
                                    int volume);

        int CreateNewOrder(TradingMode tradingMode, 
                        string seccode,
                        OrderDirection orderDirection,
                        bool bymarket, 
                        double price, 
                        int volume);

        int CreateNewStopOrder(TradingMode tradingMode, 
                            string seccode,
                            OrderDirection orderDirection,
                            double SLPrice, 
                            double TPPrice, 
                            int volume, 
                            long orderno = 0, 
                            double correction = 0);

        int CreateNewStopOrderWithDistance(TradingMode tradingMode, 
                                        string seccode,
                                        OrderDirection orderDirection, 
                                        double price, 
                                        double SLDistance, 
                                        double TPDistance, 
                                        int volume, 
                                        long orderno = 0);

        Task<List<Models.TransaqConnector.Ingoing.securities_ns.security>?> GetSecurities();

        void SubscribeQuotes(TradingMode tradingMode, string seccode);

        void SubscribeQuotations(TradingMode tradingMode, string seccode);

        Task<List<candle>?> GetHistoryData(string seccode,
                                            TradingMode tradingMode = TradingMode.Futures, 
                                            SecurityPeriods periodId = SecurityPeriods.M1, 
                                            int candlesCount = 1);

        int GetOpenPositions(string seccode);

        List<Application.Models.TransaqConnector.Ingoing.orders_ns.order>? OpenOrders { get; }
    }
}
