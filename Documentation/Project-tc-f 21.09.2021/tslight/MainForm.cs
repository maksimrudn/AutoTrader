using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;




namespace tslight
{


	/// Description of MainForm.
	public partial class MainForm : Form
	{
		public string AppDir; // путь к папке приложения
		public string sLogin; // логин пользователя для сервера Transaq
		public string sPassword; // пароль пользователя для сервера Transaq
		public string ServerIP; // IP адрес сервера Transaq
		public string ServerPort; // номер порта сервера Transaq
        public int session_timeout;
        public int request_timeout;

        private bool bConnected;
        private bool bConnecting;
		
		public DataSet_tslight DTS; // DataSet для таблиц с данными
		
		public char PointChar; // символ для замены точки в числах
        private bool PassHide = true; // флаг прячем ли пароль


        private event NewStringDataHandler onNewFormDataEvent;
        private event NewStringDataHandler onNewSecurityEvent;
        private event NewStringDataHandler onNewTimeframeEvent;
        private event NewBoolDataHandler onNewStatusEvent;

 
		//================================================================================
		public MainForm()
		{
			// определение папки, в которой запущена программа
			string path = Application.ExecutablePath;
			AppDir = path.Substring(0, path.LastIndexOf('\\')+1);

			// The InitializeComponent() call is required for Windows Forms designer support.
			InitializeComponent();

			// определение разделителя в числах на компьютере (запятая или точка)
			PointChar = ',';
			string str = (1.2).ToString();
			if (str.IndexOf('.') > 0) PointChar = '.';
		}

		//================================================================================
		void MainFormLoad(object sender, EventArgs e)
		{

			// параметры по умолчанию

           
            session_timeout = 25;
            request_timeout = 10;


			edt_Login.Text = sLogin;
			edt_Password.Text = sPassword;



            bConnected = false;
            bConnecting = false;

            Enable_Password_Controls(false);

            Init_Data();

            // открытие лог-файла
            log.StartLogging(AppDir + "log" + DateTime.Now.ToString("yyMMdd") + ".txt");

            TXmlConnector.statusTimeout = session_timeout * 1000;

            TXmlConnector.ConnectorSetCallback(OnNewFormData, OnNewSecurity, OnNewTimeframe, OnNewStatus);

            this.onNewFormDataEvent += new NewStringDataHandler(Add_FormData);
            this.onNewSecurityEvent += new NewStringDataHandler(Add_Security);
            this.onNewTimeframeEvent += new NewStringDataHandler(Add_Timeframe);
            this.onNewStatusEvent += new NewBoolDataHandler(ConnectionStatusReflect);

            TXmlConnector.FormReady = true;

            string LogPath = AppDir + "\0";

            if (TXmlConnector.ConnectorInitialize(LogPath, 3)) TXmlConnector.statusDisconnected.Set();
           
		}

        private void OnNewFormData(string data)
        {
            this.Invoke(onNewFormDataEvent, new object[] { data }); 
        }
        private void OnNewSecurity(string data)
        {
            this.Invoke(onNewSecurityEvent, new object[] { data }); 
        }
        private void OnNewTimeframe(string data)
        {
            this.Invoke(onNewTimeframeEvent, new object[] { data }); 
        }
        private void OnNewStatus(bool status)
        {
            this.Invoke(onNewStatusEvent, new object[] { status }); 
        }

		//================================================================================
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
            TXmlConnector.FormReady = false;

            if (bConnected || bConnecting)
			{
				Transaq_Disconnect();
			}

            TXmlConnector.ConnectorUnInitialize();

            log.StopLogging();
		}

		//================================================================================
		public void ShowStatus(string status_str)
		{
			// вывод сообщения в строке статуса формы
			txt_Status.Text = status_str;
			txt_Status.Refresh();
		}
		
		//================================================================================
		public void Init_Data()
		{
			// создание объекта DataSet с таблицами 
			DTS = new DataSet_tslight();
		}
        //================================================================================
        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            switch (chs_Server.SelectedIndex)
            {
                case 0:
                    edt_Port.Text = "3900";
                    break;
                case 1:
                    edt_Port.Text = "3324";
                    break;
                case 2:
                    edt_Port.Text = "3939";
                    break;
            }
        }
        void Transaq_Connect()
		{
            // чтение параметров из формы
            switch (chs_Server.Text)
            {
                case "АО \"Финам\"":
                    ServerIP = "tr1.finam.ru";
                    break;
                case "АО \"Банк Финам\"":
                    ServerIP = "tr1.finambank.ru";
                    break;
                case "Демо":
                    ServerIP = "tr1-demo5.finam.ru";
                    break;
            }
            if (chs_Server.Text != "АО \"Финам\"" && chs_Server.Text != "АО \"Банк Финам\"" && chs_Server.Text != "Демо" && chs_Server.Text != "tr1.finam.ru" && chs_Server.Text != "tr1.finambank.ru" && chs_Server.Text != "tr1-demo5.finam.ru")
            {
                ServerIP = chs_Server.Text;
            }

            sLogin = edt_Login.Text;
			sPassword = edt_Password.Text;
			ServerPort = edt_Port.Text;
            			
			// проверка наличия параметров
			if (sLogin.Length == 0)
			{
				ShowStatus("Не указан логин");
				return;
			}
			if (sPassword.Length == 0)
			{
				ShowStatus("Не указан пароль");
				return;
			}
			if (ServerIP.Length == 0)
			{
				ShowStatus("Не указан IP-адрес");
				return;
			}
			if (ServerPort.Length == 0)
			{
				ShowStatus("Не указан порт");
				return;
			}

            ConnectingReflect();
            // очистка таблиц с данными
            DTS.t_timeframe.Clear();
			DTS.t_security.Clear();
            DTS.t_candle.Clear();
		
			// формирование текста команды
			string cmd = "<command id=\"connect\">";
			cmd = cmd + "<login>" + sLogin + "</login>";
			cmd = cmd + "<password>" + sPassword + "</password>";
			cmd = cmd + "<host>" + ServerIP + "</host>";
			cmd = cmd + "<port>" + ServerPort + "</port>";
            cmd = cmd + "<rqdelay>100</rqdelay>";
            cmd = cmd + "<session_timeout>" + session_timeout.ToString() + "</session_timeout> ";
            cmd = cmd + "<request_timeout>" + request_timeout.ToString() + "</request_timeout>"; 
			cmd = cmd + "</command>";

			// отправка команды
            log.WriteLog("SendCommand: <command id=\"connect\"><login>" + sLogin + "</login><password>*</password><host>" + ServerIP + "</host><port>" + ServerPort + "</port><logsdir>" + AppDir + "</logsdir><rqdelay>100</rqdelay></command>");
            TXmlConnector.statusDisconnected.Reset();
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);
        }
		
		//================================================================================
		void Transaq_Disconnect()
		{
			// отключение коннектора от сервера Транзак


            DisconnectingReflect();
			
			string cmd = "<command id=\"disconnect\"/>";

            log.WriteLog("SendCommand: " + cmd);
            TXmlConnector.statusDisconnected.Reset();
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);
		}

		//================================================================================
		void Get_Transaq_History(int SecurityID, int TimeframeID, int HistoryLength, bool ResetFlag)
		{
			// запрос исторических данных для инструмента
			string cmd = "<command id=\"gethistorydata\" ";
			cmd = cmd + "secid=\"" + SecurityID.ToString() + "\" ";
			cmd = cmd + "period=\"" + TimeframeID.ToString() + "\" ";
			cmd = cmd + "count=\"" + HistoryLength.ToString() + "\" ";
			string s = "false";
			if (ResetFlag) s = "true";
			cmd = cmd + "reset=\""+s+"\"/>";

            log.WriteLog("SendCommand: " + cmd);
            string res = TXmlConnector.ConnectorSendCommand(cmd);
            log.WriteLog("ServerReply: " + res);
        }

        void ConnectingReflect()
        {
            bConnecting = true;
            txt_Connect.Text = "connecting";
            btn_Connect.Text = "Подключаю";
            btn_Connect.Refresh();
            txt_Connect.Refresh();
            ShowStatus("Подключение к серверу...");
         }

        void DisconnectingReflect()
        {
            bConnecting = false;
            txt_Connect.Text = "disconnecting";
            btn_Connect.Text = "Отключаю";
            btn_Connect.Refresh();
            txt_Connect.Refresh();
            ShowStatus("Отключение от сервера...");
        }

            
		//================================================================================
		void ConnectionStatusReflect(bool connected)
		{
			// отображение состояния подключения на форме

            bConnected = connected; 
            bConnecting = false;

            if (connected)
                {
                    dg_Security.DataSource = DTS.t_security;
                    dg_Security.Refresh();

                    txt_Connect.Text = "online";
                    btn_Connect.Text = "Отключить";
                    ShowStatus("Подключение установлено");
                    Enable_Password_Controls(true);
                    
                }
                else
                {

                    DTS.t_timeframe.Clear();
                    DTS.t_security.Clear();
                    DTS.t_candle.Clear();
                    dg_Security.Refresh();

                    txt_Connect.Text = "offline";
			        btn_Connect.Text  = "Подключить";
			        ShowStatus("Подключение разъединено");
                    Enable_Password_Controls(false);
                }

 
			btn_Connect.Refresh();
			txt_Connect.Refresh();
		}

        public void Add_FormData(string data)
        {
            txtBoxAns.Text = txtBoxAns.Text + DateTime.Now.ToString("HH:mm:ss.fff") + "   " + data + "\n =================================================== \n";
        }

		//================================================================================
		public void Add_Timeframe(string data)
		{
			// добавление записи о таймфрейме
			string[] tf = data.Split(';');
			if (tf.Length < 3) return;
			
			int id = int.Parse(tf[0]);
			int length = int.Parse(tf[1]);
			string name = tf[2];
			
			DTS.t_timeframe.Add_Row(id, length, name);
		}
		
		//================================================================================
		public void Add_Security(string data)
		{
			// добавление записи об инструменте
			string[] sec = data.Split(';');
			if (sec.Length < 18) return;
			int id = int.Parse(sec[0]);
			string code = sec[2];
            DTS.t_security.Add_Row(id, code);
		}
		
		
		
		
		//================================================================================
		void btn_Connect_Click(object sender, EventArgs e)
		{
			// если подключен или подключается
            if (bConnected || bConnecting)
			{
				Transaq_Disconnect();
			}
			else
			{
                if (!bConnecting) Transaq_Connect();
			}
			
		}

        void Enable_Password_Controls(bool bEnable)
        {

            // установка состояния элементов управления для смены пароля
            txtOldPass.Enabled = bEnable;
            txtNewPass.Enabled = bEnable;
            checkHide.Enabled = bEnable;
            btnPassChange.Enabled = bEnable;
        }

        private void btnPassChange_Click(object sender, EventArgs e)
        {
            // Проверяем пароль
            // Правило: только латинские буквы и цифры, минимум 4, максимум 19
            string passPattern = @"^[a-zA-Z0-9]{4,20}$";

            string oldPass = txtOldPass.Text;
            string newPass = txtNewPass.Text;

            string result;
            Match m = Regex.Match(newPass, passPattern);

            if (!bConnected)
            {
                ShowStatus("Для смены пароля подключитесь к серверу");
            }
            else if (txtNewPass.Text.Length == 0)
            {
                ShowStatus("Введите новый пароль");
            }
            else if (m.Success)
            {
                string cmd = String.Format("<command id=\"change_pass\" oldpass=\"{0}\" newpass=\"{1}\" />", oldPass, newPass);
                log.WriteLog("SendCommand: <command id=\"change_pass\" oldpass=\"*\" newpass=\"*\" />");
                result = TXmlConnector.ConnectorSendCommand(cmd);
                log.WriteLog("ServerReply: " + result);
                ShowStatus(result);
                XDocument aXmlDoc = XDocument.Load(new System.IO.StringReader(result));
                if (aXmlDoc.Root.Name == "result")
                {
                    if (aXmlDoc.Root.Attribute("success").Value == "true")
                    {
                        ShowStatus("Пароль изменен");
                        log.WriteLog("Password was changed");
                        txtOldPass.Text = "";
                        txtNewPass.Text = "";
                    }
                    else if (aXmlDoc.Root.Attribute("success").Value == "false")
                    {
                        ShowStatus(aXmlDoc.Root.Value);
                    }
                }
                else
                {
                    ShowStatus("Произошла ошибка");
                }
            }
            else
            {
                ShowStatus("Введите верный новый пароль");
            }

        }

        private void checkHide_CheckedChanged(object sender, EventArgs e)
        {
            PassHide = checkHide.Checked;
            if (PassHide)
            {
                txtNewPass.PasswordChar = '*';
            }
            else
            {
                txtNewPass.PasswordChar = (char)0;
            }

        }


    }
}
