namespace Steve_s_Super_Support_Console
{
    partial class ScriptForm
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
            this.lbx_script_list = new System.Windows.Forms.ListBox();
            this.rtb_editor = new System.Windows.Forms.RichTextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_add_script = new System.Windows.Forms.Button();
            this.btn_remove_script = new System.Windows.Forms.Button();
            this.tbx_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbx_script_list
            // 
            this.lbx_script_list.BackColor = System.Drawing.Color.White;
            this.lbx_script_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbx_script_list.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_script_list.ForeColor = System.Drawing.Color.Black;
            this.lbx_script_list.FormattingEnabled = true;
            this.lbx_script_list.ItemHeight = 18;
            this.lbx_script_list.Items.AddRange(new object[] {
            "these",
            "are",
            "some",
            "scripts",
            "and",
            "other ",
            "stuff"});
            this.lbx_script_list.Location = new System.Drawing.Point(12, 239);
            this.lbx_script_list.Name = "lbx_script_list";
            this.lbx_script_list.Size = new System.Drawing.Size(175, 612);
            this.lbx_script_list.TabIndex = 0;
            this.lbx_script_list.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // rtb_editor
            // 
            this.rtb_editor.BackColor = System.Drawing.Color.White;
            this.rtb_editor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_editor.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.rtb_editor.ForeColor = System.Drawing.Color.Black;
            this.rtb_editor.Location = new System.Drawing.Point(193, 167);
            this.rtb_editor.Name = "rtb_editor";
            this.rtb_editor.Size = new System.Drawing.Size(1379, 684);
            this.rtb_editor.TabIndex = 1;
            this.rtb_editor.Text = "";
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_save.Location = new System.Drawing.Point(1388, 88);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(184, 70);
            this.btn_save.TabIndex = 3;
            this.btn_save.Text = "Save Changes";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Preset variables:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "POS1, POS2, POS3, POS4, POS5, POS6, POS7";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(9, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(252, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "PP1, PP2, PP3, PP4, PP5, PP6, PP7";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(9, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(378, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "POSTEC, Router, Switch, BOC, ID, DOMS38, DOMS58";
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_exit.Location = new System.Drawing.Point(1388, 9);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(184, 70);
            this.btn_exit.TabIndex = 8;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_add_script
            // 
            this.btn_add_script.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_add_script.Location = new System.Drawing.Point(12, 167);
            this.btn_add_script.Name = "btn_add_script";
            this.btn_add_script.Size = new System.Drawing.Size(84, 70);
            this.btn_add_script.TabIndex = 9;
            this.btn_add_script.Text = "Add";
            this.btn_add_script.UseVisualStyleBackColor = false;
            this.btn_add_script.Click += new System.EventHandler(this.btn_add_script_Click);
            // 
            // btn_remove_script
            // 
            this.btn_remove_script.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_remove_script.Location = new System.Drawing.Point(102, 167);
            this.btn_remove_script.Name = "btn_remove_script";
            this.btn_remove_script.Size = new System.Drawing.Size(84, 70);
            this.btn_remove_script.TabIndex = 10;
            this.btn_remove_script.Text = "Delete";
            this.btn_remove_script.UseVisualStyleBackColor = false;
            this.btn_remove_script.Click += new System.EventHandler(this.btn_remove_script_Click);
            // 
            // tbx_search
            // 
            this.tbx_search.Location = new System.Drawing.Point(12, 145);
            this.tbx_search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbx_search.Name = "tbx_search";
            this.tbx_search.Size = new System.Drawing.Size(135, 20);
            this.tbx_search.TabIndex = 11;
            this.tbx_search.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tbx_search_PreviewKeyDown);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(150, 145);
            this.btn_search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(36, 17);
            this.btn_search.TabIndex = 12;
            this.btn_search.Text = ">";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 18);
            this.label1.TabIndex = 13;
            this.label1.Text = "NAME, NETIP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(9, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "Search:";
            // 
            // ScriptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1581, 861);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.tbx_search);
            this.Controls.Add(this.btn_remove_script);
            this.Controls.Add(this.btn_add_script);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.rtb_editor);
            this.Controls.Add(this.lbx_script_list);
            this.Name = "ScriptForm";
            this.Text = "ScriptForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbx_script_list;
        private System.Windows.Forms.RichTextBox rtb_editor;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_add_script;
        private System.Windows.Forms.Button btn_remove_script;
        private System.Windows.Forms.TextBox tbx_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
    }
}