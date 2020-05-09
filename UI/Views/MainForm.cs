using AutoTraderSDK.Domain;
using AutoTraderSDK.Domain.InputXML;
using AutoTraderSDK.Domain.OutputXML;
using AutoTraderSDK.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApplication;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        TXMLConnector _cl1 = new TXMLConnector("txmlconnector1.dll");
        TXMLConnector _cl2 = new TXMLConnector("txmlconnector2.dll");

        System.Timers.Timer _timerClock;
        public MainForm()
        {
            InitializeComponent();

            comboBoxConnectionType.DataSource = new List<string> { string.Empty, "Prod", "Demo" };

            _handleDisconnected();
            _loadSettings();

            _timerClock = new System.Timers.Timer();
            _timerClock.Interval = 100;
            _timerClock.Elapsed += new ElapsedEventHandler(timerClock_Elapsed);
            _timerClock.Start();
        }

        private void timerClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            labelTime.BeginInvoke(new MethodInvoker(() =>
            {
                labelTime.Text = $"Time: {DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
            }));

        }

        private void _loadSeccodeSettings()
        {
            comboBoxSeccode.SelectedItem = Globals.Settings.Seccode;
        }

        private void _loadSettings()
        {
            textBoxUsername.Text = Globals.Settings.GetUsername();
            textBoxPassword.Text = Globals.Settings.GetPassword();
            textBoxUsername2.Text = Globals.Settings.GetUsername2();
            textBoxPassword2.Text = Globals.Settings.GetPassword2();
            textBoxTP.Text = Globals.Settings.TP.ToString();
            textBoxSL.Text = Globals.Settings.SL.ToString();
            textBoxPrice.Text = Globals.Settings.Price.ToString();
            checkBoxByMarket.Checked = Globals.Settings.ByMarket;
            textBoxVolume.Text = Globals.Settings.Volume.ToString();
            radioButtonComboTypeContidion.Checked = Globals.Settings.ComboOrderType == 1;
            radioButtonComboTypeStop.Checked = Globals.Settings.ComboOrderType == 2;
            comboBoxConnectionType.SelectedItem = Globals.Settings.ConnectionType;
            
        }

        private void _saveSettings()
        {
            Globals.Settings.SetUsername(textBoxUsername.Text);
            Globals.Settings.SetPassword(textBoxPassword.Text);
            Globals.Settings.SetUsername2(textBoxUsername2.Text);
            Globals.Settings.SetPassword2(textBoxPassword2.Text);
            Globals.Settings.TP = int.Parse(textBoxTP.Text);
            Globals.Settings.SL = int.Parse(textBoxSL.Text);
            Globals.Settings.Price = int.Parse(textBoxPrice.Text);
            Globals.Settings.Volume = int.Parse(textBoxVolume.Text);
            Globals.Settings.Seccode = comboBoxSeccode.Text;
            Globals.Settings.ByMarket = checkBoxByMarket.Checked;
            Globals.Settings.ComboOrderType = radioButtonComboTypeContidion.Checked ? 1 : 2;
            Globals.Settings.ConnectionType = comboBoxConnectionType.Text;
            Globals.Settings.Save();
        }

        private void buttonComboBuy_Click(object sender, EventArgs e)
        {
            ComboOrder co = new ComboOrder();
            co.SL = int.Parse(textBoxSL.Text.Trim());
            co.TP = int.Parse(textBoxTP.Text.Trim());
            co.Price = int.Parse(textBoxPrice.Text.Trim());
            co.Vol = int.Parse(textBoxVolume.Text.Trim());
            co.ByMarket = checkBoxByMarket.Checked;
            co.Seccode = comboBoxSeccode.Text;
            co.BuySell = buysell.B;

            try
            {
                _handleComboOperation(_cl1, co);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonComboSell_Click(object sender, EventArgs e)
        {
            ComboOrder co = new ComboOrder();
            co.SL = int.Parse(textBoxSL.Text.Trim());
            co.TP = int.Parse(textBoxTP.Text.Trim());
            co.Price = int.Parse(textBoxPrice.Text.Trim());
            co.Vol = int.Parse(textBoxVolume.Text.Trim());
            co.ByMarket = checkBoxByMarket.Checked;
            co.Seccode = comboBoxSeccode.Text;
            co.BuySell = buysell.S;

            try
            {
                _handleComboOperation(_cl1, co);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _handleComboOperation(TXMLConnector cl, ComboOrder co )
        {

            if (string.IsNullOrEmpty(co.Seccode))
            {
                MessageBox.Show("Не выбран код инструмента");
            }
            else
            {
                cl.NewComboOrder(co);
            }
        }



        

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _saveSettings();
            _cl1.Dispose();
        }

        

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (comboBoxConnectionType.Text == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), comboBoxConnectionType.Text);

                _cl1.Login(textBoxUsername.Text, textBoxPassword.Text, connType);
                _handleConnected();

                textBoxClientId.Text = _cl1.FortsClientId;
                textBoxFreeMoney.Text = _cl1.Money.ToString();
                textBoxUnion.Text = _cl1.Union;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLogin2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxConnectionType.Text == string.Empty)
                    throw new Exception("Не выбран режим доступа Демо/Прод");

                ConnectionType connType = (ConnectionType)Enum.Parse(typeof(ConnectionType), comboBoxConnectionType.Text);


                _cl2.Login(textBoxUsername2.Text, textBoxPassword2.Text, connType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            try
            {
                _cl1.Logout();
                _handleDisconnected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                _cl1.ChangePassword(textBoxPasswordOld.Text, textBoxPasswordNew.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonChangePassword2_Click(object sender, EventArgs e)
        {
            try
            {
                _cl2.ChangePassword(textBoxPasswordOld2.Text, textBoxPasswordNew2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _handleConnected()
        {
            List<string> seccodes = new List<string>()
            {
                string.Empty
            };

            seccodes.AddRange(_cl1.GetSecurities().Where(x => x.board == boardsCode.FUT).Select(x => x.seccode).OrderBy(x => x).ToList());

            comboBoxSeccode.DataSource = seccodes;
            _loadSeccodeSettings();

            ((Control)tabPage1).Enabled = true;
            groupBoxChangePassword.Enabled = true;
            buttonLogin.Enabled = false;
            buttonLogout.Enabled = true;
        }

        private void _handleDisconnected()
        {
            ((Control)tabPage1).Enabled = false;
            groupBoxChangePassword.Enabled = false;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;

            textBoxUnion.Text = string.Empty;
            textBoxClientId.Text = string.Empty;
            textBoxFreeMoney.Text = string.Empty;
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void buttonMakeMultidirect_Click(object sender, EventArgs e)
        {
            if (!_cl1.Connected || !_cl2.Connected)
            {
                MessageBox.Show("Не все клиенты авторизованы!");
                return;
            }

            if (comboBoxSeccode.Text == string.Empty)
            {
                MessageBox.Show("Не выбран код инструмента");
                return;
            }

            ComboOrder comboOrder1 = new ComboOrder();
            comboOrder1.Board = boardsCode.FUT;
            comboOrder1.SL = int.Parse(textBoxSL.Text.Trim());
            comboOrder1.TP = int.Parse(textBoxTP.Text.Trim());
            comboOrder1.Price = int.Parse(textBoxPrice.Text.Trim());
            comboOrder1.Vol = int.Parse(textBoxVolume.Text.Trim());
            comboOrder1.ByMarket = checkBoxByMarket.Checked;
            comboOrder1.Seccode = comboBoxSeccode.Text;
            comboOrder1.BuySell = buysell.B;

            ComboOrder comboOrder2 = (ComboOrder)comboOrder1.Clone();
            comboOrder2.BuySell = buysell.S;

            Task md1 = Task.Run(() =>
            {
                _handleComboOperation(_cl1, comboOrder1);
            });

            Task md2 = Task.Run(() =>
            {
                _handleComboOperation(_cl2, comboOrder2);
            });

            md1.Wait();
            md2.Wait();
        }




        System.Timers.Timer _timerMultidirect;

        private void buttonStartMultidirectTimer_Click(object sender, EventArgs e)
        {
            _timerMultidirect = new System.Timers.Timer();
            _timerMultidirect.Interval = 100;
            _timerMultidirect.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timerMultidirect.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == dateTimePickerMultidirectExecute.Value.Hour && 
                DateTime.Now.Minute == dateTimePickerMultidirectExecute.Value.Minute &&
                DateTime.Now.Second == dateTimePickerMultidirectExecute.Value.Second)
            {
                _timerMultidirect.Stop();
                buttonMakeMultidirect_Click(null, null);
            }
        }
    }
}
