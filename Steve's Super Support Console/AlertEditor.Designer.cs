namespace Steve_s_Super_Support_Console
{
    partial class AlertEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAlertSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxSiteID = new System.Windows.Forms.TextBox();
            this.tbxSDate = new System.Windows.Forms.TextBox();
            this.tbxEDate = new System.Windows.Forms.TextBox();
            this.btnAlertClose = new System.Windows.Forms.Button();
            this.tbxAlertNotes = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site ID";
            // 
            // btnAlertSave
            // 
            this.btnAlertSave.Location = new System.Drawing.Point(15, 116);
            this.btnAlertSave.Name = "btnAlertSave";
            this.btnAlertSave.Size = new System.Drawing.Size(121, 42);
            this.btnAlertSave.TabIndex = 10;
            this.btnAlertSave.Text = "Save";
            this.btnAlertSave.UseVisualStyleBackColor = true;
            this.btnAlertSave.Click += new System.EventHandler(this.btnAlertSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Date/Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "End Date/Time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Alert Notes:";
            // 
            // tbxSiteID
            // 
            this.tbxSiteID.Location = new System.Drawing.Point(108, 12);
            this.tbxSiteID.Name = "tbxSiteID";
            this.tbxSiteID.Size = new System.Drawing.Size(100, 20);
            this.tbxSiteID.TabIndex = 6;
            // 
            // tbxSDate
            // 
            this.tbxSDate.Location = new System.Drawing.Point(108, 38);
            this.tbxSDate.Name = "tbxSDate";
            this.tbxSDate.Size = new System.Drawing.Size(100, 20);
            this.tbxSDate.TabIndex = 7;
            // 
            // tbxEDate
            // 
            this.tbxEDate.Location = new System.Drawing.Point(108, 64);
            this.tbxEDate.Name = "tbxEDate";
            this.tbxEDate.Size = new System.Drawing.Size(100, 20);
            this.tbxEDate.TabIndex = 8;
            // 
            // btnAlertClose
            // 
            this.btnAlertClose.Location = new System.Drawing.Point(267, 116);
            this.btnAlertClose.Name = "btnAlertClose";
            this.btnAlertClose.Size = new System.Drawing.Size(121, 42);
            this.btnAlertClose.TabIndex = 11;
            this.btnAlertClose.Text = "Exit";
            this.btnAlertClose.UseVisualStyleBackColor = true;
            this.btnAlertClose.Click += new System.EventHandler(this.btnAlertClose_Click);
            // 
            // tbxAlertNotes
            // 
            this.tbxAlertNotes.Location = new System.Drawing.Point(108, 90);
            this.tbxAlertNotes.Name = "tbxAlertNotes";
            this.tbxAlertNotes.Size = new System.Drawing.Size(280, 20);
            this.tbxAlertNotes.TabIndex = 9;
            // 
            // AlertEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(400, 163);
            this.Controls.Add(this.tbxAlertNotes);
            this.Controls.Add(this.btnAlertClose);
            this.Controls.Add(this.tbxEDate);
            this.Controls.Add(this.tbxSDate);
            this.Controls.Add(this.tbxSiteID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAlertSave);
            this.Controls.Add(this.label1);
            this.Name = "AlertEditor";
            this.Text = "Alert Editor";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAlertSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxSiteID;
        private System.Windows.Forms.TextBox tbxSDate;
        private System.Windows.Forms.TextBox tbxEDate;
        private System.Windows.Forms.Button btnAlertClose;
        private System.Windows.Forms.TextBox tbxAlertNotes;
    }
}