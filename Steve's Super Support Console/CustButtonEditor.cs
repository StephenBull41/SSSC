using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Steve_s_Super_Support_Console
{
    public partial class CustButtonEditor : Form
    {
        int buttonNum;
        public CustButtonEditor(int buttonNumber, string buttonText, string localPath, string fileFolder, string device, string saved_script, string drive)
        {            
            InitializeComponent();
            tbxName.Text = buttonText;
            tbxPath.Text = localPath;
            buttonNum = buttonNumber;

            string valid_drive = "CDEFGHIJKLMNOPQRSTUVWXYZ";
            if (!valid_drive.Contains(drive))
            {
                drive = "C"; // either nothing specified(Legacy config) or config has been messed with
            }

            int drive_index = valid_drive.Remove(valid_drive.IndexOf(drive)).Length;
            cbx_drive.SelectedIndex = drive_index;

            //populate script list
            string path = getConfigValue("script_path");
            string[] scriptArray = Directory.GetFiles(path);
            string scriptname;

            // load script list and select the script bound to the button
            foreach(string script in scriptArray)
            {
                scriptname = script.Remove(0, path.Length);
                scriptname = scriptname.Remove(scriptname.IndexOf('.'));
                lbx_scripts.Items.Add(scriptname);
            }
            int i = 0;
            foreach (string script in lbx_scripts.Items)
            {
                if(script == saved_script)
                {
                    goto script_found;
                }
                i++;
            }
            i = 0;//no script was found, there will be no index beyond 0
            script_found:;
            lbx_scripts.SelectedIndex = i;


            //choose which checkbox starts checked based on what's in the config
            if (fileFolder == "file")
            {
                rtbTextFile.Checked = true;
            }
            else if (fileFolder == "script")
            {
                rbt_scripts.Checked = true;
            }
            else if (fileFolder == "folder")
            {
                if(localPath.Length == 0) { goto skip; } //for shortcuts to drives that have no path field
                string pathcheck = localPath.Remove(0, (localPath.Length - 1)); // get the last character
                if (pathcheck == @"\")
                {
                    rtbFolder.Checked = true;
                }
                else
                {
                    rtbExe.Checked = true;
                }
            }
            skip:;
            //Device Index: 0=BOC, 1=Postec, 2=MWS, 3=CWS1, 4=CWS2, 5=CWS3, 6=Local
            int deviceIndex;
            switch (device)
            {
                case "BOC":
                    deviceIndex = 0;
                    break;
                case "Postec":
                    deviceIndex = 1;
                    break;
                case "MWS":
                    deviceIndex = 2;
                    break;
                case "CWS1":
                    deviceIndex = 3;
                    break;
                case "CWS2":
                    deviceIndex = 4;
                    break;
                case "CWS3":
                    deviceIndex = 5;
                    break;
                case "Local":
                    deviceIndex = 6;
                    break;
                default:
                    deviceIndex = 6;
                    break;
            }
            lbxTarget.SelectedIndex = deviceIndex;

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            if (rbt_scripts.Checked)
            {
                tbxPath.Enabled = false;
                lbxTarget.Enabled = false;
                lbx_scripts.Enabled = true;
            }
            else
            {
                tbxPath.Enabled = true;
                lbxTarget.Enabled = true;
                lbx_scripts.Enabled = false;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Write to file new settings

            // load current config
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            string[] allConfig = File.ReadAllLines($@"{getConfigValue("cbc_root")}{UserName}_CBC.txt");
            string type;
            //create new config line
            string pathcheck;
            try
            {
                pathcheck = tbxPath.Text.Remove(0, (tbxPath.Text.Length - 1));
            }
            catch (Exception)
            {
                pathcheck = @"\"; // should only occur if the user is making a shortcut to a devices C drive as no text would be entered
            }            

            if (rtbExe.Checked && pathcheck != @"\")
            {
                type = "folder";
            }
            else if (rtbFolder.Checked && pathcheck == @"\")
            {
                type = "folder";
            }
            else if (rtbTextFile.Checked && pathcheck != @"\")
            {
                type = "file";
            }
            else if (rbt_scripts.Checked)
            {
                type = "script";
            }
            else
            {
                lblPathError.Text = "Error: invalid file path or need to check a type";
                lblPathError.ForeColor = Color.Red;
                goto Exit;
            }

            if (lbxTarget.SelectedIndex < 0 || lbxTarget.SelectedIndex > 6)
            {
                lblPathError.Text = "Error: Select a device";
                goto Exit;
            }
            string device = lbxTarget.SelectedItem.ToString();
            string scriptname = "";
            string path = getConfigValue("script_path");
            string[] allscripts = Directory.GetFiles(path);
            string selected_script = "";
            if (type == "script")
            {
                foreach (string script in allscripts)
                {
                    scriptname = script.Remove(0, path.Length);
                    scriptname = scriptname.Remove(scriptname.IndexOf('.'));
                    if (scriptname == lbx_scripts.SelectedItem.ToString())
                    {
                        selected_script = scriptname;
                    }
                }
            }

            string newLine = tbxName.Text + "," + tbxPath.Text + "," + type + ",1," + device + "," + selected_script + "," + cbx_drive.Text;

            allConfig[buttonNum] = newLine;
            File.WriteAllLines($@"{getConfigValue("cbc_root")}{UserName}_CBC.txt", allConfig);

            //Update Main UI
            if (System.Windows.Forms.Application.OpenForms["Main"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["Main"] as Main).CustomButtonInit();
            }
            this.Close();
            Exit:;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void rbt_scripts_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void rtbFolder_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void rtbExe_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void rtbTextFile_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void cbx_drive_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_driveprompt.Text = cbx_drive.Text + ":\\";
        }
    }
}
