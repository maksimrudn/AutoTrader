namespace AutoTraderUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected volatile bool _disposed = false;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {                
                _timerClock.Dispose();
                _ct_timerMultidirectOrder.Dispose();

                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                _disposed = true;
                base.Dispose(disposing);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxSL = new System.Windows.Forms.TextBox();
            buttonComboBuy = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            checkBoxByMarket = new System.Windows.Forms.CheckBox();
            textBoxPrice = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            buttonComboSell = new System.Windows.Forms.Button();
            dataGridViewFortsPositions = new System.Windows.Forms.DataGridView();
            button1 = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            textBoxVolume = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            textBoxTP = new System.Windows.Forms.TextBox();
            buttonChangePassword = new System.Windows.Forms.Button();
            comboBoxSeccode = new System.Windows.Forms.ComboBox();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            buttonQuotationsUnSubscribe = new System.Windows.Forms.Button();
            buttonQuotationsSubscribe = new System.Windows.Forms.Button();
            checkBoxShutdown = new System.Windows.Forms.CheckBox();
            label7 = new System.Windows.Forms.Label();
            dateTimePickerMultidirectExecute = new System.Windows.Forms.DateTimePicker();
            buttonStartMultidirectTimer = new System.Windows.Forms.Button();
            buttonMakeMultidirect = new System.Windows.Forms.Button();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radioButtonComboTypeStop = new System.Windows.Forms.RadioButton();
            radioButtonComboTypeContidion = new System.Windows.Forms.RadioButton();
            textBoxDifference = new System.Windows.Forms.TextBox();
            labelDifference = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            groupBox4 = new System.Windows.Forms.GroupBox();
            textBoxUnion = new System.Windows.Forms.TextBox();
            label17 = new System.Windows.Forms.Label();
            textBoxFreeMoney = new System.Windows.Forms.TextBox();
            label16 = new System.Windows.Forms.Label();
            textBoxClientId = new System.Windows.Forms.TextBox();
            label15 = new System.Windows.Forms.Label();
            groupBoxChangePassword = new System.Windows.Forms.GroupBox();
            textBoxPasswordNew = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            textBoxPasswordOld = new System.Windows.Forms.TextBox();
            label12 = new System.Windows.Forms.Label();
            groupBoxLogin = new System.Windows.Forms.GroupBox();
            label18 = new System.Windows.Forms.Label();
            comboBoxConnectionType = new System.Windows.Forms.ComboBox();
            buttonLogout = new System.Windows.Forms.Button();
            buttonLogin = new System.Windows.Forms.Button();
            textBoxPassword = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            textBoxUsername = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            tabPage3 = new System.Windows.Forms.TabPage();
            groupBoxChangePassword2 = new System.Windows.Forms.GroupBox();
            textBoxPasswordNew2 = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBoxPasswordOld2 = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            buttonChangePassword2 = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            buttonLogout2 = new System.Windows.Forms.Button();
            buttonLogin2 = new System.Windows.Forms.Button();
            textBoxPassword2 = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            textBoxUsername2 = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            tabPage4 = new System.Windows.Forms.TabPage();
            groupBox5 = new System.Windows.Forms.GroupBox();
            dataGridViewPositions = new System.Windows.Forms.DataGridView();
            Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            BalancePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Vol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            PNL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox2 = new System.Windows.Forms.GroupBox();
            textBoxFreeMoney2 = new System.Windows.Forms.TextBox();
            label20 = new System.Windows.Forms.Label();
            textBoxFreeMoney1 = new System.Windows.Forms.TextBox();
            label19 = new System.Windows.Forms.Label();
            tabPage5 = new System.Windows.Forms.TabPage();
            buttonStopAll = new System.Windows.Forms.Button();
            buttonRunAll = new System.Windows.Forms.Button();
            groupBoxObservers = new System.Windows.Forms.GroupBox();
            buttonAddObserver = new System.Windows.Forms.Button();
            observersDataGridView = new Controls.ObserversDataGridView();
            testButton = new System.Windows.Forms.Button();
            labelTime = new System.Windows.Forms.Label();
            comboBoxTimezone = new System.Windows.Forms.ComboBox();
            label21 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFortsPositions).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBoxChangePassword.SuspendLayout();
            groupBoxLogin.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBoxChangePassword2.SuspendLayout();
            groupBox3.SuspendLayout();
            tabPage4.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPositions).BeginInit();
            groupBox2.SuspendLayout();
            tabPage5.SuspendLayout();
            groupBoxObservers.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxSL
            // 
            textBoxSL.Location = new System.Drawing.Point(63, 223);
            textBoxSL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxSL.Name = "textBoxSL";
            textBoxSL.Size = new System.Drawing.Size(116, 23);
            textBoxSL.TabIndex = 0;
            textBoxSL.Text = "0";
            // 
            // buttonComboBuy
            // 
            buttonComboBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonComboBuy.Location = new System.Drawing.Point(413, 98);
            buttonComboBuy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonComboBuy.Name = "buttonComboBuy";
            buttonComboBuy.Size = new System.Drawing.Size(140, 83);
            buttonComboBuy.TabIndex = 2;
            buttonComboBuy.Text = "COMBO BUY";
            buttonComboBuy.UseVisualStyleBackColor = true;
            buttonComboBuy.Click += buttonComboBuy_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(10, 13);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(110, 13);
            label2.TabIndex = 4;
            label2.Text = "FORTS SECCODE";
            // 
            // checkBoxByMarket
            // 
            checkBoxByMarket.AutoSize = true;
            checkBoxByMarket.Checked = true;
            checkBoxByMarket.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxByMarket.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            checkBoxByMarket.Location = new System.Drawing.Point(187, 195);
            checkBoxByMarket.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxByMarket.Name = "checkBoxByMarket";
            checkBoxByMarket.Size = new System.Drawing.Size(83, 17);
            checkBoxByMarket.TabIndex = 11;
            checkBoxByMarket.Text = "By Market";
            checkBoxByMarket.UseVisualStyleBackColor = true;
            // 
            // textBoxPrice
            // 
            textBoxPrice.Location = new System.Drawing.Point(63, 193);
            textBoxPrice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPrice.Name = "textBoxPrice";
            textBoxPrice.Size = new System.Drawing.Size(116, 23);
            textBoxPrice.TabIndex = 12;
            textBoxPrice.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label8.Location = new System.Drawing.Point(10, 197);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(36, 13);
            label8.TabIndex = 13;
            label8.Text = "Price";
            // 
            // buttonComboSell
            // 
            buttonComboSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonComboSell.Location = new System.Drawing.Point(564, 98);
            buttonComboSell.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonComboSell.Name = "buttonComboSell";
            buttonComboSell.Size = new System.Drawing.Size(140, 83);
            buttonComboSell.TabIndex = 22;
            buttonComboSell.Text = "COMBO SELL";
            buttonComboSell.UseVisualStyleBackColor = true;
            buttonComboSell.Click += buttonComboSell_Click;
            // 
            // dataGridViewFortsPositions
            // 
            dataGridViewFortsPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFortsPositions.Location = new System.Drawing.Point(14, 369);
            dataGridViewFortsPositions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridViewFortsPositions.Name = "dataGridViewFortsPositions";
            dataGridViewFortsPositions.Size = new System.Drawing.Size(698, 99);
            dataGridViewFortsPositions.TabIndex = 21;
            // 
            // button1
            // 
            button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            button1.Location = new System.Drawing.Point(14, 300);
            button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(700, 62);
            button1.TabIndex = 20;
            button1.Text = "CLOSE ALL FUTURES POSITIONS";
            button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(281, 196);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(31, 13);
            label1.TabIndex = 19;
            label1.Text = "VOL";
            // 
            // textBoxVolume
            // 
            textBoxVolume.Location = new System.Drawing.Point(334, 192);
            textBoxVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxVolume.Name = "textBoxVolume";
            textBoxVolume.Size = new System.Drawing.Size(72, 23);
            textBoxVolume.TabIndex = 18;
            textBoxVolume.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(10, 227);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(22, 13);
            label10.TabIndex = 17;
            label10.Text = "SL";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(10, 167);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(23, 13);
            label9.TabIndex = 15;
            label9.Text = "TP";
            // 
            // textBoxTP
            // 
            textBoxTP.Location = new System.Drawing.Point(63, 163);
            textBoxTP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxTP.Name = "textBoxTP";
            textBoxTP.Size = new System.Drawing.Size(116, 23);
            textBoxTP.TabIndex = 14;
            textBoxTP.Text = "0";
            // 
            // buttonChangePassword
            // 
            buttonChangePassword.Location = new System.Drawing.Point(354, 20);
            buttonChangePassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonChangePassword.Name = "buttonChangePassword";
            buttonChangePassword.Size = new System.Drawing.Size(392, 55);
            buttonChangePassword.TabIndex = 16;
            buttonChangePassword.Text = "Change password";
            buttonChangePassword.UseVisualStyleBackColor = true;
            buttonChangePassword.Click += buttonChangePassword_Click;
            // 
            // comboBoxSeccode
            // 
            comboBoxSeccode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSeccode.FormattingEnabled = true;
            comboBoxSeccode.Location = new System.Drawing.Point(14, 35);
            comboBoxSeccode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxSeccode.Name = "comboBoxSeccode";
            comboBoxSeccode.Size = new System.Drawing.Size(140, 23);
            comboBoxSeccode.TabIndex = 17;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Location = new System.Drawing.Point(14, 46);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(780, 646);
            tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(buttonQuotationsUnSubscribe);
            tabPage1.Controls.Add(buttonQuotationsSubscribe);
            tabPage1.Controls.Add(checkBoxShutdown);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(dateTimePickerMultidirectExecute);
            tabPage1.Controls.Add(buttonStartMultidirectTimer);
            tabPage1.Controls.Add(buttonMakeMultidirect);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(dataGridViewFortsPositions);
            tabPage1.Controls.Add(comboBoxSeccode);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(buttonComboSell);
            tabPage1.Controls.Add(textBoxDifference);
            tabPage1.Controls.Add(textBoxTP);
            tabPage1.Controls.Add(buttonComboBuy);
            tabPage1.Controls.Add(textBoxPrice);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(textBoxSL);
            tabPage1.Controls.Add(textBoxVolume);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label8);
            tabPage1.Controls.Add(label10);
            tabPage1.Controls.Add(labelDifference);
            tabPage1.Controls.Add(checkBoxByMarket);
            tabPage1.Controls.Add(label9);
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(772, 618);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Trading";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonQuotationsUnSubscribe
            // 
            buttonQuotationsUnSubscribe.Location = new System.Drawing.Point(208, 583);
            buttonQuotationsUnSubscribe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonQuotationsUnSubscribe.Name = "buttonQuotationsUnSubscribe";
            buttonQuotationsUnSubscribe.Size = new System.Drawing.Size(187, 27);
            buttonQuotationsUnSubscribe.TabIndex = 33;
            buttonQuotationsUnSubscribe.Text = "UnSubscribe to Quotations ";
            buttonQuotationsUnSubscribe.UseVisualStyleBackColor = true;
            // 
            // buttonQuotationsSubscribe
            // 
            buttonQuotationsSubscribe.Location = new System.Drawing.Point(14, 583);
            buttonQuotationsSubscribe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonQuotationsSubscribe.Name = "buttonQuotationsSubscribe";
            buttonQuotationsSubscribe.Size = new System.Drawing.Size(187, 27);
            buttonQuotationsSubscribe.TabIndex = 33;
            buttonQuotationsSubscribe.Text = "Subscribe to Quotations ";
            buttonQuotationsSubscribe.UseVisualStyleBackColor = true;
            buttonQuotationsSubscribe.Click += buttonQuotationsSubscribe_Click;
            // 
            // checkBoxShutdown
            // 
            checkBoxShutdown.AutoSize = true;
            checkBoxShutdown.Location = new System.Drawing.Point(63, 261);
            checkBoxShutdown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxShutdown.Name = "checkBoxShutdown";
            checkBoxShutdown.Size = new System.Drawing.Size(80, 19);
            checkBoxShutdown.TabIndex = 30;
            checkBoxShutdown.Text = "Shutdown";
            checkBoxShutdown.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(265, 242);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(140, 15);
            label7.TabIndex = 29;
            label7.Text = "Time to make multidirect";
            // 
            // dateTimePickerMultidirectExecute
            // 
            dateTimePickerMultidirectExecute.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dateTimePickerMultidirectExecute.Location = new System.Drawing.Point(268, 261);
            dateTimePickerMultidirectExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dateTimePickerMultidirectExecute.Name = "dateTimePickerMultidirectExecute";
            dateTimePickerMultidirectExecute.Size = new System.Drawing.Size(137, 23);
            dateTimePickerMultidirectExecute.TabIndex = 28;
            // 
            // buttonStartMultidirectTimer
            // 
            buttonStartMultidirectTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonStartMultidirectTimer.ForeColor = System.Drawing.Color.Red;
            buttonStartMultidirectTimer.Location = new System.Drawing.Point(413, 242);
            buttonStartMultidirectTimer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonStartMultidirectTimer.Name = "buttonStartMultidirectTimer";
            buttonStartMultidirectTimer.Size = new System.Drawing.Size(290, 47);
            buttonStartMultidirectTimer.TabIndex = 27;
            buttonStartMultidirectTimer.Text = "MULTIDIRECT BY TIMER";
            buttonStartMultidirectTimer.UseVisualStyleBackColor = true;
            buttonStartMultidirectTimer.Click += buttonStartMultidirectTimer_Click;
            // 
            // buttonMakeMultidirect
            // 
            buttonMakeMultidirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonMakeMultidirect.ForeColor = System.Drawing.Color.Red;
            buttonMakeMultidirect.Location = new System.Drawing.Point(413, 188);
            buttonMakeMultidirect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonMakeMultidirect.Name = "buttonMakeMultidirect";
            buttonMakeMultidirect.Size = new System.Drawing.Size(290, 47);
            buttonMakeMultidirect.TabIndex = 26;
            buttonMakeMultidirect.Text = "MULTIDIRECT!";
            buttonMakeMultidirect.UseVisualStyleBackColor = true;
            buttonMakeMultidirect.Click += buttonMakeMultidirect_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButtonComboTypeStop);
            groupBox1.Controls.Add(radioButtonComboTypeContidion);
            groupBox1.Location = new System.Drawing.Point(413, 8);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(290, 83);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Combo order type";
            // 
            // radioButtonComboTypeStop
            // 
            radioButtonComboTypeStop.AutoSize = true;
            radioButtonComboTypeStop.Location = new System.Drawing.Point(7, 57);
            radioButtonComboTypeStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonComboTypeStop.Name = "radioButtonComboTypeStop";
            radioButtonComboTypeStop.Size = new System.Drawing.Size(110, 19);
            radioButtonComboTypeStop.TabIndex = 23;
            radioButtonComboTypeStop.Text = "With Stop Order";
            radioButtonComboTypeStop.UseVisualStyleBackColor = true;
            // 
            // radioButtonComboTypeContidion
            // 
            radioButtonComboTypeContidion.AutoSize = true;
            radioButtonComboTypeContidion.Checked = true;
            radioButtonComboTypeContidion.Location = new System.Drawing.Point(7, 24);
            radioButtonComboTypeContidion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonComboTypeContidion.Name = "radioButtonComboTypeContidion";
            radioButtonComboTypeContidion.Size = new System.Drawing.Size(139, 19);
            radioButtonComboTypeContidion.TabIndex = 24;
            radioButtonComboTypeContidion.TabStop = true;
            radioButtonComboTypeContidion.Text = "With Condition Order";
            radioButtonComboTypeContidion.UseVisualStyleBackColor = true;
            // 
            // textBoxDifference
            // 
            textBoxDifference.Location = new System.Drawing.Point(500, 510);
            textBoxDifference.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxDifference.Name = "textBoxDifference";
            textBoxDifference.Size = new System.Drawing.Size(116, 23);
            textBoxDifference.TabIndex = 14;
            textBoxDifference.Text = "462";
            // 
            // labelDifference
            // 
            labelDifference.AutoSize = true;
            labelDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            labelDifference.Location = new System.Drawing.Point(497, 492);
            labelDifference.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDifference.Name = "labelDifference";
            labelDifference.Size = new System.Drawing.Size(223, 13);
            labelDifference.TabIndex = 15;
            labelDifference.Text = "Difference between 2 candles (points)";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox4);
            tabPage2.Controls.Add(groupBoxChangePassword);
            tabPage2.Controls.Add(groupBoxLogin);
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Size = new System.Drawing.Size(772, 618);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Settings 1";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBoxUnion);
            groupBox4.Controls.Add(label17);
            groupBox4.Controls.Add(textBoxFreeMoney);
            groupBox4.Controls.Add(label16);
            groupBox4.Controls.Add(textBoxClientId);
            groupBox4.Controls.Add(label15);
            groupBox4.Location = new System.Drawing.Point(12, 13);
            groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox4.Size = new System.Drawing.Size(746, 114);
            groupBox4.TabIndex = 19;
            groupBox4.TabStop = false;
            groupBox4.Text = "Info";
            // 
            // textBoxUnion
            // 
            textBoxUnion.Location = new System.Drawing.Point(139, 17);
            textBoxUnion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxUnion.Name = "textBoxUnion";
            textBoxUnion.ReadOnly = true;
            textBoxUnion.Size = new System.Drawing.Size(268, 23);
            textBoxUnion.TabIndex = 5;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(12, 21);
            label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(39, 15);
            label17.TabIndex = 4;
            label17.Text = "Union";
            // 
            // textBoxFreeMoney
            // 
            textBoxFreeMoney.Location = new System.Drawing.Point(139, 77);
            textBoxFreeMoney.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxFreeMoney.Name = "textBoxFreeMoney";
            textBoxFreeMoney.ReadOnly = true;
            textBoxFreeMoney.Size = new System.Drawing.Size(268, 23);
            textBoxFreeMoney.TabIndex = 3;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(12, 81);
            label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(69, 15);
            label16.TabIndex = 2;
            label16.Text = "Free Money";
            // 
            // textBoxClientId
            // 
            textBoxClientId.Location = new System.Drawing.Point(139, 47);
            textBoxClientId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxClientId.Name = "textBoxClientId";
            textBoxClientId.ReadOnly = true;
            textBoxClientId.Size = new System.Drawing.Size(268, 23);
            textBoxClientId.TabIndex = 1;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(12, 51);
            label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(48, 15);
            label15.TabIndex = 0;
            label15.Text = "ClientId";
            // 
            // groupBoxChangePassword
            // 
            groupBoxChangePassword.Controls.Add(textBoxPasswordNew);
            groupBoxChangePassword.Controls.Add(label11);
            groupBoxChangePassword.Controls.Add(textBoxPasswordOld);
            groupBoxChangePassword.Controls.Add(label12);
            groupBoxChangePassword.Controls.Add(buttonChangePassword);
            groupBoxChangePassword.Location = new System.Drawing.Point(12, 255);
            groupBoxChangePassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxChangePassword.Name = "groupBoxChangePassword";
            groupBoxChangePassword.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxChangePassword.Size = new System.Drawing.Size(752, 135);
            groupBoxChangePassword.TabIndex = 18;
            groupBoxChangePassword.TabStop = false;
            groupBoxChangePassword.Text = "Change Password";
            // 
            // textBoxPasswordNew
            // 
            textBoxPasswordNew.Location = new System.Drawing.Point(41, 52);
            textBoxPasswordNew.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPasswordNew.Name = "textBoxPasswordNew";
            textBoxPasswordNew.Size = new System.Drawing.Size(305, 23);
            textBoxPasswordNew.TabIndex = 20;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(7, 55);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(31, 15);
            label11.TabIndex = 19;
            label11.Text = "New";
            // 
            // textBoxPasswordOld
            // 
            textBoxPasswordOld.Location = new System.Drawing.Point(41, 22);
            textBoxPasswordOld.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPasswordOld.Name = "textBoxPasswordOld";
            textBoxPasswordOld.Size = new System.Drawing.Size(305, 23);
            textBoxPasswordOld.TabIndex = 18;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(7, 25);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(26, 15);
            label12.TabIndex = 17;
            label12.Text = "Old";
            // 
            // groupBoxLogin
            // 
            groupBoxLogin.Controls.Add(label18);
            groupBoxLogin.Controls.Add(comboBoxConnectionType);
            groupBoxLogin.Controls.Add(buttonLogout);
            groupBoxLogin.Controls.Add(buttonLogin);
            groupBoxLogin.Controls.Add(textBoxPassword);
            groupBoxLogin.Controls.Add(label13);
            groupBoxLogin.Controls.Add(textBoxUsername);
            groupBoxLogin.Controls.Add(label14);
            groupBoxLogin.Location = new System.Drawing.Point(12, 134);
            groupBoxLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLogin.Name = "groupBoxLogin";
            groupBoxLogin.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxLogin.Size = new System.Drawing.Size(752, 114);
            groupBoxLogin.TabIndex = 17;
            groupBoxLogin.TabStop = false;
            groupBoxLogin.Text = "Connection";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(9, 27);
            label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(39, 15);
            label18.TabIndex = 12;
            label18.Text = "Server";
            // 
            // comboBoxConnectionType
            // 
            comboBoxConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxConnectionType.FormattingEnabled = true;
            comboBoxConnectionType.Location = new System.Drawing.Point(80, 23);
            comboBoxConnectionType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxConnectionType.Name = "comboBoxConnectionType";
            comboBoxConnectionType.Size = new System.Drawing.Size(261, 23);
            comboBoxConnectionType.TabIndex = 11;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new System.Drawing.Point(556, 52);
            buttonLogout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new System.Drawing.Size(189, 55);
            buttonLogout.TabIndex = 10;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            buttonLogout.Click += buttonLogout_Click;
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new System.Drawing.Point(354, 52);
            buttonLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new System.Drawing.Size(196, 55);
            buttonLogin.TabIndex = 9;
            buttonLogin.Text = "Login";
            buttonLogin.UseVisualStyleBackColor = true;
            buttonLogin.Click += buttonLogin_Click;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new System.Drawing.Point(80, 84);
            textBoxPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.PasswordChar = 'X';
            textBoxPassword.Size = new System.Drawing.Size(261, 23);
            textBoxPassword.TabIndex = 8;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(9, 88);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(57, 15);
            label13.TabIndex = 7;
            label13.Text = "Password";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Location = new System.Drawing.Point(80, 54);
            textBoxUsername.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new System.Drawing.Size(261, 23);
            textBoxUsername.TabIndex = 6;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(9, 58);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(60, 15);
            label14.TabIndex = 5;
            label14.Text = "Username";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBoxChangePassword2);
            tabPage3.Controls.Add(groupBox3);
            tabPage3.Location = new System.Drawing.Point(4, 24);
            tabPage3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage3.Size = new System.Drawing.Size(772, 618);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Settings 2";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBoxChangePassword2
            // 
            groupBoxChangePassword2.Controls.Add(textBoxPasswordNew2);
            groupBoxChangePassword2.Controls.Add(label3);
            groupBoxChangePassword2.Controls.Add(textBoxPasswordOld2);
            groupBoxChangePassword2.Controls.Add(label4);
            groupBoxChangePassword2.Controls.Add(buttonChangePassword2);
            groupBoxChangePassword2.Location = new System.Drawing.Point(7, 128);
            groupBoxChangePassword2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxChangePassword2.Name = "groupBoxChangePassword2";
            groupBoxChangePassword2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxChangePassword2.Size = new System.Drawing.Size(752, 135);
            groupBoxChangePassword2.TabIndex = 20;
            groupBoxChangePassword2.TabStop = false;
            groupBoxChangePassword2.Text = "Change Password";
            // 
            // textBoxPasswordNew2
            // 
            textBoxPasswordNew2.Location = new System.Drawing.Point(41, 52);
            textBoxPasswordNew2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPasswordNew2.Name = "textBoxPasswordNew2";
            textBoxPasswordNew2.Size = new System.Drawing.Size(305, 23);
            textBoxPasswordNew2.TabIndex = 20;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(7, 55);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(31, 15);
            label3.TabIndex = 19;
            label3.Text = "New";
            // 
            // textBoxPasswordOld2
            // 
            textBoxPasswordOld2.Location = new System.Drawing.Point(41, 22);
            textBoxPasswordOld2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPasswordOld2.Name = "textBoxPasswordOld2";
            textBoxPasswordOld2.Size = new System.Drawing.Size(305, 23);
            textBoxPasswordOld2.TabIndex = 18;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(7, 25);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(26, 15);
            label4.TabIndex = 17;
            label4.Text = "Old";
            // 
            // buttonChangePassword2
            // 
            buttonChangePassword2.Location = new System.Drawing.Point(354, 20);
            buttonChangePassword2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonChangePassword2.Name = "buttonChangePassword2";
            buttonChangePassword2.Size = new System.Drawing.Size(392, 55);
            buttonChangePassword2.TabIndex = 16;
            buttonChangePassword2.Text = "Change password";
            buttonChangePassword2.UseVisualStyleBackColor = true;
            buttonChangePassword2.Click += buttonChangePassword2_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(buttonLogout2);
            groupBox3.Controls.Add(buttonLogin2);
            groupBox3.Controls.Add(textBoxPassword2);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(textBoxUsername2);
            groupBox3.Controls.Add(label6);
            groupBox3.Location = new System.Drawing.Point(7, 7);
            groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox3.Size = new System.Drawing.Size(752, 114);
            groupBox3.TabIndex = 19;
            groupBox3.TabStop = false;
            groupBox3.Text = "Connection";
            // 
            // buttonLogout2
            // 
            buttonLogout2.Location = new System.Drawing.Point(556, 20);
            buttonLogout2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogout2.Name = "buttonLogout2";
            buttonLogout2.Size = new System.Drawing.Size(189, 55);
            buttonLogout2.TabIndex = 10;
            buttonLogout2.Text = "Logout";
            buttonLogout2.UseVisualStyleBackColor = true;
            buttonLogout2.Click += buttonLogout2_Click;
            // 
            // buttonLogin2
            // 
            buttonLogin2.Location = new System.Drawing.Point(354, 20);
            buttonLogin2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonLogin2.Name = "buttonLogin2";
            buttonLogin2.Size = new System.Drawing.Size(196, 55);
            buttonLogin2.TabIndex = 9;
            buttonLogin2.Text = "Login";
            buttonLogin2.UseVisualStyleBackColor = true;
            buttonLogin2.Click += buttonLogin2_Click;
            // 
            // textBoxPassword2
            // 
            textBoxPassword2.Location = new System.Drawing.Point(80, 52);
            textBoxPassword2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPassword2.Name = "textBoxPassword2";
            textBoxPassword2.PasswordChar = 'X';
            textBoxPassword2.Size = new System.Drawing.Size(261, 23);
            textBoxPassword2.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(9, 55);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(57, 15);
            label5.TabIndex = 7;
            label5.Text = "Password";
            // 
            // textBoxUsername2
            // 
            textBoxUsername2.Location = new System.Drawing.Point(80, 22);
            textBoxUsername2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxUsername2.Name = "textBoxUsername2";
            textBoxUsername2.Size = new System.Drawing.Size(261, 23);
            textBoxUsername2.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(9, 25);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(60, 15);
            label6.TabIndex = 5;
            label6.Text = "Username";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(groupBox5);
            tabPage4.Controls.Add(groupBox2);
            tabPage4.Location = new System.Drawing.Point(4, 24);
            tabPage4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage4.Size = new System.Drawing.Size(772, 618);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Portfolio";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(dataGridViewPositions);
            groupBox5.Location = new System.Drawing.Point(7, 96);
            groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox5.Size = new System.Drawing.Size(757, 395);
            groupBox5.TabIndex = 1;
            groupBox5.TabStop = false;
            groupBox5.Text = "Positions";
            // 
            // dataGridViewPositions
            // 
            dataGridViewPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPositions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Code, NameColumn, BalancePrice, Price, Vol, PNL });
            dataGridViewPositions.Location = new System.Drawing.Point(7, 22);
            dataGridViewPositions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridViewPositions.Name = "dataGridViewPositions";
            dataGridViewPositions.ReadOnly = true;
            dataGridViewPositions.RowHeadersVisible = false;
            dataGridViewPositions.Size = new System.Drawing.Size(743, 366);
            dataGridViewPositions.TabIndex = 0;
            // 
            // Code
            // 
            Code.HeaderText = "Code";
            Code.Name = "Code";
            Code.ReadOnly = true;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Name";
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            // 
            // BalancePrice
            // 
            BalancePrice.HeaderText = "Balance Price";
            BalancePrice.Name = "BalancePrice";
            BalancePrice.ReadOnly = true;
            // 
            // Price
            // 
            Price.HeaderText = "Price";
            Price.Name = "Price";
            Price.ReadOnly = true;
            // 
            // Vol
            // 
            Vol.HeaderText = "Vol";
            Vol.Name = "Vol";
            Vol.ReadOnly = true;
            // 
            // PNL
            // 
            PNL.HeaderText = "PNL";
            PNL.Name = "PNL";
            PNL.ReadOnly = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBoxFreeMoney2);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(textBoxFreeMoney1);
            groupBox2.Controls.Add(label19);
            groupBox2.Location = new System.Drawing.Point(7, 7);
            groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(757, 82);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Money";
            // 
            // textBoxFreeMoney2
            // 
            textBoxFreeMoney2.Location = new System.Drawing.Point(97, 45);
            textBoxFreeMoney2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxFreeMoney2.Name = "textBoxFreeMoney2";
            textBoxFreeMoney2.ReadOnly = true;
            textBoxFreeMoney2.Size = new System.Drawing.Size(174, 23);
            textBoxFreeMoney2.TabIndex = 3;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(7, 48);
            label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(78, 15);
            label20.TabIndex = 2;
            label20.Text = "Free money 2";
            // 
            // textBoxFreeMoney1
            // 
            textBoxFreeMoney1.Location = new System.Drawing.Point(97, 15);
            textBoxFreeMoney1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxFreeMoney1.Name = "textBoxFreeMoney1";
            textBoxFreeMoney1.ReadOnly = true;
            textBoxFreeMoney1.Size = new System.Drawing.Size(174, 23);
            textBoxFreeMoney1.TabIndex = 1;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(7, 18);
            label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(78, 15);
            label19.TabIndex = 0;
            label19.Text = "Free money 1";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(buttonStopAll);
            tabPage5.Controls.Add(buttonRunAll);
            tabPage5.Controls.Add(groupBoxObservers);
            tabPage5.Location = new System.Drawing.Point(4, 24);
            tabPage5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage5.Size = new System.Drawing.Size(772, 618);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Strategies";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonStopAll
            // 
            buttonStopAll.Location = new System.Drawing.Point(7, 406);
            buttonStopAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonStopAll.Name = "buttonStopAll";
            buttonStopAll.Size = new System.Drawing.Size(203, 27);
            buttonStopAll.TabIndex = 1;
            buttonStopAll.Text = "StopAll";
            buttonStopAll.UseVisualStyleBackColor = true;
            buttonStopAll.Click += buttonStopAll_Click;
            // 
            // buttonRunAll
            // 
            buttonRunAll.Location = new System.Drawing.Point(7, 362);
            buttonRunAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRunAll.Name = "buttonRunAll";
            buttonRunAll.Size = new System.Drawing.Size(203, 27);
            buttonRunAll.TabIndex = 1;
            buttonRunAll.Text = "RunAll";
            buttonRunAll.UseVisualStyleBackColor = true;
            buttonRunAll.Click += buttonRunAll_Click;
            // 
            // groupBoxObservers
            // 
            groupBoxObservers.Controls.Add(buttonAddObserver);
            groupBoxObservers.Controls.Add(observersDataGridView);
            groupBoxObservers.Location = new System.Drawing.Point(7, 7);
            groupBoxObservers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxObservers.Name = "groupBoxObservers";
            groupBoxObservers.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxObservers.Size = new System.Drawing.Size(757, 305);
            groupBoxObservers.TabIndex = 0;
            groupBoxObservers.TabStop = false;
            groupBoxObservers.Text = "Observers";
            // 
            // buttonAddObserver
            // 
            buttonAddObserver.Location = new System.Drawing.Point(8, 263);
            buttonAddObserver.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAddObserver.Name = "buttonAddObserver";
            buttonAddObserver.Size = new System.Drawing.Size(195, 27);
            buttonAddObserver.TabIndex = 1;
            buttonAddObserver.Text = "Add";
            buttonAddObserver.UseVisualStyleBackColor = true;
            buttonAddObserver.Click += buttonAddObserver_Click;
            // 
            // observersDataGridView
            // 
            observersDataGridView.Location = new System.Drawing.Point(7, 22);
            observersDataGridView.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            observersDataGridView.Name = "observersDataGridView";
            observersDataGridView.Size = new System.Drawing.Size(743, 220);
            observersDataGridView.TabIndex = 0;
            // 
            // testButton
            // 
            testButton.Location = new System.Drawing.Point(439, 13);
            testButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            testButton.Name = "testButton";
            testButton.Size = new System.Drawing.Size(219, 27);
            testButton.TabIndex = 35;
            testButton.Text = "Test";
            testButton.UseVisualStyleBackColor = true;
            testButton.Click += TestButton_Click;
            // 
            // labelTime
            // 
            labelTime.AutoSize = true;
            labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            labelTime.Location = new System.Drawing.Point(19, 15);
            labelTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTime.Name = "labelTime";
            labelTime.Size = new System.Drawing.Size(94, 13);
            labelTime.TabIndex = 19;
            labelTime.Text = "Time: hh:mm:ss";
            // 
            // comboBoxTimezone
            // 
            comboBoxTimezone.FormattingEnabled = true;
            comboBoxTimezone.Location = new System.Drawing.Point(320, 13);
            comboBoxTimezone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxTimezone.Name = "comboBoxTimezone";
            comboBoxTimezone.Size = new System.Drawing.Size(80, 23);
            comboBoxTimezone.TabIndex = 36;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(251, 18);
            label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(58, 15);
            label21.TabIndex = 37;
            label21.Text = "Timezone";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(805, 706);
            Controls.Add(label21);
            Controls.Add(comboBoxTimezone);
            Controls.Add(testButton);
            Controls.Add(labelTime);
            Controls.Add(tabControl1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "AUTO TRADER";
            ((System.ComponentModel.ISupportInitialize)dataGridViewFortsPositions).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBoxChangePassword.ResumeLayout(false);
            groupBoxChangePassword.PerformLayout();
            groupBoxLogin.ResumeLayout(false);
            groupBoxLogin.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBoxChangePassword2.ResumeLayout(false);
            groupBoxChangePassword2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tabPage4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewPositions).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage5.ResumeLayout(false);
            groupBoxObservers.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSL;
        private System.Windows.Forms.Button buttonComboBuy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxByMarket;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dataGridViewFortsPositions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxVolume;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxTP;
        private System.Windows.Forms.Button buttonChangePassword;
        private System.Windows.Forms.Button buttonComboSell;
        private System.Windows.Forms.ComboBox comboBoxSeccode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxChangePassword;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.TextBox textBoxPasswordNew;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxPasswordOld;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonLogout;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonComboTypeStop;
        private System.Windows.Forms.RadioButton radioButtonComboTypeContidion;
        private System.Windows.Forms.Button buttonMakeMultidirect;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBoxChangePassword2;
        private System.Windows.Forms.TextBox textBoxPasswordNew2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPasswordOld2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonChangePassword2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonLogout2;
        private System.Windows.Forms.Button buttonLogin2;
        private System.Windows.Forms.TextBox textBoxPassword2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxUsername2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonStartMultidirectTimer;
        private System.Windows.Forms.DateTimePicker dateTimePickerMultidirectExecute;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxUnion;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxFreeMoney;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxClientId;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox comboBoxConnectionType;
        private System.Windows.Forms.CheckBox checkBoxShutdown;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dataGridViewPositions;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxFreeMoney2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox textBoxFreeMoney1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalancePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PNL;
        private System.Windows.Forms.Button buttonQuotationsSubscribe;
        private System.Windows.Forms.Button buttonQuotationsUnSubscribe;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox textBoxDifference;
        private System.Windows.Forms.Label labelDifference;
        private System.Windows.Forms.GroupBox groupBoxObservers;
        private Controls.ObserversDataGridView observersDataGridView;
        private System.Windows.Forms.Button buttonAddObserver;
        private System.Windows.Forms.Button buttonStopAll;
        private System.Windows.Forms.Button buttonRunAll;
        private System.Windows.Forms.ComboBox comboBoxTimezone;
        private System.Windows.Forms.Label label21;
    }
}