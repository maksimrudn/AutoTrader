using AutoTrader.Domain.Models.Strategies;
using System;

namespace AutoTraderUI.Controls
{
    public class ObserversDataGridViewEventArgs: EventArgs
    {
        public StrategiesActions Action { get; set; }
        public string Security { get; set; }
    }
}
