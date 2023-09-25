using AutoTraderUI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTraderUI.Controls
{
    public class ObserversDataGridViewEventArgs: EventArgs
    {
        public StrategiesActions Action { get; set; }
        public string Security { get; set; }
    }
}
