using System;

namespace Launcher
{
    public class LauncherConfig : RegistryConfig
    {
        private bool _bSaveWhenValueSet;
        private static string ApiURLCONST = "apiURL";
        private static string autoPatch = "autopatch";
        private static string backgrounPatchCheckTime = "PatchTime";
        private static string gameopenURLCONST = "gameopenURL";
        private static string LastGameCONST = "LastGame";
        private static string patchdbCONST = "patchdb";
        private static string pathCONST = "path";
        private static string runInTaskBar = "runInTaskBar";
        private static string systemGUIDCONST = "systemGUID";
        private static string VersionCONST = "Version";

        public LauncherConfig(string strKey, bool bSaveWhenValueSet) : base(strKey)
        {
            this._bSaveWhenValueSet = bSaveWhenValueSet;
            string strReturn = null;
            base.getStringValue(ApiURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(ApiURLCONST, strReturn);
            }
            base.getStringValue(gameopenURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(gameopenURLCONST, strReturn);
            }
            base.getStringValue(LastGameCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(LastGameCONST, strReturn);
            }
            base.getStringValue(pathCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(pathCONST, strReturn);
            }
            base.getStringValue(patchdbCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(patchdbCONST, strReturn);
            }
            base.getStringValue(systemGUIDCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(systemGUIDCONST, strReturn);
            }
            int integerReturn = 0;
            if (base.getIntegerValue(VersionCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(VersionCONST, integerReturn);
            }
            if (base.getIntegerValue(autoPatch, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(autoPatch, integerReturn);
            }
            if (base.getIntegerValue(runInTaskBar, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(runInTaskBar, integerReturn);
            }
            if (base.getIntegerValue(backgrounPatchCheckTime, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(backgrounPatchCheckTime, integerReturn);
            }
        }

        public int iAutoPatch
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(autoPatch, out num))
                {
                    return num;
                }
                return -1;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(autoPatch, value);
                }
                base._IntegerConfigsDictionary[autoPatch] = value;
            }
        }

        public int iPatchTime
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(backgrounPatchCheckTime, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(backgrounPatchCheckTime, value);
                }
                base._IntegerConfigsDictionary[backgrounPatchCheckTime] = value;
            }
        }

        public int iRunInTaskBar
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(runInTaskBar, out num))
                {
                    return num;
                }
                return -1;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(runInTaskBar, value);
                }
                base._IntegerConfigsDictionary[runInTaskBar] = value;
            }
        }

        public int iVersion
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
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(VersionCONST, value);
                }
                base._IntegerConfigsDictionary[VersionCONST] = value;
            }
        }

        public string patchdb
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(patchdbCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(patchdbCONST, value);
                }
                base._StringConfigsDictionary[patchdbCONST] = value;
            }
        }

        public string strApiURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(ApiURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(ApiURLCONST, value);
                }
                base._StringConfigsDictionary[ApiURLCONST] = value;
            }
        }

        public string strGameopenURL
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
                if (this._bSaveWhenValueSet)
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
                if (base._StringConfigsDictionary.TryGetValue(LastGameCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(LastGameCONST, value);
                }
                base._StringConfigsDictionary[LastGameCONST] = value;
            }
        }

        public string strOutsparkPath
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
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(pathCONST, value);
                }
                base._StringConfigsDictionary[pathCONST] = value;
            }
        }

        public string strSystemGUID
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(systemGUIDCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(systemGUIDCONST, value);
                }
                base._StringConfigsDictionary[systemGUIDCONST] = value;
            }
        }
    }
}

