using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uninstaller
{
    class UninstallProgram
    {
        public List<Tuple<string, string>> getApplication()
        {
            var RegPrograms = new List<Tuple<string, string>>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            RegistryKey keySixtyfour = Registry.LocalMachine.OpenSubKey("Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            foreach (var v in key.GetSubKeyNames())
            {
                RegistryKey productKey = key.OpenSubKey(v);
                if (productKey != null)
                {
                    string productName = Convert.ToString(productKey.GetValue("DisplayName"));
                    string uninstallString = Convert.ToString(productKey.GetValue("UninstallString"));
                    if (productName != "" && uninstallString != "")
                    {
                        RegPrograms.Add(Tuple.Create(productName, uninstallString));
                    }
                }
            }
            foreach (var d in keySixtyfour.GetSubKeyNames())
            {
                RegistryKey productKey = key.OpenSubKey(d);
                if (productKey != null)
                {
                    string productName = Convert.ToString(productKey.GetValue("DisplayName"));
                    string uninstallString = Convert.ToString(productKey.GetValue("UninstallString"));
                    //check to make sure not empty
                    if (productName != "" && uninstallString != "")
                    {
                        //Check if Dups would exist
                        if (!RegPrograms.Contains(Tuple.Create(productName, uninstallString)))
                        {
                            RegPrograms.Add(Tuple.Create(productName, uninstallString));
                        }
                    }
                }
            }
            return RegPrograms;
        }
    }
}
