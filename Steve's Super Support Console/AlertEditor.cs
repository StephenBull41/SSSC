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
    public partial class AlertEditor : Form
    {
        public string EditSite;
        public string AlertSDate;
        public string AlertEDate;
        public string AlertAllText;

        public AlertEditor(string Site, string AlertStart, string AlertEnd, string AlertText)
        {
            InitializeComponent();
            EditSite = Site;
            AlertSDate = AlertStart;
            AlertEDate = AlertEnd;
            AlertAllText = AlertText;
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

        private void Form3_Load(object sender, EventArgs e)
        {
            if (EditSite == "new") // adding a new entry
            {

            }
            else if (EditSite != "") // editing an entry
            {
                tbxSiteID.Text = EditSite;
                tbxSiteID.Enabled = false;
                tbxSDate.Text = AlertSDate;
                tbxEDate.Text = AlertEDate;
                tbxAlertNotes.Text = AlertAllText;
            }
            else // edit button selected without selecting what to edit
            {
                this.Close();
            }
        }

        private void btnAlertClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAlertSave_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, 5);

            string line1 = tbxSiteID.Text;
            line1 = line1.Replace(',','`') + ",";
            string line2 = tbxSDate.Text;
            line2 = line2.Replace(',', '`') + ",";
            string line3 = tbxEDate.Text;
            line3 = line3.Replace(',', '`') + ",";
            string line4 = UserName;
            line4 = line4.Replace(',', '`') + ",";
            string line5 = tbxAlertNotes.Text;
            line5 = line5.Replace(',', '`');
            string NewLine = line1 + line2 + line3 + line4 + line5;

            if (EditSite == "new") // write a new line to file
            {
                File.AppendAllText(@"C:\SSSC\Resources\SSSCAlerts.csv", Environment.NewLine + NewLine);
            }
            else // edit existing line
            {
                string[] AllLines = File.ReadAllLines(@"C:\SSSC\Resources\SSSCAlerts.csv");
                File.WriteAllText(@"C:\SSSC\Resources\SSSCAlerts.csv", String.Empty);
                int i = 0;
                foreach (string line in AllLines)
                {
                    string[] row = line.Split(',');
                    if (row[0] == tbxSiteID.Text)
                    {
                        AllLines[i] = NewLine;
                    }
                    i++;
                }
                File.WriteAllLines(@"C:\SSSC\Resources\SSSCAlerts.csv", AllLines);
                // there's a rouge breakline somewhere in the above code causing issues, the rest of the code rewrites the file without it
                string[] lines = File.ReadAllLines(@"C:\SSSC\Resources\SSSCAlerts.csv");
                File.WriteAllText(@"C:\SSSC\Resources\SSSCAlerts.csv", String.Empty, Encoding.UTF8);
                bool fLine = true;
                foreach (string line in lines)
                {
                    if (line != "" && fLine == false)
                    {
                        File.AppendAllText(@"C:\SSSC\Resources\SSSCAlerts.csv", Environment.NewLine + line, Encoding.UTF8);
                    }
                    if (line != "" && fLine == true)
                    {
                        File.AppendAllText(@"C:\SSSC\Resources\SSSCAlerts.csv", line, Encoding.UTF8);
                        fLine = false;
                    }
                }
            }
            this.Close();
        }
    }
}
