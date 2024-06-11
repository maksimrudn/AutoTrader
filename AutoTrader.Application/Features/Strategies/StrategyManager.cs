using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.TXMLConnector;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Domain.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.Strategies
{
    public class StrategyManager
    {
        AutoTrader.Application.Features.Settings.Settings _settings;
        private readonly IAppSettingsService _appSettingsService;
        List<ITXMLConnector> _connectors;
        private readonly IEmailService _emailService;

        public StrategyManager(IAppSettingsService appSettingsService, List<ITXMLConnector> connectors, IEmailService emailService)
        {
            _appSettingsService = appSettingsService;
            _settings = _appSettingsService.GetSettings();
            _connectors = connectors;
            this._emailService = emailService;
            StrategyWorkers = _settings.StrategiesCollection
                                        .StrategyList
                                        .Select(strategySettings => { 
                                            return new StrategyWorker(
                                                        strategySettings, 
                                                        _connectors, 
                                                        _emailService,
                                                        _settings.StrategiesCollection.NotificationFilename, 
                                                        _settings.StrategiesCollection.NotificationEmail); 
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
