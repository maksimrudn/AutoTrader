using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Domain.Models.Strategies;
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Models;
using AutoTrader.Application.Contracts.UI;
using AutoTrader.Application.Features.Login;
using MediatR;
using AutoTrader.Application.Features.Logout;
using AutoTrader.Application.Features.ChangePassword;
using AutoTrader.Application.Features.ComboBuy;
using AutoTrader.Application.Features.ComboSell;
using AutoTrader.Application.Features.MultidirectOrder;
using AutoTrader.Application.Features.SubscribeOnQuotations;
using System.Threading;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Services;
using AutoTraderUI.Views;
using AutoTrader.Application.Helpers;
using System.Linq;

namespace AutoTraderUI
{
    public partial class MainForm : Form, IMainFormView
    {
        readonly System.Timers.Timer _timerClock;
        readonly IEmailService _emailService;
        readonly IMediator _mediator;
        readonly ISettingsService _settingsService;
        readonly IDualStockClient _dualStockClient;
        readonly StrategyManager _strategyManager;
        readonly Settings _settings;
        readonly Action<Func<Task>> _commandWrapperAction = null;
        List<string> _seccodeList = new List<string>();

        System.Timers.Timer _timerMultidirectOrder;
        CancellationTokenSource _ct_timerMultidirectOrder = new CancellationTokenSource();

        public MainForm(IEmailService emailService,
                        IMediator mediator,
                        ISettingsService settingsService,
                        IDualStockClient dualStockClient,
                        StrategyManager strategyManager)
        {
            InitializeComponent();

            _emailService = emailService;
            _mediator = mediator;
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            _dualStockClient = dualStockClient;
            _strategyManager = strategyManager;
            
            _strategyManager.ObserverListChanged += ObserversCollection_ObserverListChanged;
            _dualStockClient.Master.MCPositionsUpdated += (target, args) =>
            {
                this.LoadPositions(args.data);
            };

            comboBoxConnectionType.DataSource = new List<string> { string.Empty, "Prod", "Demo" };

            _timerClock = new System.Timers.Timer();
            _timerClock.Interval = 100;
            _timerClock.Elapsed += new ElapsedEventHandler(TimerClock_Elapsed);

            comboBoxTimezone.Items.Add(4);
            comboBoxTimezone.Items.Add(7);
            comboBoxTimezone.SelectedValueChanged += TimezoneChanged;
            comboBoxSeccode.SelectedValueChanged += ComboBoxSeccode_SelectedValueChanged;
            _dualStockClient.Master.MCPositionsUpdated += MCPositionsUpdated;
            _dualStockClient.Master.SecuritiesUpdated += SecuritiesUpdated;

            _commandWrapperAction = async (act) =>
            {
                try
                {
                    this.FreezUI();
                    await act();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message);
                }
                finally
                {
                    this.UnFreezUI();
                }
            };

            HandleDisconnected();
        }

        protected override void OnClosed(EventArgs e)
        {
            _timerClock.Stop();
            _ct_timerMultidirectOrder.Cancel();

            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadSettings(_settings);
            _timerClock.Start(); 
            base.OnLoad(e);
        }

        #region FIELDS

        public int Timezone { get { return int.Parse(comboBoxTimezone.SelectedItem.ToString()); } }
        public string ComboBoxConnectionType { get { return comboBoxConnectionType.Text; } }

        public string ComboBoxSeccode
        {
            get
            {
                string res = "";

                this.Invoke(new MethodInvoker(delegate { res = comboBoxSeccode.Text; }));

                return res;
            }
        }
        public string Username1 { get { return textBoxUsername.Text; } }
        public string Password1 { get { return textBoxPassword.Text; } }
        public string ClientId1 { set { textBoxClientId.Text = value; } }

        public string FreeMoney { set { textBoxFreeMoney.Text = value; } }
        public string FreeMoney1 { set { textBoxFreeMoney1.Text = value; } }
        public string FreeMoney2 { set { textBoxFreeMoney2.Text = value; } }

        public double ObserveDifference
        {
            get
            {
                double res = 0;

                this.Invoke(new MethodInvoker(delegate { res = string.IsNullOrEmpty(textBoxDifference.Text) ? 0 : double.Parse(textBoxDifference.Text); }));

                return res;
            }
        }

        public string Union1 { set { textBoxUnion.Text = value; } }

        #endregion

        #region HELPER METHODS
        public void LoadSeccodeList(List<string> lst)
        {
            List<string> seccodes = new List<string>()
            {
                string.Empty
            };

            seccodes.AddRange(lst);

            this.Invoke(new MethodInvoker(() =>
            {
                comboBoxSeccode.DataSource = seccodes;
            }));
        }

        public void LoadSettings(Settings settings)
        {
            textBoxUsername.Text = settings.GetUsername();
            textBoxPassword.Text = settings.GetPassword();
            textBoxUsername2.Text = settings.GetUsername2();
            textBoxPassword2.Text = settings.GetPassword2();
            textBoxTP.Text = settings.TP.ToString();
            textBoxSL.Text = settings.SL.ToString();
            textBoxPrice.Text = settings.Price.ToString();
            checkBoxByMarket.Checked = settings.ByMarket;
            textBoxVolume.Text = settings.Volume.ToString();
            radioButtonComboTypeContidion.Checked = settings.ComboOrderType == 1;
            radioButtonComboTypeStop.Checked = settings.ComboOrderType == 2;
            comboBoxConnectionType.SelectedItem = settings.ConnectionType;
            dateTimePickerMultidirectExecute.Value = settings.MultidirectExecuteTime;
            checkBoxShutdown.Checked = settings.Shutdown;

            for (int i = 0; i < comboBoxTimezone.Items.Count; i++)
            {
                // Check if the value of the current item is equal to 3
                if (comboBoxTimezone.Items[i].ToString() == settings.Timezone.ToString())
                {
                    // Set the SelectedIndex of the ComboBox to the index of the item with value 3
                    comboBoxTimezone.SelectedIndex = i;
                    break; // Exit the loop once the item is found
                }
            }
        }

        public void UpdateObserversListInformation(List<StrategySettings> e)
        {
            observersDataGridView.LoadObservers(e);
        }

        public void UpdateSettings(Settings settings)
        {
            //settings.SetUsername(textBoxUsername.Text);
            //settings.SetPassword(textBoxPassword.Text);
            //settings.SetUsername2(textBoxUsername2.Text);
            //settings.SetPassword2(textBoxPassword2.Text);
            //settings.TP = int.Parse(textBoxTP.Text);
            //settings.SL = int.Parse(textBoxSL.Text);
            //settings.Price = int.Parse(textBoxPrice.Text);
            //settings.Volume = int.Parse(textBoxVolume.Text);
            //settings.Seccode = comboBoxSeccode.Text;
            //settings.ByMarket = checkBoxByMarket.Checked;
            //settings.ComboOrderType = radioButtonComboTypeContidion.Checked ? 1 : 2;
            //settings.ConnectionType = comboBoxConnectionType.Text;
            //settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value;

            this.Invoke(new MethodInvoker(() => { settings.SetUsername(textBoxUsername.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.SetPassword(textBoxPassword.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.SetUsername2(textBoxUsername2.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.SetPassword2(textBoxPassword2.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.TP = int.Parse(textBoxTP.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.SL = int.Parse(textBoxSL.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.Price = int.Parse(textBoxPrice.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.Volume = int.Parse(textBoxVolume.Text); }));
            this.Invoke(new MethodInvoker(() => { settings.Seccode = comboBoxSeccode.Text; }));
            this.Invoke(new MethodInvoker(() => { settings.ByMarket = checkBoxByMarket.Checked; }));
            this.Invoke(new MethodInvoker(() => { settings.ComboOrderType = radioButtonComboTypeContidion.Checked ? 1 : 2; }));
            this.Invoke(new MethodInvoker(() => { settings.ConnectionType = comboBoxConnectionType.Text; }));
            this.Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
            this.Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
        }

        public void LoadPositions(mc_portfolio portfolio)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                dataGridViewPositions.Rows.Clear();

                foreach (var pos in portfolio.securities)
                {
                    dataGridViewPositions.Rows.Add(new object[]
                    {
                    pos.seccode,
                    //pos.name,
                    pos.balance_prc,
                    pos.price,
                    pos.balance,
                        //pos.securityElement.unrealized_pnl
                    });
                }
            }));
        }

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        public void SetSelectedSeccode(string seccode)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                comboBoxSeccode.SelectedItem = seccode;
            }));
        }

        public void HandleConnected(int connectorNumber)
        {
            if (connectorNumber == 0)
            {
                ((Control)tabPage1).Enabled = true;
                groupBoxChangePassword.Enabled = true;
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true;
                textBoxPassword.Enabled = false;
                textBoxUsername.Enabled = false;
            }
            else
            {
                groupBoxChangePassword2.Enabled = true;
                buttonLogin2.Enabled = false;
                buttonLogout2.Enabled = true;

                textBoxPassword2.Enabled = false;
                textBoxUsername2.Enabled = false;
            }
        }

        public void HandleDisconnected(int connNumber = -1)
        {
            if (connNumber == 0)
            {
                ((Control)tabPage1).Enabled = false;
                groupBoxChangePassword.Enabled = false;
                buttonLogin.Enabled = true;
                buttonLogout.Enabled = false;

                textBoxUnion.Text = string.Empty;
                textBoxClientId.Text = string.Empty;
                textBoxFreeMoney.Text = string.Empty;

                textBoxPassword.Enabled = true;
                textBoxUsername.Enabled = true;
            }
            else if (connNumber == 1)
            {
                groupBoxChangePassword2.Enabled = false;
                buttonLogin2.Enabled = true;
                buttonLogout2.Enabled = false;

                textBoxPassword2.Enabled = true;
                textBoxUsername2.Enabled = true;
            }
            else
            {
                ((Control)tabPage1).Enabled = false;
                groupBoxChangePassword.Enabled = false;
                buttonLogin.Enabled = true;
                buttonLogout.Enabled = false;
                textBoxPassword.Enabled = true;
                textBoxUsername.Enabled = true;

                textBoxUnion.Text = string.Empty;
                textBoxClientId.Text = string.Empty;
                textBoxFreeMoney.Text = string.Empty;

                groupBoxChangePassword2.Enabled = false;
                buttonLogin2.Enabled = true;
                buttonLogout2.Enabled = false;
                textBoxPassword2.Enabled = true;
                textBoxUsername2.Enabled = true;
            }
        }
            
        public void FreezUI()
        {
            tabControl1.Enabled = false;
            comboBoxTimezone.Enabled = false;
            testButton.Enabled = false;
        }

        public void UnFreezUI()
        {
            tabControl1.Enabled = true;
            comboBoxTimezone.Enabled = true;
            testButton.Enabled = true;
        }

        #endregion

        #region EVENT HANDLERS
        private void ObserversCollection_ObserverListChanged(object sender, List<StrategySettings> e)
        {
            this.UpdateObserversListInformation(e);
        }

        private void TimezoneChanged(object sender, EventArgs eventArgs)
        {
            _settings.Timezone = this.Timezone;
            _settingsService.UpdateSettings(_settings);
        }

        private void TimerClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_ct_timerMultidirectOrder.IsCancellationRequested) return;

            if (!_disposed)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    labelTime.Text = $"Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                }));
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            _emailService.SendEmailAsync("m.rudneov@yandex.ru", "Autotrader", "test");

            //try
            //{
            //    var res = _connectors.Master.GetHistoryData(_view.ComboBoxSeccode, TradingMode.Futures, SecurityPeriods.M1, 2);
            //}
            //catch (Exception e)
            //{
            //    _view.ShowMessage(e.Message);
            //}
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);

                var resp = await _mediator.Send(new Login1Command()
                {
                    Settings = _settings
                });

                _seccodeList = resp.SeccodeList;
                this.LoadSeccodeList(resp.SeccodeList);
                this.SetSelectedSeccode(resp.SelectedSeccode);
                this.HandleConnected(0);
                this.ClientId1 = resp.ClientId;
                this.Union1 = resp.Union;
                this.FreeMoney = resp.FreeMoney;
                this.FreeMoney1 = resp.FreeMoney;
            });
        }

        private async void buttonLogin2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);

                var resp = await _mediator.Send(new Login2Command()
                {
                    Settings = _settings
                });

                this.HandleConnected(1);
                this.FreeMoney2 = resp.FreeMoney;
            });
        }

        private async void buttonLogout_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                var id = await _mediator.Send(new Logout1Command());
                this.HandleDisconnected(0);
            });
        }

        private async void buttonLogout2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                var id = await _mediator.Send(new Logout2Command());
                this.HandleDisconnected(1);
            });
        }

        private async void buttonChangePassword_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new ChangePassword1Command()
                {
                    Settings = _settings
                });
            });
        }

        private async void buttonChangePassword2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new ChangePassword2Command()
                {
                    Settings = _settings
                });
            });
        }

        private void buttonComboBuy_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new ComboBuyCommand()
                {
                    Settings = _settings
                });
            });
        }

        private void buttonComboSell_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new ComboSellCommand()
                {
                    Settings = _settings
                });
            });
        }

        private void buttonMakeMultidirect_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new CreateMultidirectOrderCommand()
                {
                    Settings = _settings
                });
            });
        }

        private void buttonQuotationsSubscribe_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                this.UpdateSettings(_settings);
                var id = await _mediator.Send(new SubscribeOnQuotationsCommand()
                {
                    Seccode = this.ComboBoxSeccode
                });
            });
        }

        private void buttonRunAll_Click(object sender, EventArgs e)
        {
            Trace.TraceInformation("_view_RunAllStrategies: begin");

            foreach (var strategy in _strategyManager.StrategyWorkers)
            {
                try
                {
                    strategy.Start(this.Timezone);
                }
                catch (Exception ex)
                {
                    this.ShowMessage(ex.Message);
                }
            }

            Trace.TraceInformation("_view_RunAllStrategies: completed");
        }

        private void buttonStopAll_Click(object sender, EventArgs e)
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
                    this.ShowMessage(ex.Message);
                }
            }

            Trace.TraceInformation("_view_StopAllStrategies: completed");
        }

        private void buttonAddObserver_Click(object sender, EventArgs e)
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
                });
            }
        }

        private void buttonStartMultidirectTimer_Click(object sender, EventArgs e)
        {
            this.UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            if (!_dualStockClient.Master.Connected || !_dualStockClient.Slave.Connected)
            {
                this.ShowMessage("Не все клиенты авторизованы!");
                return;
            }

            if (string.IsNullOrEmpty(_settings.Seccode))
            {
                this.ShowMessage("Не выбран код инструмента");
                return;
            }

            //if (_settings.MultidirectExecuteTime < DateTime.Now)
            //{
            //    _view.ShowMessage("Время выполнения операции не может быть меньше чем текущая дата/время");
            //    return;
            //}

            _timerMultidirectOrder = new System.Timers.Timer();
            _timerMultidirectOrder.Interval = 100;
            _timerMultidirectOrder.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timerMultidirectOrder.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == _settings.MultidirectExecuteTime.Hour &&
                DateTime.Now.Minute == _settings.MultidirectExecuteTime.Minute &&
                DateTime.Now.Second == _settings.MultidirectExecuteTime.Second)
            {
                _timerMultidirectOrder.Stop();

                await StockOperationHelper.MakeMultidirect(_dualStockClient, _settings.Price,
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

        private void ComboBoxSeccode_SelectedValueChanged(object sender, EventArgs e)
        {
            _settings.Seccode = (string)comboBoxSeccode.SelectedValue;
            _settingsService.UpdateSettings(_settings);
        }

        private void MCPositionsUpdated(object sender,
                                                TXMLEventArgs<mc_portfolio> e)
        {
            this.LoadPositions(e.data);
        }

        private void SecuritiesUpdated(object sender, TXMLEventArgs<HashSet<AutoTrader.Application.Models.TXMLConnector.Ingoing.securities_ns.security>> e)
        {
            _seccodeList = e.data.Where(x => x.board == AutoTrader.Application.Models.TXMLConnector.Outgoing.boardsCode.FUT.ToString())
                                    .Select(x => x.seccode)
                                    .OrderBy(x => x).ToList();

            this.LoadSeccodeList(_seccodeList);
            this.SetSelectedSeccode(_settings.Seccode);
        }
        #endregion
    }
}
