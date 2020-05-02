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

            comboBoxSeccode.DataSource = _cl1.GetSecurities().Where(x=>x.board == boardsCode.FUT).Select(x=>x.seccode).ToList();
        }

        private void buttonCombo_Click(object sender, EventArgs e)
        {
            int sl = int.Parse(textBoxSL.Text.Trim());

            _cl1.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM6", buysell.B, 1, sl, 0);
        }

        private void buttonOldTest_Click(object sender, EventArgs e)
        {
            try
            {
                //_cl1.NewOrder(boardsCode.FUT, "SiM0", buysell.B, bymarket.yes, 66350, 1);


                _cl1.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM0", buysell.B, 1, 10, 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            var changePassForm = new ChangePasswordForm(_cl1);

            changePassForm.Show();
        }

        private void buttonComboSell_Click(object sender, EventArgs e)
        {

            //_cl2.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM6", buysell.S, 1, sl, 0);
        }
    }
}
