using System;

namespace Launcher
{
    internal interface IConfig
    {
        int getIntegerValue(string strValueKey, ref int integerReturn);
        int getStringValue(string strValueKey, ref string strReturn);
        int setStringValue(string strValueKey, string strValue);
    }
}

