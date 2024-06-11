
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
using AutoTrader.Application.Features.Settings;
using AutoTrader.Application.Contracts.Infrastructure.TXMLConnector;
using AutoTrader.Application.Features.Strategies;
using AutoTrader.Domain.Models.Strategies;
using AutoTrader.Infrastructure;
using AutoTrader.Domain.Models;
using AutoTrader.Application.Contracts.Infrastructure;

namespace AutoTraderUI.Presenters
{
    public class MainFormPresenter: IPresenter
    {
        MainForm _view;
        IAppSettingsService _settingsService;
        Settings _settings;
        List<ITXMLConnector> _connectors;
        System.Timers.Timer _timerMultidirect;

        List<string> _seccodeList = new List<string>();


        StrategyManager _strategyManager;

        public MainFormPresenter(MainForm view, IAppSettingsService settingsService, List<ITXMLConnector> connectors, StrategyManager strategyManager)
        {
            if (connectors.Count != 2) throw new Exception("Загружено недопустимое колличество коннекторов");

            _view = view;
            this._settingsService = settingsService;
            _settings = settingsService.GetSettings();
            _connectors = connectors;
            _strategyManager = strategyManager;

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

            _connectors[0].OnMCPositionsUpdated += (target, args) =>
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
                var res = _connectors[0].GetHistoryData( _view.ComboBoxSeccode, boardsCode.FUT, SecurityPeriods.M1, 2);
            }
            catch (Exception e)
            {
                _view.ShowMessage(e.Message);
            }
        }

        

        private void UnSubscribeOnQuotations()
        {
            
        }

        private void SubscribeOnQuotations()
        {
            try
            {
                _connectors[0].SubscribeQuotations(boardsCode.FUT, _view.ComboBoxSeccode);
            }
            catch (Exception e)
            {
                _view.ShowMessage(e.Message);
            }
        }

        private void StartMakeMultidirectByTimer()
        {
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            if (!_connectors[0].Connected || !_connectors[1].Connected)
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
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            if (!_connectors[0].Connected || !_connectors[1].Connected)
            {
                _view.ShowMessage("Не все клиенты авторизованы!");
                return;
            }

            if (string.IsNullOrEmpty(_settings.Seccode))
            {
                _view.ShowMessage("Не выбран код инструмента");
                return;
            }

            int sl = _settings.SL;
            int tp = _settings.TP;
            int price = _settings.Price;
            int vol = _settings.Volume;
            bool bymarket = _settings.ByMarket;
            string seccode = _settings.Seccode;

            await _makeMultidirect(price, vol, sl, tp, bymarket, seccode);
        }

        private async Task _makeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode)
        {
            ComboOrder comboOrder1 = new ComboOrder();
            comboOrder1.Board = boardsCode.FUT;
            comboOrder1.SL = sl;
            comboOrder1.TP = tp;
            comboOrder1.Price = price;
            comboOrder1.Vol = vol;
            comboOrder1.ByMarket = bymarket;
            comboOrder1.Seccode = seccode;
            comboOrder1.BuySell = buysell.B;

            ComboOrder comboOrder2 = (ComboOrder)comboOrder1.Clone();
            comboOrder2.BuySell = buysell.S;

            Task md1 = Task.Run(async () =>
            {
                await _handleComboOperation(_connectors[0], comboOrder1);
            });

            Task md2 = Task.Run(async () =>
            {
                await _handleComboOperation(_connectors[1], comboOrder2);
            });

            await Task.WhenAll(md1, md2);
        }

        private async void ComboSell()
        {
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            ComboOrder co = new ComboOrder();
            co.SL = _settings.SL;
            co.TP = _settings.TP;
            co.Price = _settings.Price;
            co.Vol = _settings.Volume;
            co.ByMarket = _settings.ByMarket;
            co.Seccode = _settings.Seccode;
            co.BuySell = buysell.S;

            try
            {
                await _handleComboOperation(_connectors[0], co);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private async void ComboBuy()
        {
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            ComboOrder co = new ComboOrder();
            co.SL = _settings.SL;
            co.TP = _settings.TP;
            co.Price = _settings.Price;
            co.Vol = _settings.Volume;
            co.ByMarket = _settings.ByMarket;
            co.Seccode = _settings.Seccode;
            co.BuySell = buysell.B;

            try
            {
                await _handleComboOperation(_connectors[0], co);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        private async Task _handleComboOperation(ITXMLConnector cl, ComboOrder co)
        {

            if (string.IsNullOrEmpty(co.Seccode))
            {
                _view.ShowMessage("Не выбран код инструмента");
            }
            else
            {
                await cl.NewComboOrder(co);
            }
        }

        private void ChangePassword2()
        {
            try
            {
                _view.UpdateSettings(_settings);
                _settingsService.UpdateSettings(_settings);
                _connectors[1].ChangePassword(_settings.Username2, _settings.Password2);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void ChangePassword1()
        {
            try
            {
                _view.UpdateSettings(_settings);
                _settingsService.UpdateSettings(_settings);
                _connectors[0].ChangePassword(_settings.Username, _settings.Password);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private async void Close()
        {
            _view.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);      

            await _connectors[0].DisposeAsync();
            await _connectors[1].DisposeAsync();
        }

        /// <summary>
        /// Only this method (from Login1, Login2) gets securities infor
        /// </summary>
        private async void Login1()
        {
            Trace.TraceInformation("Login1: begin");

            try
            {
                if (_view.ComboBoxConnectionType == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                int connectorNumber = 0;
                _view.UpdateSettings(_settings);
                _settingsService.UpdateSettings(_settings);

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _view.ComboBoxConnectionType);

                await _connectors[connectorNumber].Login(_settings.GetUsername(), _settings.GetPassword(), connType);

                _seccodeList = (await _connectors[connectorNumber].GetSecurities())
                                            .Where(x => x.board == boardsCode.FUT.ToString())
                                            .Select(x => x.seccode)
                                            .OrderBy(x => x)
                                            .ToList();

                _view.LoadSeccodeList(_seccodeList);
                _view.SetSelectedSeccode(_settings.Seccode);
                _view.HandleConnected(connectorNumber);

                _view.ClientId1 = _connectors[connectorNumber].FortsClientId;
                _view.Union1 = _connectors[connectorNumber].Union;
                _view.FreeMoney1 = _connectors[connectorNumber].Money.ToString();

                _view.FreeMoney1 = _connectors[connectorNumber].Money.ToString("N");
                _view.FreeMoney = _connectors[connectorNumber].Money.ToString("N");

                Trace.TraceInformation("Login1: completed");
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private async void Login2()
        {
            try
            {
                if (_view.ComboBoxConnectionType == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                int connectorNumber = 1;
                _view.UpdateSettings(_settings);
                _settingsService.UpdateSettings(_settings);

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _view.ComboBoxConnectionType);

                await _connectors[connectorNumber].Login(_settings.GetUsername2(), _settings.GetPassword2(), connType);

                _view.HandleConnected(connectorNumber);
                _view.FreeMoney2 = _connectors[connectorNumber].Money.ToString("N");
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void Logout1()
        {
            try
            {
                int connNumber = 0;
                _connectors[connNumber].Logout();
                _view.HandleDisconnected(connNumber);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void Logout2()
        {
            try
            {
                int connNumber = 0;
                _connectors[connNumber].Logout();
                _view.HandleDisconnected(connNumber);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        public void Run()
        {
            _view.LoadSettings(_settings);
            _view.Show();
        }
    }
}
