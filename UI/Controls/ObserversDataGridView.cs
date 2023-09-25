using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoTraderUI.Core;

namespace AutoTraderUI.Controls
{
    public partial class ObserversDataGridView : UserControl //, IObserversDataGridView
    {
        public ObserversDataGridView()
        {
            InitializeComponent();            

            DataGridViewTextBoxColumn seccodeColumn = new DataGridViewTextBoxColumn();
            seccodeColumn.HeaderText = "Seccode";
            seccodeColumn.Name = "Seccode";
            seccodeColumn.ReadOnly = true;
            DataGridViewTextBoxColumn differenceColumn = new DataGridViewTextBoxColumn();
            differenceColumn.HeaderText = "Difference";
            differenceColumn.Name = "Difference";
            differenceColumn.ReadOnly = true;
            DataGridViewTextBoxColumn periodColumn = new DataGridViewTextBoxColumn();
            periodColumn.HeaderText = "Difference";
            periodColumn.Name = "Difference";
            periodColumn.ReadOnly = true;
            DataGridViewTextBoxColumn delayColumn = new DataGridViewTextBoxColumn();
            delayColumn.HeaderText = "Delay";
            delayColumn.Name = "Delay";
            delayColumn.ReadOnly = true;
            DataGridViewManageButtonColumn startButtonColumn = new DataGridViewManageButtonColumn();
            startButtonColumn.HeaderText = "Start";
            startButtonColumn.Text = "Start";
            startButtonColumn.UseColumnTextForButtonValue = true;
            DataGridViewManageButtonColumn stopButtonColumn = new DataGridViewManageButtonColumn();
            stopButtonColumn.HeaderText = "Stop";
            stopButtonColumn.Text = "Stop";
            stopButtonColumn.UseColumnTextForButtonValue = true;
            DataGridViewManageButtonColumn configButtonColumn = new DataGridViewManageButtonColumn();
            configButtonColumn.HeaderText = "Config";
            configButtonColumn.Text = "Config";
            configButtonColumn.UseColumnTextForButtonValue = true;
            DataGridViewManageButtonColumn deleteButtonColumn = new DataGridViewManageButtonColumn();
            deleteButtonColumn.HeaderText = "Remove";
            deleteButtonColumn.Text = "Remove";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            DataGridViewManageButtonColumn logButtonColumn = new DataGridViewManageButtonColumn();
            logButtonColumn.HeaderText = "Log";
            logButtonColumn.Text = "Log";
            logButtonColumn.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(seccodeColumn);
            dataGridView1.Columns.Add(differenceColumn);
            dataGridView1.Columns.Add(periodColumn);
            dataGridView1.Columns.Add(delayColumn);
            dataGridView1.Columns.Add(startButtonColumn);
            dataGridView1.Columns.Add(stopButtonColumn);
            dataGridView1.Columns.Add(configButtonColumn);
            dataGridView1.Columns.Add(deleteButtonColumn);
            dataGridView1.Columns.Add(logButtonColumn);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            int totalWidth = dataGridView1.Columns[0].Width +
                            dataGridView1.Columns[1].Width +
                            dataGridView1.Columns[2].Width +
                            dataGridView1.Columns[3].Width +
                            dataGridView1.Columns[4].Width +
                            dataGridView1.Columns[5].Width;
            Width = totalWidth;

            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
        }


        public event EventHandler<ObserversDataGridViewEventArgs> StartEvent;

        public event EventHandler<ObserversDataGridViewEventArgs> StopEvent;

        public event EventHandler<ObserversDataGridViewEventArgs> ConfigEvent;

        public event EventHandler<ObserversDataGridViewEventArgs> LogEvent;

        public event EventHandler<ObserversDataGridViewEventArgs> DeleteEvent;

        public void Add(StrategySettings observer)
        {
            dataGridView1.Rows.Add( observer.Seccode, observer.Difference, observer.Delay, observer.Period);
        }

        private bool AccountAlreadyExist(string username)
        {
            bool res = false;

            foreach (DataGridViewRow accountRow in dataGridView1.Rows)
            {
                if (accountRow.Cells[0].Value.ToString() == username)
                {
                    res = true;
                    break;
                }
            }

            return res;
        }

        public void LoadObservers(List<StrategySettings> observerList)
        {
            dataGridView1.Rows.Clear();

            foreach (StrategySettings observer in observerList)
            {
                Add(observer);
            }

            dataGridView1.Invalidate();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //ЗАПУСТИТЬ 
            if (e.ColumnIndex == 1)
            {
                if (StartEvent != null && e.RowIndex != -1 && !IsStarted(e.RowIndex))
                    StartEvent.Invoke(this, new ObserversDataGridViewEventArgs()
                    {
                        Action = StrategiesActions.Start,
                        Security = GetUsernameByIndex(e.RowIndex)
                    });
            }
            //ОСТАНОВИТЬ
            else if (e.ColumnIndex == 2)
            {
                if (StopEvent != null && e.RowIndex != -1 && IsStarted(e.RowIndex))
                    StopEvent.Invoke(this, new ObserversDataGridViewEventArgs()
                    {
                        Action = StrategiesActions.Stop,
                        Security = GetUsernameByIndex(e.RowIndex)
                    });
            }
            //ИЗМЕНИТЬ НАСТРОЙКИ АККАУНТА
            else if (e.ColumnIndex == 3 && !IsStarted(e.RowIndex))
            {
                if (ConfigEvent != null && e.RowIndex != -1)
                    ConfigEvent.Invoke(this, new ObserversDataGridViewEventArgs()
                    {
                        Action = StrategiesActions.Config,
                        Security = GetUsernameByIndex(e.RowIndex)
                    });
            }
            //УДАЛИТЬ АККАУНТ
            else if (e.ColumnIndex == 4 && !IsStarted(e.RowIndex))
            {             

                if (DeleteEvent != null && e.RowIndex!=-1)
                    DeleteEvent.Invoke(this, new ObserversDataGridViewEventArgs()
                    {
                        Action = StrategiesActions.Delete,
                        Security = GetUsernameByIndex(e.RowIndex)
                    });

                if (e.RowIndex != -1)
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
            //ПРОСМОТРЕТЬ ЛОГ
            else if (e.ColumnIndex == 5)
            {
                if (LogEvent != null && e.RowIndex != -1)
                    LogEvent.Invoke(this, new ObserversDataGridViewEventArgs()
                    {
                        Action = StrategiesActions.Log,
                        Security = GetUsernameByIndex(e.RowIndex)
                    });
            }
        }

        private bool IsStarted(int index)
        {
            DataGridViewManageButtonCell button = dataGridView1.Rows[index].Cells[1] as DataGridViewManageButtonCell;

            if (button.Enabled)
                return false;
            else
                return true;
        }

        private string GetSelectedUsername()
        {
            return GetUsernameByIndex(GetSelectedIndex());
        }

        private string GetUsernameByIndex(int i)
        {
            return dataGridView1.Rows[i].Cells[0].Value.ToString();
        }

        private int GetIndexByUsername(string username)
        {
            int res = -1;

            for (int i = 0; i< dataGridView1.Rows.Count; i++)
                if ((dataGridView1.Rows[i].Cells[0].Value as string) == username)
                {
                    res = i;
                    break;
                }

            return res;
        }



        private int GetSelectedIndex()
        {
            return dataGridView1.SelectedRows[0].Index;
        }

        public void SwitchStateToStarted(string username)
        {
            DataGridViewRow row = dataGridView1.Rows[GetIndexByUsername(username)];

            (row.Cells[1] as DataGridViewManageButtonCell).Enabled = false;
            (row.Cells[2] as DataGridViewManageButtonCell).Enabled = true;
            (row.Cells[3] as DataGridViewManageButtonCell).Enabled = false;
            (row.Cells[4] as DataGridViewManageButtonCell).Enabled = false;

            dataGridView1.Invalidate();
        }

        public void SwitchStateToStoped(string username)
        {
            DataGridViewRow row = dataGridView1.Rows[GetIndexByUsername(username)];

            (row.Cells[1] as DataGridViewManageButtonCell).Enabled = true;
            (row.Cells[2] as DataGridViewManageButtonCell).Enabled = false;
            (row.Cells[3] as DataGridViewManageButtonCell).Enabled = true;
            (row.Cells[4] as DataGridViewManageButtonCell).Enabled = true;

            dataGridView1.Invalidate();
        }

        
    }
}
