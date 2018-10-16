using CrystalDecisions.Shared.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYFLEX_HMS.CLASS
{
    class CLS_REGISTER
    {
        public static string getMachineID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }

        public static bool GetProgramCurrentStatus()
        {
            bool res = Properties.Settings.Default.IsBlock;
            return res;
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
       
        public static void CheckStatus()
        {
            int m = 0;
            try
            {
                if (CheckForInternetConnection() == true)
                {
                    string res = GetDataFromCloude();
                    JArray a = JArray.Parse(res);
                    bool LOCK = true;
                    foreach (JObject o in a.Children<JObject>())
                    {
                        foreach (JProperty p in o.Properties())
                        {
                            string name = p.Name;
                            string value = (string)p.Value;
                            if (name == "installationkey" && value == CLS_REGISTER.getMachineID())
                            {
                                LOCK = false;
                                Properties.Settings.Default.IsBlock = false;
                                Properties.Settings.Default.Save();
                               // break;
                            }
                        }

                    }

                    if (LOCK == true)
                    {
                        Properties.Settings.Default.IsBlock = true;
                        Properties.Settings.Default.Save();
                        if(m <1)
                        {
                            Application.Restart();
                        }
                        m++;
                    }
                }
            }
            catch (Exception)
            {

            }
            
        }
        public static string GetDataFromCloude()
        {
            try
            {
                WebClient client = new WebClient();
                string url = "http://ezpos.hyperflex.lk/OfficialData/GetInstalledSystemDetails.php";
                byte[] html = client.DownloadData(url);
                UTF8Encoding utf = new UTF8Encoding();
                string mystring = utf.GetString(html);
                return mystring;
            }
            catch (Exception)
            {
                return "NO";
            }
        }
    }
}
