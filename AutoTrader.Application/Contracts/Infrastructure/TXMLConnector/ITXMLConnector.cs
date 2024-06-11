using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using AutoTrader.Domain.Models;
using System;
using System.Collections.Generic;

namespace AutoTrader.Application.Contracts.Infrastructure.TXMLConnector
{
    public interface ITXMLConnector : IAsyncDisposable
    {
        bool Connected { get; }

        string FortsClientId { get; }

        Models.TXMLConnector.Ingoing.positions Positions { get; }

        List<Models.TXMLConnector.Ingoing.quotes_ns.quote> QuotesBuy { get; }

        List<Models.TXMLConnector.Ingoing.quotes_ns.quote> QuotesSell { get; }


        string Union { get; }

        double Money { get; }

        event EventHandler<OnMCPositionsUpdatedEventArgs> OnMCPositionsUpdated;

        Task Login(string username, string password, ConnectionType connectionType);

        void Logout();

        void ChangePassword(string oldpass, string newpass);

        Task NewComboOrder(boardsCode board,
                                        string seccode,
                                        buysell buysell,
                                        bool bymarket,
                                        int price,
                                        int volume,
                                        int slDistance,
                                        int tpDistance,
                                        int comboType = 1);

        Task NewComboOrder(ComboOrder co);

        int NewConditionOrder(boardsCode board,
                                    string seccode,
                                    buysell buysell,
                                    bool bymarket,
                                    Models.TXMLConnector.Outgoing.cond_type condtype,
                                    double condvalue,
                                    int volume);

        int NewOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, double price, int volume);

        int NewStopOrder(boardsCode board, string seccode, buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0);

        int NewStopOrderWithDistance(boardsCode board, string seccode, buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0);



        Task<List<Models.TXMLConnector.Ingoing.securities_ns.security>> GetSecurities();


        void SubscribeQuotes(boardsCode board, string seccode);

        void SubscribeQuotations(boardsCode board, string seccode);




        Task<List<candle>> GetHistoryData(string seccode, boardsCode board = boardsCode.FUT, SecurityPeriods periodId = SecurityPeriods.M1, int candlesCount = 1);
    }
}
