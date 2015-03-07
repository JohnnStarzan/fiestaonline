using System;

namespace Launcher
{
    public class GameConfigSpecial : GameConfig
    {
        private bool _bSaveWhenValueSet;
        private GameConfig _gameConfig;

        public GameConfigSpecial(string strKey, string strSpecial, bool bSaveWhenValueSet) : base(strKey, bSaveWhenValueSet)
        {
            this._bSaveWhenValueSet = bSaveWhenValueSet;
            string str = strKey + strSpecial;
            this._gameConfig = new GameConfig(str, bSaveWhenValueSet);
            str = null;
            base.getStringValue(GameConfig.BrowserURLCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.BrowserURLCONST, str);
            }
            base.getStringValue(GameConfig.ExecutableCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.ExecutableCONST, str);
            }
            base.getStringValue(GameConfig.GameNameCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.GameNameCONST, str);
            }
            base.getStringValue(GameConfig.Login_ipCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.Login_ipCONST, str);
            }
            base.getStringValue(GameConfig.pathCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.pathCONST, str);
            }
            base.getStringValue(GameConfig.SignUpURLCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.SignUpURLCONST, str);
            }
            base.getStringValue(GameConfig.SupportURLCONST, ref str);
            if (str != null)
            {
                base._StringConfigsDictionary.Add(GameConfig.SupportURLCONST, str);
            }
            int integerReturn = 0;
            if (base.getIntegerValue(GameConfig.VersionCONST, ref integerReturn) != -1)
            {
                base._IntegerConfigsDictionary.Add(GameConfig.VersionCONST, integerReturn);
            }
        }

        public override int iVersion
        {
            get
            {
                int num;
                if (base._IntegerConfigsDictionary.TryGetValue(GameConfig.VersionCONST, out num))
                {
                    return num;
                }
                return base.iVersion;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setIntegerValue(GameConfig.VersionCONST, value);
                }
                base._IntegerConfigsDictionary[GameConfig.VersionCONST] = value;
            }
        }

        public override string strBrowserURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.BrowserURLCONST, out str))
                {
                    return str;
                }
                return base.strBrowserURL;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.BrowserURLCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.BrowserURLCONST] = value;
            }
        }

        public override string strExecutable
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.ExecutableCONST, out str))
                {
                    return str;
                }
                return base.strExecutable;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.ExecutableCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.ExecutableCONST] = value;
            }
        }

        public override string strGameName
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.GameNameCONST, out str))
                {
                    return str;
                }
                return base.strGameName;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.GameNameCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.GameNameCONST] = value;
            }
        }

        public override string strLogin_ip
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.Login_ipCONST, out str))
                {
                    return str;
                }
                return base.strLogin_ip;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.Login_ipCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.Login_ipCONST] = value;
            }
        }

        public override string strPath
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.pathCONST, out str))
                {
                    return str;
                }
                return base.strPath;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.pathCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.pathCONST] = value;
            }
        }

        public override string strSignUpURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.SignUpURLCONST, out str))
                {
                    return str;
                }
                return base.strSignUpURL;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.SignUpURLCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.SignUpURLCONST] = value;
            }
        }

        public override string strSupportURL
        {
            get
            {
                string str;
                if (base._StringConfigsDictionary.TryGetValue(GameConfig.SupportURLCONST, out str))
                {
                    return str;
                }
                return base.strSupportURL;
            }
            set
            {
                if (this._bSaveWhenValueSet)
                {
                    base.setStringValue(GameConfig.SupportURLCONST, value);
                }
                base._StringConfigsDictionary[GameConfig.SupportURLCONST] = value;
            }
        }
    }
}

