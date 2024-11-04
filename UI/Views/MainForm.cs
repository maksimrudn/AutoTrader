using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using AutoTrader.Application.Contracts.Infrastructure;
using AutoTrader.Application.Contracts.Infrastructure.Stock;
using AutoTrader.Application.Contracts.UI;
using AutoTrader.Application.Enums;
using AutoTrader.Application.Features.ChangePassword;
using AutoTrader.Application.Features.ComboBuy;
using AutoTrader.Application.Features.ComboSell;
using AutoTrader.Application.Features.Login;
using AutoTrader.Application.Features.Logout;
using AutoTrader.Application.Features.MultidirectOrder;
using AutoTrader.Application.Features.SubscribeOnQuotations;
using AutoTrader.Application.Models;
using AutoTrader.Application.Models.TransaqConnector.Ingoing;
using AutoTrader.Application.Models.TransaqConnector.Ingoing.securities_ns;
using AutoTrader.Application.Models.TransaqConnector.Outgoing;
using AutoTrader.Application.Services;
using AutoTrader.Domain.Models.Strategies;
using MediatR;
using Timer = System.Timers.Timer;

namespace AutoTraderUI.Views
{
    public partial class MainForm : Form, IMainFormView
    {
        private readonly Timer _timerUpdateUi;
        private readonly IMediator _mediator;
        private readonly ISettingsService _settingsService;
        private readonly IDualStockClient _dualStockClient;
        private readonly StrategyManager _strategyManager;
        private readonly Settings _settings;
        private readonly Action<Func<Task>> _commandWrapperAction;
        private List<string> _seccodeList = new();

        private readonly BindingList<forts_position> _fortsPositionsBindingList = new();

        Timer _timerMultidirectOrder;
        private readonly CancellationTokenSource _ct_timerMultidirectOrder = new();

        public MainForm(IMediator mediator,
                        ISettingsService settingsService,
                        IDualStockClient dualStockClient,
                        StrategyManager strategyManager)
        {
            InitializeComponent();

            _mediator = mediator;
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            _dualStockClient = dualStockClient;
            _strategyManager = strategyManager;
            
            _strategyManager.ObserverListChanged += ObserversCollection_ObserverListChanged;

            comboBoxConnectionType.DataSource = new List<string> { string.Empty, "Prod", "Demo" };

            _timerUpdateUi = new Timer();
            _timerUpdateUi.Interval = 1000;
            _timerUpdateUi.Elapsed += TimerUpdateUI_Elapsed;

            comboBoxTimezone.Items.Add(4);
            comboBoxTimezone.Items.Add(7);
            comboBoxTimezone.SelectedValueChanged += TimezoneChanged;
            comboBoxSeccode.SelectedValueChanged += ComboBoxSeccode_SelectedValueChanged;

            dataGridViewFortsPositions.DataSource = _fortsPositionsBindingList;

            _commandWrapperAction = async act =>
            {
                try
                {
                    FreezUi();
                    await act();
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                }
                finally
                {
                    UnFreezUi();
                }
            };
            
            _timerMultidirectOrder = new Timer();
            _timerMultidirectOrder.Interval = 100;
            _timerMultidirectOrder.Elapsed += timer_Elapsed;
            
            HandleDisconnected();
        }

        protected override void OnClosed(EventArgs e)
        {
            _timerUpdateUi.Stop();
            _ct_timerMultidirectOrder.Cancel();
            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadSettings(_settings);
            _timerUpdateUi.Start(); 
            base.OnLoad(e);
        }

        #region FIELDS

        public int Timezone => int.Parse(comboBoxTimezone.SelectedItem.ToString() ?? throw new ArgumentNullException());
        public string ComboBoxConnectionType => comboBoxConnectionType.Text;
        public string ComboBoxSeccode
        {
            get
            {
                string res = "";
                Invoke(new MethodInvoker(delegate { res = comboBoxSeccode.Text; }));
                return res;
            }
        }
        public string Username1 => textBoxUsername.Text;
        public string Password1 => textBoxPassword.Text;
        public string ClientId1 { set => textBoxClientId.Text = value; }
        public string FreeMoney { set => textBoxFreeMoney.Text = value; }
        public string FreeMoney1 { set => textBoxFreeMoney1.Text = value; }
        public string FreeMoney2 { set => textBoxFreeMoney2.Text = value; }
        public double ObserveDifference
        {
            get
            {
                double res = 0;
                Invoke(new MethodInvoker(
                    delegate
                    {
                        res = string.IsNullOrEmpty(textBoxDifference.Text) ? 
                            0 : 
                            double.Parse(textBoxDifference.Text);
                    }));
                return res;
            }
        }

        public string Union1 { set => textBoxUnion.Text = value; }

        #endregion

        #region HELPER METHODS
        public void LoadSettings(Settings settings)
        {
            textBoxUsername.Text = settings.GetUsername();
            textBoxPassword.Text = settings.GetPassword();
            textBoxPasswordOld.Text = settings.GetOldPassword();
            textBoxPasswordNew.Text = settings.GetNewPassword();
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
            Invoke(new MethodInvoker(() => { settings.SetUsername(textBoxUsername.Text); }));
            Invoke(new MethodInvoker(() => { settings.SetPassword(textBoxPassword.Text); }));
            Invoke(new MethodInvoker(() => { settings.SetOldPassword(textBoxPasswordOld.Text); }));
            Invoke(new MethodInvoker(() => { settings.SetNewPassword(textBoxPasswordNew.Text); }));
            Invoke(new MethodInvoker(() => { settings.SetUsername2(textBoxUsername2.Text); }));
            Invoke(new MethodInvoker(() => { settings.SetPassword2(textBoxPassword2.Text); }));
            Invoke(new MethodInvoker(() => { settings.TP = int.Parse(textBoxTP.Text); }));
            Invoke(new MethodInvoker(() => { settings.SL = int.Parse(textBoxSL.Text); }));
            Invoke(new MethodInvoker(() => { settings.Price = int.Parse(textBoxPrice.Text); }));
            Invoke(new MethodInvoker(() => { settings.Volume = int.Parse(textBoxVolume.Text); }));
            Invoke(new MethodInvoker(() => { settings.Seccode = comboBoxSeccode.Text; }));
            Invoke(new MethodInvoker(() => { settings.ByMarket = checkBoxByMarket.Checked; }));
            Invoke(new MethodInvoker(() => { settings.ComboOrderType = radioButtonComboTypeContidion.Checked ? 1 : 2; }));
            Invoke(new MethodInvoker(() => { settings.ConnectionType = comboBoxConnectionType.Text; }));
            Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
            Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
        }

        public void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }
                
        public void HandleConnected(StockUserConnectionTypes connectionType)
        {
            if (!Enum.IsDefined(typeof(StockUserConnectionTypes), connectionType))
            {
                throw new ArgumentException("Invalid enum value passed.");
            }
            
            comboBoxConnectionType.Enabled = false;

            if (connectionType == StockUserConnectionTypes.Master)
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

        public void HandleDisconnected(StockUserConnectionTypes? connectionType = null)
        {
            if (connectionType != null && !Enum.IsDefined(typeof(StockUserConnectionTypes), connectionType))
            {
                throw new ArgumentException("Invalid enum value passed.");
            }
            
            comboBoxConnectionType.Enabled = false;
            if (connectionType == StockUserConnectionTypes.Master)
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
            else if (connectionType == StockUserConnectionTypes.Slave)
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
            
        public void FreezUi()
        {
            tabControl1.Enabled = false;
            comboBoxTimezone.Enabled = false;
            testButton.Enabled = false;
        }

        public void UnFreezUi()
        {
            tabControl1.Enabled = true;
            comboBoxTimezone.Enabled = true;
            testButton.Enabled = true;
        }

        #endregion

        #region EVENT HANDLERS
        private void ObserversCollection_ObserverListChanged(object sender, List<StrategySettings> e)
        {
            UpdateObserversListInformation(e);
        }

        private void TimezoneChanged(object sender, EventArgs eventArgs)
        {
            _settings.Timezone = Timezone;
            _settingsService.UpdateSettings(_settings);
        }

        private void TimerUpdateUI_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_ct_timerMultidirectOrder.IsCancellationRequested) return;

            if (!_disposed)
            {
                Invoke(new MethodInvoker(() =>
                {
                    labelTime.Text = $"Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
                }));

                Invoke(new MethodInvoker(() =>
                {
                    UpdateFortsPositions(_dualStockClient.Master.FortsPositions);
                }));
            }
        }

        private void UpdateSecurities(List<security>? securities)
        {
            if (securities == null) return;

            _seccodeList = securities.Where(x => x.board == boardsCode.FUT.ToString())
                                    .Select(x => x.seccode)
                                    .OrderBy(x => x).ToList();

            LoadSeccodeList(_seccodeList);
            _isInitializedComboboxSeccode = true;
            SetSelectedSeccode(_settings.Seccode);
        }

        public void LoadSeccodeList(List<string> lst)
        {
            List<string> seccodes = new List<string>
            {
                string.Empty
            };

            seccodes.AddRange(lst);

            Invoke(new MethodInvoker(() =>
            {
                comboBoxSeccode.DataSource = seccodes;
            }));
        }

        public void SetSelectedSeccode(string seccode)
        {
            Invoke(new MethodInvoker(() =>
            {
                comboBoxSeccode.SelectedItem = seccode;
            }));
        }

        private void UpdateFortsPositions(IEnumerable<forts_position> newPositions)
        {
            var existingPositions = _fortsPositionsBindingList.ToDictionary(p => p.GetKey()); // Assuming forts_position has an Id property for unique identification.

            foreach (var newPosition in newPositions)
            {
                if (existingPositions.TryGetValue(newPosition.GetKey(), out var existingPosition))
                {
                    // Update existing position
                    existingPosition.openbuys = newPosition.openbuys; // Update properties as needed
                    existingPosition.opensells = newPosition.opensells;
                    existingPosition.varmargin = newPosition.varmargin;
                    existingPosition.openbuys = existingPosition.openbuys;
                                                                              // Trigger notification to the DataGridView (if BindingList does not auto-refresh)
                    _fortsPositionsBindingList.ResetItem(_fortsPositionsBindingList.IndexOf(existingPosition));
                }
                else
                {
                    // Add new position
                    _fortsPositionsBindingList.Add(newPosition);
                }
            }

            // Remove positions that are no longer in the new list
            for (int i = _fortsPositionsBindingList.Count - 1; i >= 0; i--)
            {
                var position = _fortsPositionsBindingList[i];
                if (!newPositions.Any(p => p.GetKey() == position.GetKey()))
                {
                    _fortsPositionsBindingList.Remove(position);
                }
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            // todo: use this code for email testing
            //_emailService.SendEmailAsync("m.rudneov@yandex.ru", "Autotrader", "test");

            //try
            //{
            //    var res = _connectors.Master.GetHistoryData(_view.ComboBoxSeccode, TradingMode.Futures, SecurityPeriods.M1, 2);
            //}
            //catch (Exception e)
            //{
            //    _view.ShowMessage(e.Message);
            //}
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var resp = await _mediator.Send(new LoginMasterCommand
                {
                    Settings = _settings
                });
                _seccodeList = resp.SeccodeList;
                LoadSeccodeList(resp.SeccodeList);
                SetSelectedSeccode(resp.SelectedSeccode);
                HandleConnected(StockUserConnectionTypes.Master);
                ClientId1 = resp.ClientId ?? string.Empty;
                Union1 = resp.Union ?? string.Empty;
                FreeMoney = resp.FreeMoney ?? string.Empty;
                FreeMoney1 = resp.FreeMoney ?? string.Empty;
                Invoke(new MethodInvoker(() =>
                {
                    UpdateSecurities(_dualStockClient.Master.Securities);
                }));
            });
        }

        private void buttonLogin2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var resp = await _mediator.Send(new LoginSlaveCommand
                {
                    Settings = _settings
                });
                HandleConnected(StockUserConnectionTypes.Slave);
                FreeMoney2 = resp.FreeMoney ?? string.Empty;
            });
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                await _mediator.Send(new LogoutMasterCommand());
                HandleDisconnected(0);
                _isInitializedComboboxSeccode = false;
            });
        }

        private void buttonLogout2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                await _mediator.Send(new LogoutSlaveCommand());
                HandleDisconnected(StockUserConnectionTypes.Slave);
            });
        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var id = await _mediator.Send(new ChangePassword1Command
                {
                    Settings = _settings
                });
            });
        }

        private void buttonChangePassword2_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var id = await _mediator.Send(new ChangePassword2Command
                {
                    Settings = _settings
                });
            });
        }

        private void buttonComboBuy_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var id = await _mediator.Send(new ComboBuyCommand
                {
                    Settings = _settings
                });
            });
        }

        private void buttonComboSell_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                await _mediator.Send(new ComboSellCommand
                {
                    Settings = _settings
                });
            });
        }

        private void buttonMakeMultidirect_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                await _mediator.Send(new CreateMultidirectOrderCommand
                {
                    Settings = _settings
                });
            });
        }

        private void buttonQuotationsSubscribe_Click(object sender, EventArgs e)
        {
            _commandWrapperAction(async () =>
            {
                UpdateSettings(_settings);
                var id = await _mediator.Send(new SubscribeOnQuotationsCommand
                {
                    Seccode = ComboBoxSeccode
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
                    strategy.Start(Timezone);
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
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
                    ShowMessage(ex.Message);
                }
            }

            Trace.TraceInformation("_view_StopAllStrategies: completed");
        }

        private void buttonAddObserver_Click(object sender, EventArgs e)
        {
            var addForm = new CreateEditObserver(_seccodeList);

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                _strategyManager.Add(new StrategySettings
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
            UpdateSettings(_settings);
            _settingsService.UpdateSettings(_settings);

            if (!_dualStockClient.Master.Connected || !_dualStockClient.Slave.Connected)
            {
                ShowMessage("Не все клиенты авторизованы!");
                return;
            }

            if (string.IsNullOrEmpty(_settings.Seccode))
            {
                ShowMessage("Не выбран код инструмента");
                return;
            }

            //if (_settings.MultidirectExecuteTime < DateTime.Now)
            //{
            //    _view.ShowMessage("Время выполнения операции не может быть меньше чем текущая дата/время");
            //    return;
            //}

            _timerMultidirectOrder.Start();
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == _settings.MultidirectExecuteTime.Hour &&
                DateTime.Now.Minute == _settings.MultidirectExecuteTime.Minute &&
                DateTime.Now.Second == _settings.MultidirectExecuteTime.Second)
            {
                _timerMultidirectOrder.Stop();

                await _dualStockClient.MakeMultidirect(_settings.Price,
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

        bool _isInitializedComboboxSeccode;
        private void ComboBoxSeccode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_isInitializedComboboxSeccode) return;

            _settings.Seccode = (string)comboBoxSeccode.SelectedValue;
            _settingsService.UpdateSettings(_settings);
        }
        #endregion
    }
}
