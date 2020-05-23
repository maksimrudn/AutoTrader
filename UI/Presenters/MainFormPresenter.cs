using AutoTraderSDK.Domain;
using AutoTraderSDK.Domain.OutputXML;
using AutoTraderSDK.Kernel;
using AutoTraderUI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoTraderUI.Presenters
{
    public class MainFormPresenter: IPresenter
    {
        ApplicationController _applicationController;
        IMainFormView _view;
        Settings _settings;
        List<TXMLConnector> _connectors;
        System.Timers.Timer _timerMultidirect;

        public MainFormPresenter(ApplicationController applicationController, IMainFormView view, Settings settings, List<TXMLConnector> connectors)
        {
            if (connectors.Count != 2) throw new Exception("Загружено недопустимое колличество коннекторов");

            _applicationController = applicationController;
            _view = view;
            _settings = settings;
            _connectors = connectors;

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
        }

        private void StartMakeMultidirectByTimer()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();

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

            if (_settings.MultidirectExecuteTime < DateTime.Now)
            {
                _view.ShowMessage("Дата выполнения операции не может быть меньше чем текущая дата/время");
                return;
            }

            _timerMultidirect = new System.Timers.Timer();
            _timerMultidirect.Interval = 100;
            _timerMultidirect.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timerMultidirect.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == _settings.MultidirectExecuteTime.Hour &&
                DateTime.Now.Minute == _settings.MultidirectExecuteTime.Minute &&
                DateTime.Now.Second == _settings.MultidirectExecuteTime.Second)
            {
                _timerMultidirect.Stop();

                _makeMultidirect(_settings.Price, 
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

        private void MakeMultidirect()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();

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

            _makeMultidirect(price, vol, sl, tp, bymarket, seccode);
        }

        private void _makeMultidirect(int price, int vol, int sl, int tp, bool bymarket, string seccode)
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

            Task md1 = Task.Run(() =>
            {
                _handleComboOperation(_connectors[0], comboOrder1);
            });

            Task md2 = Task.Run(() =>
            {
                _handleComboOperation(_connectors[1], comboOrder2);
            });

            md1.Wait();
            md2.Wait();
        }

        private void ComboSell()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();

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
                _handleComboOperation(_connectors[0], co);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void ComboBuy()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();

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
                _handleComboOperation(_connectors[0], co);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void _handleComboOperation(TXMLConnector cl, ComboOrder co)
        {

            if (string.IsNullOrEmpty(co.Seccode))
            {
                _view.ShowMessage("Не выбран код инструмента");
            }
            else
            {
                cl.NewComboOrder(co);
            }
        }

        private void ChangePassword2()
        {
            try
            {
                _view.UpdateSettings(_settings);
                _settings.Save();
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
                _settings.Save();
                _connectors[0].ChangePassword(_settings.Username, _settings.Password);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void Close()
        {
            _view.UpdateSettings(_settings);
            _settings.Save();
            _connectors[0].Dispose();
            _connectors[1].Dispose();
        }

        private void Login1()
        {
            try
            {
                if (_view.ComboBoxConnectionType == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                int connectorNumber = 0;
                _view.UpdateSettings(_settings);
                _settings.Save();

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _view.ComboBoxConnectionType);

                _connectors[connectorNumber].Login(_settings.GetUsername(), _settings.GetPassword(), connType);
                _view.LoadSeccodeList(_connectors[connectorNumber].GetSecurities().Where(x => x.board == boardsCode.FUT).Select(x => x.seccode).OrderBy(x => x).ToList());
                _view.SetSelectedSeccode(_settings.Seccode);
                _view.HandleConnected(connectorNumber);

                _view.ClientId1 = _connectors[connectorNumber].FortsClientId;
                _view.FreeMoney1 = _connectors[connectorNumber].Money.ToString();
                _view.Union1 = _connectors[connectorNumber].Union;
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

        private void Login2()
        {
            try
            {
                if (_view.ComboBoxConnectionType == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                int connectorNumber = 1;
                _view.UpdateSettings(_settings);
                _settings.Save();

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), _view.ComboBoxConnectionType);

                _connectors[connectorNumber].Login(_settings.GetUsername2(), _settings.GetPassword2(), connType);
                _view.HandleConnected(connectorNumber);

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
