namespace Steve_s_Super_Support_Console
{
    partial class LogViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewer));
            this.lbx_UIDList = new System.Windows.Forms.ListBox();
            this.rtb_EventView = new System.Windows.Forms.RichTextBox();
            this.lbx_TypeList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Get_ELog = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxInformation = new System.Windows.Forms.CheckBox();
            this.cbxException = new System.Windows.Forms.CheckBox();
            this.cbxVerbose = new System.Windows.Forms.CheckBox();
            this.cbxWarning = new System.Windows.Forms.CheckBox();
            this.cbxCritical = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudHours = new System.Windows.Forms.NumericUpDown();
            this.tbxAddress = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btn_save_log = new System.Windows.Forms.Button();
            this.btn_load_log = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours)).BeginInit();
            this.SuspendLayout();
            // 
            // lbx_UIDList
            // 
            this.lbx_UIDList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_UIDList.FormattingEnabled = true;
            this.lbx_UIDList.ItemHeight = 18;
            this.lbx_UIDList.Items.AddRange(new object[] {
            " "});
            this.lbx_UIDList.Location = new System.Drawing.Point(12, 269);
            this.lbx_UIDList.Name = "lbx_UIDList";
            this.lbx_UIDList.Size = new System.Drawing.Size(285, 580);
            this.lbx_UIDList.TabIndex = 0;
            this.lbx_UIDList.SelectedIndexChanged += new System.EventHandler(this.lbx_UIDList_SelectedIndexChanged);
            // 
            // rtb_EventView
            // 
            this.rtb_EventView.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtb_EventView.Location = new System.Drawing.Point(629, 12);
            this.rtb_EventView.Name = "rtb_EventView";
            this.rtb_EventView.Size = new System.Drawing.Size(943, 837);
            this.rtb_EventView.TabIndex = 1;
            this.rtb_EventView.Text = "";
            // 
            // lbx_TypeList
            // 
            this.lbx_TypeList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_TypeList.FormattingEnabled = true;
            this.lbx_TypeList.ItemHeight = 18;
            this.lbx_TypeList.Items.AddRange(new object[] {
            " "});
            this.lbx_TypeList.Location = new System.Drawing.Point(322, 269);
            this.lbx_TypeList.Name = "lbx_TypeList";
            this.lbx_TypeList.Size = new System.Drawing.Size(285, 580);
            this.lbx_TypeList.TabIndex = 2;
            this.lbx_TypeList.SelectedIndexChanged += new System.EventHandler(this.lbx_TypeList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Unique Events:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(318, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Individual Events:";
            // 
            // btn_Get_ELog
            // 
            this.btn_Get_ELog.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_Get_ELog.ForeColor = System.Drawing.Color.Black;
            this.btn_Get_ELog.Location = new System.Drawing.Point(12, 175);
            this.btn_Get_ELog.Name = "btn_Get_ELog";
            this.btn_Get_ELog.Size = new System.Drawing.Size(134, 55);
            this.btn_Get_ELog.TabIndex = 6;
            this.btn_Get_ELog.Text = "get log";
            this.btn_Get_ELog.UseVisualStyleBackColor = false;
            this.btn_Get_ELog.Click += new System.EventHandler(this.btn_Get_ELog_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxInformation);
            this.groupBox2.Controls.Add(this.cbxException);
            this.groupBox2.Controls.Add(this.cbxVerbose);
            this.groupBox2.Controls.Add(this.cbxWarning);
            this.groupBox2.Controls.Add(this.cbxCritical);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 68);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Filter";
            // 
            // cbxInformation
            // 
            this.cbxInformation.AutoSize = true;
            this.cbxInformation.Location = new System.Drawing.Point(78, 43);
            this.cbxInformation.Name = "cbxInformation";
            this.cbxInformation.Size = new System.Drawing.Size(78, 17);
            this.cbxInformation.TabIndex = 10;
            this.cbxInformation.Text = "Information";
            this.cbxInformation.UseVisualStyleBackColor = true;
            // 
            // cbxException
            // 
            this.cbxException.AutoSize = true;
            this.cbxException.Location = new System.Drawing.Point(6, 43);
            this.cbxException.Name = "cbxException";
            this.cbxException.Size = new System.Drawing.Size(73, 17);
            this.cbxException.TabIndex = 3;
            this.cbxException.Text = "Exception";
            this.cbxException.UseVisualStyleBackColor = true;
            // 
            // cbxVerbose
            // 
            this.cbxVerbose.AutoSize = true;
            this.cbxVerbose.Location = new System.Drawing.Point(150, 20);
            this.cbxVerbose.Name = "cbxVerbose";
            this.cbxVerbose.Size = new System.Drawing.Size(65, 17);
            this.cbxVerbose.TabIndex = 2;
            this.cbxVerbose.Text = "Verbose";
            this.cbxVerbose.UseVisualStyleBackColor = true;
            // 
            // cbxWarning
            // 
            this.cbxWarning.AutoSize = true;
            this.cbxWarning.Location = new System.Drawing.Point(78, 20);
            this.cbxWarning.Name = "cbxWarning";
            this.cbxWarning.Size = new System.Drawing.Size(66, 17);
            this.cbxWarning.TabIndex = 1;
            this.cbxWarning.Text = "Warning";
            this.cbxWarning.UseVisualStyleBackColor = true;
            // 
            // cbxCritical
            // 
            this.cbxCritical.AutoSize = true;
            this.cbxCritical.Location = new System.Drawing.Point(6, 20);
            this.cbxCritical.Name = "cbxCritical";
            this.cbxCritical.Size = new System.Drawing.Size(57, 17);
            this.cbxCritical.TabIndex = 0;
            this.cbxCritical.Text = "Critical";
            this.cbxCritical.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "Computer name / IP address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "Search last X hours:";
            // 
            // nudHours
            // 
            this.nudHours.Location = new System.Drawing.Point(157, 44);
            this.nudHours.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.nudHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHours.Name = "nudHours";
            this.nudHours.Size = new System.Drawing.Size(47, 20);
            this.nudHours.TabIndex = 12;
            this.nudHours.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbxAddress
            // 
            this.tbxAddress.Location = new System.Drawing.Point(216, 15);
            this.tbxAddress.MaxLength = 999;
            this.tbxAddress.Name = "tbxAddress";
            this.tbxAddress.Size = new System.Drawing.Size(134, 20);
            this.tbxAddress.TabIndex = 13;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.LightGreen;
            this.lblStatus.Location = new System.Drawing.Point(9, 148);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(123, 18);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "No search active ";
            // 
            // btn_save_log
            // 
            this.btn_save_log.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_save_log.ForeColor = System.Drawing.Color.Black;
            this.btn_save_log.Location = new System.Drawing.Point(371, 15);
            this.btn_save_log.Name = "btn_save_log";
            this.btn_save_log.Size = new System.Drawing.Size(117, 48);
            this.btn_save_log.TabIndex = 15;
            this.btn_save_log.Text = "Save Log";
            this.btn_save_log.UseVisualStyleBackColor = false;
            this.btn_save_log.Click += new System.EventHandler(this.btn_save_log_Click);
            // 
            // btn_load_log
            // 
            this.btn_load_log.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_load_log.ForeColor = System.Drawing.Color.Black;
            this.btn_load_log.Location = new System.Drawing.Point(494, 15);
            this.btn_load_log.Name = "btn_load_log";
            this.btn_load_log.Size = new System.Drawing.Size(117, 48);
            this.btn_load_log.TabIndex = 16;
            this.btn_load_log.Text = "Load Log";
            this.btn_load_log.UseVisualStyleBackColor = false;
            this.btn_load_log.Click += new System.EventHandler(this.btn_load_log_Click);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.btn_load_log);
            this.Controls.Add(this.btn_save_log);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tbxAddress);
            this.Controls.Add(this.nudHours);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_Get_ELog);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbx_TypeList);
            this.Controls.Add(this.rtb_EventView);
            this.Controls.Add(this.lbx_UIDList);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogViewer";
            this.Text = "LogViewer";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbx_UIDList;
        private System.Windows.Forms.RichTextBox rtb_EventView;
        private System.Windows.Forms.ListBox lbx_TypeList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Get_ELog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbxInformation;
        private System.Windows.Forms.CheckBox cbxException;
        private System.Windows.Forms.CheckBox cbxVerbose;
        private System.Windows.Forms.CheckBox cbxWarning;
        private System.Windows.Forms.CheckBox cbxCritical;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudHours;
        private System.Windows.Forms.TextBox tbxAddress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btn_save_log;
        private System.Windows.Forms.Button btn_load_log;
    }
}