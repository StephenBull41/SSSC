namespace Steve_s_Super_Support_Console
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbx_Var_List = new System.Windows.Forms.ListBox();
            this.tbx_Var_Value = new System.Windows.Forms.TextBox();
            this.btnVarSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(414, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "TODO: Make this look not shit";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 49);
            this.button1.TabIndex = 5;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(137, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 49);
            this.button2.TabIndex = 6;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbx_Var_List
            // 
            this.lbx_Var_List.FormattingEnabled = true;
            this.lbx_Var_List.Location = new System.Drawing.Point(12, 219);
            this.lbx_Var_List.Name = "lbx_Var_List";
            this.lbx_Var_List.Size = new System.Drawing.Size(257, 355);
            this.lbx_Var_List.TabIndex = 7;
            this.lbx_Var_List.SelectedIndexChanged += new System.EventHandler(this.lbx_Var_List_SelectedIndexChanged);
            // 
            // tbx_Var_Value
            // 
            this.tbx_Var_Value.Location = new System.Drawing.Point(12, 580);
            this.tbx_Var_Value.Name = "tbx_Var_Value";
            this.tbx_Var_Value.Size = new System.Drawing.Size(256, 20);
            this.tbx_Var_Value.TabIndex = 8;
            // 
            // btnVarSave
            // 
            this.btnVarSave.Location = new System.Drawing.Point(12, 606);
            this.btnVarSave.Name = "btnVarSave";
            this.btnVarSave.Size = new System.Drawing.Size(256, 23);
            this.btnVarSave.TabIndex = 9;
            this.btnVarSave.Text = "save";
            this.btnVarSave.UseVisualStyleBackColor = true;
            this.btnVarSave.Click += new System.EventHandler(this.btnVarSave_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.btnVarSave);
            this.Controls.Add(this.tbx_Var_Value);
            this.Controls.Add(this.lbx_Var_List);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfigForm";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox lbx_Var_List;
        private System.Windows.Forms.TextBox tbx_Var_Value;
        private System.Windows.Forms.Button btnVarSave;
    }
}