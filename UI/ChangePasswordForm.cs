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
    public partial class ChangePasswordForm : Form
    {
        AutoTraderSDK.Kernel.TXMLConnectorWrapper _cl = null;

        public ChangePasswordForm(AutoTraderSDK.Kernel.TXMLConnectorWrapper cl)
        {
            InitializeComponent();

            _cl = cl;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                _cl.ChangePassword(textBoxPasswordOld.Text, textBoxPasswordNew.Text);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
