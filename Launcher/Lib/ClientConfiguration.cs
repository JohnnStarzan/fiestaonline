using System;

namespace Launcher
{
    public class ClientConfiguration
    {
        private string _browserURL;
        private string _loginIP;
        private string _longName;
        private string _patchURL;
        private string _path;
        private string _productCode;
        private string _signUpURL;
        private string _store;
        private string _supportURL;
        private string _version;

        public ClientConfiguration(string realm)
        {
            this.LoadConfig(realm);
        }

        private void LoadConfig(string realm)
        {
            throw new Exception("Il metodo o l'operazione non è gestite.");
        }

        public string BrowserURL
        {
            get
            {
                return this._browserURL;
            }
        }

        public string LoginIP
        {
            get
            {
                return this._loginIP;
            }
        }

        public string LongName
        {
            get
            {
                return this._longName;
            }
        }

        public string PatchURL
        {
            get
            {
                return this._patchURL;
            }
        }

        public string Path
        {
            get
            {
                return this._path;
            }
        }

        public string ProductCode
        {
            get
            {
                return this._productCode;
            }
        }

        public string SignUpURL
        {
            get
            {
                return this._signUpURL;
            }
        }

        public string Store
        {
            get
            {
                return this._store;
            }
        }

        public string SupportURL
        {
            get
            {
                return this._supportURL;
            }
        }

        public string Version
        {
            get
            {
                return this._version;
            }
        }
    }
}

