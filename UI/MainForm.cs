using AutoTraderSDK.Domain.OutputXML;
using AutoTraderSDK.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class MainForm : Form
    {
        TXMLConnectorWrapper _cl1 = TXMLConnectorWrapper.GetInstance();
        TXMLConnectorWrapper2 _cl2 = TXMLConnectorWrapper2.GetInstance();

        
        public MainForm()
        {
            InitializeComponent();

            var loginForm = new LoginForm(_cl1);
            loginForm.ShowDialog();
        }

        private void buttonCombo_Click(object sender, EventArgs e)
        {
            int sl = int.Parse(textBoxSL.Text.Trim());

            _cl1.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM6", buysell.B, 1, sl, 0);
            _cl2.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM6", buysell.S, 1, sl, 0);
        }

        private void buttonOldTest_Click(object sender, EventArgs e)
        {
            //_cl1.NewOrder(boardsCode.FUT, "SiM0", buysell.B, bymarket.yes, 66350, 1);


            _cl1.NewComboOrder(AutoTraderSDK.Domain.OutputXML.boardsCode.FUT, "SiM0", buysell.B, 1, 10, 0);
        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            var changePassForm = new ChangePasswordForm(_cl1);

            changePassForm.Show();
        }
    }
}
