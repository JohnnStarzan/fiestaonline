namespace Launcher
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;

    public class ReportMetrics
    {
        [DllImport("kernel32", CharSet=CharSet.Auto)]
        private static extern int GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong lpTotalNumberOfFreeBytes);
        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(out MemoryStatus stat);
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(ref uint connected, uint reserved);
        public static void report(string step, string game, string version, string guid, string strMore)
        {
            string requestUriString = "http://www.pcprime.it/installer/";
            try
            {
                ulong num2;
                ulong num3;
                ulong num4;
                requestUriString = (requestUriString + step + "?game=" + game) + "&version=" + version;
                OperatingSystem oSVersion = Environment.OSVersion;
                requestUriString = requestUriString + "&windowsVersion=" + oSVersion.Version.ToString();
                string servicePack = oSVersion.ServicePack;
                if ((servicePack != null) && (servicePack != ""))
                {
                    string str3 = servicePack.Replace(' ', '-');
                    requestUriString = requestUriString + "&servicePack=" + str3;
                }
                else
                {
                    requestUriString = requestUriString + "&servicePack=0";
                }
                MemoryStatus stat = new MemoryStatus();
                GlobalMemoryStatus(out stat);
                long totalPhysical = stat.TotalPhysical;
                requestUriString = requestUriString + "&physhicalMemory=" + (totalPhysical / 0x400L);
                GetDiskFreeSpaceEx("C:", out num2, out num3, out num4);
                requestUriString = requestUriString + "&freeCDriveSpace=" + (num4 / ((ulong) 0x400L));
                string str4 = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727", "Version", "-1");
                if (str4 == null)
                {
                    str4 = "-1";
                }
                requestUriString = requestUriString + "&DOTNET20VERSION=" + str4;
                str4 = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0", "Version", "-1");
                if (str4 == null)
                {
                    str4 = "-1";
                }
                requestUriString = requestUriString + "&DOTNET30VERSION=" + str4;
                str4 = (string) Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5", "Version", "-1");
                if (str4 == null)
                {
                    str4 = "-1";
                }
                requestUriString = requestUriString + "&DOTNET35VERSION=" + str4;
                string[] subKeyNames = Registry.LocalMachine.OpenSubKey(@"Software\pcprime.it\").GetSubKeyNames();
                int num5 = 0;
                for (int i = 0; i < subKeyNames.Length; i++)
                {
                    string name = @"Software\pcprime.it\" + subKeyNames[i];
                    RegistryKey key2 = Registry.LocalMachine.OpenSubKey(name);
                    object obj2 = key2.GetValue("Version", -1);
                    if ((obj2 != null) && (obj2.GetType() == typeof(int)))
                    {
                        int num7 = (int) key2.GetValue("Version", -1);
                        if (num7 != -1)
                        {
                            if (num5 == 0)
                            {
                                requestUriString = requestUriString + "&installed=";
                            }
                            else
                            {
                                requestUriString = requestUriString + "|";
                            }
                            requestUriString = requestUriString + subKeyNames[i] + "," + num7.ToString();
                            num5++;
                        }
                    }
                }
                requestUriString = requestUriString + "&guid=" + guid;
                if (strMore != null)
                {
                    requestUriString = requestUriString + strMore;
                }
                HttpWebRequest request = WebRequest.Create(requestUriString) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    HttpStatusCode statusCode = response.StatusCode;
                    new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                Launcher.Logging.LogInfo(exception.Message);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryStatus
        {
            public uint Length;
            public uint MemoryLoad;
            public uint TotalPhysical;
            public uint AvailablePhysical;
            public uint TotalPageFile;
            public uint AvailablePageFile;
            public uint TotalVirtual;
            public uint AvailableVirtual;
        }
    }
}

