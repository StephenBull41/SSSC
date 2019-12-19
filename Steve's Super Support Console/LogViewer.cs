using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Steve_s_Super_Support_Console
{
    public partial class LogViewer : Form
    {
        public LogViewer()
        {
            InitializeComponent();
            //nudHours.Maximum = Convert.ToInt32(getConfigValue("win_log_max_time_range"));
            //if(nudHours.Maximum == 0) { nudHours.Maximum = 12; }
            int lim = Convert.ToInt32(getConfigValue("win_log_max_time_range"));
            if(lim < 1 || lim > 168) { lim = 12; }
            nudHours.Maximum = lim;
        }

        private void btn_Get_ELog_Click(object sender, EventArgs e)
        {
            Thread logGet = new Thread(eventSearcher);
            logGet.Start();
        }

        private void eventSearcher()
        {
            this.Invoke(new MethodInvoker(delegate { lbx_TypeList.Items.Clear(); lbx_UIDList.Items.Clear(); rtb_EventView.Clear(); }));
            this.Invoke(new MethodInvoker(delegate { lblStatus.Text = "Fetching logs"; lblStatus.ForeColor = Color.Yellow; }));

            
            string path = $@"{getConfigValue("win_log")}winlog_{GetRand8()}.csv";
            string endOfFileFlag = "endoflog";

            string address = tbxAddress.Text;
            string time = nudHours.Value.ToString();
            string filter = "";
            if (cbxCritical.Checked) { filter += "c"; }
            if (cbxException.Checked) { filter += "e"; }
            if (cbxInformation.Checked) { filter += "i"; }
            if (cbxVerbose.Checked) { filter += "v"; }
            if (cbxWarning.Checked) { filter += "w"; }

            if(filter == "" || address.Length < 7) { goto exit; }

            string line0 = "@echo off";
            string line1 = @"cd C:\";
            string line2 = $"echo SSSC - searching far and wide for logs >> {path}";
            string line3 = $@"psloglist \\{address} -f {filter} -h {time} application -s >> {path}";
            string line4 = $"timeout -t 1"; 
            string line5 = $"echo endoflog>> {path}";
            string[] alltext = { line0, line1, line2, line3, line4, line5 };

            string rand8 = GetRand8();
            batrand = rand8; // I really don't understand why I need to do this, rand8 & batchpath are somehow set to null by the end of this thread
            string batchpath = $@"{getConfigValue("logs")}LogFetch_{rand8}.bat";
            string deletepath = batchpath;
            if (File.Exists(batchpath)) { goto exit; }
            File.Create(batchpath).Close();            
            File.WriteAllLines(batchpath, alltext);
            Process.Start(batchpath);            
            Thread.Sleep(5000);
            int i = 0;
            bool readComplete = false;
            bool unique = false;

            while(!readComplete && i < Convert.ToInt32(getConfigValue("win_log_search_timeout")))
            {
                if (File.Exists(path))
                {
                    try
                    {
                        log = File.ReadAllLines(path);
                    }catch (Exception)
                    {
                        goto loopend;
                    }

                    if(log.Length < 3) { goto loopend; }

                    if(log[log.Length -1] == endOfFileFlag || log[log.Length -2] == endOfFileFlag)
                    {
                        readComplete = true;
                        foreach(string logEvent in log)
                        {
                            Debug.WriteLine("Entered foreach");
                            unique = false;
                            string[] property = { "","",""};
                            try
                            {
                                Debug.WriteLine("entered try");
                                if (!logEvent.Contains(',')) { goto foreachend; }
                                Debug.WriteLine("passed , check");
                                property = logEvent.Split(',');
                                Debug.WriteLine("event = " + property[2]);
                                unique = true;
                                foreach (var item in lbx_UIDList.Items)
                                {
                                    Debug.WriteLine(item.ToString());
                                    if (property[2] == item.ToString()) { unique = false; }
                                    Debug.WriteLine("end foreach");
                                }
                            }
                            catch (Exception)
                            {
                                //probably end of file marker or last line, ignore
                            }
                            Debug.WriteLine(property[0]);
                            if (unique) {
                                this.Invoke(new MethodInvoker(delegate {lbx_UIDList.Items.Add(property[2]);}));
                            }
                            foreachend:;
                        }
                    }

                }
                loopend:;
                i++;
                Thread.Sleep(1000);
            }
            exit:;
            this.Invoke(new MethodInvoker(delegate { lblStatus.Text = "No search active"; lblStatus.ForeColor = Color.LightGreen; }));
            Thread.Sleep(15000);
            try
            {
                File.Delete($@"{getConfigValue("logs")}LogFetch_{batrand}.bat");
                File.Delete(path);
            }
            catch (Exception)
            {

            }
        }

        private void lbx_UIDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbx_TypeList.Items.Clear();
            string item = lbx_UIDList.SelectedItem.ToString();
            try
            {
                foreach (string logEvent in log)
                {
                    if (logEvent.Contains(','))
                    {
                        string[] property = logEvent.Split(',');
                        if (property[2] == item) { lbx_TypeList.Items.Add(property[0] + " - " + property[5]); }
                    }
                }
            }
            catch (Exception) { } //expected exception when user clicks in the UID form but no log is loaded
        }

        private void lbx_TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = lbx_TypeList.SelectedItem.ToString();
            item = item.Remove(item.IndexOf(" ")); // get just the event ID
            foreach (string logEvent in log)
            {
                if (logEvent.Contains(','))
                {
                    string[] property = logEvent.Split(',');
                    if (property[0] == item)
                    {
                        rtb_EventView.Text = "Event ID: " + property[0] + Environment.NewLine
                            + "Source: " + property[2] + Environment.NewLine
                            + "Level: " + property[3] + Environment.NewLine
                            + "Host: " + property[4] + Environment.NewLine
                            + "Date / time: " + property[5] + Environment.NewLine
                            + "Event ID: " + property[6] + Environment.NewLine
                            + "User: " + property[7] + Environment.NewLine + Environment.NewLine
                            + "Details:" + Environment.NewLine + property[8] + Environment.NewLine;

                        //bloody commies putting commas in my logs

                        if (property.Length > 9)
                        {
                            int i = 0;
                            foreach (string a in property)
                            {
                                if (i > 8) //8 is the first info field for the event & the last actual field, any field after that is the same info & needs to be added
                                {
                                    rtb_EventView.AppendText(a);
                                }
                                i++;
                            }
                        }
                    }
                }
            }
        }

        private string GetRand8()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[8];
            var random = new Random();
            for (int a = 0; a < stringChars.Length; a++)
            {
                stringChars[a] = chars[random.Next(chars.Length)];
            }
            var RandS = new String(stringChars);
            return RandS;
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

        //Vars

        public string[] log;
        public string batrand;

        private void btn_save_log_Click(object sender, EventArgs e)
        {
            using (var saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.InitialDirectory = getConfigValue("win_log_save_directory");
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFile.FileName, log);
                }
            }
        }

        private void btn_load_log_Click(object sender, EventArgs e)
        {
            using(var load_file = new OpenFileDialog())
            {
                load_file.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                load_file.FilterIndex = 1;
                load_file.InitialDirectory = getConfigValue("win_log_save_directory");
                if (load_file.ShowDialog() == DialogResult.OK)
                {
                    log = File.ReadAllLines(load_file.FileName);
                    
                    bool unique = false;
                    foreach (string logEvent in log)
                    {
                        Debug.WriteLine("Entered foreach");
                        unique = false;
                        string[] property = { "", "", "" };
                        try
                        {
                            if (!logEvent.Contains(',')) { goto foreachend; }
                            property = logEvent.Split(',');
                            unique = true;
                            foreach (var item in lbx_UIDList.Items)
                            {
                                Debug.WriteLine(item.ToString());
                                if (property[2] == item.ToString()) { unique = false; }
                                Debug.WriteLine("end foreach");
                            }
                        }
                        catch (Exception)
                        {
                            //probably end of file marker or last line, ignore
                        }
                        Debug.WriteLine(property[0]);
                        if (unique)
                        {
                            this.Invoke(new MethodInvoker(delegate { lbx_UIDList.Items.Add(property[2]); }));
                        }
                        foreachend:;
                    }
                }
            }
        }
    }
}
