namespace Steve_s_Super_Support_Console
{
    partial class AlertViewer
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
            this.rtbAlarm = new System.Windows.Forms.RichTextBox();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbAlarm
            // 
            this.rtbAlarm.Location = new System.Drawing.Point(12, 12);
            this.rtbAlarm.Name = "rtbAlarm";
            this.rtbAlarm.Size = new System.Drawing.Size(264, 148);
            this.rtbAlarm.TabIndex = 1;
            this.rtbAlarm.Text = "";
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.Location = new System.Drawing.Point(43, 172);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(188, 34);
            this.btnCloseForm.TabIndex = 2;
            this.btnCloseForm.Text = "Close";
            this.btnCloseForm.UseVisualStyleBackColor = true;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // AlertViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(286, 217);
            this.Controls.Add(this.btnCloseForm);
            this.Controls.Add(this.rtbAlarm);
            this.Name = "AlertViewer";
            this.Text = "Alert";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbAlarm;
        private System.Windows.Forms.Button btnCloseForm;
    }
}