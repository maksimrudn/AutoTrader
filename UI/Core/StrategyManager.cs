using AutoTraderSDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public class StrategyManager
    {
        Settings _settings;
        List<ITXMLConnector> _connectors;
        public StrategyManager(Settings settings, List<ITXMLConnector> connectors)
        {
            _settings = settings;
            _connectors = connectors;

            StrategyWorkers = settings.StrategiesCollection
                                        .StrategyList
                                        .Select(x => { 
                                            return new StrategyWorker(x, _connectors, _settings.StrategiesCollection.NotificationFilename); 
                                        }).ToList();
        }


        public List<StrategyWorker> StrategyWorkers { get; set; }

        public void Add(StrategySettings observerSettings)
        {
            _settings.StrategiesCollection.StrategyList.Add(observerSettings);

            if (ObserverListChanged != null) ObserverListChanged.Invoke(this, _settings.StrategiesCollection.StrategyList);
        }

        public void Remove(StrategySettings observerSettings)
        {
            _settings.StrategiesCollection.StrategyList.Add(observerSettings);
        }

        public event EventHandler<List<StrategySettings>> ObserverListChanged;
    }
}
