using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //string ComboBoxConnectionType2 { get; }

        void LoadSettings(Settings settings);

        void ShowMessage(string msg);
        void LoadSeccodeList(List<string> list);
        void SetSelectedSeccode(string seccode);
        void HandleConnected(int conectorNumber);
        void HandleDisconnected(int connNumber);
        void UpdateSettings(Settings settings);
        void FreezUI();
        void UnFreezUI();
    }
}
