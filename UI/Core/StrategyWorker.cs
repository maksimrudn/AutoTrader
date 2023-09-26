using AutoTraderSDK.Core;
using AutoTraderSDK.Model.Outgoing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTraderUI.Core
{
    public class StrategyWorker
    {
        StrategySettings _settings;
        List<ITXMLConnector> _connectors;
        string _notificationFile;
        public StrategyWorker(StrategySettings settings, List<ITXMLConnector> connectors, string notificationFile)
        {
            _settings = settings;
            _connectors = connectors;
            _notificationFile = notificationFile;
        }

        CancellationTokenSource _cts;
        Task _strategyTask;
        public void Start()
        {
            Trace.TraceInformation($"Strategy start {_settings.Seccode}: begin");


            if (_strategyTask != null && _strategyTask.Status == TaskStatus.Running) _cts.Cancel();


            if (string.IsNullOrEmpty(_settings.Seccode)) throw new Exception("seccode...");

            _cts = new CancellationTokenSource();

            _strategyTask = Task.Run(_strategyFunction, _cts.Token);

            Trace.TraceInformation($"Strategy start {_settings.Seccode}: completed");
        }

        private async void _strategyFunction()
        {
            while (true)
            {
                var res = _connectors[0].GetHistoryData(_settings.Seccode, boardsCode.FUT, _settings.Period, 2);

                double prevPrice = res[0].close;
                double curPrice = res[1].close;

                double diff = prevPrice - curPrice;

                if (_settings.DifferenceType == DifferenceTypes.Oversold)
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference && prevPrice > curPrice)
                    {
                        if (_settings.NotificationType == NotificationTypes.File)
                        {
                            File.AppendAllText(_notificationFile, $"time {DateTime.Now}; diff = {diff}; prev close = {res[0].close}; prev close = {res[1].close}; ");
                        }
                    }
                }
                if (_settings.DifferenceType == DifferenceTypes.Overbought)
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference && prevPrice > curPrice)
                    {
                        if (_settings.NotificationType == NotificationTypes.File)
                        {
                            File.AppendAllText(_notificationFile, $"time {DateTime.Now}; diff = {diff}; prev close = {res[0].close}; prev close = {res[1].close}; ");
                        }
                    }
                }
                else
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference)
                    {
                        if (_settings.NotificationType == NotificationTypes.File)
                        {
                            File.AppendAllText(_notificationFile, $"time {DateTime.Now}; diff = {diff}; prev close = {res[0].close}; prev close = {res[1].close}; ");
                        }
                    }
                }



                if (_cts.Token.IsCancellationRequested)
                {
                    Trace.TraceInformation($"Strategy stop {_settings.Seccode}: begin");
                    return;
                }

                await Task.Delay(_settings.Delay);
            }
        }

        public void Stop()
        {
            Trace.TraceInformation($"Strategy stop {_settings.Seccode}: begin");

            _cts.Cancel();
        }
    }
}
