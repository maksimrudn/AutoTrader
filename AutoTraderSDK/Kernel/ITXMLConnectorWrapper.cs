using System;
namespace AutoTraderSDK.Kernel
{
    interface ITXMLConnectorWrapper
    {
        string ClientId { get; }
        AutoTraderSDK.Domain.InputXML.result ConnectorSendCommand(object commandInfo, Type type);
        Trader CreateTrader(string board, string seccode);
        void Dispose();
        AutoTraderSDK.Domain.InputXML.candle GetCurrentCandle(string seccode);
        void GetFortsPositions(string board, string seccode);
        AutoTraderSDK.Domain.InputXML.order GetOrderByTransactionId(int transactionid);
        AutoTraderSDK.Domain.InputXML.trade GetTradeByOrderNo(long orderno);
        bool IsConnected { get; }
        void Login(string username, string password);
        void Logout();
        void NewComboOrder(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, int volume, int slDistance, int tpDistance);
        int NewCondOrder(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, AutoTraderSDK.Domain.OutputXML.bymarket bymarket, AutoTraderSDK.Domain.OutputXML.cond_type condtype, double condvalue, int volume);
        void NewMarketOrderWithCondOrder(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, int volume, int slDistance);
        int NewOrder(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, AutoTraderSDK.Domain.OutputXML.bymarket bymarket, double price, int volume);
        int NewStopOrder(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, double SLPrice, double TPPrice, int volume, long orderno = 0, double correction = 0);
        int NewStopOrderWithDistance(string board, string seccode, AutoTraderSDK.Domain.OutputXML.buysell buysell, double price, double SLDistance, double TPDistance, int volume, long orderno = 0);
        AutoTraderSDK.Domain.InputXML.positions Positions { get; }
        System.Collections.Generic.List<AutoTraderSDK.Domain.InputXML.quote> QuotesBuy { get; }
        System.Collections.Generic.List<AutoTraderSDK.Domain.InputXML.quote> QuotesSell { get; }
        void Subscribe(string board, string seccode);
    }
}
