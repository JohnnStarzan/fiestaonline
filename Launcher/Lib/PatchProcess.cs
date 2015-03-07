using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Launcher
{
    internal class PatchProcess
    {
        public static uint Launch(string game, string patchFile)
        {
            try
            {
                uint num = 0;
                int num2 = (int) Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System", "EnableLUA", 0);
                if (patchFile.ToLower().Contains(".msi"))
                {
                    MsiInstallProduct(patchFile, "");
                }
                else if (patchFile.ToLower().Contains(".msp"))
                {
                    MsiApplyPatch(patchFile, "", 0, "");
                }
                else if (patchFile.ToLower().Contains(".rtp"))
                {
                    GameConfig gameConfig = GameLauncher._ConfigManager.GetGameConfig(game);
                    uint num3 = RTPatchApply32WithCall(0, patchFile, null, "Outspark", "-nop", 0x201L, 0L, 0L);
                    num = num3;
                    string strMore = "&patchFile=" + patchFile;
                    if (num3 == 0)
                    {
                        gameConfig.iNumPatchErrors = 0;
                        ReportMetrics.report("patchSuccess", game, gameConfig.iVersion.ToString(), GameLauncher._ConfigManager.PrimeConfiguration.strSystemGUID, strMore);
                    }
                    else
                    {
                        gameConfig.iNumPatchErrors++;
                        strMore = strMore + "&patchError=" + num3.ToString();
                        ReportMetrics.report("patchFailed", game, gameConfig.iVersion.ToString(), GameLauncher._ConfigManager.PrimeConfiguration.strSystemGUID, strMore);
                        if (gameConfig.iNumPatchErrors >= 3)
                        {
                        }
                    }
                }
                else
                {
                    string str2 = " /i \"" + patchFile + "\"";
                    if (num2 == 0)
                    {
                        str2 = str2 + " /passive";
                    }
                    ProcessStartInfo info = new ProcessStartInfo(patchFile) {
                        UseShellExecute = true
                    };
                    Process process = new Process {
                        StartInfo = info
                    };
                    try
                    {
                        process.Start();
                        process.WaitForExit(0xdbba0);
                        num = 1;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
                return num;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [DllImport("msi.dll")]
        public static extern int MsiApplyPatch(string szPatchPackage, string szInstallPackage, int eInstallType, string szCommandLine);
        [DllImport("msi.dll")]
        public static extern int MsiInstallProduct(string szPackagePath, string szCommandLine);
        [DllImport("rtp32cb.dll")]
        private static extern uint RTPatchApply32WithCall(int hWindow, string PatchFile, string ApplyDir, string Title, string Options, ulong Options2, ulong HorizontalPos, ulong VerticalPos);
    }
}

