using AutoTraderSDK.Model;
using AutoTraderSDK.Model.Outgoing;
using System;
using System.Collections.Generic;

namespace AutoTraderSDK.Core
{
    public interface ITXMLConnector: IDisposable
    {
        bool Connected { get; }

        string FortsClientId { get; }

        AutoTraderSDK.Model.Ingoing.positions Positions { get; }

        System.Collections.Generic.List<Model.Ingoing.quotes_ns.quote> QuotesBuy { get; }

        System.Collections.Generic.List<Model.Ingoing.quotes_ns.quote> QuotesSell { get; }


        string Union { get; }

        double Money { get; }

        event EventHandler<OnMCPositionsUpdatedEventArgs> OnMCPositionsUpdated;

        void Login(string username, string password, ConnectionType connectionType);

        void Logout();

        void ChangePassword(string oldpass, string newpass);

        void NewComboOrder(boardsCode board,
                                        string seccode,
                                        buysell buysell,
                                        bool bymarket,
                                        int price,
                                        int volume,
                                        int slDistance,
                                        int tpDistance,
                                        int comboType = 1);

        void NewComboOrder(ComboOrder co);

        int NewConditionOrder(boardsCode board,
                                    string seccode,
                                    Model.Outgoing.buysell buysell,
                                    bool bymarket,
                                    cond_type condtype,
                                    double condvalue,
                                    int volume);

        int NewOrder(boardsCode board, string seccode, buysell buysell, bool bymarket, double price, int volume);

        int NewStopOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0);

        int NewStopOrderWithDistance(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0);



        List<Model.Ingoing.securities_ns.security> GetSecurities();


        void SubscribeQuotes(AutoTraderSDK.Model.Outgoing.boardsCode board, string seccode);

        void SubscribeQuotations(AutoTraderSDK.Model.Outgoing.boardsCode board, string seccode);




        List<Model.Ingoing.candle> GetHistoryData(string seccode, boardsCode board = boardsCode.FUT, int periodId = 1, int candlesCount = 1);
    }
}
