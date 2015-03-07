namespace Launcher
{
    using System;

    public class UserConfig : RegistryConfig
    {
        private bool _bSaveWhenValueSet;
        private static string LastGameCONST = "LastGame";
        private static string loginEmailRegistryName = "loginEmail";

        public UserConfig(string strKey, bool bSaveWhenValueSet) : base(strKey)
        {
            this._bSaveWhenValueSet = bSaveWhenValueSet;
            string strReturn = null;
            base.getStringValue(loginEmailRegistryName, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(loginEmailRegistryName, strReturn);
            }
            base.getStringValue(LastGameCONST, ref strReturn);
            if (strReturn != null)
            {
                base._StringConfigsDictionary.Add(LastGameCONST, strReturn);
            }
        }

        public void reload()
        {
            string strReturn = null;
            base.getStringValue(loginEmailRegistryName, ref strReturn);
            base._StringConfigsDictionary[loginEmailRegistryName] = strReturn;
            base.getStringValue(LastGameCONST, ref strReturn);
            base._StringConfigsDictionary[LastGameCONST] = strReturn;
        }

        public string strEmail
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(loginEmailRegistryName, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(loginEmailRegistryName, value);
                }
                base._StringConfigsDictionary[loginEmailRegistryName] = value;
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
    }
}

