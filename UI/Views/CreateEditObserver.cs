﻿using AutoTraderUI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTraderUI.Views
{
    public partial class CreateEditObserver : Form
    {
        public CreateEditObserver(List<string> seccodes)
        {
            InitializeComponent();

            comboBoxSeccode.DataSource = seccodes;
        }
        public CreateEditObserver(List<string> seccodes,  StrategySettings observer)
        {
            InitializeComponent();

            comboBoxSeccode.DataSource = seccodes;

            comboBoxSeccode.SelectedItem = observer.Seccode;
        }


        //public event Action Ok;
        //public event Action Cancel;

        public string Seccode { get { return comboBoxSeccode.Text;  } }

        public string Difference { get { return textBoxDifference.Text; } }

        public string Period { get { return textBoxPeriod.Text; } }

        public string Delay { get { return textBoxDelay.Text; } }

        public NotificationTypes NotificationType { 
            get { 


                return radioButtonNotificationEmail.Checked? NotificationTypes.Email: NotificationTypes.File; 
            } 
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}