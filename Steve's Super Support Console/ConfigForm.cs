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
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            config = File.ReadAllLines(@"C:\SSSC\Resources\Config.txt");
            LoadVarList();
        }

        private void LoadVarList()
        {
            foreach(string var in config)
            {
                if (var.Contains("="))
                {
                    string varname = var.Remove(var.IndexOf("="));
                    if (!varname.Contains("//")) //A comment before the variable value is not a valid line
                    {
                        lbx_Var_List.Items.Add(varname);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //exit button
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // save button

        }

        public string getConfigValue(string settingName)
        {
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

        public string[] getConfigValueWithComments(string settingName)
        {
            // searches the config file for settings, each line should be settingname=settingvalue in no particular order
            string value;
            string comment = null;
            string[] retval = { null, null };
            foreach (string setting in config)
            {

                try
                {
                    if (setting.Remove(setting.IndexOf('=')) == settingName)
                    {
                        value = setting.Remove(0, setting.IndexOf('=') + 1);
                        if (value.Contains("//"))
                        {
                            comment = value.Remove(0, value.IndexOf("//"));
                            value = value.Remove(value.IndexOf("//"));
                        }
                        retval[0] = value;
                        retval[1] = comment;
                        return retval;
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

        private void lbx_Var_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            string var = getConfigValue(lbx_Var_List.SelectedItem.ToString());
            tbx_Var_Value.Text = var;
        }

        private void btnVarSave_Click(object sender, EventArgs e)
        {
            if(lbx_Var_List.SelectedIndex == -1) { goto exit; } //no selection was made
            config = File.ReadAllLines(@"C:\SSSC\Resources\Config.txt");
            string[] varinfo = getConfigValueWithComments(lbx_Var_List.SelectedItem.ToString());
            string varname = null;
            int i = 0;
            bool change = false;
            int index = lbx_Var_List.SelectedIndex;
            foreach(string vartext in config)
            {

                if (vartext.Contains("="))
                {
                    varname = vartext.Remove(vartext.IndexOf("="));
                    if (varname == lbx_Var_List.SelectedItem.ToString())
                    {
                        config[i] = varname + "=" + tbx_Var_Value.Text + varinfo[1];
                        change = true;
                    }

                }
                i++;
            }
            if (change) { File.WriteAllLines(@"C:\SSSC\Resources\Config.txt", config); }

            lbx_Var_List.Items.Clear();
            LoadVarList();
            lbx_Var_List.SelectedIndex = -1;
            tbx_Var_Value.Clear();
            exit:;
        }


        //Vars
        public static string[] config;

    }
}
