namespace Launcher
{
    using System;

    public class SharpLauncherConfig : RegistryConfig
    {
        private static bool _bSaveWhenValueSet;
        private static string DebugCONST = "Debug";
        private static string gameopenURLCONST = "gameopenURL";
        private static string lastGameCONST = "lastGame";
        private static string pathCONST = "path";
        private static string VersionCONST = "Version";

        public SharpLauncherConfig(string strKey, bool bSaveWhenValueSet) : base(strKey)
        {
            _bSaveWhenValueSet = bSaveWhenValueSet;
            string strReturn = null;
            base.getStringValue(pathCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(pathCONST, strReturn);
            }
            base.getStringValue(gameopenURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(gameopenURLCONST, strReturn);
            }
            base.getStringValue(lastGameCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(lastGameCONST, strReturn);
            }
            int integerReturn = 0;
            if (base.getIntegerValue(DebugCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(DebugCONST, integerReturn);
            }
            integerReturn = 0;
            if (base.getIntegerValue(VersionCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(VersionCONST, integerReturn);
            }
        }

        public int Debug
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(DebugCONST, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (_bSaveWhenValueSet)
                {
                    base.setIntegerValue(DebugCONST, value);
                }
                base._IntegerConfigsDictionary[DebugCONST] = value;
            }
        }

        public string strGameOpenURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(gameopenURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (_bSaveWhenValueSet)
                {
                    base.setStringValue(gameopenURLCONST, value);
                }
                base._StringConfigsDictionary[gameopenURLCONST] = value;
            }
        }

        public string strLastGame
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(lastGameCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (_bSaveWhenValueSet)
                {
                    base.setStringValue(lastGameCONST, value);
                }
                base._StringConfigsDictionary[lastGameCONST] = value;
            }
        }

        public virtual string strPath
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(pathCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (_bSaveWhenValueSet)
                {
                    base.setStringValue(pathCONST, value);
                }
                base._StringConfigsDictionary[pathCONST] = value;
            }
        }

        public virtual int strVersion
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(VersionCONST, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (_bSaveWhenValueSet)
                {
                    base.setIntegerValue(VersionCONST, value);
                }
                base._IntegerConfigsDictionary[VersionCONST] = value;
            }
        }
    }
}

