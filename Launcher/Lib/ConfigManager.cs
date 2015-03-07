using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
namespace Launcher
{
    public class ConfigManager
    {
        private bool _bSaveWhenValueSet;
        private LauncherConfig _LauncherConfig;
        private SharpLauncherConfig _SharpLauncherConfig;
        private UserConfig _UserConfig;
        private const string launcherRoot = @"HKEY_LOCAL_MACHINE\Software\pcprime.it\SharpLauncher\";
        public static string RegistryRoot = @"HKEY_LOCAL_MACHINE\Software\pcprime.it\";
        private const string ShortRegistryRoot = @"Software\pcprime.it\";
        public static string UserRegistryRoot = @"HKEY_CURRENT_USER\Software\pcprime.it\";

        public ConfigManager(bool bSaveWhenValueSet)
        {
            this._bSaveWhenValueSet = bSaveWhenValueSet;
            this._LauncherConfig = new LauncherConfig(RegistryRoot, bSaveWhenValueSet);
            this._SharpLauncherConfig = new SharpLauncherConfig(launcherRoot, bSaveWhenValueSet); // Added launcherRoot
            this._UserConfig = new UserConfig(UserRegistryRoot, bSaveWhenValueSet);
        }

        public static void FixClientRegistrySettings(string client)
        {
            string str = (string) Registry.GetValue(RegistryRoot + @"\" + client, "executable", "");
            string str2 = (string) Registry.GetValue(RegistryRoot + @"\" + client, "path", "");
            string str3 = str2.Substring(str2.LastIndexOf(@"\"));
            
            if (((str == "") || str3.Contains(".bin")) || str3.Contains(".exe"))
            {
                str = str2.Substring(str2.LastIndexOf(@"\") + 1);
                str2 = str2.Substring(0, str2.LastIndexOf(@"\"));
                if (client.ToLower() == "blackshot")
                {
                    str = @"System\" + str;
                    str2 = str2.Substring(0, str2.LastIndexOf(@"\"));
                }
                if (str2.LastIndexOf(@"\") != (str2.Length - 1))
                {
                    str2 = str2 + @"\";
                }
                Registry.SetValue(RegistryRoot + @"\" + client, "path", str2);
                Registry.SetValue(RegistryRoot + @"\" + client, "executable", str);
            //  Registry.SetValue(RegistryRoot + @"\" + GameConfig.Login_ipCONST, "regIP"); // Non necessario per ora.
            }
        }

        public GameConfig GetGameConfig(string realm)
        {
            return new GameConfig(RegistryRoot + realm, this._bSaveWhenValueSet);
        }

        public List<string> getInstalledGames()
        {
            List<string> list = new List<string>();
            string[] subKeyNames = Registry.LocalMachine.OpenSubKey(@"Software\pcprime.it\").GetSubKeyNames(); // Eccezione non gestita, causata dal asssenza della chiave di registro specificata
            for (int i = 0; i < subKeyNames.Length; i++)
            {
                string name = @"Software\pcprime.it\" + subKeyNames[i];
                if (!name.Contains("Launcher"))
                {
                    RegistryKey key2 = Registry.LocalMachine.OpenSubKey(name);
                    string str3 = (string) key2.GetValue("path", null);
                    if (str3 != null)
                    {
                        string str2 = (string) key2.GetValue("executable", null);
                        if ((str2 != null) && File.Exists(str3 + str2))
                        {
                            list.Add(name.Substring(name.LastIndexOf(@"\") + 1).ToLower());
                        }
                    }
                }
            }
            return list;
        }

        public LauncherConfig PrimeConfiguration
        {
            get
            {
                return this._LauncherConfig;
            }
        }

        public SharpLauncherConfig SharpLauncherConfiguration
        {
            get
            {
                return this._SharpLauncherConfig;
            }
        }

        public UserConfig UserConfiguration
        {
            get
            {
                return this._UserConfig;
            }
        }
    }
}

