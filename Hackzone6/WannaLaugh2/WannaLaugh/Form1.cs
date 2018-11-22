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
using System.Media;
using Microsoft.Win32;
using System.Globalization;
using System.Diagnostics;

namespace WannaLaugh
{
    public partial class Form1 : Form
    {
        public Thread PingThread;
        public Thread laughThread;
        private string timeStamp;
        public string addressMac;
        private bool PingStart = false;
        public string payLoad;
        public bool laugh = false;
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
        public Form1()
        {
            if (!checkVM())
            {
                InitializeComponent();
                if (CheckFrameworkInstalled())
                {
                    timeStamp = DateTime.Now.ToFileTime().ToString();
                    if (!string.IsNullOrEmpty(timeStamp))
                    {
                        addressMac = getRandomDomain();
                        if (!string.IsNullOrEmpty(addressMac))
                        {
                            payLoad = generateCode(addressMac, timeStamp);
                            PingStart = true;
                            laugh = true;
                            PingThread = new Thread(ping);
                            PingThread.Start();
                            timer1.Interval = 1000;
                            timer1.Start();
                        }
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
                MessageBox.Show("Sorry you cannot run this challenge inside a virtual machine...");
                Process.GetCurrentProcess().Kill();
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
        
        public string generateCode(string addressMac,string thisTime)
        {
            try
            {
                string ad1 = addressMac.Substring(0, 2);
                string ad2 = addressMac.Substring(2, 2);
                string ad3 = addressMac.Substring(4, 2);
                string ad4 = addressMac.Substring(6, 2);
                string ad5 = addressMac.Substring(8, 2);
                string ad6 = addressMac.Substring(10, 2);
                string t1 = thisTime.Substring(0, 3);
                string t2 = thisTime.Substring(3, 3);
                string t3 = thisTime.Substring(6, 3);
                string t4 = thisTime.Substring(9, 3);
                string t5 = thisTime.Substring(12, 3);
                string t6 = thisTime.Substring(15, 3);
                return t1 + ad1 + t2 + ad2 + t3 + ad3 + t4 + ad4 + t5 + ad5 + t6 + ad6;
            }
            catch (Exception ex)
            {

                return "";
            }
            
                
        }

        public string getRandomDomain()
        {
            try
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
            catch (Exception)
            {

                return "";
            }
            
        }

        public void ping()
        {
            while (PingStart)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://54.38.137.176:8076/laugh ");
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Timeout = 10000;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] byte1 = encoding.GetBytes(payLoad);
                    request.ContentLength = byte1.Length;
                    Stream requestWriter = request.GetRequestStream();

                    try
                    {
                        requestWriter.Write(byte1, 0, byte1.Length);
                    }
                    catch( Exception ex)
                    {
                        Thread.Sleep(5000);
                    }

                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string strResponse= sr.ReadToEnd();
                            if(strResponse.Contains("flag"))
                            {
                                string flag = strResponse.Split(',')[2];
                                MessageBox.Show(flag);
                                stopThreads();

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Thread.Sleep(5000);
                    }
                }
                catch (Exception ex)
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

        private void labelX6_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopThreads();
        }
    }
}
