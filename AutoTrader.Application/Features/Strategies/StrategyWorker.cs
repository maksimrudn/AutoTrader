using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models;
using AutoTrader.Domain.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTrader.Application.Features.Strategies
{
    public class StrategyWorker
    {
        StrategySettings _settings;
        List<IStockClient> _connectors;
        IEmailService _emailService;
        string _notificationFile;
        string _notificationEmail;

        /// <summary>
        /// msk 3
        /// hcmc 7
        /// </summary>
        int? _timezone = null;

        public StrategyWorker(StrategySettings settings, List<IStockClient> connectors, IEmailService emailService, string notificationFile, string notificationEmail)
        {
            _settings = settings;
            _connectors = connectors;
            this._emailService = emailService;
            _notificationFile = notificationFile;
            _notificationEmail = notificationEmail;
        }

        CancellationTokenSource _cts;
        Task _strategyTask;
        public void Start(int? timezone = null)
        {
            Trace.TraceInformation($"Strategy start {_settings.Seccode}: begin");

            _timezone = timezone;

            try
            {
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

        private async Task _strategyFunction()
        {
            DateTime lastSignalTime = DateTime.Now.AddDays(-1);

            while (true)
            {
                // strategy works only stock exchange works (9:00-23.50)
                if (_timezone != null)
                {
                    // Get the current time
                    DateTime currentTime = DateTime.Now;

                    if (_timezone == 7) currentTime.AddHours(-4);

                    // Set the start and end times for the range
                    TimeSpan startTime = new TimeSpan(9, 0, 0); // 9:00 AM
                    TimeSpan endTime = new TimeSpan(23, 50, 0); // 11:50 PM
                                                                // Check if the current time is within the range
                    if (!(currentTime.TimeOfDay >= startTime && currentTime.TimeOfDay <= endTime))
                    {
                        await Task.Delay(_settings.Delay);
                        continue;
                    }
                }

                var res = await _connectors[0].GetHistoryData(_settings.Seccode, TradingMode.Futures, _settings.Period, 2);

                double prevPrice = res[0].close;
                double curPrice = res[1].close;

                double diff = prevPrice - curPrice;

                // Signal must be send only once at minute. Initial value lastSignalTime NOW - 1D
                if ((DateTime.Now - lastSignalTime).TotalMinutes < 1)
                {
                    await Task.Delay(_settings.Delay);
                    continue;
                }

                if (_settings.DifferenceType == DifferenceTypes.Oversold)
                {
                    if (diff < 0) diff = diff * -1;

                    if (diff > _settings.Difference && prevPrice > curPrice)
                    {
                        await _sendNotification(_settings, diff, res);

                        lastSignalTime = DateTime.Now;
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

        private async Task _sendNotification(StrategySettings settings, double diff, List<Models.TXMLConnector.Ingoing.candle> historyData)
        {
            string result = $"Time {DateTime.Now}; Seccode {settings.Seccode}; Period {settings.Period.ToString()}; Diff = {diff}; Prev close = {historyData[0].close}; Prev close = {historyData[1].close}; Signal {settings.DifferenceType};\n";

            if (_settings.NotificationType == NotificationTypes.File)
            {
                File.AppendAllText(_notificationFile, result);
            }
            else if (_settings.NotificationType == NotificationTypes.Email)
            {
                await _emailService.SendEmailAsync(_notificationEmail, "Autotrader", result);
            }
            else
            {
                File.AppendAllText(_notificationFile, result);
                await _emailService.SendEmailAsync(_notificationEmail, "Autotrader", result);
            }
        }

        public void Stop()
        {
            Trace.TraceInformation($"Strategy stop {_settings.Seccode}: begin");

            _cts.Cancel();
        }
    }
}
