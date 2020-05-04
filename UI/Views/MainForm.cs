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

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        TXMLConnector _cl1 = new TXMLConnector("txmlconnector1.dll");
        TXMLConnector _cl2 = new TXMLConnector("txmlconnector2.dll");

        
        public MainForm()
        {
            InitializeComponent();

            var loginForm = new LoginForm(_cl1);
            loginForm.ShowDialog();

            comboBoxSeccode.DataSource = _cl1.GetSecurities().Where(x=>x.board == boardsCode.FUT).Select(x=>x.seccode).OrderBy(x=>x).ToList();
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

        

        private void buttonOldTest_Click(object sender, EventArgs e)
        {
           

        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            var changePassForm = new ChangePasswordForm(_cl1);

            changePassForm.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cl1.Dispose();
        }
    }
}
