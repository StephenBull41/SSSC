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
    public partial class CustButtonManager : Form
    {
        public CustButtonManager()
        {
            InitializeComponent();
            if (File.Exists($@"C:\SSSC\Resources\Config.txt"))
            {
                config = File.ReadAllLines($@"C:\SSSC\Resources\Config.txt");
            }
            else //if the above config file is missing config must exist in the same directory as the executable
            {
                config = File.ReadAllLines(System.Reflection.Assembly.GetEntryAssembly().Location.Remove(System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\") + 1) + "Config.txt");
            }
            current_user = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));

            foreach (string script in Directory.GetFiles(getConfigValue("script_path")))
            {
                string scriptname = script.Remove(0, script.LastIndexOf('\\') + 1);
                scriptname = scriptname.Remove(scriptname.IndexOf('.'));
                lbx_scripts.Items.Add(scriptname);
            }
            RefreshButtonList();
        }

        private void RefreshButtonList()
        {
            lbx_buttons.Items.Clear();
            configpath = $@"{getConfigValue("cbc_root")}{current_user}_CBC.txt";
            button_config = File.ReadAllLines(configpath);
            //new config:     button / divider text, localPath, Type, device, drive, script, function, font size, font color(R), font color(G), font color(B), background color(R),background color(G),background color(B), width
            //type = file / folder / executable / function / script / divider


            int i = 0;
            foreach (string button in button_config)
            {
                if (i != 0 && button != "")
                {
                    //create string to display in the list box
                    //"[name] - [type] - [path/script/function]
                    //file folder executable function
                    string[] config = button.Split(',');
                    string vartext = "";
                    switch (config[2])
                    {
                        case "file":
                            vartext = config[1];
                            break;
                        case "folder":
                            vartext = config[1];
                            break;
                        case "executable":
                            vartext = config[1];
                            break;
                        case "function":
                            vartext = config[6];
                            break;
                        case "script":
                            vartext = config[5];
                            break;
                        default:
                            break;
                    }
                    string display_txt = config[0] + " - " + config[2] + " - " + vartext;
                    lbx_buttons.Items.Add(display_txt);
                }
                i++;
            }
            lbx_buttons.SelectedIndex = 0;
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

        private void btn_up_actual_Click(object sender, EventArgs e)
        {
            if (lbx_buttons.SelectedIndex == 0) { MessageBox.Show("Can't move the first item up"); goto exit; }
            int i = 0;
            if(lbx_buttons.SelectedIndex > 0){
                i = lbx_buttons.SelectedIndex + 1;
                string move_up_ln = button_config[i];
                string move_dn_ln = button_config[i - 1];
                button_config[i] = move_dn_ln;
                button_config[i - 1] = move_up_ln;
            }
            //File.WriteAllLines(configpath, button_config);
            File.WriteAllText(configpath, "");
            for (int j = 0; j < button_config.Length - 1; j++)
            {
                File.AppendAllText(configpath, button_config[j] + Environment.NewLine);
            }
            File.AppendAllText(configpath, button_config[button_config.Length - 1]);

            RefreshButtonList();
            lbx_buttons.SelectedIndex = i - 2;
            exit:;
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            if(lbx_buttons.SelectedIndex == lbx_buttons.Items.Count - 1) { MessageBox.Show("Can't move the last item down"); goto exit; }
            int i = 0;
            if (lbx_buttons.SelectedIndex >= 0)
            {
                i = lbx_buttons.SelectedIndex + 1;
                string move_up_ln = button_config[i + 1];
                string move_dn_ln = button_config[i];
                button_config[i] = move_up_ln;
                button_config[i + 1] = move_dn_ln;
            }
            //File.WriteAllLines(configpath, button_config);
            File.WriteAllText(configpath, "");
            for (int j = 0; j < button_config.Length - 1; j++)
            {
                File.AppendAllText(configpath, button_config[j] + Environment.NewLine);
            }
            File.AppendAllText(configpath, button_config[button_config.Length - 1]);

            RefreshButtonList();
            lbx_buttons.SelectedIndex = i;
            exit:;
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string defaultline = @"new button,SSSC\,folder,Local,C,,,8,0,0,0,220,220,220,84";
            File.AppendAllText(configpath, Environment.NewLine + defaultline);
            RefreshButtonList();
            lbx_buttons.SelectedIndex = lbx_buttons.Items.Count - 1;
        }

        private void lbx_buttons_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(lbx_buttons.SelectedIndex.ToString());
            //Update_editor(lbx_buttons.SelectedIndex + 1);            
            lbx_scripts.Items.Clear();
            lbx_functions.SelectedIndex = -1;
            DirectoryInfo d = new DirectoryInfo(getConfigValue("script_path"));
            foreach (var file in d.GetFiles("*.txt"))
            {
                lbx_scripts.Items.Add(file.ToString().Remove(file.ToString().IndexOf(".")));
            }
            Update_editor(lbx_buttons.SelectedIndex + 1);
            btn_save.BackColor = Color.FromArgb(220, 220, 220);
        }

        private void Update_editor(int button_index)
        {
            string[] a = button_config[button_index].Split(',');
            string text = a[0];
            string path = a[1];
            string type = a[2];
            string device = a[3];
            string drive = a[4];
            string script = a[5];
            string function = a[6];
            string font_s = a[7];
            string font_r = a[8];
            string font_g = a[9];
            string font_b = a[10];
            string back_r = a[11];
            string back_g = a[12];
            string back_b = a[13];
            string width = a[14];

            lbx_scripts.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(getConfigValue("script_path"));
            foreach (var file in d.GetFiles("*.txt"))
            {
                lbx_scripts.Items.Add(file.ToString().Remove(file.ToString().IndexOf(".")));
            }

            tbx_text.Text = text;
            tbx_path.Text = path;
            
            switch (type)
            {
                case "folder":
                    rbt_folder.Checked = true;
                    break;
                case "file":
                    rbt_text.Checked = true;
                    break;
                case "executable":
                    rbt_executable.Checked = true;
                    break;
                case "script":
                    rbt_script.Checked = true;
                    break;
                case "function":
                    rbt_function.Checked = true;
                    break;
                case "divider":
                    rbt_divider.Checked = true;
                    break;
                case "ps1":
                    rbt_ps1.Checked = true;
                    break;
                default:
                    MessageBox.Show("unsupported type");
                    break;
            }

            cbx_target.Text = device;
            cbx_drive.Text = drive;

            int i = 0;
            int selectedscript = 999;
            int selectedfunction = 999;
            foreach (string item in lbx_scripts.Items)
            {
                //if (item == script) { lbx_scripts.SelectedIndex = i; }
                if (item == script) { selectedscript = i; }
                i++;
            }
            if(selectedscript != 999) { lbx_scripts.SelectedIndex = selectedscript; }

            i = 0;
            foreach(string item in lbx_functions.Items)
            {
                //if (item == function) { lbx_functions.SelectedIndex = i; }
                if (item == function) { selectedfunction = i; }
                i++;
            }
            if(selectedfunction != 999) { lbx_functions.SelectedIndex = selectedfunction; }

            tbx_font_size.Text = font_s;
            tbx_font_R.Text = font_r;
            tbx_font_G.Text = font_g;
            tbx_font_B.Text = font_b;

            tbx_background_R.Text = back_r;
            tbx_background_G.Text = back_g;
            tbx_background_B.Text = back_b;

            if(width == "84") { rbt_width_half.Checked = true; } else { rbt_width_full.Checked = true; }

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Main"] != null)
            {
                (System.Windows.Forms.Application.OpenForms["Main"] as Main).CustomButtonInit();
            }
            this.Close();
        }

        public string[] config;
        public string[] button_config;
        public string current_user;
        public string configpath = "";

        private void btn_save_Click(object sender, EventArgs e)
        {
            int s = lbx_buttons.SelectedIndex + 1;
            string text = tbx_text.Text;
            string path = tbx_path.Text;
            string device = cbx_target.Text;
            string drive = cbx_drive.Text;
            string font_s = tbx_font_size.Text;
            string font_r = tbx_font_R.Text;
            string font_g = tbx_font_G.Text;
            string font_b = tbx_font_B.Text;
            string back_r = tbx_background_R.Text;
            string back_g = tbx_background_G.Text;
            string back_b = tbx_background_B.Text;



            string type = "";
            if (rbt_divider.Checked == true)
            {
                type = "divider";
            }
            else if (rbt_executable.Checked == true)
            {
                type = "executable";
            }
            else if (rbt_folder.Checked == true)
            {
                type = "folder";
            }
            else if (rbt_function.Checked == true)
            {
                type = "function";
            }
            else if (rbt_script.Checked == true)
            {
                type = "script";
            }
            else if (rbt_text.Checked == true)
            {
                type = "file";
            }
            else if (rbt_ps1.Checked == true)
            {
                type = "ps1";
            }

            string script = "";
            if (lbx_scripts.Items.Count > 0 && lbx_scripts.SelectedIndex >= 0)
            {
                script = lbx_scripts.SelectedItem.ToString();
            }

            string function = "";
            if (lbx_functions.Items.Count > 0 && lbx_functions.SelectedIndex >= 0 && type == "function")
            {
                function = lbx_functions.SelectedItem.ToString();
            }

            string width = "84";
            if (rbt_width_half.Checked) { width = "84"; } else { width = "174"; }

            string config_line = text + "," + path + "," + type + "," + device + "," + drive + "," + script + "," + function + "," + font_s + "," + font_r + "," + font_g + "," + font_b + "," + back_r + "," + back_g + "," + back_b + "," + width;
            button_config[s] = config_line;
            //File.WriteAllLines(configpath, button_config);

            //Cannot have a blank line in the file
            File.WriteAllText(configpath, "");
            for(int i = 0; i < button_config.Length - 1; i++)
            {
                File.AppendAllText(configpath,button_config[i] + Environment.NewLine);
            }
            File.AppendAllText(configpath, button_config[button_config.Length - 1]);
            


            RefreshButtonList();
            lbx_buttons.SelectedIndex = s - 1;
            btn_save.BackColor = Color.FromArgb(220, 220, 200);
        }

        //Save button color changes

        private void rbt_folder_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_text_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_executable_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_script_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_function_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_divider_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_text_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_width_half_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void rbt_width_full_CheckedChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_font_size_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_font_R_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_font_G_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_font_B_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_background_R_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_background_G_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_background_B_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void cbx_target_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void cbx_drive_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void tbx_path_TextChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void lbx_scripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void lbx_functions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_save.BackColor = Color.FromArgb(220, 250, 200);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            File.WriteAllText(configpath, "");

            if (lbx_buttons.SelectedIndex == lbx_buttons.Items.Count - 1)
            {
                for (int i = 0; i < button_config.Length - 2; i++)
                {
                    if (i != lbx_buttons.SelectedIndex + 1) { File.AppendAllText(configpath, button_config[i] + Environment.NewLine); }
                }
                File.AppendAllText(configpath, button_config[button_config.Length - 2]);
            }
            else
            {
                for (int i = 0; i < button_config.Length - 1; i++)
                {
                    if (i != lbx_buttons.SelectedIndex + 1) { File.AppendAllText(configpath, button_config[i] + Environment.NewLine); }
                }
                File.AppendAllText(configpath, button_config[button_config.Length - 1]);
            }
            RefreshButtonList();
        }
    }
}
