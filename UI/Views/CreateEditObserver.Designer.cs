namespace AutoTraderUI.Views
{
    partial class CreateEditObserver
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxSeccode = new System.Windows.Forms.ComboBox();
            this.textBoxDifference = new System.Windows.Forms.TextBox();
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonNotificationFile = new System.Windows.Forms.RadioButton();
            this.radioButtonNotificationEmail = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(9, 278);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(57, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(73, 278);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(58, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // comboBoxSeccode
            // 
            this.comboBoxSeccode.FormattingEnabled = true;
            this.comboBoxSeccode.Location = new System.Drawing.Point(9, 28);
            this.comboBoxSeccode.Name = "comboBoxSeccode";
            this.comboBoxSeccode.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSeccode.TabIndex = 2;
            // 
            // textBoxDifference
            // 
            this.textBoxDifference.Location = new System.Drawing.Point(9, 77);
            this.textBoxDifference.Name = "textBoxDifference";
            this.textBoxDifference.Size = new System.Drawing.Size(121, 20);
            this.textBoxDifference.TabIndex = 3;
            // 
            // textBoxPeriod
            // 
            this.textBoxPeriod.Location = new System.Drawing.Point(9, 127);
            this.textBoxPeriod.Name = "textBoxPeriod";
            this.textBoxPeriod.Size = new System.Drawing.Size(121, 20);
            this.textBoxPeriod.TabIndex = 4;
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(9, 177);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(121, 20);
            this.textBoxDelay.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonNotificationFile);
            this.groupBox1.Controls.Add(this.radioButtonNotificationEmail);
            this.groupBox1.Location = new System.Drawing.Point(9, 203);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(81, 69);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notification";
            // 
            // radioButtonNotificationFile
            // 
            this.radioButtonNotificationFile.AutoSize = true;
            this.radioButtonNotificationFile.Checked = true;
            this.radioButtonNotificationFile.Location = new System.Drawing.Point(7, 43);
            this.radioButtonNotificationFile.Name = "radioButtonNotificationFile";
            this.radioButtonNotificationFile.Size = new System.Drawing.Size(41, 17);
            this.radioButtonNotificationFile.TabIndex = 0;
            this.radioButtonNotificationFile.TabStop = true;
            this.radioButtonNotificationFile.Text = "File";
            this.radioButtonNotificationFile.UseVisualStyleBackColor = true;
            // 
            // radioButtonNotificationEmail
            // 
            this.radioButtonNotificationEmail.AutoSize = true;
            this.radioButtonNotificationEmail.Location = new System.Drawing.Point(7, 20);
            this.radioButtonNotificationEmail.Name = "radioButtonNotificationEmail";
            this.radioButtonNotificationEmail.Size = new System.Drawing.Size(50, 17);
            this.radioButtonNotificationEmail.TabIndex = 0;
            this.radioButtonNotificationEmail.TabStop = true;
            this.radioButtonNotificationEmail.Text = "Email";
            this.radioButtonNotificationEmail.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Seccode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Difference";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Period";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Delay";
            // 
            // CreateEditObserver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(143, 309);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.textBoxPeriod);
            this.Controls.Add(this.textBoxDifference);
            this.Controls.Add(this.comboBoxSeccode);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Name = "CreateEditObserver";
            this.Text = "AddObserver";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBoxSeccode;
        private System.Windows.Forms.TextBox textBoxDifference;
        private System.Windows.Forms.TextBox textBoxPeriod;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonNotificationFile;
        private System.Windows.Forms.RadioButton radioButtonNotificationEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}