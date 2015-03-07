using System;

namespace Launcher
{
    public class GameConfig : RegistryConfig
    {
        private bool _bSaveWhenValueSet;
        protected static string BrowserURLCONST = "BrowserURL";
        protected static string ExecutableCONST = "executable";
        protected static string FirstLaunchCONST = "FirstLaunch";
        protected static string GameNameCONST = "GameName";
        protected static string Login_ipCONST = "login_ip";
        protected static string numPatchErrors = "numPatchErrors";
        protected static string pathCONST = "path";
        protected static string SignUpURLCONST = "SignUpURL";
        protected static string StoreURLCONST = "StoreURL";
        protected static string SupportURLCONST = "SupportURL";
        protected static string VersionCONST = "Version";

        public GameConfig(string strKey, bool bSaveWhenValueSet) : base(strKey)
        {
            this._bSaveWhenValueSet = bSaveWhenValueSet;
            string strReturn = null;
            base.getStringValue(BrowserURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(BrowserURLCONST, strReturn);
            }
            base.getStringValue(ExecutableCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(ExecutableCONST, strReturn);
            }
            base.getStringValue(GameNameCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(GameNameCONST, strReturn);
            }
            base.getStringValue(Login_ipCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(Login_ipCONST, strReturn);
            }
            base.getStringValue(pathCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(pathCONST, strReturn);
            }
            base.getStringValue(SignUpURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(SignUpURLCONST, strReturn);
            }
            base.getStringValue(SupportURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(SupportURLCONST, strReturn);
            }
            base.getStringValue(StoreURLCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(StoreURLCONST, strReturn);
            }
            int integerReturn = 0;
            if (base.getIntegerValue(VersionCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(VersionCONST, integerReturn);
            }
            if (base.getIntegerValue(FirstLaunchCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(FirstLaunchCONST, integerReturn);
            }
        }

        public void reload()
        {
            string strReturn = null;
            base.getStringValue(BrowserURLCONST, ref strReturn);
            base._StringConfigsDictionary[BrowserURLCONST] = strReturn;
            base.getStringValue(ExecutableCONST, ref strReturn);
            base._StringConfigsDictionary[ExecutableCONST] = strReturn;
            base.getStringValue(GameNameCONST, ref strReturn);
            base._StringConfigsDictionary[GameNameCONST] = strReturn;
            base.getStringValue(Login_ipCONST, ref strReturn);
            base._StringConfigsDictionary[Login_ipCONST] = strReturn;
            base.getStringValue(pathCONST, ref strReturn);
            base._StringConfigsDictionary[pathCONST] = strReturn;
            base.getStringValue(SignUpURLCONST, ref strReturn);
            base._StringConfigsDictionary[SignUpURLCONST] = strReturn;
            base.getStringValue(SupportURLCONST, ref strReturn);
            base._StringConfigsDictionary[SupportURLCONST] = strReturn;
            base.getStringValue(StoreURLCONST, ref strReturn);
            base._StringConfigsDictionary[StoreURLCONST] = strReturn;
            int integerReturn = 0;
            base.getIntegerValue(VersionCONST, ref integerReturn);
            base._IntegerConfigsDictionary[VersionCONST] = integerReturn;
        }

        public virtual int iFirstLaunch
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(FirstLaunchCONST, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(FirstLaunchCONST, value);
                }
                base._IntegerConfigsDictionary[FirstLaunchCONST] = value;
            }
        }

        public virtual int iNumPatchErrors
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(numPatchErrors, out num))
                {
                    return num;
                }
                return 0;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(numPatchErrors, value);
                }
                base._IntegerConfigsDictionary[numPatchErrors] = value;
            }
        }

        public virtual int iVersion
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

        public virtual string strBrowserURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(BrowserURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(BrowserURLCONST, value);
                }
                base._StringConfigsDictionary[BrowserURLCONST] = value;
            }
        }

        public virtual string strExecutable
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(ExecutableCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(ExecutableCONST, value);
                }
                base._StringConfigsDictionary[ExecutableCONST] = value;
            }
        }

        public virtual string strGameName
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameNameCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameNameCONST, value);
                }
                base._StringConfigsDictionary[GameNameCONST] = value;
            }
        }

        public virtual string strLogin_ip
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(Login_ipCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(Login_ipCONST, value);
                }
                base._StringConfigsDictionary[Login_ipCONST] = value;
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
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(pathCONST, value);
                }
                base._StringConfigsDictionary[pathCONST] = value;
            }
        }

        public virtual string strSignUpURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(SignUpURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(SignUpURLCONST, value);
                }
                base._StringConfigsDictionary[SignUpURLCONST] = value;
            }
        }

        public virtual string strStoreURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(StoreURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(StoreURLCONST, value);
                }
                base._StringConfigsDictionary[StoreURLCONST] = value;
            }
        }

        public virtual string strSupportURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(SupportURLCONST, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(SupportURLCONST, value);
                }
                base._StringConfigsDictionary[SupportURLCONST] = value;
            }
        }
    }
}

