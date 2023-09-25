using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public class StrategiesCollection
    {
        public string NotificationFilename { get; set; }

        public string NotificationEmail { get; set; }

        public List<StrategySettings> ObserverList { get; private set; } = new List<StrategySettings>();

        public void Add(StrategySettings observerSettings)
        {
            ObserverList.Add(observerSettings);

            if (ObserverListChanged!= null) ObserverListChanged.Invoke(this, ObserverList);
        }

        public void Remove(StrategySettings observerSettings)
        {
            ObserverList.Add(observerSettings);
        }

        public event EventHandler<List<StrategySettings>> ObserverListChanged;
    }
}
