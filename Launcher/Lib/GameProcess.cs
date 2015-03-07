using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Launcher
{
    internal class GameProcess
    {
        public static string login_ip;

        public static bool Launch(string realm, string shop_url, string token, ref string fullcmd)
        {
            bool flag = false;
            login_ip = GameLauncher.Launcher.GameConfiguration.strLogin_ip;
            if (login_ip == "")
            {
                MessageBox.Show("L'indirizzo IP di Login non è presente nel registro.");
                return flag;
            }
            GameLauncher.FixClientRegistrySettings(realm);
            string str = GameLauncher.Launcher.GameConfiguration.strPath + GameLauncher.Launcher.GameConfiguration.strExecutable;
            string gamepath = str.Substring(0, str.LastIndexOf('\\') + 1);
            if (str == "")
            {
                MessageBox.Show("Il gioco non è stato installato correttamente. Ritentare l'installazione e assicurarsi di eseguire il riavvio al termine.");
                return flag;
            }
            string fileName = "\"" + str + "\"";
            string arguments = "";
            if (realm == "fiesta")
            {
                arguments = "-t " + token + " -i " + login_ip + " -u " + shop_url + " -osk_token " + token + " -osk_server " + login_ip + " -osk_store \"" + shop_url + "\"";
            }
            else if (realm == "solstice")
            {
                arguments = "-t " + token + " -i " + login_ip + " -U " + shop_url + " -p \"http://cdn2.outspark.com/sos\" -osk_token " + token + " -osk_server " + login_ip + " -osk_store \"" + shop_url + "\"";
            }
            else
            {
                arguments = " -osk_token " + token + " -osk_server " + login_ip + " -osk_store \"" + shop_url + "\"";
            }
            ProcessStartInfo info = new ProcessStartInfo(fileName, arguments) {
                WorkingDirectory = gamepath,
                Verb = "runas",
                UseShellExecute = false
            };
            Process process = new Process {
                StartInfo = info
            };
            fullcmd = fileName + arguments;
            try
            {
                if (realm == "propow")
                {
                    string pArgv = "660970B4786BCC46A5316D9B44CFE662F247AC42751AB5718C1667B8A953FD5F9F30355AFB423D6730A08A0731EF40007DBA3A3B30B71605F3D8D13D46F8B0BF0F7D04245246E404A17B5E10D445631E8828EF536410D76F434E4ED2C641869E64BB6419934B830A6A1E5488075C56FBC2771B288A569E";
                    S1(pArgv, gamepath, 60);
                }
                if (realm == "blackshot")
                {
                    string str6 = "660970B41969CB67C5306D9844CFE8624188D87A4C3D79750D07464CCE09B4721EF453E5B2C050128FC635E14D4D8E5D812F8364E4C9313C8CC0639B258C52C70F7D04245246E404A17B5E10D445631E8828EF536410D76F435C50DCCF4F9682B69183B7E892D52704675F93F64FF6F67552C8";
                    S1(str6, gamepath, 60);
                }
                if (realm == "fiesta")
                {
                    string str7 = "660970B4785BCA4650356D9844CFE86274070A7C1A847A5C80B29E977955C46513BED3B744670DBD38F568E3B7410CEF739AE61146936A57AA62122BFB6F0BC90F7D04245246E404A17B5E10D445631E8828EF536410D76F435855D8DF5084C58AA808F21BFA785D77D49CE9673A77";
                    S1(str7, gamepath, 60);
                }
                if (realm == "fistsoffu")
                {
                    string str8 = "660970B4957BDD4690256D9844CFE8624EF2DBEA3587E6E2478EB3AF792ACEE13C7FAFB8FA8AD1A3FC358182C821194EB310ADC8CDDE224251BE5F8E9B8CA6080F7D04245246E404A17B5E10D445631E8828EF536410D76F435855CED8578A8CA80E732953854CF740B5CF48ACCFC9A8F29D";
                    S1(str8, gamepath, 60);
                }
                if (realm == "divinesouls")
                {
                    string str9 = "660970B4956BDC4690256D9844CFE862A9E73F674AAD028DC9AF960B2BB8D0F0FF674882FE209B920C73B54D18C68133A8A4D0580FBCE70C62D61DC6B0BBA08F0F7D04245246E404A17B5E10D445631E8828EF536410D76F435A55CBC54A8099C836BE12A63C3FE1FED902365F2A0002F77B9B65";
                    S1(str9, gamepath, 60);
                }
                process.Start();
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Console.WriteLine(exception.Message);
            }
            return flag;
        }

        [DllImport("MiddEngn.dll")]
        public static extern int S1(string pArgv, string Gamepath, int dwTimeout);
    }
}

