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
    public partial class AlertViewer : Form
    {
        public AlertViewer(string SiteID)
        {
            InitializeComponent();
            string[] AllLines = File.ReadAllLines(getConfigValue("alerts"));
            foreach (string line in AllLines)
            {
                string[] row = line.Split(',');
                if (row[0] == SiteID)
                {
                    int i = 0;
                    foreach(string value in row)
                    {
                        try
                        {
                            string newvalue = value.Replace('`', ',');
                            row[i] = newvalue;
                        } catch
                        {
                            // no commas, do nothing
                        }
                        i++;
                    }
                    rtbAlarm.Text = "Start: " + row[1] + Environment.NewLine + "End: " + row[2] + Environment.NewLine + "Last edit: " + row[3] + Environment.NewLine + row[4];
                }
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

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
