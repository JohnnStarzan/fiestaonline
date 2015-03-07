using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher
{
    public class MyMessageFilter : IMessageFilter
    {
        private static uint UserMsg;

        public bool PreFilterMessage(ref Message m)
        {
            m.Msg.ToString();
            if (UserMsg == 0)
            {
                UserMsg = RegisterWindowMessage("GameLauncher");
            }
            if (m.Msg != UserMsg)
            {
                return false;
            }
            Logging.LogInfo("PreFilterMessage: got switching msg");
            GameLauncher.Launcher._UserConfig.reload();
            GameLauncher.Launcher.game = GameLauncher.Launcher._UserConfig.strLastGame;
            GameConfig gameConfig = GameLauncher._ConfigManager.GetGameConfig(GameLauncher.Launcher.game);
            GameLauncher.Launcher.GameConfiguration = gameConfig;
            string strBrowserURL = gameConfig.strBrowserURL;
            if (strBrowserURL != "")
            {
                GameLauncher.Launcher.webBrowser.Navigate(strBrowserURL);
                GameLauncher.Launcher.UpdateStatus.Text = " ";
            }
            else
            {
                GameLauncher.Launcher.UpdateStatus.Text = "Link Browser incorporato non trovato in registro.";
            }
            GameLauncher.Launcher.Activate();
            GameLauncher.Launcher.WindowState = FormWindowState.Normal;
            GameLauncher.Launcher.Visible = true;
            return true;
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern uint RegisterWindowMessage(string lpString);
    }
}

