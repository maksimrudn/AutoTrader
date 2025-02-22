﻿using AutoTrader.Domain.Models.Strategies;
using AutoTrader.Domain.Models.Types;
using System;
using System.Collections.Generic;
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

        public int Difference { get { return int.Parse( textBoxDifference.Text); } }

        public SecurityPeriods Period { get { return (SecurityPeriods)int.Parse(textBoxPeriod.Text); } }

        public int Delay { get { return int.Parse(textBoxDelay.Text); } }

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
