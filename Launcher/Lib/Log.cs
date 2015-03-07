namespace Launcher
{
    using System;
    using System.IO;

    internal class Log
    {
        private static string logfile = "osklauncher.log";

        public static void debug(string msg)
        {
            StreamWriter writer = File.AppendText(logfile);
            try
            {
                string str = string.Format("{0:G}: {1}.", DateTime.Now, msg);
                writer.WriteLine(str);
            }
            finally
            {
                writer.Close();
            }
        }
    }
}

