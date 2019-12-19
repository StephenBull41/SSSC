namespace Steve_s_Super_Support_Console
{
    partial class CustButtonEditor
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
            this.tbxName = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxPath = new System.Windows.Forms.TextBox();
            this.rtbTextFile = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbt_scripts = new System.Windows.Forms.RadioButton();
            this.rtbExe = new System.Windows.Forms.RadioButton();
            this.rtbFolder = new System.Windows.Forms.RadioButton();
            this.lbxTarget = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblPathError = new System.Windows.Forms.Label();
            this.lbx_scripts = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbx_drive = new System.Windows.Forms.ComboBox();
            this.lbl_driveprompt = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(76, 40);
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(293, 20);
            this.tbxName.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.Gainsboro;
            this.btnApply.Location = new System.Drawing.Point(278, 599);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(91, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Save and close";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(161, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Do an edit";
            // 
            // tbxPath
            // 
            this.tbxPath.Location = new System.Drawing.Point(36, 428);
            this.tbxPath.Name = "tbxPath";
            this.tbxPath.Size = new System.Drawing.Size(333, 20);
            this.tbxPath.TabIndex = 3;
            // 
            // rtbTextFile
            // 
            this.rtbTextFile.AutoSize = true;
            this.rtbTextFile.Location = new System.Drawing.Point(21, 19);
            this.rtbTextFile.Name = "rtbTextFile";
            this.rtbTextFile.Size = new System.Drawing.Size(62, 17);
            this.rtbTextFile.TabIndex = 4;
            this.rtbTextFile.TabStop = true;
            this.rtbTextFile.Text = "Text file";
            this.rtbTextFile.UseVisualStyleBackColor = true;
            this.rtbTextFile.CheckedChanged += new System.EventHandler(this.rtbTextFile_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbt_scripts);
            this.groupBox1.Controls.Add(this.rtbExe);
            this.groupBox1.Controls.Add(this.rtbFolder);
            this.groupBox1.Controls.Add(this.rtbTextFile);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(15, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 69);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target type";
            // 
            // rbt_scripts
            // 
            this.rbt_scripts.AutoSize = true;
            this.rbt_scripts.Location = new System.Drawing.Point(104, 43);
            this.rbt_scripts.Name = "rbt_scripts";
            this.rbt_scripts.Size = new System.Drawing.Size(52, 17);
            this.rbt_scripts.TabIndex = 7;
            this.rbt_scripts.TabStop = true;
            this.rbt_scripts.Text = "Script";
            this.rbt_scripts.UseVisualStyleBackColor = true;
            this.rbt_scripts.CheckedChanged += new System.EventHandler(this.rbt_scripts_CheckedChanged);
            // 
            // rtbExe
            // 
            this.rtbExe.AutoSize = true;
            this.rtbExe.Location = new System.Drawing.Point(104, 19);
            this.rtbExe.Name = "rtbExe";
            this.rtbExe.Size = new System.Drawing.Size(78, 17);
            this.rtbExe.TabIndex = 6;
            this.rtbExe.TabStop = true;
            this.rtbExe.Text = "Executable";
            this.rtbExe.UseVisualStyleBackColor = true;
            this.rtbExe.CheckedChanged += new System.EventHandler(this.rtbExe_CheckedChanged);
            // 
            // rtbFolder
            // 
            this.rtbFolder.AutoSize = true;
            this.rtbFolder.Location = new System.Drawing.Point(21, 42);
            this.rtbFolder.Name = "rtbFolder";
            this.rtbFolder.Size = new System.Drawing.Size(54, 17);
            this.rtbFolder.TabIndex = 5;
            this.rtbFolder.TabStop = true;
            this.rtbFolder.Text = "Folder";
            this.rtbFolder.UseVisualStyleBackColor = true;
            this.rtbFolder.CheckedChanged += new System.EventHandler(this.rtbFolder_CheckedChanged);
            // 
            // lbxTarget
            // 
            this.lbxTarget.FormattingEnabled = true;
            this.lbxTarget.Items.AddRange(new object[] {
            "BOC",
            "Postec",
            "MWS",
            "CWS1",
            "CWS2",
            "CWS3",
            "Local"});
            this.lbxTarget.Location = new System.Drawing.Point(15, 491);
            this.lbxTarget.Name = "lbxTarget";
            this.lbxTarget.Size = new System.Drawing.Size(182, 95);
            this.lbxTarget.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Button text";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 408);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Device local file path:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 475);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Target device";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Gainsboro;
            this.btnExit.Location = new System.Drawing.Point(15, 599);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 23);
            this.btnExit.TabIndex = 10;
            this.btnExit.Text = "Cancel";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblPathError
            // 
            this.lblPathError.AutoSize = true;
            this.lblPathError.Location = new System.Drawing.Point(12, 451);
            this.lblPathError.Name = "lblPathError";
            this.lblPathError.Size = new System.Drawing.Size(25, 13);
            this.lblPathError.TabIndex = 11;
            this.lblPathError.Text = "___";
            // 
            // lbx_scripts
            // 
            this.lbx_scripts.FormattingEnabled = true;
            this.lbx_scripts.Location = new System.Drawing.Point(15, 163);
            this.lbx_scripts.Name = "lbx_scripts";
            this.lbx_scripts.Size = new System.Drawing.Size(354, 238);
            this.lbx_scripts.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(204, 475);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Drive Letter";
            // 
            // cbx_drive
            // 
            this.cbx_drive.FormattingEnabled = true;
            this.cbx_drive.Items.AddRange(new object[] {
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z"});
            this.cbx_drive.Location = new System.Drawing.Point(207, 491);
            this.cbx_drive.Name = "cbx_drive";
            this.cbx_drive.Size = new System.Drawing.Size(162, 21);
            this.cbx_drive.TabIndex = 14;
            this.cbx_drive.SelectedIndexChanged += new System.EventHandler(this.cbx_drive_SelectedIndexChanged);
            // 
            // lbl_driveprompt
            // 
            this.lbl_driveprompt.AutoSize = true;
            this.lbl_driveprompt.ForeColor = System.Drawing.Color.White;
            this.lbl_driveprompt.Location = new System.Drawing.Point(12, 431);
            this.lbl_driveprompt.Name = "lbl_driveprompt";
            this.lbl_driveprompt.Size = new System.Drawing.Size(22, 13);
            this.lbl_driveprompt.TabIndex = 15;
            this.lbl_driveprompt.Text = "C:\\";
            // 
            // CustButtonEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(381, 626);
            this.Controls.Add(this.lbl_driveprompt);
            this.Controls.Add(this.cbx_drive);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbx_scripts);
            this.Controls.Add(this.lblPathError);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbxTarget);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbxPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbxName);
            this.Name = "CustButtonEditor";
            this.Text = "Button Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxPath;
        private System.Windows.Forms.RadioButton rtbTextFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rtbFolder;
        private System.Windows.Forms.ListBox lbxTarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.RadioButton rtbExe;
        private System.Windows.Forms.Label lblPathError;
        private System.Windows.Forms.RadioButton rbt_scripts;
        private System.Windows.Forms.ListBox lbx_scripts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbx_drive;
        private System.Windows.Forms.Label lbl_driveprompt;
    }
}