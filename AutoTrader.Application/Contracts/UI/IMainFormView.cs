using AutoTrader.Application.Enums;
using AutoTrader.Application.Models;

namespace AutoTrader.Application.Contracts.UI
{
    public interface IMainFormView
    {
        string ComboBoxConnectionType { get; }
        string ComboBoxSeccode { get; }
        string Username1 { get; }
        string Password1 { get; }
        string ClientId1 { set; }
        string FreeMoney { set; }
        string FreeMoney1 { set; }
        string FreeMoney2 { set; }
        double ObserveDifference { get;  }
        string Union1 { set; }
        void LoadSettings(Settings settings);
        void ShowMessage(string msg);
        void LoadSeccodeList(List<string> list);
        void SetSelectedSeccode(string seccode);
        void HandleConnected(StockUserConnectionTypes connectionType);
        void HandleDisconnected(StockUserConnectionTypes? connectionType);
        void UpdateSettings(Settings settings);
        void FreezUi();
        void UnFreezUi();
    }
}
