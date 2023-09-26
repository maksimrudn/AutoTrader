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
        string _notificationEmail;

        public StrategyWorker(StrategySettings settings, List<ITXMLConnector> connectors, string notificationFile, string notificationEmail)
        {
            _settings = settings;
            _connectors = connectors;
            _notificationFile = notificationFile;
            _notificationEmail = notificationEmail;
        }

        CancellationTokenSource _cts;
        Task _strategyTask;
        public void Start()
        {
            Trace.TraceInformation($"Strategy start {_settings.Seccode}: begin");

            try
            {
                if (_strategyTask != null && _strategyTask.Status == TaskStatus.Running) _cts.Cancel();


                if (string.IsNullOrEmpty(_settings.Seccode)) throw new Exception("Seccode is not specified");

                _cts = new CancellationTokenSource();

                _strategyTask = Task.Run(_strategyFunction, _cts.Token);

                Trace.TraceInformation($"Strategy start {_settings.Seccode}: completed");
            }
            catch (Exception ex)
            {
                Trace.TraceInformation($"Strategy start {_settings.Seccode}: exception {ex.Message}");
            }
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
                        await _sendNotification(_settings, diff, res);
                        
                    }
                }
                if (_settings.DifferenceType == DifferenceTypes.Overbought)
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference && prevPrice > curPrice)
                    {
                        await _sendNotification(_settings, diff, res);
                    }
                }
                else
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference)
                    {
                        await _sendNotification(_settings, diff, res);
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

        private async Task _sendNotification(StrategySettings settings, double diff, List<AutoTraderSDK.Model.Ingoing.candle> historyData)
        {

            string result =  $"Time {DateTime.Now}; Seccode {settings.Seccode}; Diff = {diff}; Prev close = {historyData[0].close}; Prev close = {historyData[1].close}; Signal {settings.DifferenceType}\n";
            
            if (_settings.NotificationType == NotificationTypes.File)
            {
                File.AppendAllText(_notificationFile, result);
            }
            else if (_settings.NotificationType == NotificationTypes.Email){
                await EmailService.SendEmailAsync(_notificationEmail, "Autotrader", result);
            }
            else
            {
                File.AppendAllText(_notificationFile, result);
                await EmailService.SendEmailAsync(_notificationEmail, "Autotrader", result);
            }
        }

        public void Stop()
        {
            Trace.TraceInformation($"Strategy stop {_settings.Seccode}: begin");

            _cts.Cancel();
        }
    }
}
