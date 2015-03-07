using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Launcher
{
    internal static class Program
    {
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWNORMAL = 1;
        private static uint UserMsg;

        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClassName, string strWindowName);
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        [STAThread]
        private static void Main(string[] args)
        {
            bool flag;
            Mutex mutex = new Mutex(true, "Game Launcher", out flag);
            GameLauncher._ConfigManager = new ConfigManager(true);
            if (!flag)
            {
                new Process();
                Process currentProcess = Process.GetCurrentProcess();
                Process[] processesByName = Process.GetProcessesByName("GameLauncher");
                for (int i = 0; i < processesByName.Length; i++)
                {
                    Process process2 = processesByName[i];
                    if (((processesByName.Length > 0) && (currentProcess.Handle != process2.Handle)) && (args.Length > 0))
                    {
                        if (UserMsg == 0)
                        {
                            UserMsg = RegisterWindowMessage("GameLauncher");
                        }
                        string str = (string)args.GetValue(0);
                        GameLauncher._ConfigManager.UserConfiguration.strLastGame = str;
                        if (process2.MainWindowHandle == IntPtr.Zero)
                        {
                            IntPtr zero = IntPtr.Zero;
                            do
                            {
                                int processId = 0;
                                zero = FindWindowEx(IntPtr.Zero, zero, null, null);
                                GetWindowThreadProcessId(zero, out processId);
                                if (processId == process2.Id)
                                {
                                    PostMessage(zero.ToInt32(), UserMsg, 0L, 0L);
                                    break;
                                }
                            }
                            while (!zero.Equals(IntPtr.Zero));
                        }
                        else
                        {
                            PostMessage((int)process2.MainWindowHandle, UserMsg, 0L, 0L);
                        }
                        Logging.LogInfo("Program.cs, Main: sending switching msg");
                    }
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GameLauncher(args));
                mutex.ReleaseMutex();
            }
        }

        [DllImport("user32.dll")]
        public static extern int PostMessage(int hWnd, uint Msg, long wParam, long lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll")]
        public static extern bool SetActiveWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool SetFocus(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("User32")]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool SwitchToThisWindow(IntPtr hwnd, bool flag);
    }
}
