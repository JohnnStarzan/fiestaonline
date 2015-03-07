using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher
{
    internal static class Program
    {
        private const string debugKey = @"HKEY_LOCAL_MACHINE\Software\pcprime.it\SharpLauncher";

        [STAThread]
        private static void Main(string[] args)
        {
            int num = (int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\pcprime.it\SharpLauncher", "Debug", 0);
            if (args.Length < 2)
            {
                MessageBox.Show("OutsparkPatcher requires 2 arguments");
            }
            else
            {
                int num2;
                string arguments = (string)args.GetValue(0);
                string szPackagePath = (string)args.GetValue(1);
                string szCommandLine = "";
                if (szPackagePath.Contains(".msi"))
                {
                    num2 = MsiInstallProduct(szPackagePath, szCommandLine);
                }
                else
                {
                    num2 = MsiApplyPatch(szPackagePath, "", 0, "");
                }
                if (num2 != 0)
                {
                    MessageBox.Show("Il tuo Game Launcher necessita di un aggiornamento. Devi procurarti l'ultima versione.");
                }
                else
                {
                    string str4 = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\pcprime.it\SharpLauncher", "path", "");
                    if (str4 == "")
                    {
                        MessageBox.Show("Percorso per il Launcher non presente nel Registro di sistema");
                    }
                    else
                    {
                        ProcessStartInfo info = new ProcessStartInfo(str4 + "GameLauncher.exe", arguments)
                        {
                            UseShellExecute = false
                        };
                        Process process = new Process
                        {
                            StartInfo = info
                        };
                        try
                        {
                            process.Start();
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                        if (num == 1)
                        {
                        }
                    }
                }
            }
        }

        [DllImport("msi.dll")]
        public static extern int MsiApplyPatch(string szPatchPackage, string szInstallPackage, int eInstallType, string szCommandLine);
        [DllImport("msi.dll")]
        public static extern int MsiInstallProduct(string szPackagePath, string szCommandLine);
    }
}
