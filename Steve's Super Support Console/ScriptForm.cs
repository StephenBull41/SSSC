using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Steve_s_Super_Support_Console
{
    public partial class ScriptForm : Form
    {
        
        public ScriptForm()
        {
            InitializeComponent();
            refresh_scripts();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lbx_script_list
            rtb_editor.Clear();
            try
            {
                rtb_editor.Text = File.ReadAllText(allscripts[lbx_script_list.SelectedIndex]);
            }
            catch (Exception ex)
            {
                rtb_editor.Text = "Failed to read file, Exception text:" + Environment.NewLine + Environment.NewLine + Environment.NewLine + ex;
            }            
        }

        public string getConfigValue(string settingName)
        {
            string[] config;
            if (File.Exists($@"C:\SSSC\Resources\Config.txt"))
            {
                config = File.ReadAllLines($@"C:\SSSC\Resources\Config.txt");
            }
            else //if the above config file is missing config must exist in the same directory as the executable
            {
                config = File.ReadAllLines(System.Reflection.Assembly.GetEntryAssembly().Location.Remove(System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\") + 1) + "Config.txt");
            }
            // searches the config file for settings, each line should be settingname=settingvalue in no particular order
            string value;
            foreach (string setting in config)
            {
                try
                {
                    if (setting.Remove(setting.IndexOf('=')) == settingName)
                    {
                        value = setting.Remove(0, setting.IndexOf('=') + 1);
                        if (value.Contains("//"))
                        {
                            value = value.Remove(value.IndexOf("//"));
                        }
                        return value;
                    }
                }
                catch (Exception)
                {
                    //Debug.WriteLine("Line ignored: " + setting);
                    //line was a comment / blank space / not formatted correctly, ignore it
                }

            }
            return null; //no matching setting was found
        }

        private void refresh_scripts()
        {
            string scriptpath = getConfigValue("script_path");
            allscripts = Directory.GetFiles(scriptpath);
            lbx_script_list.Items.Clear();
            foreach (string script in allscripts)
            {
                string scriptname = script.Remove(0, scriptpath.Length);
                scriptname = scriptname.Remove(scriptname.IndexOf('.'));
                lbx_script_list.Items.Add(scriptname);
            }
        }
        private void refresh_scripts(string searchpattern)
        {
            string scriptpath = getConfigValue("script_path");
            allscripts = Directory.GetFiles(scriptpath, searchpattern);
            lbx_script_list.Items.Clear();
            if(allscripts.Length > 0)
            {
                foreach (string script in allscripts)
                {
                    string scriptname = script.Remove(0, scriptpath.Length);
                    scriptname = scriptname.Remove(scriptname.IndexOf('.'));
                    lbx_script_list.Items.Add(scriptname);
                }
            } 
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Overwrite this script?", ">Prod no test", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                File.WriteAllLines(allscripts[lbx_script_list.SelectedIndex], rtb_editor.Lines);
            }            
        }

        private void btn_add_script_Click(object sender, EventArgs e)
        {
            using (var saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt"/*|All files (*.*)|*.*"*/;
                saveFile.FilterIndex = 1;
                saveFile.InitialDirectory = getConfigValue("script_path");
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.Create(saveFile.FileName).Close();                    
                }
            }            
            refresh_scripts();
            lbx_script_list.SelectedIndex = 0;
        }

        private void btn_remove_script_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Delete " + lbx_script_list.SelectedItem.ToString(), ">Prod no test", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                try
                {
                    File.Delete(allscripts[lbx_script_list.SelectedIndex]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting file: " + ex);
                }

                if (lbx_script_list.SelectedIndex == 0) { lbx_script_list.SelectedIndex = 1; }
                refresh_scripts();
                lbx_script_list.SelectedIndex = 0;
            }            
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {            
           this.Close();
        }

        //vars
        public string[] allscripts;

        private void btn_search_Click(object sender, EventArgs e)
        {
            refresh_scripts("*" + tbx_search.Text + "*");
        }

        private void tbx_search_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                refresh_scripts("*" + tbx_search.Text + "*");
            }
        }
    }
}
