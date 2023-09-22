using AutoTraderSDK.Model.Outgoing;
using System;
namespace AutoTraderSDK.Core
{
    public interface ITXMLConnector
    {
        bool IsConnected { get; }

        string ClientId { get; }

        AutoTraderSDK.Model.Ingoing.positions Positions { get; }

        System.Collections.Generic.List<Model.Ingoing.quotes_ns.quote> QuotesBuy { get; }

        System.Collections.Generic.List<Model.Ingoing.quotes_ns.quote> QuotesSell { get; }

        //AutoTraderSDK.Domain.InputXML.candle GetCurrentCandle(string seccode);
        //void GetFortsPositions(string board, string seccode);
        //AutoTraderSDK.Domain.InputXML.order GetOrderByTransactionId(int transactionid);
        //AutoTraderSDK.Domain.InputXML.trade GetTradeByOrderNo(long orderno);

        void Login(string username, string password, string server = "tr1.finam.ru", int port = 3900);

        void Logout();

        void ChangePassword(string oldpass, string newpass);

        void NewComboOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, int volume, int slDistance, int tpDistance);

        int NewCondOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, AutoTraderSDK.Model.Outgoing.bymarket bymarket, AutoTraderSDK.Model.Outgoing.cond_type condtype, double condvalue, int volume);

        void NewMarketOrderWithCondOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, int volume, int slDistance);

        int NewOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, AutoTraderSDK.Model.Outgoing.bymarket bymarket, double price, int volume);

        int NewStopOrder(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0);

        int NewStopOrderWithDistance(boardsCode board, string seccode, AutoTraderSDK.Model.Outgoing.buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0);
        


        void Subscribe(boardsCode board, string seccode);
    }
}
