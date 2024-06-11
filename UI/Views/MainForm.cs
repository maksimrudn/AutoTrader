using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using AutoTraderUI;
using AutoTraderUI.Common;
using System.Globalization;
using AutoTrader.Application.Models.TXMLConnector.Ingoing;
using AutoTrader.Domain.Models.Strategies;
using AutoTrader.Application.Features.Settings;
using AutoTrader.Infrastructure.Stock;
using AutoTrader.Application.Contracts.Infrastructure;

namespace AutoTraderUI
{
    public partial class MainForm : Form, IMainFormView
    {
        System.Timers.Timer _timerClock;
        protected ApplicationContext _context;
        private readonly IEmailService _emailService;

        public MainForm(ApplicationContext context, IEmailService emailService)
        {
            Trace.TraceInformation("MainForm: begin creation");

            _context = context;
            this._emailService = emailService;
            InitializeComponent();


            buttonLogin.Click += (sender, args) => Invoke(Login1);
            buttonLogout.Click += (sender, args) => Invoke(Logout1);
            buttonChangePassword.Click += (sender, args) => Invoke(ChangePassword1);
            buttonLogin2.Click += (sender, args) => Invoke(Login2);
            buttonLogout2.Click += (sender, args) => Invoke(Logout2);
            buttonChangePassword2.Click += (sender, args) => Invoke(ChangePassword2);
            buttonComboBuy.Click += (sender, args) => Invoke(ComboBuy);
            buttonComboSell.Click += (sender, args) => Invoke(ComboSell);
            buttonMakeMultidirect.Click += (sender, args) => Invoke(MakeMultidirect);
            buttonStartMultidirectTimer.Click += (sender, args) => Invoke(GetUnion);
            button2.Click += (sender, args) => Invoke(GetUnion);

            buttonAddObserver.Click += (sender, args) => Invoke(AddObserver);

            buttonRunAll.Click += (sender, args) => Invoke(RunAllStrategies);
            buttonStopAll.Click += (sender, args) => Invoke(StopAllStrategies);



            //testButton.Click += (sender, args) => Invoke(Test);

            quotationsSubscribebutton.Click += (sender, args) => Invoke(SubscribeOnQuotations);
            quotationsUnSubscribebutton.Click += (sender, args) => Invoke(UnSubscribeOnQuotations);
            observeButton.Click += (sender, args) => Invoke(Observe);

            this.FormClosing += (sender, args) =>
            {
                _timerClock.Stop();
                Invoke(OnClose);
            };

            comboBoxConnectionType.DataSource = new List<string> { string.Empty, "Prod", "Demo" };

            HandleDisconnected();

            _timerClock = new System.Timers.Timer();
            _timerClock.Interval = 100;
            _timerClock.Elapsed += new ElapsedEventHandler(timerClock_Elapsed);

            comboBoxTimezone.Items.Add(4);
            comboBoxTimezone.Items.Add(7);
            comboBoxTimezone.SelectedValueChanged += (sender, args) => Invoke(TimezoneChanged);

        }

        public event Action TimezoneChanged;

        public event Action Login1;
        public event Action Logout1;
        public event Action ChangePassword1;

        public void UpdateObserversListInformation(List<StrategySettings> e)
        {
            observersDataGridView.LoadObservers(e);
        }

        public event Action Login2;
        public event Action Logout2;
        public event Action ChangePassword2;
        public event Action ComboBuy;
        public event Action ComboSell;
        public event Action MakeMultidirect;
        public event Action StartMakeMultidirectByTimer;
        public event Action GetUnion;
        public event Action SubscribeOnQuotations;
        public event Action UnSubscribeOnQuotations;

        public event Action RunAllStrategies;
        public event Action StopAllStrategies;


        public event Action Observe;


        public event Action AddObserver;

        public event Action Test;

        public event Action OnClose;

        private void Invoke(Action action)
        {
            if (action != null) action();
        }

        private void timerClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            labelTime.Invoke(new MethodInvoker(() =>
            {
                labelTime.Text = $"Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
            }));

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _timerClock.Start();

           


            Trace.TraceInformation("MainForm: created");
        }

        public void LoadSeccodeList(List<string> lst)
        {
            List<string> seccodes = new List<string>()
            {
                string.Empty
            };

            seccodes.AddRange(lst);
            comboBoxSeccode.DataSource = seccodes;
        }


        public void LoadSettings(AppSettings settings)
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

        public void UpdateSettings(AppSettings settings)
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

            textBoxUsername.Invoke(new MethodInvoker(() => { settings.SetUsername(textBoxUsername.Text); }));
            textBoxPassword.Invoke(new MethodInvoker(() => { settings.SetPassword(textBoxPassword.Text); }));
            textBoxUsername2.Invoke(new MethodInvoker(() => { settings.SetUsername2(textBoxUsername2.Text); }));
            textBoxPassword2.Invoke(new MethodInvoker(() => { settings.SetPassword2(textBoxPassword2.Text); }));
            textBoxTP.Invoke(new MethodInvoker(() => { settings.TP = int.Parse(textBoxTP.Text); }));
            textBoxSL.Invoke(new MethodInvoker(() => { settings.SL = int.Parse(textBoxSL.Text); }));
            textBoxPrice.Invoke(new MethodInvoker(() => { settings.Price = int.Parse(textBoxPrice.Text); }));
            textBoxVolume.Invoke(new MethodInvoker(() => { settings.Volume = int.Parse(textBoxVolume.Text); }));
            comboBoxSeccode.Invoke(new MethodInvoker(() => { settings.Seccode = comboBoxSeccode.Text; }));
            checkBoxByMarket.Invoke(new MethodInvoker(() => { settings.ByMarket = checkBoxByMarket.Checked; }));
            radioButtonComboTypeContidion.Invoke(new MethodInvoker(() => { settings.ComboOrderType = radioButtonComboTypeContidion.Checked ? 1 : 2; }));
            comboBoxConnectionType.Invoke(new MethodInvoker(() => { settings.ConnectionType = comboBoxConnectionType.Text; }));
            dateTimePickerMultidirectExecute.Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
            checkBoxShutdown.Invoke(new MethodInvoker(() => { settings.MultidirectExecuteTime = dateTimePickerMultidirectExecute.Value; }));
        }

        public int Timezone { get { return int.Parse(comboBoxTimezone.SelectedItem.ToString()); } }
        public string ComboBoxConnectionType { get { return comboBoxConnectionType.Text; } }

        public string ComboBoxSeccode { get {

                string res = "";

                comboBoxSeccode.Invoke(new MethodInvoker(delegate { res = comboBoxSeccode.Text; }));

                return res; 
            } 
        }
        public string Username1 { get { return textBoxUsername.Text; } }
        public string Password1 { get { return textBoxPassword.Text; } }
        public string ClientId1 { set { textBoxClientId.Text = value; } }

        public string FreeMoney { set { textBoxFreeMoney.Text = value; } }
        public string FreeMoney1 { set { textBoxFreeMoney1.Text = value; } }
        public string FreeMoney2 { set { textBoxFreeMoney2.Text = value; } }

        public double ObserveDifference { 
            get
            {
                double res = 0;

                textBoxDifference.Invoke(new MethodInvoker(delegate { res = string.IsNullOrEmpty(textBoxDifference.Text) ? 0 : double.Parse(textBoxDifference.Text); }));

                return res;
            } 
        }

        public string Union1 { set { textBoxUnion.Text = value; } }

        public void LoadPositions(mc_portfolio portfolio)
        {
            dataGridViewPositions.BeginInvoke(new MethodInvoker(() =>
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
            comboBoxSeccode.SelectedItem = seccode;
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


        public new void Show()
        {
            _context.MainForm = this;
            Application.Run(_context);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void testButton_Click(object sender, EventArgs e)
        {
            _emailService.SendEmailAsync("m.rudneov@yandex.ru", "Autotrader", "test");
        }
    }
}
