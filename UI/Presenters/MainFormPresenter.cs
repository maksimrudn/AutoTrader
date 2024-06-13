
using AutoTraderSDK.Core;
using AutoTraderUI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.IO;
using AutoTraderUI.Views;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Domain.Models.Strategies;
using AutoTrader.Infrastructure;
using AutoTrader.Domain.Models;
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Models.TXMLConnector.Outgoing;
using AutoTrader.Application.Models;
using AutoTrader.Domain.Models.Types;
using MediatR;
using AutoTrader.Application.Features.Login;
using AutoTrader.Application.Features.Logout;
using AutoTrader.Application.Features.ChangePassword;
using AutoTrader.Application.Features.MultidirectOrder;
using AutoTrader.Application.Services;

namespace AutoTraderUI.Presenters
{
    public class MainFormPresenter: IPresenter
    {
        MainForm _view;
        ISettingsService _settingsService;
        Settings _settings;
        IDoubleStockClient _connectors;
        System.Timers.Timer _timerMultidirect;

        List<string> _seccodeList = new List<string>();


        StrategyManager _strategyManager;
        private readonly IMediator _mediator;

        public MainFormPresenter(MainForm view, ISettingsService settingsService, IDoubleStockClient connectors, StrategyManager strategyManager, IMediator mediator)
        {
            _view = view;
            this._settingsService = settingsService;
            _settings = settingsService.GetSettings();
            _connectors = connectors;
            _strategyManager = strategyManager;
            this._mediator = mediator;
            _timerMultidirect = new System.Timers.Timer();
            _timerMultidirect.Interval = 100;
            _timerMultidirect.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            _view.Login1 += Login1;
            _view.Logout1 += Logout1;
            _view.ChangePassword1 += ChangePassword1;
            _view.Login2 += Login2;
            _view.Logout2 += Logout2;
            _view.ChangePassword2 += ChangePassword2;
            _view.OnClose += Close;

            _view.ComboBuy += ComboBuy;
            _view.ComboSell += ComboSell;
            _view.MakeMultidirect += MakeMultidirect;
            _view.StartMakeMultidirectByTimer += StartMakeMultidirectByTimer;


            _view.SubscribeOnQuotations += SubscribeOnQuotations;
            _view.UnSubscribeOnQuotations += UnSubscribeOnQuotations;


            _view.AddObserver += _view_AddObserver; ;



            _view.Test += Test;

            _connectors.Master.OnMCPositionsUpdated += (target, args) =>
            {
                _view.LoadPositions(args.data);
            };

            _strategyManager.ObserverListChanged += _observersCollection_ObserverListChanged;


            _view.RunAllStrategies += _view_RunAllStrategies;
            _view.StopAllStrategies += _view_StopAllStrategies;

            _view.TimezoneChanged += _view_TimezoneChanged;
        }

        private void _view_TimezoneChanged()
        {
            _settings.Timezone = _view.Timezone;
            _settingsService.UpdateSettings(_settings);
        }

        private void _view_StopAllStrategies()
        {
            Trace.TraceInformation("_view_StopAllStrategies: begin");

            foreach (var strategy in _strategyManager.StrategyWorkers)
            {
                try
                {
                    strategy.Stop();
                }
                catch (Exception ex)
                {
                    _view.ShowMessage(ex.Message);
                }
            }

            Trace.TraceInformation("_view_StopAllStrategies: completed");
        }

        private void _view_RunAllStrategies()
        {
            Trace.TraceInformation("_view_RunAllStrategies: begin");

            foreach (var strategy in _strategyManager.StrategyWorkers)
            {
                try
                {
                    strategy.Start(_view.Timezone);
                }
                catch (Exception ex)
                {
                    _view.ShowMessage(ex.Message);
                }
            }

            Trace.TraceInformation("_view_RunAllStrategies: completed");
        }

        private void _observersCollection_ObserverListChanged(object sender, List<StrategySettings> e)
        {
            _view.UpdateObserversListInformation(e);
        }

        private void _view_AddObserver()
        {
            var addForm = new CreateEditObserver(_seccodeList);

            if (addForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _strategyManager.Add(new StrategySettings()
                {
                    Seccode = addForm.Seccode,
                    Difference = addForm.Difference,
                    Period = addForm.Period,
                    Delay = addForm.Delay,
                    NotificationType = addForm.NotificationType
                }) ;
            }
        }

        private void Test()
        {
            try
            {
                var res = _connectors.Master.GetHistoryData( _view.ComboBoxSeccode, TradingMode.Futures, SecurityPeriods.M1, 2);
            }
            catch (Exception e)
            {
                _view.ShowMessage(e.Message);
            }
        }

        

        private void UnSubscribeOnQuotations()
        {
            
        }

        private async void SubscribeOnQuotations()
        {
            var id = await _mediator.Send(new CreateMultidirectOrderCommand());
        }

        private void StartMakeMultidirectByTimer()
        {
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            if (!_connectors.Master.Connected || !_connectors.Slave.Connected)
            {
                _view.ShowMessage("Не все клиенты авторизованы!");
                return;
            }

            if (string.IsNullOrEmpty(_settings.Seccode))
            {
                _view.ShowMessage("Не выбран код инструмента");
                return;
            }

            //if (_settings.MultidirectExecuteTime < DateTime.Now)
            //{
            //    _view.ShowMessage("Время выполнения операции не может быть меньше чем текущая дата/время");
            //    return;
            //}

            _timerMultidirect = new System.Timers.Timer();
            _timerMultidirect.Interval = 100;
            _timerMultidirect.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timerMultidirect.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == _settings.MultidirectExecuteTime.Hour &&
                DateTime.Now.Minute == _settings.MultidirectExecuteTime.Minute &&
                DateTime.Now.Second == _settings.MultidirectExecuteTime.Second)
            {
                _timerMultidirect.Stop();

                await _makeMultidirect(_settings.Price, 
                                    _settings.Volume, 
                                    _settings.SL, 
                                    _settings.TP, 
                                    _settings.ByMarket, 
                                    _settings.Seccode);

                if (_settings.Shutdown)
                {
                    Process.Start("shutdown", "/s /t 0");
                }
            }
        }

        private async void MakeMultidirect()
        {
            var id = await _mediator.Send(new CreateMultidirectOrderCommand());
        }

        private async Task _makeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode)
        {
            ComboOrder comboOrder1 = new ComboOrder();
            comboOrder1.TradingMode = TradingMode.Futures;
            comboOrder1.SL = sl;
            comboOrder1.TP = tp;
            comboOrder1.Price = price;
            comboOrder1.Vol = vol;
            comboOrder1.ByMarket = bymarket;
            comboOrder1.Seccode = seccode;
            comboOrder1.OrderDirection = OrderDirection.Buy;

            ComboOrder comboOrder2 = (ComboOrder)comboOrder1.Clone();
            comboOrder2.OrderDirection = OrderDirection.Sell;

            Task md1 = Task.Run(async () =>
            {
                await _handleComboOperation(_connectors.Master, comboOrder1);
            });

            Task md2 = Task.Run(async () =>
            {
                await _handleComboOperation(_connectors.Slave, comboOrder2);
            });

            await Task.WhenAll(md1, md2);
        }

        private async void ComboSell()
        {
            var id = await _mediator.Send(new ChangePassword2Command());
        }

        private async void ComboBuy()
        {
            var id = await _mediator.Send(new ChangePassword2Command());            
        }

        private async Task _handleComboOperation(IStockClient cl, ComboOrder co)
        {

            if (string.IsNullOrEmpty(co.Seccode))
            {
                _view.ShowMessage("Не выбран код инструмента");
            }
            else
            {
                await cl.CreateNewComboOrder(co);
            }
        }

        private async void ChangePassword2()
        {
            var id = await _mediator.Send(new ChangePassword2Command());
        }

        private async void ChangePassword1()
        {
            var id = await _mediator.Send(new ChangePassword1Command());
        }

        private async void Close()
        {
            //_view.UpdateSettings(_settings);
            //_settingsService.UpdateSettings(_settings);      

            //await _connectors[0].DisposeAsync();
            //await _connectors[1].DisposeAsync();
        }

        /// <summary>
        /// Only this method (from Login1, Login2) gets securities infor
        /// </summary>
        private async void Login1()
        {
            var id = await _mediator.Send(new Login1Command());
        }

        private async void Login2()
        {
            var id = await _mediator.Send(new Login2Command());
        }

        private async void Logout1()
        {
            var id = await _mediator.Send(new Logout1Command());
        }

        private async void Logout2()
        {
            var id = await _mediator.Send(new Logout2Command());
        }

        public void Run()
        {
            _view.LoadSettings(_settings);
            _view.Show();
        }
    }
}
