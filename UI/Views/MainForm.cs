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
using System.Windows.Forms;
using WindowsFormsApplication;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        TXMLConnector _cl1 = new TXMLConnector("txmlconnector1.dll");
        TXMLConnector _cl2 = new TXMLConnector("txmlconnector2.dll");

        
        public MainForm()
        {
            InitializeComponent();


            _handleDisconnected();
            _loadSettings();
        }

        private void _loadSeccodeSettings()
        {
            comboBoxSeccode.SelectedItem = Globals.Settings.Seccode;
        }

        private void _loadSettings()
        {
            textBoxUsername.Text = Globals.Settings.GetUsername();
            textBoxPassword.Text = Globals.Settings.GetPassword();
            textBoxTP.Text = Globals.Settings.TP.ToString();
            textBoxSL.Text = Globals.Settings.SL.ToString();
            textBoxPrice.Text = Globals.Settings.Price.ToString();
            checkBoxByMarket.Checked = Globals.Settings.ByMarket;
            textBoxVolume.Text = Globals.Settings.Volume.ToString();
            
        }

        private void _saveSettings()
        {
            Globals.Settings.SetUsername(textBoxUsername.Text);
            Globals.Settings.SetPassword(textBoxPassword.Text);
            Globals.Settings.TP = int.Parse(textBoxTP.Text);
            Globals.Settings.SL = int.Parse(textBoxSL.Text);
            Globals.Settings.Price = int.Parse(textBoxPrice.Text);
            Globals.Settings.Volume = int.Parse(textBoxVolume.Text);
            Globals.Settings.Seccode = comboBoxSeccode.Text;
            Globals.Settings.ByMarket = checkBoxByMarket.Checked;
            Globals.Settings.Save();
        }

        private void buttonComboBuy_Click(object sender, EventArgs e)
        {
            try
            {
                _handleComboOperation(buysell.B);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonComboSell_Click(object sender, EventArgs e)
        {
            try
            {
                _handleComboOperation(buysell.S);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _handleComboOperation(buysell buysell)
        {
            int sl = int.Parse(textBoxSL.Text.Trim());
            int tp = int.Parse(textBoxTP.Text.Trim());
            int price = int.Parse(textBoxPrice.Text.Trim());
            int vol = int.Parse(textBoxVolume.Text.Trim());
            bool bymarket = checkBoxByMarket.Checked;
            string seccode = comboBoxSeccode.Text;

            if (string.IsNullOrEmpty(seccode))
            {
                MessageBox.Show("Не выбран код инструмента");
            }
            else
            {
                _cl1.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, seccode, buysell, bymarket, price, vol, sl, tp);
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
                _cl1.Login(textBoxUsername.Text, textBoxPassword.Text);
                _handleConnected();
                
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

        private void _handleConnected()
        {
            comboBoxSeccode.DataSource = _cl1.GetSecurities().Where(x => x.board == boardsCode.FUT).Select(x => x.seccode).OrderBy(x => x).ToList();
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
        }
    }
}
