using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Models;
using AutoTrader.Domain.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader.Application.Services
{
    public class StrategyManager
    {
        Settings _settings;
        private readonly ISettingsService _settingsService;
        IDoubleStockClient _connectors;
        private readonly IEmailService _emailService;

        public StrategyManager(ISettingsService settingsService, IDoubleStockClient connectors, IEmailService emailService)
        {
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            _connectors = connectors;
            _emailService = emailService;
            StrategyWorkers = _settings.StrategiesCollection
                                        .StrategyList
                                        .Select(strategySettings =>
                                        {
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
