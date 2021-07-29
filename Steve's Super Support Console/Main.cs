//https://stackoverflow.com/questions/13405848/how-to-perform-multiple-pings-in-parallel-using-c-sharp
//https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Steve_s_Super_Support_Console
{

    public partial class Main : Form
    {
        #region Startup/Init
        public Main()
        {

            InitializeComponent();

            this.ActiveControl = tbxSiteID;
            tbxSiteID.Focus();
            lblMismatch.Text = "";

            //some windows version return domain name & some don't, remove if a domain was returned
            if (currentUser.Contains("\\")) { currentUser = currentUser.Remove(0, currentUser.LastIndexOf('\\') + 1); }

            if (File.Exists($@"C:\SSSC\Resources\Config.txt"))
            {
                config = File.ReadAllLines($@"C:\SSSC\Resources\Config.txt");
            }
            else //if the above config file is missing config must exist in the same directory as the executable
            {
                config = File.ReadAllLines(System.Reflection.Assembly.GetEntryAssembly().Location.Remove(System.Reflection.Assembly.GetEntryAssembly().Location.LastIndexOf("\\") + 1) + "Config.txt");
            }

            //currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Remove(0, System.Security.Principal.WindowsIdentity.GetCurrent().Name.LastIndexOf('\\') + 1);

            Thread countDownThread = new Thread(Countdown);
            countDownThread.IsBackground = false;
            countDownThread.Start();

            this.KeyPreview = true;

            LoadIcons();

            //If not the current version then quit
            bool whitelisted = false;
            string Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string[] Peeps = File.ReadAllLines(getConfigValue("whitelist"));
            foreach (string Dude in Peeps) //check if user is whitelisted
            {
                if (Username.ToUpper() == Dude.ToUpper())
                {
                    whitelisted = true;
                    //btnOPTAddEntry.Visible = true;    buttons are bugged and I don't intend on fixing them
                    //btnOPTEdit.Visible = true;
                }
            }

            string live = File.ReadAllText(getConfigValue("version"));

            if (version != live)
            {
                if (whitelisted)
                {
                    lblMismatch.Text = "";
                }
                else
                {
                   Environment.Exit(5);
                }
            }

            //check if user is blacklisted, if a user is blaclisted they're in training & locked out of scripts
            // and context menus
            Peeps = File.ReadAllLines(getConfigValue("blacklist"));
            foreach(string dude in Peeps)
            {
                if(Username.ToUpper() == dude.ToUpper())
                {
                    fnc_blacklisted = true;
                }
            }

            onLoadFormat();
            CustomButtonInit();

            // load sites data
            siteIPData = File.ReadAllLines(getConfigValue("ip_data_path"));
            siteInventoryData = File.ReadAllLines(getConfigValue("inventory_data_path"));

            //more misc shit
            lbl_cust_device1.Text = getConfigValue("custom_device_1_name");
            lbl_cust_device2.Text = getConfigValue("custom_device_2_name");
            lbl_cust_device3.Text = getConfigValue("custom_device_3_name");
            lbl_cust_device4.Text = getConfigValue("custom_device_4_name");

            initTT();
            init_conlist();
        }

        //              Init & misc events

        public void init_conlist()
        {
            conlist.Add(pbxPos1);
            conlist.Add(pbxPos2);
            conlist.Add(pbxPos3);
            conlist.Add(pbxPos4);
            conlist.Add(pbxPP1);
            conlist.Add(pbxPP2);
            conlist.Add(pbxPP3);
            conlist.Add(pbxPP4);
            conlist.Add(pbxBoc);
            conlist.Add(pbxPostec);
            conlist.Add(pbxSwitch);
            conlist.Add(pbxRouter);

            conlist.Add(pbx_POS5);
            conlist.Add(pbx_PP5);
            conlist.Add(pbx_POS6);
            conlist.Add(pbx_PP6);
            conlist.Add(pbx_POS7);
            conlist.Add(pbx_PP7);
            conlist.Add(pbx_DOMS);
            conlist.Add(pbx_DOMS58);

            conlist.Add(pbx_cust1);
            conlist.Add(pbx_cust2);
            conlist.Add(pbx_cust3);
            conlist.Add(pbx_cust4);
        }

        public void CustomButtonInit()
        {
            List<Control> conlist = flowLayoutPanel1.Controls.Cast<Control>().ToList();
            foreach (Control cb in conlist)
            {
                if (cb.Name != "btn_CBManage")
                {
                    flowLayoutPanel1.Controls.Remove(cb);
                    cb.Dispose();
                }
            }

            //MessageBox.Show(currentUser);

            if (File.Exists(getConfigValue("cbc_root") + $"{currentUser}_CBC.txt"))
            {
                string[] config = File.ReadAllLines(getConfigValue("cbc_root") + $"{currentUser}_CBC.txt");
                int i = 0;
                foreach (string button in config)
                {
                    if (i > 0 && button != "")
                    {

                        string[] fields = button.Split(',');
                        if (fields[2] == "divider")
                        {
                            Label div_label = new Label();
                            div_label.Name = "cbc_" + i;
                            div_label.Text = fields[0];
                            div_label.Font = new Font(div_label.Font.FontFamily, Convert.ToInt32(fields[7]));
                            div_label.Size = new Size(172, 21);
                            div_label.TextAlign = ContentAlignment.MiddleCenter;
                            div_label.ForeColor = Color.FromArgb(Convert.ToInt32(fields[8]), Convert.ToInt32(fields[9]), Convert.ToInt32(fields[10]));
                            div_label.BackColor = Color.FromArgb(Convert.ToInt32(fields[11]), Convert.ToInt32(fields[12]), Convert.ToInt32(fields[13]));
                            flowLayoutPanel1.Controls.Add(div_label);
                        }
                        else
                        {
                            Button newbutton = new Button();
                            newbutton.Name = "cbc_" + i;
                            newbutton.Text = fields[0];
                            newbutton.FlatStyle = FlatStyle.Flat;
                            newbutton.Font = new Font(newbutton.Font.FontFamily, Convert.ToInt32(fields[7]));
                            newbutton.Size = new Size(Convert.ToInt32(fields[14]), 23);
                            newbutton.ForeColor = Color.FromArgb(Convert.ToInt32(fields[8]), Convert.ToInt32(fields[9]), Convert.ToInt32(fields[10]));
                            newbutton.BackColor = Color.FromArgb(Convert.ToInt32(fields[11]), Convert.ToInt32(fields[12]), Convert.ToInt32(fields[13]));
                            newbutton.Click += new EventHandler(CB_Click);
                            flowLayoutPanel1.Controls.Add(newbutton);
                        }
                    }
                    i++;
                }
            }
            else
            {
                string[] default_cbc = File.ReadAllLines(getConfigValue("cbc_root") + "Default.txt");
                File.WriteAllLines(getConfigValue("cbc_root") + $"{currentUser}_CBC.txt", default_cbc);
                int i = 0;
                foreach (string button in default_cbc)
                {
                    if (i > 0 && button != "")
                    {

                        string[] fields = button.Split(',');
                        if (fields[2] == "divider")
                        {
                            Label div_label = new Label();
                            div_label.Name = "cbc_" + i;
                            div_label.Text = fields[0];
                            div_label.Font = new Font(div_label.Font.FontFamily, Convert.ToInt32(fields[7]));
                            div_label.Size = new Size(172, 21);
                            div_label.TextAlign = ContentAlignment.MiddleCenter;
                            div_label.ForeColor = Color.FromArgb(Convert.ToInt32(fields[8]), Convert.ToInt32(fields[9]), Convert.ToInt32(fields[10]));
                            div_label.BackColor = Color.FromArgb(Convert.ToInt32(fields[11]), Convert.ToInt32(fields[12]), Convert.ToInt32(fields[13]));
                            flowLayoutPanel1.Controls.Add(div_label);
                        }
                        else
                        {
                            Button newbutton = new Button();
                            newbutton.Name = "cbc_" + i;
                            newbutton.Text = fields[0];
                            newbutton.FlatStyle = FlatStyle.Flat;
                            newbutton.Font = new Font(newbutton.Font.FontFamily, Convert.ToInt32(fields[7]));
                            newbutton.Size = new Size(Convert.ToInt32(fields[14]), 23);
                            newbutton.ForeColor = Color.FromArgb(Convert.ToInt32(fields[8]), Convert.ToInt32(fields[9]), Convert.ToInt32(fields[10]));
                            newbutton.BackColor = Color.FromArgb(Convert.ToInt32(fields[11]), Convert.ToInt32(fields[12]), Convert.ToInt32(fields[13]));
                            newbutton.Click += new EventHandler(CB_Click);
                            flowLayoutPanel1.Controls.Add(newbutton);
                        }
                    }
                    i++;
                }
                //MessageBox.Show("Couldn't find: " + getConfigValue("cbc_root") + $"{currentUser}_CBC.txt");
            }


        }

        void initTT()
        {
            btn_tt1.Text = getConfigValue("text_tt1");
            btn_tt2.Text = getConfigValue("text_tt2");
            btn_tt3.Text = getConfigValue("text_tt3");
            btn_tt4.Text = getConfigValue("text_tt4");
            btn_tt5.Text = getConfigValue("text_tt5");
            btn_tt6.Text = getConfigValue("text_tt6");
            btn_tt7.Text = getConfigValue("text_tt7");
            btn_tt8.Text = getConfigValue("text_tt8");
            btn_tt9.Text = getConfigValue("text_tt9");
            btn_tt10.Text = getConfigValue("text_tt10");
            btn_tt11.Text = getConfigValue("text_tt11");
            btn_tt12.Text = getConfigValue("text_tt12");
            btn_tt13.Text = getConfigValue("text_tt13");
            btn_tt14.Text = getConfigValue("text_tt14");
            btn_tt15.Text = getConfigValue("text_tt15");
            btn_tt16.Text = getConfigValue("text_tt16");
            btn_tt17.Text = getConfigValue("text_tt17");
            btn_tt18.Text = getConfigValue("text_tt18");
            btn_tt19.Text = getConfigValue("text_tt19");
            btn_tt20.Text = getConfigValue("text_tt20");
            btn_tt21.Text = getConfigValue("text_tt21");
            btn_tt22.Text = getConfigValue("text_tt22");
            btn_tt23.Text = getConfigValue("text_tt23");
            btn_tt24.Text = getConfigValue("text_tt24");
        }

        void LoadIcons()
        {
            ibxPOS1.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPOS1.Image = Image.FromFile(getConfigValue("img_pos1"));
            ibxPOS2.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPOS2.Image = Image.FromFile(getConfigValue("img_pos2"));
            ibxPOS3.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPOS3.Image = Image.FromFile(getConfigValue("img_pos3"));
            ibxPOS4.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPOS4.Image = Image.FromFile(getConfigValue("img_pos4"));

            ibxPP1.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPP1.Image = Image.FromFile(getConfigValue("img_pp1"));
            ibxPP2.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPP2.Image = Image.FromFile(getConfigValue("img_pp2"));
            ibxPP3.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPP3.Image = Image.FromFile(getConfigValue("img_pp3"));
            ibxPP4.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPP4.Image = Image.FromFile(getConfigValue("img_pp4"));

            ibxBOC.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxBOC.Image = Image.FromFile(getConfigValue("img_boc"));
            ibxSwitch.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxSwitch.Image = Image.FromFile(getConfigValue("img_switch"));
            ibxPOSTEC.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxPOSTEC.Image = Image.FromFile(getConfigValue("img_postec"));

            ibxRouter.SizeMode = PictureBoxSizeMode.StretchImage;
            ibxRouter.Image = Image.FromFile(getConfigValue("img_router"));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.F5)
            {
                loadSite();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.V))
            {
                if (tabControl1.SelectedTab == tabPage3)
                {
                    notesSave();
                }
            }

            if (keyData == (Keys.Control | Keys.R))
            {
                remoteConnectToolStripMenuItem_Click(this, null);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void Countdown()
        {
            int i = 0;
            TimeLeft = Convert.ToInt32(getConfigValue("console_timeout"));
            while (TimeLeft > 0)
            {
                TimeLeft--;
                Thread.Sleep(60000);
                i++;
                if (i >= 60)
                {
                    i = 0;
                    Push_Stats();
                }

                //GC.Collect();            
            }
            if (!testmode)
            {
                Application.Exit();
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Push_Stats();
            Environment.Exit(0);
            base.OnFormClosing(e);
        }
        #endregion

        #region On load ping
        
        void set_all_gray()
        {
            pbxPostec.BackColor = Color.FromArgb(64, 64, 64);
            pbxBoc.BackColor = Color.FromArgb(64, 64, 64);
            pbxSwitch.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos2.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP2.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP1.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP3.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP4.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP5.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP6.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP7.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos1.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos3.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos4.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS5.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS6.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS7.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust1.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust2.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust3.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust4.BackColor = Color.FromArgb(64, 64, 64);
            pbxRouter.BackColor = Color.FromArgb(64, 64, 64);
            pbx_DOMS.BackColor = Color.FromArgb(64, 64, 64);
            pbx_DOMS58.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void SetAllFail()
        {
            pbxBoc.BackColor = Color.Red;
            pbxPos1.BackColor = Color.Red;
            pbxPos2.BackColor = Color.Red;
            pbxPos3.BackColor = Color.Red;
            pbxPos4.BackColor = Color.Red;
            pbxPostec.BackColor = Color.Red;
            pbxPP1.BackColor = Color.Red;
            pbxPP2.BackColor = Color.Red;
            pbxPP3.BackColor = Color.Red;
            pbxPP4.BackColor = Color.Red;
            pbxRouter.BackColor = Color.Red;
            pbxSwitch.BackColor = Color.Red;
            pbx_DOMS.BackColor = Color.Red;
            pbx_DOMS58.BackColor = Color.Red;
            pbx_POS5.BackColor = Color.Red;
            pbx_POS6.BackColor = Color.Red;
            pbx_POS7.BackColor = Color.Red;
            pbx_PP5.BackColor = Color.Red;
            pbx_PP6.BackColor = Color.Red;
            pbx_PP7.BackColor = Color.Red;
        }

        void update_indicators(List<Site_Device> devices)
        {
            foreach(Site_Device d in devices)
            {
                if (d.online)
                {
                    d.indicator.BackColor = Color.FromArgb(13, 239, 66);
                    //check if the site has an SAH server & alert the operator if it does
                    if (d.ip.Remove(0, d.ip.Length - 3) == getConfigValue("sah1ip") && getConfigValue("enable_sah_alert") == "true")
                    {
                        MessageBox.Show(getConfigValue("sah_alert_message"));
                    }
                }
                else
                {
                    d.indicator.BackColor = Color.FromArgb(255, 0, 0);
                }
            }
        }

        public void/*List<Site_Device>*/ PollSite(List<Site_Device> devices)
        {
            List<Thread> threads = new List<Thread>();
            foreach (Site_Device d in devices)
            {
                Thread t = new Thread(() =>
                {
                    using (Ping p = new Ping())
                    {
                        try { PingReply pr = p.Send(d.ip); if (pr.Status == IPStatus.Success) { d.online = true; } } catch (Exception) { }
                    }
                });
                threads.Add(t);
                t.Start();
                Thread.Sleep(5); //try not to DDOS our shit connection
            }
            foreach (Thread t in threads)
            {
                t.Join();
            }
            this.Invoke(new MethodInvoker(delegate { SetAllFail(); }));
            this.Invoke(new MethodInvoker(delegate { update_indicators(devices); }));
            //return devices;
        }

        public void btnPing_Click(object sender, EventArgs e)
        {
            pingDevices();
            loaded++;
        }

        void pingDevices()
        {
            //make sure we know what ID we're working with as it may have changed by the time we're ready to display the results
            string ID = SiteID;

            //this is normally done when we change the site ID but do it anyway incase the user is reloading the same site
            set_all_gray();

            //get the timeout setting for the pings, anything under approx 1200 will give false positives on failures
            int timeout = 3500;
            if (getConfigValue("default_ping_timeout") != null)
            {
                try
                {
                    int timeoutconfig = Convert.ToInt32(getConfigValue("default_ping_timeout"));
                    if (timeoutconfig >= 1000 && timeoutconfig <= 5000)
                    {
                        timeout = timeoutconfig;
                    }
                }
                catch (Exception) { }
            }

            //check to see if the site is meraki, if so change the icons
            bool whitelisted = GetSiteWhitelistStatus(SiteID); 
            if (whitelisted)
            {
                ibxSwitch.Image = Image.FromFile(getConfigValue("img_meraki"));
                ibxRouter.Image = Image.FromFile(getConfigValue("img_meraki"));

                ibxSwitch.Location = new Point(ibxSwitch.Location.X, 19);
                ibxSwitch.Size = new Size(ibxSwitch.Width, 67);
            }
            else
            {
                ibxSwitch.Location = new System.Drawing.Point(ibxSwitch.Location.X, 37);
                ibxSwitch.Size = new Size(ibxSwitch.Width, 48);
            }

            //Check if we're loading an OPT site
            //use the switch image to dislay this
            bool isopt = false;
            if (ID.Remove(1) == "8")
            {
                SWITCHIP = NetIP + getConfigValue("psmip");
                ibxSwitch.Image = Image.FromFile(getConfigValue("img_psm"));
                isopt = true;
            }
            else
            {
                SWITCHIP = NetIP + getConfigValue("switchip");
            }

            //create a list of devices
            List<Site_Device> devices = new List<Site_Device>();
            for (int i = 1; i < 25; i++)
            {
                Site_Device temp = new Site_Device();
                if (getConfigValue(getConfigValue($"d{i}")) != null)
                {
                    temp.indicator = conlist[i - 1];
                    //getConfigValue($"d{i}") gets the device name which is used to get the IP for that device
                    temp.ip = NetIP + getConfigValue(getConfigValue($"d{i}"));
                    //use the switch icon as the psm for opt sites
                    if(isopt && getConfigValue(getConfigValue($"d{i}")) == ".253") { temp.ip = NetIP + getConfigValue("psmip"); }

                    devices.Add(temp);
                }
            }
            
            Thread t = new Thread(() => PollSite(devices));
            t.Start();

            //Thread PingThread = new Thread(PingAllDevices);
            //PingThread.IsBackground = false;
            //PingThread.Start();
        }

        #endregion

        #region Load site + call ping
        private void tbxSiteID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadSite();
            }
        }

        void onLoadFormat()
        {
            ibxBOC.Enabled = true;
            ibxPOS1.Enabled = true;
            ibxPOS2.Enabled = true;
            ibxPOS3.Enabled = true;
            ibxPOS4.Enabled = true;
            ibxPOSTEC.Enabled = true;
            ibxPP1.Enabled = true;
            ibxPP2.Enabled = true;
            ibxPP3.Enabled = true;
            ibxPP4.Enabled = true;
            ibxRouter.Enabled = true;
            ibxSwitch.Enabled = true;

            ibxSwitch.Image = Image.FromFile(getConfigValue("img_switch"));
            ibxRouter.Image = Image.FromFile(getConfigValue("img_router"));

            //to do *ibx not cip
            //dialer 0 cip.Enabled = true;
            //dialer 1 cip.Enabled = true;
            //dialer 2 cip.Enabled = true;
        }

        public bool findSite()
        {
            string[] row;
            string type = "";
            string name = "";
            string ID = "";
            string state = "";
            string ConType = "";
            string ntwkIP = "";
            string ADSL = "";
            string ThreeG = "";
            string deCom = "";
            string lastMod = "";
            string lastEdit = "";
            string WOWIP = "";
            bool found = false;


            foreach (string site in siteIPData)
            {
                row = site.Split(',');
                type = row[0];
                name = row[1];
                ID = row[2];
                state = row[3];
                ConType = row[4];
                ntwkIP = row[5];
                ADSL = row[6];
                ThreeG = row[7];
                deCom = row[8];
                lastMod = row[9];  // 10 & 11 are for P@P, useless info for this app
                lastEdit = row[12];
                WOWIP = row[13];

                if (ID == SiteID)
                {
                    if (deCom == "True")
                    {
                        lblMismatch.Text = "Warning: Closed Site";
                        lblSiteID2.ForeColor = Color.Red;
                        found = false;
                    }
                    else
                    {
                        lblMismatch.Text = "";
                        found = true;
                    }

                    if (ConType == "BDSL" || ConType == "3G Only")
                    {
                        lblConType.ForeColor = Color.Orange;
                        lblConType.Font = new Font(lblConType.Font, FontStyle.Bold);
                    }

                    SiteType = type;
                    SiteName = name;
                    SiteID = ID;
                    SiteState = state;
                    SiteConType = ConType;
                    NetIP = ntwkIP;
                    SiteWANIP = ADSL;
                    Site3GIP = ThreeG;
                    SiteDeCommed = deCom;
                    SiteLastModTD = lastMod;
                    SiteLastEditBy = lastEdit;
                    SiteWOWIP = WOWIP;
                    try
                    {
                        NetIP = NetIP.Remove(NetIP.IndexOf('.', 7)); //Format to XX.XXX.X
                    }
                    catch (Exception)
                    {

                    }



                    MWSIP = NetIP + getConfigValue("pos1ip");
                    CWSIP = NetIP + getConfigValue("pos2ip");
                    CWS2IP = NetIP + getConfigValue("pos3ip");
                    CWS3IP = NetIP + getConfigValue("pos4ip");
                    CWS4IP = NetIP + getConfigValue("pos5ip");
                    CWS5IP = NetIP + getConfigValue("pos6ip");
                    CWS6IP = NetIP + getConfigValue("pos7ip");
                    PP1IP = NetIP + getConfigValue("pp1ip");
                    PP2IP = NetIP + getConfigValue("pp2ip");
                    PP3IP = NetIP + getConfigValue("pp3ip");
                    PP4IP = NetIP + getConfigValue("pp4ip");
                    PP5IP = NetIP + getConfigValue("pp5ip");
                    PP6IP = NetIP + getConfigValue("pp6ip");
                    PP7IP = NetIP + getConfigValue("pp7ip");
                    POSTECIP = NetIP + getConfigValue("postecip");
                    ROUTERIP = NetIP + getConfigValue("routerip");
                    SWITCHIP = NetIP + getConfigValue("switchip");
                    BOCIP = NetIP + getConfigValue("bocip");
                    PRINTERIP = NetIP + getConfigValue("printerip");
                    DOMS38 = NetIP + getConfigValue("domsip");
                    DOMS58 = NetIP + ".58";//only for one site on legacy config,
                    CustomIP1 = NetIP + getConfigValue("custom_ip_1_suffix");
                    CustomIP2 = NetIP + getConfigValue("custom_ip_2_suffix");
                    CustomIP3 = NetIP + getConfigValue("custom_ip_3_suffix");
                    CustomIP4 = NetIP + getConfigValue("custom_ip_4_suffix");

                    lblSiteName.Text = SiteName;
                    lblSiteID2.Text = SiteID;
                    lblNetIP.Text = NetIP;
                    lblConType.Text = SiteConType + " (Confirm in RBOS)";
                    lblSiteDeCom.Text = SiteDeCommed;
                    lblSiteLastMod.Text = SiteLastModTD;

                    if (cbxAutoCopy.Checked)
                    {
                        Clipboard.SetText(MWSIP);
                    }
                }
            }
            return found;
        }

        void loadInventory()
        {
            string ID = "";
            string PSTNFNN = "";
            string StarBOS = "";
            string PAPTID = "";
            string[] row;
            foreach (string inventory in siteInventoryData)
            {
                row = inventory.Split(',');
                ID = row[0];
                PSTNFNN = row[6];
                StarBOS = row[17];
                PAPTID = row[24];
                if (ID == SiteID)
                {
                    for (int i = 1; i < 20; i = i + 4)
                    {
                        try
                        {
                            PSTNFNN = PSTNFNN.Insert(i, " "); // space out FNN for readablility
                        }
                        catch (Exception) { }
                    }
                    lblPSTN.Text = PSTNFNN;
                    lblStarBOS.Text = StarBOS;
                    //lblPAPTID.Text = PAPTID; obsolete
                    lblNetTemplate.Text = GetNetworkTemplate(SiteID);
                }
            }
            Thread t = new Thread(getNamosVersion);
            t.IsBackground = false;
            t.Start();
        }

        void getNamosVersion()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            RegistryKey r;
            string namosVersion = "";
            string remotehost = SiteID + "-mws1";
            bool failed = false;
            try
            {
                r = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, remotehost).OpenSubKey(getConfigValue("NamosVersionKeyName"));
                namosVersion = r.GetValue(getConfigValue("NamosVersionValueName")).ToString();
            }
            catch (IOException)
            {
                failed = true;
                this.Invoke(new MethodInvoker(delegate { lblPAPTID.Text = "IO Exception"; })); ;
            }  
            catch (System.Security.SecurityException)
            {
                failed = true;
                this.Invoke(new MethodInvoker(delegate { lblPAPTID.Text = "Security Exception"; })); ;
            }

            s.Stop();
            if (!failed)
            {
                //Convert Namos project number to software version
                //example config line "1.2.3.4=version6"
                namosVersion = getConfigValue(namosVersion);

                if (s.ElapsedMilliseconds <= Convert.ToInt32(getConfigValue("RegQueryTimeout")))
                {
                    lock (lblPAPTID)
                    {
                        this.Invoke(new MethodInvoker(delegate { lblPAPTID.Text = namosVersion; })); ;
                    }
                }
                else
                {
                    lock (lblPAPTID)
                    {
                        this.Invoke(new MethodInvoker(delegate { lblPAPTID.Text = "Req Timeout"; })); ;
                    }
                }
            }
        }
        
        void loadAlerts()
        {
            //string[] Alerts = File.ReadAllLines(getConfigValue("alerts"), Encoding.UTF8);
            DirectoryInfo dir = new DirectoryInfo(getConfigValue("site_alerts_dir"));
            FileInfo[] files = dir.GetFiles("*.txt");
            foreach (FileInfo f in files)
            {
                //MessageBox.Show("\"" + f.Name + "\" - \"" + SiteID + "\"" );
                if(f.Name == $"{SiteID}.txt")
                {
                    string[] at = File.ReadAllLines(f.FullName);
                    TextViewer tv = new TextViewer(at, $"Site alert {SiteID}");
                    tv.Show();
                }
            }
            /*
            string AID;
            foreach (string Alert in Alerts)
            {
                if (Alert != "")
                {
                    AID = Alert.Remove(Alert.IndexOf(","));
                    if (AID == SiteID)
                    {
                        AlertViewer f2 = new AlertViewer(SiteID);
                        f2.ShowDialog();
                    }
                }
            }
            */
        }

        void loadSite()
        {
            if (LoadActive) { goto exit; }
            LoadActive = true;
            TimeLeft = 180;
            //for testing shit and disabling auto pings
            if (tbxSiteID.Text == "99123")
            {
                testmode = true;
                loadInventory();
            }
            if (tbxSiteID.Text == "99321")
            {
                testmode = false;
                loadInventory();
            }
            // check if a valid site number
            if (tbxSiteID.TextLength != 5)
            {
                lblMismatch.Text = "Invalid site ID";
                goto exit;
            }

            SiteID = tbxSiteID.Text;
            bool siteFound = findSite();
            if (!siteFound) { goto exit; }
            loaded++;
            onLoadFormat();
            loadInventory();
            notesLoad();
            if (!testmode) //vs2017 bugs/10
            {
                pingDevices();
            }
            exit:; // important exit remains above alerts & LoadActive
            loadAlerts();
            LoadActive = false;
        }

        private void btnStreamLoader_Click(object sender, EventArgs e)
        {
            loadSite();
        }
        #endregion

        #region Site notes

        void notesSave()
        {
            String TimeStamp = DateTime.Now.ToString();
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string path = ($@"{getConfigValue("sitenotes_folder")}{SiteID}.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.Create(path).Close();

            string postec = tbxPoostec.Text;
            string cisco = tbxCriscoHamper.Text;
            string EMGReset = tbxDoesntExistIveBeenWorkingHearForTenYears.Text;
            string other = tbxOther.Text;
            string Final = "";

            // build the string to write to file
            //Postec
            if (!postecNotes) // no changes made, leave the field alone
            {
                if (postec.Contains(","))
                {
                    postec = postec.Replace(',', '`');
                }
                Final = postec + ",";
            }
            else // change was made, add timestamp & username
            {
                if (postec.Contains(","))
                {
                    postec = postec.Replace(',', '`');
                }
                Final = postec + Environment.NewLine + UserName + " " + TimeStamp + ",";
            }
            //Router
            if (!routerNotes)
            {
                if (cisco.Contains(","))
                {
                    cisco = cisco.Replace(',', '`');
                }
                Final = Final + cisco + ",";
            }
            else
            {
                if (cisco.Contains(","))
                {
                    cisco = cisco.Replace(',', '`');
                }
                Final = Final + cisco + Environment.NewLine + UserName + " " + TimeStamp + ",";
            }
            //EMG Resets
            if (!EMGNotes)
            {
                if (EMGReset.Contains(","))
                {
                    EMGReset = EMGReset.Replace(',', '`');
                }
                Final = Final + EMGReset + ",";
            }
            else
            {
                if (EMGReset.Contains(","))
                {
                    EMGReset = EMGReset.Replace(',', '`');
                }
                Final = Final + EMGReset + Environment.NewLine + UserName + " " + TimeStamp + ",";
            }
            //Other notes
            if (!otherNotes)
            {
                if (other.Contains(","))
                {
                    other = other.Replace(',', '`');
                }
                Final = Final + other + ",";
            }
            else
            {
                if (other.Contains(","))
                {
                    other = other.Replace(',', '`');
                }
                Final = Final + other + Environment.NewLine + UserName + " " + TimeStamp + ",";
            }

            Final = Final + ","; //future proofing the files this time
            File.WriteAllText(path, Final);
            notesLoad();
        }

        private void btnNoteSave_Click(object sender, EventArgs e)
        {
            notesSave();
        }

        void notesLoad()
        {
            tbxPoostec.Clear();
            tbxCriscoHamper.Clear();
            tbxDoesntExistIveBeenWorkingHearForTenYears.Clear();
            if (File.Exists(($@"{getConfigValue("sitenotes_folder")}{SiteID}.txt")))
            {
                lblHasNotes.Text = "Site has device notes";
                string allText = File.ReadAllText($@"{getConfigValue("sitenotes_folder")}{SiteID}.txt");
                string[] splitText = allText.Split(',');
                int i = 0;
                foreach (string element in splitText)
                {
                    if (element.Contains("`"))
                    {
                        splitText[i] = element.Replace('`', ','); //sub back in commas
                    }
                    i++;

                }
                tbxPoostec.Text = splitText[0];
                tbxCriscoHamper.Text = splitText[1];
                tbxDoesntExistIveBeenWorkingHearForTenYears.Text = splitText[2];
                tbxOther.Text = splitText[3];
            }
            postecNotes = false;
            routerNotes = false;
            EMGNotes = false;
        }

        private void btnNoteLoad_Click(object sender, EventArgs e)
        {
            notesLoad();
        }

        private void tbxPoostec_TextChanged(object sender, EventArgs e)
        {
            postecNotes = true;
        }
        private void tbxCriscoHamper_TextChanged(object sender, EventArgs e)
        {
            routerNotes = true;
        }
        private void tbxDoesntExistIveBeenWorkingHearForTenYears_TextChanged(object sender, EventArgs e)
        {
            EMGNotes = true;
        }
        private void tbxOther_TextChanged(object sender, EventArgs e)
        {
            otherNotes = true;
        }

        #endregion

        #region Formatting on site load
        private void tbxSiteID_TextChanged(object sender, EventArgs e)
        {
            tbxCriscoHamper.Clear();
            tbxPoostec.Clear();
            tbxDoesntExistIveBeenWorkingHearForTenYears.Clear();
            tbxOther.Clear();
            lblHasNotes.Text = "";
            TimeLeft = 120;
            SiteID = "nositeloaded";
            lblMismatch.Text = "";

            lblSiteName.Text = "";
            lblSiteID2.Text = "";
            lblNetIP.Text = "";
            lblConType.Text = "";
            lblSiteDeCom.Text = "";
            lblSiteLastMod.Text = "";
            lblStarBOS.Text = "";
            lblPAPTID.Text = "";
            lblNetTemplate.Text = "";
            lblPSTN.Text = "";
            lblSiteID2.ForeColor = Color.White;
            lblConType.ForeColor = Color.White;
            lblConType.Font = new Font(lblConType.Font, FontStyle.Regular);

            SiteWANIP2 = "noip";
            MWSIP = "noip";
            CWSIP = "noip";
            PP1IP = "noip";
            PP2IP = "noip";
            POSTECIP = "noip";
            ROUTERIP = "noip";
            SWITCHIP = "noip";
            BOCIP = "noip";
            Site3GIP = "noip";
            SiteWANIP = "noip";
            PP3IP = "noip";
            PP4IP = "noip";
            CWS3IP = "noip";
            CWS4IP = "noip";
            CWS5IP = "noip";
            CWS6IP = "noip";
            PP5IP = "noip";
            PP6IP = "noip";
            PP7IP = "noip";
            DOMS38 = "noip";
            DOMS58 = "noip";
            CustomIP1 = "noip";
            CustomIP2 = "noip";
            CustomIP3 = "noip";
            CustomIP4 = "noip";


            pbxPostec.BackColor = Color.FromArgb(64, 64, 64);
            pbxBoc.BackColor = Color.FromArgb(64, 64, 64);
            pbxSwitch.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos2.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP2.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP1.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP3.BackColor = Color.FromArgb(64, 64, 64);
            pbxPP4.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP5.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP6.BackColor = Color.FromArgb(64, 64, 64);
            pbx_PP7.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos1.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos3.BackColor = Color.FromArgb(64, 64, 64);
            pbxPos4.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS5.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS6.BackColor = Color.FromArgb(64, 64, 64);
            pbx_POS7.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust1.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust2.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust3.BackColor = Color.FromArgb(64, 64, 64);
            pbx_cust4.BackColor = Color.FromArgb(64, 64, 64);
            pbxRouter.BackColor = Color.FromArgb(64, 64, 64);
            pbx_DOMS.BackColor = Color.FromArgb(64, 64, 64);
            pbx_DOMS58.BackColor = Color.FromArgb(64, 64, 64);


            //to do *ibx not cip
            //dialer 0 cip.Enabled = false;
            //dialer 1 cip.Enabled = false;
            //dialer 2 cip.Enabled = false;

            ibxBOC.Enabled = false;
            ibxPOS1.Enabled = false;
            ibxPOS2.Enabled = false;
            ibxPOS3.Enabled = false;
            ibxPOS4.Enabled = false;
            ibxPOSTEC.Enabled = false;
            ibxPP1.Enabled = false;
            ibxPP2.Enabled = false;
            ibxPP3.Enabled = false;
            ibxPP4.Enabled = false;
            ibxRouter.Enabled = false;
            ibxSwitch.Enabled = false;

        }
        #endregion

        #region Constant Pings
        private void CPingThread()
        {
            corners_cut++;
            string number = getConfigValue("ping_limit");
            string timeout = getConfigValue("ping_timeout");
            if (number == null) { number = "240"; }; // default settings if they're removed from the config file
            if (timeout == null) { timeout = "120"; };
            string PingIt = CPingIP;
            string Device = Devcie;
            int i = 0;
            //lblCPing.Invoke(new MethodInvoker(delegate { lblCPing.Text = "Launching"; }));
            //Random string to stop file name conflicts
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[8];
            var random = new Random();
            for (int a = 0; a < stringChars.Length; a++)
            {
                stringChars[a] = chars[random.Next(chars.Length)];
            }
            var RandS = new String(stringChars);
        Start:


            if ((File.Exists($@"{getConfigValue("resources_folder")}cping{RandS}.bat") && i <= 6))
            {
                i++;
                //lblCPing.Invoke(new MethodInvoker(delegate { lblCPing.Text = "Failed, trying again please wait"; })); ;
                Thread.Sleep(500);
                goto Start;
            }
            else if (i <= 6)
            {
                string line0 = $"title {SiteID} - {SiteName} - {Device}";
                string line1 = $"ping {PingIt} -n {number} && timeout -t {timeout}";
                string line2 = "";
                string[] Lines = { line0, line1, line2 };
                File.Create($@"{getConfigValue("resources_folder")}cping{RandS}.bat").Close();
                File.WriteAllLines($@"{getConfigValue("resources_folder")}cping{RandS}.bat", Lines);
                Thread.Sleep(1500);
                System.Diagnostics.Process.Start($@"{getConfigValue("resources_folder")}cping{RandS}.bat");
                //lblCPing.Invoke(new MethodInvoker(delegate { lblCPing.Text = "Ping started"; })); ;
                Thread.Sleep(1000);
                File.Delete($@"{getConfigValue("resources_folder")}cping{RandS}.bat");
            }
            else
            {
                //lblCPing.Invoke(new MethodInvoker(delegate { lblCPing.Text = "CMD load failed, try again"; })); ;
            }
        }

        //Picturebox on click
        private void ibxPOS1_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "MWS";
            CPingIP = NetIP + getConfigValue("pos1ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPOS2_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "CWS";
            CPingIP = NetIP + ".41";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPOS3_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "CWS2";
            CPingIP = NetIP + ".42";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPOS4_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "CWS3";
            CPingIP = NetIP + ".43";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPP1_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP1";
            CPingIP = NetIP + ".50";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPP2_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP2";
            CPingIP = NetIP + ".51";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPP3_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP3";
            CPingIP = NetIP + ".52";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxPP4_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP4";
            CPingIP = NetIP + ".53";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
        }
        private void ibxBOC_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "BOC";
            CPingIP = NetIP + ".70";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void ibxPOSTEC_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "Postec";
            CPingIP = NetIP + ".39";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void ibxSwitch_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "Switch";
            if (SiteID.Remove(1) == "8")
            {
                CPingIP = NetIP + getConfigValue("psmip");
            }
            else
            {
                CPingIP = NetIP + getConfigValue("switchip");
            }

            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void ibxRouter_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "Router";
            CPingIP = NetIP + ".254";
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbxDialer0_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "Dialer 0";
            CPingIP = SiteWANIP;
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbxDialer1_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "Dialer 0(2nd adress)";
            CPingIP = SiteWANIP2;
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbxDialer2_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "3G";
            CPingIP = Site3GIP;
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_POS5_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "POS 5";
            CPingIP = NetIP + getConfigValue("pos5ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_PP5_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP5";
            CPingIP = NetIP + getConfigValue("pp5ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_POS6_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "POS 6";
            CPingIP = NetIP + getConfigValue("pos6ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_PP6_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP6";
            CPingIP = NetIP + getConfigValue("pp6ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_POS7_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "POS 7";
            CPingIP = NetIP + getConfigValue("pos7ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_PP7_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "PP7";
            CPingIP = NetIP + getConfigValue("pp7ip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_DOMS_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = "DOMS";
            CPingIP = NetIP + getConfigValue("domsip");
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_cust1_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = getConfigValue("custom_device_1_name");
            CPingIP = NetIP + getConfigValue(getConfigValue("d21"));
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_cust2_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = getConfigValue("custom_device_2_name");
            CPingIP = NetIP + getConfigValue(getConfigValue("d22"));
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_cust3_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = getConfigValue("custom_device_3_name");
            CPingIP = NetIP + getConfigValue(getConfigValue("d23"));
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }
        private void pbx_cust4_Click(object sender, EventArgs e)
        {
            dont_use_the_acronym++;
            Devcie = getConfigValue("custom_device_4_name");
            CPingIP = NetIP + getConfigValue(getConfigValue("d24"));
            Thread CPingT = new Thread(CPingThread);
            CPingT.Start();
            Log("Event", $"Launched Constant ping for {Devcie}");
        }

        #endregion

        #region Context menu stuff


        //P1
        private void P1CPing_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                Devcie = "MWS";
                CPingIP = NetIP + ".49";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
                dont_use_the_acronym++;
            }
        }
        private void P1CopyIP_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                Clipboard.SetText(MWSIP);
                no_context++;
            }
        }
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                Process.Start($@"\\{MWSIP}\c$");
                Log("Event", "Opened Pos 1 C:");
                no_context++;
            }
        }
        private void eJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                Process.Start($@"\\{MWSIP}\c$\sni\namos\cmos\Ejournal.txt");
                Log("Event", "Opened P1 EJ");
                no_context++;
            }
        }
        //P2
        private void P2CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "CWS1";
                CPingIP = NetIP + ".41";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void P2CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(CWSIP);
            }
        }
        private void P2CDMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWSIP}\c$");
                Log("Event", "Opened Pos 2 C:");
            }
        }
        private void P2EJMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWSIP}\c$\sni\namos\cmos\Ejournal.txt");
                Log("Event", "Opened P2 EJ");
            }
        }
        //P3
        private void P3CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "CWS2";
                CPingIP = NetIP + ".42";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void P3CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(CWS2IP);
            }
        }
        private void P3CDMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWS2IP}\c$");
                Log("Event", "Opened Pos 3 C:");
            }
        }
        private void P3EJMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWS2IP}\c$\sni\namos\cmos\Ejournal.txt");
                Log("Event", "Opened P3 EJ");
            }
        }
        //P4
        private void P4CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "CWS3";
                CPingIP = NetIP + ".43";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void P4CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(CWS3IP);
            }
        }
        private void P4CDMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWS3IP}\c$");
                Log("Event", "Opened Pos 4 C:");
            }
        }
        private void P4EJMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{CWS3IP}\c$\sni\namos\cmos\Ejournal.txt");
                Log("Event", "Opened P4 EJ");
            }
        }
        //PP1
        private void PP1CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "PP1";
                CPingIP = NetIP + ".50";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void PP1CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(PP1IP);
            }
        }
        //PP2
        private void PP2CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "PP2";
                CPingIP = NetIP + ".51";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void PP2CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(PP2IP);
            }
        }
        //PP3
        private void PP3CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "PP3";
                CPingIP = NetIP + ".52";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void PP3CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(PP3IP);
            }
        }
        //PP4
        private void PP4CPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "PP4";
                CPingIP = NetIP + ".53";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void PP4CIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(PP4IP);
            }
        }
        //BOC
        private void BOCCPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "BOC";
                CPingIP = NetIP + ".70";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void BOCCIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(BOCIP);
            }
        }
        private void ctmBOCCD_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Process.Start($@"\\{BOCIP}\C$\");
            }
        }
        //Postec
        private void PostecCPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "Postec";
                CPingIP = NetIP + ".39";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void PostecCIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(POSTECIP);
            }
        }
        //Switch
        private void SwitchCPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "Switch";
                CPingIP = NetIP + ".253";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void SwitchCIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(SWITCHIP);
            }
        }
        //Router
        private void RouterCPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                dont_use_the_acronym++;
                Devcie = "Router";
                CPingIP = NetIP + ".254";
                Thread CPingT = new Thread(CPingThread);
                CPingT.Start();
                Log("Event", $"Launched Constant ping for {Devcie}");
            }
        }
        private void RouterCIPMenu_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(ROUTERIP);
            }
        }
        //Dialers
        private void D0CIP_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(SiteWANIP);
            }
        }
        private void D1CIP_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(SiteWANIP2);
            }
        }
        private void D2CIP_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                Clipboard.SetText(Site3GIP);
            }
        }
        // Misc shortcuts
        private void P1Cmos_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                P1Cmos.Enabled = false;
                try
                {
                    if (SiteID != "nositeloaded")
                    {
                        Process.Start($@"\\{MWSIP}\c$\sni\namos\cmos");
                    }
                    P1Cmos.Enabled = true;
                }
                catch (Exception)
                {
                    //no connection to boc
                    P1Cmos.Enabled = true;
                }
                Log("Event", "Opened P1 CMOS");
            }
        }
        private void P2Cmos_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                P2Cmos.Enabled = false;
                try
                {
                    if (SiteID != "nositeloaded")
                    {
                        Process.Start($@"\\{CWSIP}\c$\sni\namos\cmos");
                    }
                    P2Cmos.Enabled = true;
                }
                catch (Exception)
                {
                    //no connection to boc
                    P2Cmos.Enabled = true;
                }
                Log("Event", "Opened P2 CMOS");
            }
        }
        private void P3Cmos_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                P3Cmos.Enabled = false;
                try
                {
                    if (SiteID != "nositeloaded")
                    {
                        Process.Start($@"\\{CWS2IP}\c$\sni\namos\cmos");
                    }
                    P3Cmos.Enabled = true;
                }
                catch (Exception)
                {
                    //no connection to boc
                    P3Cmos.Enabled = true;
                }
                Log("Event", "Opened P3 CMOS");
            }
        }
        private void P4Cmos_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                P4Cmos.Enabled = false;
                try
                {
                    if (SiteID != "nositeloaded")
                    {
                        Process.Start($@"\\{CWS3IP}\c$\sni\namos\cmos");
                    }
                    P4Cmos.Enabled = true;
                }
                catch (Exception)
                {
                    //no connection to boc
                    P4Cmos.Enabled = true;
                }
                Log("Event", "Opened P4 CMOS");
            }
        }


        //          Context menu scripts

        private void getLoggedUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckUser(BOCIP);
        }

        // Remote Connect


        private void LaunchRD(string IP, string name)
        {
            string text = $"Start \"RC Launch\" \"C:\\Program Files (x86)\\CA\\DSM\\Bin\\gui_rcLaunch.exe\" VIEW /M SHARED /N \"{name}\" /A {IP} /L /S SHRINK && timeout -t 5";
            int i = 0;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[8];
            var random = new Random();
            for (int a = 0; a < stringChars.Length; a++)
            {
                stringChars[a] = chars[random.Next(chars.Length)];
            }
            var RandS = new String(stringChars);
        Start:

            if ((File.Exists($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat") && i <= 6))
            {
                Log("Debug", "Bad news, " + RandS);
                i++;
                Thread.Sleep(500);
                goto Start;
            }
            else if (i <= 6)
            {
                File.Create($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat").Close();
                //Log("Debug", $"Created RDBot{RandS}.bat");
                File.WriteAllText($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat", text);
                //Log("Debug", $"Wrote to RDBot{RandS}.bat");
                Thread.Sleep(500);
                System.Diagnostics.Process.Start($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat");
                //Log("Debug", "Excecuted");
                Thread.Sleep(500);
                try
                {
                    File.Delete($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat");
                }
                catch (Exception e)
                {
                    LogError(e.ToString());
                }
                Thread.Sleep(2000);
                if (File.Exists($@"{getConfigValue("resources_folder")}RDBot{RandS}.bat"))
                {
                    LogError($@"Failed to delete RDBot{RandS}.bat");
                }
                //Log("Debug", "Deleted");
            }
            else
            {
                Log("Debug", $"File already exists RDBot{RandS}.bat");
            }
        }
        private void remoteConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchRD(MWSIP, $"{SiteID} - POS 1");
        }
        private void remoteConnectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LaunchRD(CWSIP, $"{SiteID} - POS 2");
        }
        private void remoteConnectToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            LaunchRD(CWS2IP, $"{SiteID} - POS 3");
        }
        private void remoteConnectToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            LaunchRD(CWS3IP, $"{SiteID} - POS 4");
        }
        private void remoteConnectToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            LaunchRD(BOCIP, $"{SiteID} - BOC");
        }
        private void remoteConnectToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            LaunchRD(POSTECIP, $"{SiteID} - POSTEC");
        }
        #endregion

        #region Custom Shortcut buttons

        private void btn_CBManage_Click(object sender, EventArgs e)
        {
            Form btnmgrform = new CustButtonManager();
            btnmgrform.Show();
        }

        private void CB_Click(object sender, EventArgs e)
        {

            string btn_name = ((Button)sender).Name.ToString();
            int button_num = Convert.ToInt32(btn_name.Remove(0, 4));

            if (File.Exists(getConfigValue("cbc_root") + $"{currentUser}_CBC.txt") && !fnc_blacklisted)
            {
                string[] config = File.ReadAllLines(getConfigValue("cbc_root") + $"{currentUser}_CBC.txt");
                string button = config[button_num];
                string[] fields = button.Split(',');
                string drive = fields[4];
                string device = fields[3];

                //make sure the drive prefix was not included in the path
                char[] slashCheck = fields[1].ToCharArray();
                try
                {
                    if (slashCheck[0].ToString() == "\\")
                    {
                        MessageBox.Show("File path should not begin with a \"\\\"");
                        goto exit;
                    }
                }
                catch (IndexOutOfRangeException) { }

                if (fields[1].Contains(":"))
                {
                    MessageBox.Show("File path should not contain a \":\"");
                    goto exit;
                }

                //check what device the file path refers to
                string devicePrefix = "";
                switch (device)
                {
                    case "Local":
                        devicePrefix = $@"{drive}:\";
                        break;
                    case "MWS":
                        devicePrefix = @"\\" + MWSIP + $@"\{drive}$\";
                        break;
                    case "CWS1":
                        devicePrefix = @"\\" + CWSIP + $@"\{drive}$\";
                        break;
                    case "CWS2":
                        devicePrefix = @"\\" + CWS2IP + $@"\{drive}$\";
                        break;
                    case "CWS3":
                        devicePrefix = @"\\" + CWS3IP + $@"\{drive}$\";
                        break;
                    case "BOC":
                        devicePrefix = @"\\" + BOCIP + $@"\{drive}$\";
                        break;
                    case "POSTEC":
                        devicePrefix = @"\\" + POSTECIP + $@"\{drive}$\";
                        break;
                    default:
                        MessageBox.Show("Unknown device prefix");
                        goto exit;
                }

                switch (fields[2])
                {
                    case "folder":
                        Process.Start(devicePrefix + fields[1]);
                        corners_cut++;
                        break;
                    case "script":
                        CompileAndLaunchScript(fields[5]);
                        sites_nuked++;
                        break;
                    case "function":
                        OpenForm(fields[6]);
                        break;
                    case "executable":
                        Process.Start(devicePrefix + fields[1]);
                        corners_cut++;
                        break;
                    case "file":
                        Process.Start("notepad.exe", devicePrefix + fields[1]);
                        corners_cut++;
                        break;
                    case "ps1":
                        CompileAndLaunchPs1(fields[5]);
                        sites_nuked++;
                        break;
                    default:
                        MessageBox.Show("unknown button type");
                        break;

                }
            }
            else if(!fnc_blacklisted)
            {
                MessageBox.Show("Unable to read config file: " + getConfigValue("cbc_root") + $"{currentUser}_CBC.txt");
            }
        exit:;

        }

        private void CustomLaunch(string path, string type, string device, string script, string drive)
        {/*
            if(type == "script") { sites_nuked++; } else { corners_cut++; }
            string devicePrefix = "";
            switch (device) //check what device the file path refers to
            {
                case "Local":
                    devicePrefix = $@"{drive}:\";
                    break;
                case "MWS":
                    devicePrefix = @"\\" + MWSIP + $@"\{drive}$\";
                    break;
                case "CWS1":
                    devicePrefix = @"\\" + CWSIP + $@"\{drive}$\";
                    break;
                case "CWS2":
                    devicePrefix = @"\\" + CWS2IP + $@"\{drive}$\";
                    break;
                case "CWS3":
                    devicePrefix = @"\\" + CWS3IP + $@"\{drive}$\";
                    break;
                case "BOC":
                    devicePrefix = @"\\" + BOCIP + $@"\{drive}$\";
                    break;
                case "POSTEC":
                    devicePrefix = @"\\" + POSTECIP + $@"\{drive}$\";
                    break;
                default:
                    Log("Error", "Unknown device prefix, aborting");
                    goto exit;
            }
            // check for incorrect formatting
            char[] slashCheck = path.ToCharArray();
            try
            {
                if (slashCheck[0].ToString() == @"\")
                {
                    Log("Error", "File path should not begin with a \"\\\"");
                    goto exit;
                }
            }
            catch (IndexOutOfRangeException) { }

            if (path.Contains(":"))
            {
                Log("Error", "File path should not contain a \":\"");
                goto exit;
            }
            string fullPath = devicePrefix + path;
            //Log("debug", "Prefix: " + devicePrefix + " slash check: " + slashCheck[0] + " Path: " + path + " Full path: " + fullPath);
            if (type == "folder")
            {
                Process.Start(fullPath);
            }
            else if (type == "script")
            {
                CompileAndLaunchScript(script);
            }
            else
            {
                Process.Start("notepad.exe", fullPath);
            }
            exit:;
            */
        }

        private void ResetCBConfig()
        { // run when either reset is called or an edit is made but no file exists
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                File.Delete($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
            }
            string[] defaultFileText = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\Default.txt");
            File.Create($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt").Close();
            File.WriteAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt", defaultFileText);
            CustomButtonInit();
        }

        private void CBEdit(int buttonNumber)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (!File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                ResetCBConfig();
            }
            string[] buttonConfig = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");

            // Check if current user config has enough fields for all data, some legacy config are missing scripts & drive letter
            int comma_count = 0;
            int comma_needed = 6; //current build has 7 fields
            string[] new_config = buttonConfig;
            int i = 0;
            foreach (string line in buttonConfig)
            {
                comma_count = line.Split(',').Length - 1;
                if (comma_needed - comma_count == 1)
                {
                    //one field missing
                    new_config[i] = line + ",";

                }
                else if (comma_needed - comma_count == 2)
                {
                    new_config[i] = line + ",,";
                }
                i++;
            }
            buttonConfig = new_config;

            string[] fields = buttonConfig[buttonNumber].Split(',');
            CustButtonEditor f4 = new CustButtonEditor(buttonNumber, fields[0], fields[1], fields[2], fields[4], fields[5], fields[6]);
            f4.ShowDialog();
        }

        private void CompileAndLaunchScript(string scriptname)
        {
            string path = getConfigValue("script_path");
            string[] allscripts = Directory.GetFiles(path);
            string scriptcheck;
            foreach (string script in allscripts)
            {
                scriptcheck = script.Remove(0, path.Length);
                scriptcheck = scriptcheck.Remove(scriptcheck.IndexOf('.'));
                if (scriptname == scriptcheck)
                {
                    string[] scriptPrefix = {
                        "@echo off",
                        "::SSSC loaded variables",
                        "set NETIP=" + NetIP,
                        "set POS1=" + MWSIP,
                        "set POS2=" + CWSIP,
                        "set POS3=" + CWS2IP,
                        "set POS4=" + CWS3IP,
                        "set POS5=" + CWS4IP,
                        "set POS6=" + CWS5IP,
                        "set POS7=" + CWS6IP,
                        "set PP1=" + PP1IP,
                        "set PP2=" + PP2IP,
                        "set PP3=" + PP3IP,
                        "set PP4=" + PP4IP,
                        "set PP5=" + PP5IP,
                        "set PP6=" + PP6IP,
                        "set PP7=" + PP7IP,
                        "set Postec=" + POSTECIP,
                        "set Router=" + ROUTERIP,
                        "set Switch=" + SWITCHIP,
                        "set BOC=" + BOCIP,
                        "set DOMS38=" + DOMS38,
                        "set DOMS58=" + DOMS58,
                        "set ID=" + SiteID,
                        "set NAME=" + SiteName,
                        "::Start script",
                    };

                    string[] scriptText = File.ReadAllLines(script);
                    var templist = new List<string>();
                    templist.AddRange(scriptPrefix);
                    templist.AddRange(scriptText);
                    scriptText = templist.ToArray();
                    TBatMan(path + "Launch\\" + scriptname + GetRand8() + ".bat", scriptText, 180000);
                }
            }
        }

        private void CompileAndLaunchPs1(string scriptname)
        {
            string path = getConfigValue("script_path_ps1");
            string[] allscripts = Directory.GetFiles(path);
            string scriptcheck;
            foreach (string script in allscripts)
            {
                scriptcheck = script.Remove(0, path.Length);
                scriptcheck = scriptcheck.Remove(scriptcheck.IndexOf('.'));
                if (scriptname == scriptcheck)
                {
                    string[] scriptPrefix = {
                        "#SSSC loaded variables",
                        "$POS1 = \"" + MWSIP + "\"",
                        "$POS2 = \"" + CWSIP + "\"",
                        "$POS3 = \"" + CWS2IP + "\"",
                        "$POS4 = \"" + CWS3IP + "\"",
                        "$POS5 = \"" + CWS4IP + "\"",
                        "$POS6 = \"" + CWS5IP + "\"",
                        "$POS7 = \"" + CWS6IP + "\"",
                        "$PP1 = \"" + PP1IP + "\"",
                        "$PP2 = \"" + PP2IP + "\"",
                        "$PP3 = \"" + PP3IP + "\"",
                        "$PP4 = \"" + PP4IP + "\"",
                        "$PP5 = \"" + PP5IP + "\"",
                        "$PP6 = \"" + PP6IP + "\"",
                        "$PP7 = \"" + PP7IP + "\"",
                        "$Postec = \"" + POSTECIP + "\"",
                        "$Router = \"" + ROUTERIP + "\"",
                        "$Switch = \"" + SWITCHIP + "\"",
                        "$BOC = \"" + BOCIP + "\"",
                        "$DOMS38 = \"" + DOMS38 + "\"",
                        "$DOMS58 = \"" + DOMS58 + "\"",
                        "$ID = \"" + SiteID + "\"",
                        "$NAME = \"" + SiteName + "\"",
                        "#------------------------------",
                        "#Start script",
                    };

                    string[] scriptText = File.ReadAllLines(script);
                    var templist = new List<string>();
                    templist.AddRange(scriptPrefix);
                    templist.AddRange(scriptText);
                    scriptText = templist.ToArray();
                    TPSMan(path + "Launch\\" + scriptname + GetRand8() + ".ps1", scriptText, 180000);
                }
            }
        }

        private string GetInternalValue(string var)
        {
            string value;
            switch (var) //check what device the file path refers to
            {
                case "POS1":
                    value = MWSIP;
                    break;
                case "POS2":
                    value = CWSIP;
                    break;
                case "POS3":
                    value = CWS2IP;
                    break;
                case "POS4":
                    value = CWS3IP;
                    break;
                case "ID":
                    value = SiteID;
                    break;
                case "BOC":
                    value = BOCIP;
                    break;
                case "Switch":
                    value = SWITCHIP;
                    break;
                case "Router":
                    value = ROUTERIP;
                    break;
                case "POSTEC":
                    value = POSTECIP;
                    break;
                case "PP1":
                    value = PP1IP;
                    break;
                case "PP2":
                    value = PP2IP;
                    break;
                case "PP3":
                    value = PP3IP;
                    break;
                case "PP4":
                    value = PP4IP;
                    break;
                default:
                    value = "";
                    break;
            }
            return value;
        }

        private void btnCustom1_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[1].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device,script
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom2_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[2].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom3_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[3].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom4_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[4].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom5_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[5].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom6_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[6].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom7_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[7].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnCustom8_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[8].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled)
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }

        //Quick log buttons re-purposed for custom buttons, same function as above buttons
        private void qlb1_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[9].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device                
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb2_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[10].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb3_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[11].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb4_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[12].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb5_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[13].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb6_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[14].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb7_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[15].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void qlb8_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[16].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        //edit context menus for main page buttons
        private void ctxsubEdit9_Click(object sender, EventArgs e)
        {
            CBEdit(9);
        }
        private void ctxsubEdit10_Click(object sender, EventArgs e)
        {
            CBEdit(10);
        }
        private void ctxsubEdit11_Click(object sender, EventArgs e)
        {
            CBEdit(11);
        }
        private void ctxsubEdit12_Click(object sender, EventArgs e)
        {
            CBEdit(12);
        }
        private void ctxsubEdit13_Click(object sender, EventArgs e)
        {
            CBEdit(13);
        }
        private void ctxsubEdit14_Click(object sender, EventArgs e)
        {
            CBEdit(14);
        }
        private void ctxsubEdit15_Click(object sender, EventArgs e)
        {
            CBEdit(15);
        }
        private void ctxsubEdit16_Click(object sender, EventArgs e)
        {
            CBEdit(16);
        }

        //for original shortcut tab buttons
        private void ctxsubReset1_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset2_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset3_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset4_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset5_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset6_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }
        private void ctxsubReset8_Click(object sender, EventArgs e)
        {
            ResetCBConfig();
        }

        private void ctxsubEdit1_Click(object sender, EventArgs e)
        {
            CBEdit(1);
        }
        private void ctxsubEdit2_Click(object sender, EventArgs e)
        {
            CBEdit(2);
        }
        private void ctxsubEdit3_Click(object sender, EventArgs e)
        {
            CBEdit(3);
        }
        private void ctxsubEdit4_Click(object sender, EventArgs e)
        {
            CBEdit(4);
        }
        private void ctxsubEdit5_Click(object sender, EventArgs e)
        {
            CBEdit(5);
        }
        private void ctxsubEdit6_Click(object sender, EventArgs e)
        {
            CBEdit(6);
        }
        private void ctxsubEdit7_Click(object sender, EventArgs e)
        {
            CBEdit(7);
        }
        private void ctxsubEdit8_Click(object sender, EventArgs e)
        {
            CBEdit(8);
        }

        //extra buttons from the shortcut tab repurposed for custom buttons, ignore names
        private void btnP1CD_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[17].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnBOCCD_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[18].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnP1CMOS_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[19].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnP2CMOS_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[20].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnP1Trace_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[21].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnP1SaveTraces_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[22].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnPos1EJ_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[23].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnPos2EJ_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[24].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnVJ_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[25].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnWdm_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[26].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnEODLog_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[27].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void brnInputMan_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[28].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnOpi1_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[29].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnEBon_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[30].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnP1Temp_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[31].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }
        private void btnBOCTemp_Click(object sender, EventArgs e)
        {
            String UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            UserName = UserName.Remove(0, Convert.ToInt32(getConfigValue("domain_length")));
            if (File.Exists($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt"))
            {
                string[] config = File.ReadAllLines($@"{getConfigValue("resources_folder")}CustButtonConfig\{UserName}_CBC.txt");
                string[] button = config[32].Split(','); //button_name,link_text,file/folder,enabled(1=enabled/0=disabled),device
                if (button[3] == "1")
                {
                    CustomLaunch(button[1], button[2], button[4], button[5], button[6]);
                }
            }
        }

        //context menu edit options for above block
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CBEdit(17);
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CBEdit(18);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CBEdit(19);
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CBEdit(20);
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CBEdit(21);
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            CBEdit(22);
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            CBEdit(23);
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            CBEdit(24);
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            CBEdit(25);
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            CBEdit(26);
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            CBEdit(27);
        }
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            CBEdit(28);
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            CBEdit(29);
        }
        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            CBEdit(30);
        }
        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            CBEdit(31);
        }
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            CBEdit(32);
        }

        #endregion

        #region Misc/Functions

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            //device notes tab focused
            EMG_resets_pushed++;
        }

        private void OpenForm(string form_name)
        {
            switch (form_name)
            {
                case "Script Editor":
                    ScriptForm SF = new ScriptForm();
                    SF.Show();
                    break;
                case "Remote WinLog":
                    LogViewer LV = new LogViewer();
                    LV.Show();
                    break;
                default:
                    MessageBox.Show("Invalid form name");
                    break;
            }
        }

        private bool GetSiteWhitelistStatus(string ID)
        {
            bool whitelisted = false;

            string[] noVPNSites = File.ReadAllLines(getConfigValue("no_vpn_list"));
            foreach (string site in noVPNSites)
            {
                if (site.Contains(','))
                {
                    string[] site_arr = site.Split(',');
                    if (ID == site_arr[0]) { whitelisted = true; }

                }

            }
            return whitelisted;
        }

        private string GetNetworkTemplate(string ID)
        {
            string template = null;
            string[] noVPNSites = File.ReadAllLines(getConfigValue("no_vpn_list"));
            foreach (string site in noVPNSites)
            {
                if (site.Contains(','))
                {
                    string[] site_arr = site.Split(',');
                    if (ID == site_arr[0]) { template = site_arr[1]; }

                }

            }
            return template;
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

        public void SlashCom(string command)
        {

            command = command.Remove(0, 1);
            string argument = command;
            string parameter = "";
            try
            {
                argument = command.Remove(command.IndexOf(" "));
                parameter = command.Remove(0, command.IndexOf(" ") + 1);
            }
            catch (Exception) { }


            switch (argument)
            {
                case "Testmode":
                    testmode = !testmode;
                    Log("Info", "Testmode = " + testmode.ToString());
                    break;

                case "Set_Timeleft":
                    try
                    {
                        TimeLeft = Convert.ToInt32(parameter);
                        Log("Info", "Inactivity timer = " + TimeLeft + " minutes remaining");
                    }
                    catch (Exception)
                    {
                        Log("Info", "Invalid syntax, use \"/Help\" for a list of commands");
                    }
                    break;

                case "Timeleft":
                    Log("Info", TimeLeft.ToString());
                    break;

                case "ac":
                    ConfigForm CF = new ConfigForm();
                    CF.Show();
                    break;
                case "winlog":
                    LogViewer LV = new LogViewer();
                    LV.Show();
                    break;

                case "cfg":
                    Log(parameter, getConfigValue(parameter));
                    break;

                case "directory":
                    Log("Info", System.Reflection.Assembly.GetEntryAssembly().Location);
                    string path = System.Reflection.Assembly.GetEntryAssembly().Location;
                    path = path.Remove(path.LastIndexOf('\\') + 1);
                    Log("config pointer", path + "configpath.txt");
                    break;

                case "Help":
                    string commandList = "Available commands:" + Environment.NewLine
                        + "\"/OPTUpdate\" - for updating the OPT list using OPT contacts file (make sure contact list is in csv format)" + Environment.NewLine
                        + "\"/Timeleft\" - check how long until console timeout in minutes" + Environment.NewLine
                        + "\"/cfg [variable]\" - gets the value of the specified variable" + Environment.NewLine
                        + "" + Environment.NewLine
                        + "" + Environment.NewLine
                        + "" + Environment.NewLine
                        + "" + Environment.NewLine
                        + "" + Environment.NewLine;
                    Log("Info", commandList);
                    break;

                default:
                    Log("Info", "Invalid syntax, use \"/Help\" for a list of commands(Case sensitive)");
                    break;
            }

        }

        public void Push_Stats()
        {
            //writes stats for various things to stats file

            string[] statsfile = File.ReadAllLines(getConfigValue("stats_file"));
            string[] last_line = statsfile[statsfile.Length - 1].Split(',');
            DateTime last_rec_date = new DateTime(Convert.ToInt32(last_line[0]), Convert.ToInt32(last_line[1]), Convert.ToInt32(last_line[2]));
            if (last_rec_date != DateTime.Today)
            {
                //first entry for a new day
                File.AppendAllText(getConfigValue("stats_file"), /*Environment.NewLine + */DateTime.Today.Year + "," + DateTime.Today.Month + "," + DateTime.Today.Day +
                    "," + loaded + "," + sites_nuked + "," + pre_auths_explained + "," + corners_cut + "," + EMG_resets_pushed + "," + no_context + "," + dont_use_the_acronym + "," + DateTime.Today.DayOfWeek + Environment.NewLine);
            }
            else
            {
                //add onto the current day
                int t_loaded = loaded + Convert.ToInt32(last_line[3]);
                int t_sites_nuked = sites_nuked + Convert.ToInt32(last_line[4]);
                int t_pre_auths_explained = pre_auths_explained + Convert.ToInt32(last_line[5]);
                int t_corners_cut = corners_cut + Convert.ToInt32(last_line[6]);
                int t_EMG_resets_pushed = EMG_resets_pushed + Convert.ToInt32(last_line[7]);
                int t_no_context = no_context + Convert.ToInt32(last_line[8]);
                int t_dont_use_the_acronym = dont_use_the_acronym + Convert.ToInt32(last_line[9]);

                statsfile[statsfile.Length - 1] = DateTime.Today.Year + "," + DateTime.Today.Month + "," + DateTime.Today.Day +
                    "," + loaded + "," + sites_nuked + "," + pre_auths_explained + "," + corners_cut + "," + EMG_resets_pushed + "," + no_context + "," + dont_use_the_acronym + "," + DateTime.Today.DayOfWeek;

                File.WriteAllLines(getConfigValue("stats_file"), statsfile);
            }

            /*
                Issue with stats appears to be counters(only loaded?) are not being reset before being re-applied, issue might be somewhere between the below file write & loaded = 0
                current theory is loaded was being read in from the previous total of all users(>100) + current user total which would be fine but sometime between writing the high stats file & resetting the counter that value somehow doubles
                possibly the app crashes before the counter is reset, this calls OnFormClose() (is that possible?) which again calls this function which would approx double the counter
                need to check event logs within a day of one of these events to be sure of an app crash, changes above should fix absurd numbers
            */

            if(loaded >= 100)
            {
                File.AppendAllText(getConfigValue("high_stats_file"), (currentUser + " loaded " + loaded + " sites, " + DateTime.Now.ToString("dd/MM - HH:mm:ss") + Environment.NewLine));
            }

            loaded = 0;
            sites_nuked = 0;
            pre_auths_explained = 0;
            corners_cut = 0;
            EMG_resets_pushed = 0;
            no_context = 0;
            dont_use_the_acronym = 0;

        }

        public void AddToWhitelist(string site)
        {
            File.AppendAllText(getConfigValue("no_vpn_list"), Environment.NewLine + site);
            File.AppendAllText(getConfigValue("error_log"), $"added {site} to NoVPN " + System.DateTime.Now + Environment.NewLine);
        }

        public void RemoveFromWhitelist(string site)
        {
            string[] vpnsites = File.ReadAllLines(getConfigValue("no_vpn_list"));
            int i = 0;
            foreach (string _site in vpnsites)
            {
                if (_site == site)
                {
                    vpnsites[i] = "Removed";
                }
                i++;
            }
            File.WriteAllLines(getConfigValue("no_vpn_list"), vpnsites);
        }

        public void CheckIfMeraki(string site, string routerip)
        {
            bool meraki = false;
            Ping routerping = new Ping();
            Ping mwscheck = new Ping();

            for (int i = 0; i < 5; i++)
            {
                PingReply RPR = routerping.Send(routerip, 2000);
                if (RPR.Status.ToString() == "Success")
                {
                    RemoveFromWhitelist(site);
                    goto exit;
                }
                else
                {
                    PingReply MWSPR = mwscheck.Send(site + "-mws1");
                    if (MWSPR.Status.ToString() == "Success")
                    {
                        meraki = true;
                    }
                }
            }
            if (meraki) { AddToWhitelist(site); }
        exit:;
        }

        public void Log(string a, string b)
        {
            //removed, leaving method here because cbf removing all calls to it
        }
        #endregion

        #region TimTab

        private void TimButton(string program, string path)
        {
            if (program != "" && path != "")
            {
                //make sure none of the config calls returned nothing
                Process.Start(program, path);
            }
        }

        private void btn_tt1_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt1"), getConfigValue("path_tt1"));
        }

        private void btn_tt2_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt2"), getConfigValue("path_tt2"));
        }

        private void btn_tt3_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt3"), getConfigValue("path_tt3"));
        }

        private void btn_tt4_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt4"), getConfigValue("path_tt4"));
        }

        private void btn_tt5_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt5"), getConfigValue("path_tt5"));
        }

        private void btn_tt6_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt6"), getConfigValue("path_tt6"));
        }

        private void btn_tt7_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt7"), getConfigValue("path_tt7"));
        }

        private void btn_tt8_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt8"), getConfigValue("path_tt8"));
        }

        private void btn_tt9_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt9"), getConfigValue("path_tt9"));
        }

        private void btn_tt10_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt10"), getConfigValue("path_tt10"));
        }

        private void btn_tt11_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt11"), getConfigValue("path_tt11"));
        }

        private void btn_tt12_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt12"), getConfigValue("path_tt12"));
        }

        private void btn_tt13_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt13"), getConfigValue("path_tt13"));
        }

        private void btn_tt14_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt14"), getConfigValue("path_tt14"));
        }

        private void btn_tt15_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt15"), getConfigValue("path_tt15"));
        }

        private void btn_tt16_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt16"), getConfigValue("path_tt16"));
        }

        private void btn_tt17_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt17"), getConfigValue("path_tt17"));
        }

        private void btn_tt18_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt18"), getConfigValue("path_tt18"));
        }

        private void btn_tt19_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt19"), getConfigValue("path_tt19"));
        }

        private void btn_tt20_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt20"), getConfigValue("path_tt20"));
        }

        private void btn_tt21_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt21"), getConfigValue("path_tt21"));
        }

        private void btn_tt22_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt22"), getConfigValue("path_tt22"));
        }

        private void btn_tt23_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt23"), getConfigValue("path_tt23"));
        }

        private void btn_tt24_Click(object sender, EventArgs e)
        {
            TimButton(getConfigValue("program_tt24"), getConfigValue("path_tt24"));
        }

        #endregion

        #region Public Vars


        /*Misc*/

        //------------------------
        public string version = "4.2.0.4";
        public string[] siteIPData;
        public string[] siteInventoryData;
        public string[] config;
        //------------------------

        //stats stuff
        public int loaded = 0; //sites loaded
        public int sites_nuked = 0; //scripts ran
        public int pre_auths_explained = 0; //OPTs searched 
        public int corners_cut = 0; //shortcuts used
        public int EMG_resets_pushed = 0; //times device notes tab focused 
        public int no_context = 0;//times context menu shortcut / script was used
        public int dont_use_the_acronym = 0;//time constant ping launched from clicking an icon or context menu

        // other
        public List<Control> conlist = new List<Control>();
        public int sortOffset = 64;
        public volatile bool monStop = false;
        public string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        public int resultPos = 0;
        public int editState = 1;
        public string viewerText = "";
        public bool postecNotes = false;
        public bool routerNotes = false;
        public bool EMGNotes = false;
        public bool otherNotes = false;
        public int OPTResNum;
        public int OPTResMax;
        public string[] OPTSearchRes;
        public string Devcie = "";
        public string EditSite = "";
        public string AlertSDate = "";
        public string AlertEDate = "";
        public string AlertText = "";
        public bool testmode = false;
        public bool fnc_blacklisted = false;
        public int TimeLeft = 180;
        public string CPingIP = "127.0.0.1";
        public string debuggin = "";
        public bool LogEnd = false;
        public string PAbort = "0";
        public string TextP = "";
        public string TextC = "";
        public string TextE = "";
        public string Missmatch = "";
        public string CurrRand = "";
        public bool expanded = true;
        public int AddEntryState = 1;
        public bool LoadActive = false;

        //Info Loaded from CSV

        public string SiteType = "nositeloaded";
        public string SiteName = "nositeloaded";
        public string SiteID = "nositeloaded";
        public string SiteState = "New Zealand";
        public string SiteConType = "nositeloaded";
        public string NetIP = "127.0.0";
        public string SiteWANIP = "nositeloaded";
        public string SiteWANIP2 = "nositeloaded";
        public string Site3GIP = "nositeloaded";
        public string SiteDeCommed = "nositeloaded";
        public string SiteLastModTD = "nositeloaded";
        public string SiteLastEditBy = "nositeloaded";
        public string SiteWOWIP = "nositeloaded";

        //Assumed info

        //Front Counter

        public string PP1IP = "nositeloaded";
        public string PP2IP = "nositeloaded";
        public string PP3IP = "nositeloaded";
        public string PP4IP = "nositeloaded";
        public string PP5IP = "nositeloaded";
        public string PP6IP = "nositeloaded";
        public string PP7IP = "nositeloaded";
        public string MWSIP = "nositeloaded";
        public string CWSIP = "nositeloaded";
        public string CWS2IP = "nositeloaded";
        public string CWS3IP = "nositeloaded";
        public string CWS4IP = "nositeloaded";
        public string CWS5IP = "nositeloaded";
        public string CWS6IP = "nositeloaded";

        //for future devices no-one will tell us about until they are in use
        //update: oh look they're here now, at least they told us ...  I need more devices
        public string CustomIP1 = "nositeloaded";
        public string CustomIP2 = "nositeloaded";
        public string CustomIP3 = "nositeloaded";
        public string CustomIP4 = "nositeloaded";

        //Back Office

        public string POSTECIP = "nositeloaded";
        public string ROUTERIP = "nositeloaded";
        public string SWITCHIP = "nositeloaded";
        public string BOCIP = "nositeloaded";
        public string PRINTERIP = "nositeloaded";
        public string DOMS38 = "nositeloaded";
        public string DOMS58 = "nositeloaded"; //for that one site running legacy config



        #endregion
        //          Everything under this is to be moved
        #region to be sorted

        public string[] getBatGlobalVars()
        {
            string[] allText = File.ReadAllLines(getConfigValue("script_master_path"));
            int i = 0;
            foreach (string line in allText)
            {
                string newline = line;
                try
                {
                    newline = line.Remove(line.IndexOf('/')); //remove comments
                    allText[i] = newline;
                }
                catch (Exception) { }
                if (newline.Contains("="))
                {
                    try
                    {
                        string subvar = newline.Remove(0, line.IndexOf('=') + 1);
                        string value = getVarValue(subvar);
                        newline = newline.Remove(newline.IndexOf('=') + 1) + value;
                        allText[i] = newline;
                    }
                    catch (Exception) { }
                }
                i++;
            }
            return allText;
        }

        public string getVarValue(string name)
        {
            switch (name)
            {
                case "!~site_id":
                    return SiteID;
                case "!~mws_ip":
                    return MWSIP;
                default:
                    return null;
            }
        }

        private void btnCMD_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe");
        }

        private void btnRC_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Program Files (x86)\CA\DSM\Bin\gui_rcLaunch.exe");
        }

        private void CheckUser(string deviceIP)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[8];
            var random = new Random();
            for (int a = 0; a < stringChars.Length; a++)
            {
                stringChars[a] = chars[random.Next(chars.Length)];
            }
            var RandS = new String(stringChars);

            if (File.Exists($@"{getConfigValue("resources_folder")}UCheck_{RandS}.bat"))
            {
                Log("Error", $"Unable to run user check, file UCheck_{RandS} already exists");
                goto exit;
            }
            File.Create($@"{getConfigValue("resources_folder")}UCheck_{RandS}.bat").Close();
            File.WriteAllText($@"{getConfigValue("resources_folder")}UCheck_{RandS}.bat", @"cd\" + Environment.NewLine + "@Echo off" + Environment.NewLine + $@"PsLoggedon.exe \\{deviceIP}" + Environment.NewLine + $@"del {getConfigValue("resources_folder")}UCheck_{RandS}.bat && pause");
            Thread.Sleep(100);
            System.Diagnostics.Process.Start($@"{getConfigValue("resources_folder")}UCheck_{RandS}.bat");
            exit:;
        }

        public bool CompareNoCase(string checkValue, string IsIn) // check if first value is in second ignoring case
        {
            if (IsIn.IndexOf(checkValue, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else { return false; }
        }

        public void LogError(string errorText)
        {
            errorText = currentUser + ", " + DateTime.Now.ToString() + ", " + errorText;
            File.AppendAllText(getConfigValue("error_log"), errorText + Environment.NewLine);
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

        private Thread TBatMan(string path, string[] code, int delay)
        {
            var t = new Thread(() => BatMan(path, code, delay));
            t.Start();
            return t;
        }

        private Thread TPSMan(string path, string[] code, int delay)
        {
            var t = new Thread(() => PSMan(path, code, delay));
            t.Start();
            return t;
        }

        private void BatMan(string path, string[] code, int delay)
        {
            // batch manager for writing, launching and deleting with a delay.
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllLines(path, code);
                System.Diagnostics.Process.Start(path);
                Thread.Sleep(delay);
                File.Delete(path);
            }
        }

        private void PSMan(string path, string[] code, int delay)
        {
            // batch manager for writing, launching and deleting with a delay.
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllLines(path, code);

                ProcessStartInfo si = new ProcessStartInfo();
                si.FileName = "powershell.exe";
                si.Arguments = $" -File {path}";
                //si.RedirectStandardOutput = true;
                //si.RedirectStandardError = true;
                //si.UseShellExecute = false;
                //si.CreateNoWindow = false;
                Process p = new Process();
                p.StartInfo = si;
                p.Start();

                //System.Diagnostics.Process.Start(path, "powershell.exe"/*getConfigValue("posh_path")*/);
                Thread.Sleep(delay);
                File.Delete(path);
            }
        }

        private void namosNTKillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Do a murder?", "Namos Murderator 5000", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                File.AppendAllText(getConfigValue("ntkill"), $"{currentUser} ran Namos kill on site {SiteID} POS 1 at {DateTime.Now}" + Environment.NewLine);
                string[] allLines = { $"@RCMD \\\\{MWSIP} namosctl stop namosnt", $"@RCMD \\\\{MWSIP} namosctl start namosnt"};//string[] allLines = { $"@RCMD \\\\{MWSIP} taskkill /f /IM \"NamosNT.exe\"", "pause" };
                string RandS = GetRand8();
                TBatMan($@"{getConfigValue("resources_folder")}NamosStopStart_{RandS}.bat", allLines, 20000);
            }
        }

        private void isPOSHangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.AppendAllText(getConfigValue("hangman"), $"{currentUser} ran Is POS hung on site {SiteID} at {DateTime.Now}" + Environment.NewLine);
            string[] allLines = {
                //$"@RCMD \\\\{MWSIP} tasklist | sort /+{sortOffset}",
                $"@RCMD \\\\{MWSIP} tasklist /fi \"MEMUSAGE gt 100000\"",
                $"@RCMD \\\\{MWSIP} uptime",
                $"pause"};
            string RandS = GetRand8();
            TBatMan($@"{getConfigValue("resources_folder")}POSHang_{RandS}.bat", allLines, 20000);
        }

        private void Logger(string path, string[] lines)
        {
            string[] stamp = { Environment.NewLine, System.DateTime.Now.ToString(), Environment.NewLine };
            File.AppendAllLines(path, stamp);
            File.AppendAllLines(path, lines);
        }

        private void btn_winlog_Click(object sender, EventArgs e)
        {
            LogViewer LV = new LogViewer();
            LV.Show();
        }

        private void btn_scripts_Click(object sender, EventArgs e)
        {
            ScriptForm SF = new ScriptForm();
            SF.Show();
        }

        private void btn_config_Click(object sender, EventArgs e)
        {

            Process.Start($@"{getConfigValue("resources_folder")}Config.txt");

            /*  not enough time to make this usefull
            ConfigForm CF = new ConfigForm();
            CF.Show();
            */
        }

        private void namosKillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Do a murder?", "Namos Murderator 5000", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                File.AppendAllText(getConfigValue("ntkill"), $"{currentUser} ran Namos kill on site {SiteID} POS 2 at {DateTime.Now}" + Environment.NewLine);
                string[] allLines = { $"@RCMD \\\\{MWSIP} namosctl stop namosnt", $"@RCMD \\\\{MWSIP} namosctl start namosnt" };
                string RandS = GetRand8();
                TBatMan($@"{getConfigValue("resources_folder")}NamosStopStart_{RandS}.bat", allLines, 20000);
            }
        }

        private void namosNTKillToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Do a murder?", "Namos Murderator 5000", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                File.AppendAllText(getConfigValue("ntkill"), $"{currentUser} ran Namos kill on site {SiteID} POS 3 at {DateTime.Now}" + Environment.NewLine);
                string[] allLines = { $"@RCMD \\\\{MWSIP} namosctl stop namosnt", $"@RCMD \\\\{MWSIP} namosctl start namosnt" };
                string RandS = GetRand8();
                TBatMan($@"{getConfigValue("resources_folder")}NamosStopStart_{RandS}.bat", allLines, 20000);
            }
        }

        private void namosNTKillToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult DR = MessageBox.Show("Do a murder?", "Namos Murderator 5000", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (DR == DialogResult.Yes)
            {
                File.AppendAllText(getConfigValue("ntkill"), $"{currentUser} ran Namos kill on site {SiteID} POS 4 at {DateTime.Now}" + Environment.NewLine);
                string[] allLines = { $"@RCMD \\\\{MWSIP} namosctl stop namosnt", $"@RCMD \\\\{MWSIP} namosctl start namosnt" };
                string RandS = GetRand8();
                TBatMan($@"{getConfigValue("resources_folder")}NamosStopStart_{RandS}.bat", allLines, 20000);
            }
        }







        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //change window width to show or not show custom shortcut buttons
            expanded = !expanded;
            if (expanded) { this.Width = 884; } else { this.Width = 674; }
        }

        private void tbxSiteID_MouseDown(object sender, MouseEventArgs e)
        {
            tbxSiteID.SelectionStart = 0;
            tbxSiteID.SelectionLength = tbxSiteID.Text.Length;
        }

        private void isBOSHungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.AppendAllText(getConfigValue("hangman"), $"{currentUser} ran Is BOS hung on site {SiteID} at {DateTime.Now}" + Environment.NewLine);
            string[] allLines = {
                //$"@RCMD \\\\{BOCIP} tasklist | sort /+{sortOffset}",
                $"@RCMD \\\\{BOCIP} tasklist /fi \"MEMUSAGE gt 100000\"",
                $"@RCMD \\\\{BOCIP} uptime",
                $"pause"};
            string RandS = GetRand8();
            TBatMan($@"{getConfigValue("resources_folder")}POSHang_{RandS}.bat", allLines, 20000);
        }

        private void traceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fnc_blacklisted)
            {
                no_context++;
                traceToolStripMenuItem.Enabled = false;
                try
                {
                    if (SiteID != "nositeloaded")
                    {
                        Process.Start($@"\\{MWSIP}\c$\sni\namos\trace");
                    }
                    traceToolStripMenuItem.Enabled = true;
                }
                catch (Exception)
                {
                    //no connection to boc
                    traceToolStripMenuItem.Enabled = true;
                }
            }
        }
    }

    public class Site_Device{

        public string ip = string.Empty;
        public bool online = false;
        public Control indicator;

    }

}