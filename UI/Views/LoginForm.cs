using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class LoginForm : Form
    {
        AutoTraderSDK.Kernel.TXMLConnector _cl = null;

        public LoginForm(AutoTraderSDK.Kernel.TXMLConnector cl)
        {
            InitializeComponent();

            _cl = cl;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                _cl.Login(textBoxUsername.Text, textBoxPassword.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
