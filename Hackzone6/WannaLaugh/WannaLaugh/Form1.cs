using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
using System.Diagnostics;
using System.Management;

namespace WannaLaugh
{
    public partial class Form1 : Form
    {
        public Thread PingThread;
        private string domainName;
        private bool PingStart = false;
        public int segundo = 0;
        public DateTime dt = new DateTime();
        public bool checkVM()
        {
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                {
                    using (var items = searcher.Get())
                    {
                        foreach (var item in items)
                        {
                            string manufacturer = item["Manufacturer"].ToString().ToLower();
                            if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                                || manufacturer.Contains("vmware")
                                || item["Model"].ToString() == "VirtualBox")
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        public bool CheckFrameworkInstalled()
        {
            try
            {
                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Net Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                if (Framework >= 4.0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Form1()
        {
            
            
            if (!checkVM())
            {
                InitializeComponent();

                if (CheckFrameworkInstalled())
                {
                    string subDomain = getRandomDomain();
                    if (!string.IsNullOrEmpty(subDomain))
                    {
                        domainName = "http://tinyurl.com/" + subDomain + "CSI";
                        //domainName = "http://tinyurl.com/azeazeazeazeazeazebomdskdq";
                        PingStart = true;
                        PingThread = new Thread(ping);
                        PingThread.Start();
                        timer1.Interval = 1000;
                        timer1.Start();
                    }


                }
                else
                {
                    MessageBox.Show("To begin this challenge you should install .Net Framework 4. Launch the installation and smoke a cigarette ... ");
                    Process.GetCurrentProcess().Kill();
                }

            }
            else
            {
                MessageBox.Show("Sorry this challenge cannot be running inside a virtual machine ... ");
                Process.GetCurrentProcess().Kill();
            }
        }
        public string getRandomDomain()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
        public void ping()
        {
            while (PingStart)
            {
                
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(domainName);
                    request.Timeout = 10000;
                    request.AllowAutoRedirect = true; 
                    request.Method = "GET";
                    
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        //Uri uri = response.ResponseUri;
                        //string s = uri.ToString();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            MessageBox.Show("HZVI{4lw4ys_cr34te_y0ur_k1ll_sw1tch}");
                            string path = "flag.txt";
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine("The flag is: HZVI{4lw4ys_cr34te_y0ur_k1ll_sw1tch}");
                                tw.Close();
                                stopThreads();
                            }
                        }
                        
                        
                    }
                }
                catch(Exception ex)
                {
                    Thread.Sleep(5000);
                }
                
            }
        }
        public void stopThreads()
        {
            try
            {
                PingStart = false;
                PingThread.Abort();

            }
            catch (Exception)
            {

                timer1.Stop();
                Process.GetCurrentProcess().Kill();
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void rectangleShape2_Click(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I have told you ! you don't need to pays us.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dafug ! Bettiba .. ");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            segundo++;

            labelX10.Text = dt.AddSeconds(segundo).ToString("HH:mm:ss");
            if (labelX10.Text.Equals("00:00:10"))
            {
                System.IO.Stream str = Properties.Resources.laugh;
                System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                snd.Play();
                labelX10.Text = "00:00:00";
                segundo = 0;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopThreads();
        }
    }
}
