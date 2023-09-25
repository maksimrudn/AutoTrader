using AutoTraderSDK.Model.Ingoing;
using AutoTraderUI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Common
{
    public interface IMainFormView : IView
    {
        event Action Login1;
        event Action Logout1;
        event Action ChangePassword1;
        event Action Login2;
        event Action Logout2;
        event Action ChangePassword2;
        event Action ComboBuy;
        event Action ComboSell;
        event Action MakeMultidirect;
        event Action StartMakeMultidirectByTimer;
        event Action OnClose;
        event Action GetUnion;
        event Action SubscribeOnQuotations;
        event Action UnSubscribeOnQuotations;
        event Action Observe;
        event Action Test;

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
        void LoadPositions(mc_portfolio portfolio);
    }
}
