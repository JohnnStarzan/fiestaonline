using LitJson;
using System;
using System.Runtime.InteropServices;

namespace Launcher
{
    [ComVisible(true)]
    public class MiniINCO
    {
        public string execute(string command, string json)
        {
            string str = null;
            string str2;
            command = command.ToLower();
            if (((str2 = command) == null) || !(str2 == "set_tm_session"))
            {
                return str;
            }
            try
            {
                GenericMiniINCOParam param = JsonMapper.ToObject<GenericMiniINCOParam>(json);
                GameLauncher.Launcher.TmSession = param.value;
                return "ok";
            }
            catch (Exception)
            {
                return "fail";
            }
        }
    }
}

