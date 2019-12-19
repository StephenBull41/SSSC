using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steve_s_Super_Support_Console
{
    public partial class TextViewer : Form
    {
        public TextViewer(string text, string name)
        {
            InitializeComponent();
            rtbTextViewer.Text = text;
            this.Text = name;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                if(saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFile.FileName, rtbTextViewer.Lines);
                }
            }
        }
    }
}
