using System;

namespace AutoTraderUI.Controls
{
    public interface IObserversDataGridView
    {
        event EventHandler<ObserversDataGridViewEventArgs> StartEvent;
        event EventHandler<ObserversDataGridViewEventArgs> StopEvent;
        event EventHandler<ObserversDataGridViewEventArgs> ConfigEvent;
        event EventHandler<ObserversDataGridViewEventArgs> LogEvent;
        event EventHandler<ObserversDataGridViewEventArgs> DeleteEvent;

        void Add(string username);
        void LoadObservers(string[] usernames);

        void SwitchStateToStarted(string seccode);
        void SwitchStateToStoped(string seccode);
    }
}
