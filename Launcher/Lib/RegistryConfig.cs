using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Launcher
{
    public class RegistryConfig : IConfig
    {
        protected Dictionary<string, int> _IntegerConfigsDictionary = new Dictionary<string, int>();
        protected Dictionary<string, string> _StringConfigsDictionary = new Dictionary<string, string>();
        private string _strKey;

        public RegistryConfig(string strKey)
        {
            this._strKey = strKey;
        }

        public int getIntegerValue(string strValueKey, ref int integerReturn)
        {
            try
            {
                if (Registry.GetValue(this._strKey, strValueKey, null) != null)
                {
                    integerReturn = (int) Registry.GetValue(this._strKey, strValueKey, null);
                    return 0;
                }
                return -1;
            }
            catch (Exception exception)
            {
                Logging.LogInfo(exception.Message);
                return -1;
            }
        }

        public int getStringValue(string strValueKey, ref string strReturn)
        {
            try
            {
                strReturn = (string) Registry.GetValue(this._strKey, strValueKey, null);
                if (strReturn != null)
                {
                    return 0;
                }
                return -1;
            }
            catch (Exception exception)
            {
                Logging.LogInfo(exception.Message);
                return -1;
            }
        }

        public int Save()
        {
            try
            {
                foreach (KeyValuePair<string, string> pair in this._StringConfigsDictionary)
                {
                    if (this.setStringValue(pair.Key, pair.Value) != 0)
                    {
                        return -1;
                    }
                }
                foreach (KeyValuePair<string, int> pair2 in this._IntegerConfigsDictionary)
                {
                    if (this.setIntegerValue(pair2.Key, pair2.Value) != 0)
                    {
                        return -1;
                    }
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.LogInfo(exception.Message);
                return -1;
            }
        }

        public int setIntegerValue(string strValueKey, int iValue)
        {
            try
            {
                Registry.SetValue(this._strKey, strValueKey, iValue);
                return 0;
            }
            catch (Exception exception)
            {
                Logging.LogInfo(exception.Message);
                return -1;
            }
        }

        public int setStringValue(string strValueKey, string strValue)
        {
            try
            {
                Registry.SetValue(this._strKey, strValueKey, strValue);
                return 0;
            }
            catch (Exception exception)
            {
                Logging.LogInfo(exception.Message);
                return -1;
            }
        }
    }
}

