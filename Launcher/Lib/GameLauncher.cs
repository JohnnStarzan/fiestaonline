using LitJson;
using Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Launcher
{
    public class GameLauncher : Form
    {
        private bool _bFullClientDownload;
        private bool _bLauncherDownload;
        public static ConfigManager _ConfigManager = null;
        private int _debugValue;
        private GameConfig _GameConfig;
        private SharpLauncherConfig _SharpLauncherConfig;
        public static TextBox _UpdateStatus;
        public UserConfig _UserConfig;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem alphaLoginToolStripMenuItem;
        private ToolStripMenuItem alphaPatchesToolStripMenuItem;
        private static System.Timers.Timer aTimer = null;
        private static bool bInAlready = false;
        private Button buttonProfileDelete;
        private WebClient client;
        private IContainer components;
        private ContextMenu contextMenu1;
        private string CurrentFileName;
        private TextBox debugOutput;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private static bool FirstCheck = true;
        private static string fname = "MsiInstall.msp";
        private static string fnameFileSpec;
        private static string fnamePath = @"C:\t\";
        private static string fullString = "FULL";
        public string game = "";
        private Dictionary<string, string> gameNameDictionary;
        private int gameVersion;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ComboBox inputEmail;
        private ComboBox inputGameProfile;
        private TextBox inputPassword;
        private Label label3;
        private Label labelPassword;
        private Label labelRealm;
        private string lastDownloaedPatchFileName;
        public static GameLauncher Launcher = null;
        private int launcherVersion;
        private Button loginButton;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private ToolStripMenuItem menuItemDebugOn;
        private MenuStrip menuStrip1;
        public static MiniINCO miniINCO = new MiniINCO();
        private MyMessageFilter msgFliter = new MyMessageFilter();
        private NotifyIcon notifyIcon1;
        private Panel panel1;
        public List<MyPair> PatchList;
        private PictureBox pictureBox1;
        private ToolStripMenuItem quitToolStripMenuItem;
        private ComponentResourceManager resources = new ComponentResourceManager(typeof(GameLauncher));
        private Button SignUp;
        private DateTime startDownloadTime;
        public static string strCurrentDownloadFile;
        private ToolStripMenuItem SupportMenuItem;
        private string tmSession = "";
        private ToolStripSeparator toolStripSeparator1;
        private static uint uConnection = 0x20;
        public TextBox UpdateStatus;
        public WebBrowser webBrowser;

        public GameLauncher(string[] args)
        {
            Application.AddMessageFilter(this.msgFliter);
            try
            {
                this.components = new Container();
                this.contextMenu1 = new ContextMenu();
                this._SharpLauncherConfig = _ConfigManager.SharpLauncherConfiguration;
                this._UserConfig = _ConfigManager.UserConfiguration;
                this.gameNameDictionary = new Dictionary<string, string>();
                Control.CheckForIllegalCrossThreadCalls = false;
                fnamePath = this._SharpLauncherConfig.strPath;
                Launcher = this;
                if (args.Length > 0)
                {
                    this.game = (string) args.GetValue(0);
                    if (this.game == "--run")
                    {
                        this.game = (string) args.GetValue(1);
                    }
                    this._GameConfig = _ConfigManager.GetGameConfig(this.game);
                    this._SharpLauncherConfig.setStringValue("lastgame", this.game);
                    if (args.Length > 1)
                    {
                        string str = (string) args.GetValue(1);
                        if (str == "firstlaunch")
                        {
                            this._GameConfig.iFirstLaunch = 1;
                        }
                    }
                }
                else
                {
                    this.game = _ConfigManager.SharpLauncherConfiguration.strLastGame;
                    this._GameConfig = _ConfigManager.GetGameConfig(this.game);
                    if (this.game == "")
                    {
                        MessageBox.Show("Il Launcher non dovrebbe essere eseguito direttamente . Si prega di utilizzare i collegamenti forniti dall'installatore.");
                        Environment.Exit(0);
                    }
                }
                if (this._GameConfig.iFirstLaunch == 1)
                {
                    if (_ConfigManager.PrimeConfiguration.strSystemGUID == null)
                    {
                        string str2 = Guid.NewGuid().ToString();
                        _ConfigManager.PrimeConfiguration.strSystemGUID = str2;
                    }
                    this.reportMetrics("firstlaunchStartLauncher", null);
                }
                this.InitializeComponent();
                this.PatchList = new List<MyPair>();
                this.Init();
                this.loginButton.Click += new EventHandler(this.loginButtonCallback);
                this.loginButton.MouseEnter += new EventHandler(this.loginButtonMouseEnterCallback);
                this.loginButton.MouseLeave += new EventHandler(this.loginButtonMouseLeaveCallback);
                this.SignUp.MouseEnter += new EventHandler(this.signupButtonMouseEnterCallback);
                this.SignUp.MouseLeave += new EventHandler(this.signupButtonMouseLeaveCallback);
                this.aboutToolStripMenuItem.Click += new EventHandler(this.showLauncherAbout);
                this.quitToolStripMenuItem.Click += new EventHandler(this.quit);
            }
            catch (Exception exception)
            {
                this.showExceptionError(exception, "Impossibile aprire il launcher.");
            }
        }

        private void alphaLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.alphaLogin = this.alphaLoginToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void alphaPatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.alphaPatches = this.alphaPatchesToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private string CalculateMD5Hash(string input)
        {
            MD5 md = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] buffer2 = md.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("X2"));
            }
            return builder.ToString().ToLower();
        }

        private string CalculateSHA256Hash(string input)
        {
            SHA256 sha = SHA256.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] buffer2 = sha.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("X2"));
            }
            return builder.ToString().ToLower();
        }

        public void ChangeUpdateStatus(string str)
        {
            this.UpdateStatus.Text = str;
        }

        private static int checkForUpdates()
        {
            string webRequest;
            if (FirstCheck)
            {
                Launcher.ChangeUpdateStatus("Il Launcher è aggiornato.");
            }
            FirstCheck = false;
            string realm = "propow";
            _ConfigManager.GetGameConfig(realm).reload();
            string patchdb = _ConfigManager.PrimeConfiguration.patchdb;
            if (patchdb == "")
            {
                Launcher.UpdateStatus.Text = "Patch DB server name is not in the registry";
                return -1;
            }
            List<string> list = _ConfigManager.getInstalledGames();
            if (Launcher.guestUser())
            {
                return -1;
            }
            Launcher.PatchList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                GameConfig gameConfig = _ConfigManager.GetGameConfig(list[i]);
                int num2 = gameConfig.strExecutable.LastIndexOf('.');
                if (Process.GetProcessesByName(gameConfig.strExecutable.Substring(0, gameConfig.strExecutable.Length - (gameConfig.strExecutable.Length - num2))).Length <= 0)
                {
                    string url = string.Concat(new object[] { patchdb, "v2/?application=", list[i], "&version=", gameConfig.iVersion });
                    Launcher.ChangeUpdateStatus("Checking " + gameConfig.strGameName + " Version");
                    webRequest = GetWebRequest(url);
                    if (webRequest != "")
                    {
                        if (Launcher.PatchList.Count == 0)
                        {
                            Launcher.PatchList = Launcher.getMsiIList(gameConfig.strGameName, webRequest);
                        }
                        else
                        {
                            List<MyPair> list2 = Launcher.getMsiIList(gameConfig.strGameName, webRequest);
                            for (int j = 0; j < list2.Count; j++)
                            {
                                Launcher.PatchList.Add(list2[j]);
                            }
                        }
                    }
                }
            }
            if (Launcher.PatchList.Count > 0)
            {
                webRequest = Launcher.PatchList[0].PatchFile;
                strCurrentDownloadFile = webRequest;
                bool bFullReDownload = false;
                Launcher.lastDownloaedPatchFileName = Launcher.PatchList[0].Game;
                Launcher.PatchList.RemoveAt(0);
                Launcher.Download(webRequest, false, bFullReDownload);
                string strMore = "&patchFile=" + webRequest;
                Launcher.reportMetrics("firstlaunchStartPatchDownload", strMore);
                return (Launcher.PatchList.Count + 1);
            }
            Launcher.ChangeUpdateStatus("Controllo Patch Completato");
            return 0;
        }

        public void debug(string msg)
        {
            this.debugOutput.Text = this.debugOutput.Text + msg + "\r\n";
        }

        private void debugOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.menuItemDebugOn.Checked = !this.menuItemDebugOn.Checked;
            this.debugOutput.Visible = this.menuItemDebugOn.Checked;
            this.webBrowser.Visible = !this.menuItemDebugOn.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Download(string addr, bool bLauncherDownload, bool bFullReDownload)
        {
            try
            {
                int num = 0;
                this.startDownloadTime = DateTime.Now;
                for (int i = 0; i < (addr.Length - 1); i++)
                {
                    if (addr[i] == '/')
                    {
                        num = i;
                    }
                }
                this.CurrentFileName = addr.Substring(num + 1);
                this.loginButton.Enabled = false;
                this.loginButton.Image = Resources.login_disabled;
                this._bLauncherDownload = bLauncherDownload;
                this._bFullClientDownload = bFullReDownload;
                this.client = new WebClient();
                Uri address = new Uri(addr);
                fnamePath = Environment.GetEnvironmentVariable("TEMP");
                fnamePath = fnamePath + @"\";
                fnameFileSpec = fnamePath + fname;
                if (addr.ToLower().Contains(".msi"))
                {
                    fnameFileSpec = fnamePath + "MsiInstall.msi";
                }
                else if (addr.ToLower().Contains(".msp"))
                {
                    fnameFileSpec = fnamePath + this.CurrentFileName;
                }
                else if (addr.ToLower().Contains(".rtp"))
                {
                    fnameFileSpec = fnamePath + this.CurrentFileName;
                }
                else
                {
                    fnameFileSpec = fnamePath + this.CurrentFileName;
                }
                this.client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.OnDownloadFileCompleted);
                this.client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.DownloadProgressCallback);
                this.client.DownloadFileAsync(address, fnameFileSpec, bFullReDownload);
            }
            catch (Exception exception)
            {
                this.showExceptionError(exception, "Il download non può essere eseguito. Controlla la tua connessione ad internet. Errore di rete non gestito");
                this.loginButton.Enabled = true;
                this.loginButton.Image = Resources.image_login;
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            string str = string.Format("      {0:###,###,###} KB of {1:###,###,###} KB. ({2}% complete)", e.BytesReceived / 0x400L, e.TotalBytesToReceive / 0x400L, e.ProgressPercentage);
            string str2 = "Downloading " + this.CurrentFileName + str;
            this.UpdateStatus.Text = str2;
        }

        public static void FixClientRegistrySettings(string client)
        {
            ConfigManager.FixClientRegistrySettings(client);
        }

        private void GamesMenuItemClicked(object sender, EventArgs e)
        {
            string str2;
            ToolStripItem item = (ToolStripItem) sender;
            string text = item.Text;
            if (this.gameNameDictionary.TryGetValue(text, out str2))
            {
                _ConfigManager.UserConfiguration.strLastGame = str2;
                this._UserConfig.reload();
                this.game = str2;
                GameConfig gameConfig = _ConfigManager.GetGameConfig(str2);
                Launcher.GameConfiguration = gameConfig;
                string strBrowserURL = gameConfig.strBrowserURL;
                if (strBrowserURL != "")
                {
                    this.webBrowser.Navigate(strBrowserURL);
                    this.UpdateStatus.Text = " ";
                }
                else
                {
                    this.UpdateStatus.Text = "Link Browser incorporato non trovato in registro";
                }
            }
        }

        private int getGameVersion(string realm)
        {
            int iVersion = -1;
            if (iVersion <= 0)
            {
                try
                {
                    iVersion = this._GameConfig.iVersion;
                }
                catch (Exception exception)
                {
                    Logging.LogInfo(exception.Message);
                    iVersion = -1;
                }
            }
            if ((iVersion == 0) && (realm == "fiesta"))
            {
                OperatingSystem oSVersion = Environment.OSVersion;
                string str = "";
                if (oSVersion.Version.Major == 6)
                {
                    str = Environment.GetEnvironmentVariable("PUBLIC") + @"\pcprime\";
                }
                else
                {
                    str = Environment.GetEnvironmentVariable("ALLUSERSPROFILE") + @"\Program Files (x86)\Fiesta Online\";
                }
                foreach (string str2 in System.IO.File.ReadAllLines(str + "apps.conf"))
                {
                    if (str2.Contains(","))
                    {
                        string[] strArray2 = str2.Split(new char[] { ',' });
                        if (strArray2[0] == "fiesta")
                        {
                            iVersion = short.Parse(strArray2[1]);
                        }
                    }
                }
            }
            return iVersion;
        }

        public List<MyPair> getMsiIList(string gameName, string MsiList)
        {
            string str2 = MsiList;
            List<MyPair> list = new List<MyPair>();
            string patchFile = MsiList;
            while (str2.Contains("\n"))
            {
                int index = str2.IndexOf("\n");
                patchFile = str2.Substring(0, index);
                str2 = str2.Substring(index + 1, (str2.Length - index) - 1);
                MyPair item = new MyPair(gameName, patchFile);
                list.Add(item);
            }
            return list;
        }

        private int getNewDNAFileString(string strOrginialURL, int port, ref string strDNAFileString)
        {
            strDNAFileString = string.Concat(new object[] { "http://localhost:", port, "/proxy?url=", strOrginialURL, "&service=min_rate_data&qos=50000" });
            return 0;
        }

        private HttpStatusCode getToken(ref string token, ref string strError)
        {
            HttpStatusCode oK = HttpStatusCode.OK;
            string text = this.inputGameProfile.Text;
            string text1 = this.inputEmail.Text;
            string text2 = this.inputPassword.Text;
            string strApiURL = _ConfigManager.PrimeConfiguration.strApiURL;
            if (strApiURL == "")
            {
                return HttpStatusCode.Unused;
            }
            if (this.alphaLoginToolStripMenuItem.Checked)
            {
                strApiURL = strApiURL.Replace("api", "api.alpha");
            }
            text = (this.game == "solstice") ? "sos" : this.game;
            string tmSession = "bypass";
            if ((this.TmSession != null) && (this.TmSession.Length > 0))
            {
                tmSession = this.TmSession;
            }
            string str4 = "/user/v1/token/" + text + "/" + this.inputEmail.Text + "/" + this.CalculateMD5Hash(this.inputPassword.Text) + "/" + tmSession + ".json";
            string requestUriString = strApiURL + str4;
            this.debug("Calling remote request: " + requestUriString);
            HttpWebRequest request = WebRequest.Create(requestUriString) as HttpWebRequest;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    object obj2;
                    oK = response.StatusCode;
                    if (oK != HttpStatusCode.OK)
                    {
                        strError = string.Concat(new object[] { "The server returned an error: ", response.StatusCode, " ", response.StatusDescription });
                        return oK;
                    }
                    if (this.readAPIResponseStream(response.GetResponseStream(), out obj2))
                    {
                        APISuccessResult result = obj2 as APISuccessResult;
                        if (((result != null) && (result.data != null)) && ((result.data.token != null) && (result.data.token.Length > 0)))
                        {
                            token = result.data.token;
                        }
                        return oK;
                    }
                    APIFailResult result2 = obj2 as APIFailResult;
                    if ((result2 != null) && (result2.data != null))
                    {
                        oK = (HttpStatusCode) result2.data.code;
                        strError = result2.data.error;
                    }
                    return oK;
                }
            }
            catch (WebException exception)
            {
                if (exception.Response == null)
                {
                    oK = HttpStatusCode.ServiceUnavailable;
                    strError = exception.Message;
                    return oK;
                }
                HttpWebResponse response2 = exception.Response as HttpWebResponse;
                oK = response2.StatusCode;
                strError = exception.Message;
            }
            return oK;
        }

        private static string GetWebRequest(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                HttpStatusCode statusCode = response.StatusCode;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }
        }

        public bool guestUser()
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal currentPrincipal = (WindowsPrincipal) Thread.CurrentPrincipal;
            return currentPrincipal.IsInRole(WindowsBuiltInRole.Guest);
        }

        private bool Init()
        {
            try
            {
                if (_ConfigManager.PrimeConfiguration.iAutoPatch == -1)
                {
                    _ConfigManager.PrimeConfiguration.iAutoPatch = 1;
                }
                if (_ConfigManager.PrimeConfiguration.iRunInTaskBar == -1)
                {
                    _ConfigManager.PrimeConfiguration.iRunInTaskBar = 1;
                }
                ConfigManager.FixClientRegistrySettings(this.game);
                this.alphaLoginToolStripMenuItem.Checked = Properties.Settings.Default.alphaLogin;
                this.alphaPatchesToolStripMenuItem.Checked = Properties.Settings.Default.alphaPatches;
                string strEmail = this._UserConfig.strEmail;
                if (strEmail != null)
                {
                    this.inputEmail.Text = strEmail;
                }
                new EventArgs();
                this.gameVersion = this.getGameVersion(this.game);
                if (this.gameVersion == -1)
                {
                    MessageBox.Show(this.game + " was not properly installed.");
                    return false;
                }
                Version version = Assembly.Load("GameLauncher").GetName().Version;
                this.launcherVersion = version.Build;
                this.webBrowser.ObjectForScripting = miniINCO;
                string strBrowserURL = this._GameConfig.strBrowserURL;
                if (strBrowserURL != "-1")
                {
                    this.webBrowser.Navigate(strBrowserURL + "/?launcher=" + this.launcherVersion);
                }
                else
                {
                    this.UpdateStatus.Text = "Embedded Browser link not found in registry";
                }
                this._debugValue = this._SharpLauncherConfig.Debug;
                if (this._debugValue != 0)
                {
                    this.debugOutput.Visible = true;
                    this.debugToolStripMenuItem.Visible = true;
                }
            }
            catch (Exception exception)
            {
                this.showExceptionError(exception, "Launcher Initialization Error.");
                return false;
            }
            try
            {
                new Thread(new ThreadStart(this.ThreadGetWebRequestProc)).Start();
            }
            catch (Exception exception2)
            {
                this.showExceptionError(exception2, "Launcher Threading Error.");
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.inputPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.inputEmail = new System.Windows.Forms.ComboBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SupportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDebugOn = new System.Windows.Forms.ToolStripMenuItem();
            this.alphaLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alphaPatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateStatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelRealm = new System.Windows.Forms.Label();
            this.buttonProfileDelete = new System.Windows.Forms.Button();
            this.inputGameProfile = new System.Windows.Forms.ComboBox();
            this.debugOutput = new System.Windows.Forms.TextBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.loginButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SignUp = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // inputPassword
            // 
            this.inputPassword.Location = new System.Drawing.Point(354, 437);
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '*';
            this.inputPassword.Size = new System.Drawing.Size(102, 20);
            this.inputPassword.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(299, 440);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(53, 13);
            this.labelPassword.TabIndex = 7;
            this.labelPassword.Text = "Password";
            // 
            // inputEmail
            // 
            this.inputEmail.FormattingEnabled = true;
            this.inputEmail.Location = new System.Drawing.Point(110, 437);
            this.inputEmail.Name = "inputEmail";
            this.inputEmail.Size = new System.Drawing.Size(176, 21);
            this.inputEmail.TabIndex = 1;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quitToolStripMenuItem.Text = "Esci";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SupportMenuItem,
            this.toolStripSeparator1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // SupportMenuItem
            // 
            this.SupportMenuItem.Name = "SupportMenuItem";
            this.SupportMenuItem.Size = new System.Drawing.Size(152, 22);
            this.SupportMenuItem.Text = "Supporto";
            this.SupportMenuItem.Click += new System.EventHandler(this.OnSupportMenuItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "Informazioni";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(632, 24);
            this.menuStrip1.TabIndex = 0;
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDebugOn,
            this.alphaLoginToolStripMenuItem,
            this.alphaPatchesToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            this.debugToolStripMenuItem.Visible = false;
            // 
            // menuItemDebugOn
            // 
            this.menuItemDebugOn.Name = "menuItemDebugOn";
            this.menuItemDebugOn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.menuItemDebugOn.Size = new System.Drawing.Size(201, 22);
            this.menuItemDebugOn.Text = "Debug Output";
            // 
            // alphaLoginToolStripMenuItem
            // 
            this.alphaLoginToolStripMenuItem.CheckOnClick = true;
            this.alphaLoginToolStripMenuItem.Name = "alphaLoginToolStripMenuItem";
            this.alphaLoginToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.alphaLoginToolStripMenuItem.Text = "Alpha Login";
            // 
            // alphaPatchesToolStripMenuItem
            // 
            this.alphaPatchesToolStripMenuItem.CheckOnClick = true;
            this.alphaPatchesToolStripMenuItem.Name = "alphaPatchesToolStripMenuItem";
            this.alphaPatchesToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.alphaPatchesToolStripMenuItem.Text = "Alpha Patches";
            // 
            // UpdateStatus
            // 
            this.UpdateStatus.BackColor = System.Drawing.SystemColors.ControlLight;
            this.UpdateStatus.Location = new System.Drawing.Point(0, 464);
            this.UpdateStatus.Multiline = true;
            this.UpdateStatus.Name = "UpdateStatus";
            this.UpdateStatus.ReadOnly = true;
            this.UpdateStatus.Size = new System.Drawing.Size(632, 21);
            this.UpdateStatus.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 440);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Email";
            // 
            // labelRealm
            // 
            this.labelRealm.AutoSize = true;
            this.labelRealm.Location = new System.Drawing.Point(645, 166);
            this.labelRealm.Name = "labelRealm";
            this.labelRealm.Size = new System.Drawing.Size(98, 13);
            this.labelRealm.TabIndex = 14;
            this.labelRealm.Text = "Game Profile Name";
            // 
            // buttonProfileDelete
            // 
            this.buttonProfileDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonProfileDelete.Location = new System.Drawing.Point(784, 183);
            this.buttonProfileDelete.Name = "buttonProfileDelete";
            this.buttonProfileDelete.Size = new System.Drawing.Size(46, 20);
            this.buttonProfileDelete.TabIndex = 21;
            this.buttonProfileDelete.Text = "Delete";
            this.buttonProfileDelete.UseVisualStyleBackColor = true;
            // 
            // inputGameProfile
            // 
            this.inputGameProfile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.inputGameProfile.FormattingEnabled = true;
            this.inputGameProfile.Location = new System.Drawing.Point(648, 182);
            this.inputGameProfile.Name = "inputGameProfile";
            this.inputGameProfile.Size = new System.Drawing.Size(130, 21);
            this.inputGameProfile.TabIndex = 4;
            // 
            // debugOutput
            // 
            this.debugOutput.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.debugOutput.Location = new System.Drawing.Point(-4, 0);
            this.debugOutput.Multiline = true;
            this.debugOutput.Name = "debugOutput";
            this.debugOutput.ReadOnly = true;
            this.debugOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugOutput.Size = new System.Drawing.Size(628, 400);
            this.debugOutput.TabIndex = 0;
            this.debugOutput.Visible = false;
            // 
            // webBrowser
            // 
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(-4, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(628, 402);
            this.webBrowser.TabIndex = 22;
            this.webBrowser.Url = new System.Uri("http://www.pcprime.it", System.UriKind.Absolute);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.webBrowser);
            this.panel1.Controls.Add(this.debugOutput);
            this.panel1.Controls.Add(this.inputGameProfile);
            this.panel1.Controls.Add(this.buttonProfileDelete);
            this.panel1.Controls.Add(this.labelRealm);
            this.panel1.Location = new System.Drawing.Point(4, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(628, 400);
            this.panel1.TabIndex = 22;
            // 
            // loginButton
            // 
            this.loginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.loginButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginButton.FlatAppearance.BorderSize = 0;
            this.loginButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.loginButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginButton.ForeColor = System.Drawing.SystemColors.Control;
            this.loginButton.Image = global::Launcher.Properties.Resources.login;
            this.loginButton.Location = new System.Drawing.Point(460, 434);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(64, 24);
            this.loginButton.TabIndex = 3;
            this.loginButton.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = global::Launcher.Properties.Resources.sparkID;
            this.pictureBox1.Location = new System.Drawing.Point(4, 437);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 20);
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // SignUp
            // 
            this.SignUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SignUp.FlatAppearance.BorderSize = 0;
            this.SignUp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.SignUp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.SignUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SignUp.ForeColor = System.Drawing.SystemColors.Control;
            this.SignUp.Image = global::Launcher.Properties.Resources.signUP;
            this.SignUp.Location = new System.Drawing.Point(523, 434);
            this.SignUp.Name = "SignUp";
            this.SignUp.Size = new System.Drawing.Size(105, 23);
            this.SignUp.TabIndex = 23;
            this.SignUp.UseVisualStyleBackColor = false;
            this.SignUp.Click += new System.EventHandler(this.OnFreeSignUpClicked);
            // 
            // GameLauncher
            // 
            this.AcceptButton = this.loginButton;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(632, 485);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.UpdateStatus);
            this.Controls.Add(this.SignUp);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.inputEmail);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.inputPassword);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "GameLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Launcher";
            this.MinimumSizeChanged += new System.EventHandler(this.OnMinmizedSizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnFormLoaded);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(ref uint connected, uint reserved);
        private void loginButtonCallback(object sender, EventArgs e)
        {
            string token = "";
            string fullcmd = "";
            this._GameConfig.reload();
            string patchdb = _ConfigManager.PrimeConfiguration.patchdb;
            if (this.alphaPatchesToolStripMenuItem.Checked)
            {
                patchdb = patchdb.Replace("patchdb", "alpha.patchdb");
            }
            Launcher.gameVersion = this.getGameVersion(Launcher.game);
            string url = string.Concat(new object[] { patchdb, "v2/?application=", Launcher.game, "&version=", Launcher.gameVersion });
            Launcher.ChangeUpdateStatus("Checking " + Launcher.game + " Version");
            if ((GetWebRequest(url) != "") && !this.guestUser())
            {
                try
                {
                    new Thread(new ThreadStart(this.ThreadGetWebRequestProc)).Start();
                }
                catch (Exception exception)
                {
                    this.showExceptionError(exception, "LoginButtonCallback checking error.");
                }
            }
            else
            {
                string strError = null;
                HttpStatusCode code = this.getToken(ref token, ref strError);
                if (code != HttpStatusCode.OK)
                {
                    MessageBox.Show(strError, ((int) code).ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    string strStoreURL;
                    if (!this.guestUser())
                    {
                        this._UserConfig.strEmail = this.inputEmail.Text;
                    }
                    if (this._GameConfig.strStoreURL == null)
                    {
                        strStoreURL = "http://www.pcprime/game/" + this.game;
                        this._GameConfig.strStoreURL = strStoreURL;
                    }
                    else
                    {
                        strStoreURL = this._GameConfig.strStoreURL;
                    }
                    if ((this._debugValue == 0) || (this._debugValue == 1))
                    {
                        Logging.LogInfo("Launching " + this.game);
                        GameProcess.Launch(this.game, strStoreURL, token, ref fullcmd);
                        this.reportMetrics("firstlaunchStartGame", null);
                        this._GameConfig.iFirstLaunch = 0;
                        if (this._debugValue == 0)
                        {
                            Application.Exit();
                        }
                        this.debug("Running:\n" + fullcmd);
                    }
                    else if ((this.inputGameProfile.Text != null) || (this.inputGameProfile.Text != ""))
                    {
                        this.debug("token:\n" + token);
                        if (code == HttpStatusCode.OK)
                        {
                            Logging.LogInfo("Launching " + this.game);
                            GameProcess.Launch(this.inputGameProfile.Text, strStoreURL, token, ref fullcmd);
                            this.reportMetrics("firstlaunchStartGame", null);
                            this._GameConfig.iFirstLaunch = 0;
                        }
                        this.debug("Running:\n" + fullcmd);
                    }
                    else
                    {
                        MessageBox.Show("Select a profile before logging in");
                    }
                }
            }
        }

        private void loginButtonMouseEnterCallback(object sender, EventArgs e)
        {
            if (this.loginButton.Enabled)
            {
                this.loginButton.Image = Resources.login_hover; // Genera un eccezione: 'System.Resources.MissingManifestResourceException' in mscorlib.dll
            }
        }

        private void loginButtonMouseLeaveCallback(object sender, EventArgs e)
        {
            if (this.loginButton.Enabled)
            {
                this.loginButton.Image = Resources.image_login;
            }
        }

        private void menuItem_Exit(object Sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem_Restore(object Sender, EventArgs e)
        {
            bool flag = false;
            List<string> list = _ConfigManager.getInstalledGames();
            for (int i = 0; i < list.Count; i++)
            {
                if (this.game == list[i])
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                if (list.Count <= 0)
                {
                    MessageBox.Show("There are no more Outspark Games on your system. The Outspark Launcher is existing.");
                    Application.Exit();
                    return;
                }
                string str = list[0];
                string realm = str;
                _ConfigManager.UserConfiguration.strLastGame = realm;
                this._UserConfig.reload();
                this.game = realm;
                GameConfig gameConfig = _ConfigManager.GetGameConfig(realm);
                Launcher.GameConfiguration = gameConfig;
                string strBrowserURL = gameConfig.strBrowserURL;
                if (strBrowserURL != "")
                {
                    this.webBrowser.Navigate(strBrowserURL);
                    this.UpdateStatus.Text = " ";
                }
                else
                {
                    this.UpdateStatus.Text = "Embedded Browser link not found in registry";
                }
            }
            base.WindowState = FormWindowState.Normal;
            base.Visible = true;
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            base.Visible = true;
            base.WindowState = FormWindowState.Normal;
            base.Activate();
        }

        private void OnCheckForUpdatesClick(object sender, EventArgs e)
        {
            if (!HasInternetConnection)
            {
                MessageBox.Show("Il launcher non riesce a contattare la rete pubblica. Controlla di essere connesso alla rete internet.");
            }
            else
            {
                this.setNewPatchTimer();
                if (checkForUpdates() == 0)
                {
                    MessageBox.Show("Non ci sono nuovi aggiornamenti.");
                }
            }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Normal;
            base.Visible = true;
        }

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                this.UpdateStatus.Text = "Download Completato";
                if (!this._bLauncherDownload)
                {
                    string strMore = "&patchFile=" + strCurrentDownloadFile;
                    this.reportMetrics("firstlaunchFinishPatchDownload", strMore);
                    uint num = PatchProcess.Launch(this.lastDownloaedPatchFileName, fnameFileSpec);
                    if (fnameFileSpec.ToLower().Contains(".rtp") && (num != 0))
                    {
                        string text = "Patching Error with patch file:  " + this.lastDownloaedPatchFileName + ".";
                        this.UpdateStatus.Text = text;
                        MessageBox.Show(text);
                    }
                    else if (this.PatchList.Count > 0)
                    {
                        string patchFile = this.PatchList[0].PatchFile;
                        this.lastDownloaedPatchFileName = this.PatchList[0].Game;
                        this.PatchList.RemoveAt(0);
                        strMore = "&patchFile=" + patchFile;
                        this.reportMetrics("firstlaunchStartPatchDownload", strMore);
                        Launcher.Download(patchFile, false, false);
                        return;
                    }
                }
                this.client.DownloadFileCompleted -= new AsyncCompletedEventHandler(this.OnDownloadFileCompleted);
                this.client.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(this.DownloadProgressCallback);
                this.loginButton.Enabled = true;
                this.loginButton.Invalidate();
                this.loginButton.Image = Resources.image_login;
                bInAlready = false;
            }
            catch (Exception exception)
            {
                this.showExceptionError(exception, "Launcher Threading Error.");
            }
            if (this._bLauncherDownload)
            {
                if (!fnameFileSpec.Contains("Patcher"))
                {
                    string strPath = this._SharpLauncherConfig.strPath;
                    if (strPath == "")
                    {
                        MessageBox.Show("Path for Launcher does not exist in registry");
                        return;
                    }
                    string fileName = strPath = strPath + "Patcher.exe";
                    string arguments = this.game + " " + fnameFileSpec;
                    if (fnameFileSpec.ToLower().Contains(".exe"))
                    {
                        fileName = fnameFileSpec;
                        arguments = "";
                    }
                    ProcessStartInfo info = new ProcessStartInfo(fileName, arguments) {
                        UseShellExecute = false
                    };
                    Process process = new Process {
                        StartInfo = info
                    };
                    try
                    {
                        process.Start();
                    }
                    catch (Exception exception2)
                    {
                        this.showExceptionError(exception2, "Launching patcher Error: ");
                    }
                    this._UserConfig.strLastGame = this.game;
                    base.Visible = false;
                    this.quit(null, null);
                }
                else
                {
                    PatchProcess.Launch("SharpLauncher", fnameFileSpec);
                }
            }
            if (this._bFullClientDownload)
            {
                base.Visible = false;
                Application.Exit();
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void OnFormLoaded(object sender, EventArgs e)
        {
        }

        private void OnFreeSignUpClicked(object sender, EventArgs e)
        {
            string strSignUpURL = this._GameConfig.strSignUpURL;
            if (strSignUpURL != null)
            {
                Process.Start(strSignUpURL);
            }
            else
            {
                this.UpdateStatus.Text = "Link non presente nel registro di sistema.";
            }
        }

        private void OnGamesMenuDropDown(object sender, EventArgs e)
        {
        }

        private void OnInstallMore(object sender, EventArgs e)
        {
            string fileName = "http://www.pcprime.it";
            if (fileName != null)
            {
                Process.Start(fileName);
            }
            else
            {
                this.UpdateStatus.Text = "Link non presente nel registro di sistema";
            }
        }

        private void OnMinmizedSizeChanged(object sender, EventArgs e)
        {
        }

        private void OnSettings(object sender, EventArgs e)
        {
            new Settings().ShowDialog();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
        }

        private void OnSupportMenuItemClicked(object sender, EventArgs e)
        {
            string strSupportURL = this._GameConfig.strSupportURL;
            if (strSupportURL == null)
            {
                strSupportURL = "http://www.pcprime.it/support";
            }
            if (strSupportURL != null)
            {
                Process.Start(strSupportURL);
            }
            else
            {
                this.UpdateStatus.Text = "Link non presente nel registro di sistema";
            }
        }

        private static void OnTimedPatchEvent(object myObject, EventArgs myEventArgs)
        {
            if (!bInAlready)
            {
                bInAlready = true;
                checkForUpdates();
            }
        }

        private void quit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool readAPIResponseStream(Stream stream, out object response)
        {
            response = null;
            string json = new StreamReader(stream).ReadToEnd();
            try
            {
                APISuccessResult result = JsonMapper.ToObject<APISuccessResult>(json);
                response = result;
                return true;
            }
            catch (Exception)
            {
                try
                {
                    APIFailResult result2 = JsonMapper.ToObject<APIFailResult>(json);
                    response = result2;
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        public void reportMetrics(string strStep, string strMore)
        {
            ReportMetrics.report(strStep, this.game, this._GameConfig.iVersion.ToString(), _ConfigManager.PrimeConfiguration.strSystemGUID, strMore);
        }

        private void setNewPatchTimer()
        {
            double num2;
            if (aTimer == null)
            {
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(GameLauncher.OnTimedPatchEvent);
            }
            else
            {
                aTimer.Elapsed -= new ElapsedEventHandler(GameLauncher.OnTimedPatchEvent);
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(GameLauncher.OnTimedPatchEvent);
            }
            if (_ConfigManager.PrimeConfiguration.iPatchTime == 0)
            {
                int num = 720;
                num2 = 0xea60 * num;
                _ConfigManager.PrimeConfiguration.iPatchTime = num;
            }
            else
            {
                num2 = 0xea60 * _ConfigManager.PrimeConfiguration.iPatchTime;
            }
            aTimer.Interval = num2;
            aTimer.Enabled = true;
        }

        private void showExceptionError(Exception exception, string prefix)
        {
            string strInfo = prefix + " " + exception.Message;
            Logging.LogInfo(strInfo);
            Logging.LogInfo(exception.StackTrace);
            MessageBox.Show(strInfo);
        }

        private void showLauncherAbout(object sender, EventArgs e)
        {
            new GameAboutBox().ShowDialog();
        }

        private void signupButtonMouseEnterCallback(object sender, EventArgs e)
        {
            this.SignUp.Image = Resources.signup_hover;
        }

        private void signupButtonMouseLeaveCallback(object sender, EventArgs e)
        {
            this.SignUp.Image = Resources.signup;
        }

        public void ThreadGetWebRequestProc()
        {
            this._GameConfig.reload();
            string patchdb = _ConfigManager.PrimeConfiguration.patchdb;
            if (patchdb == "")
            {
                Launcher.UpdateStatus.Text = "Patch DB server name is not in the registry";
            }
            else
            {
                if (this.alphaPatchesToolStripMenuItem.Checked)
                {
                    patchdb = patchdb.Replace("patchdb", "alpha.patchdb");
                }
                string url = patchdb + "v2/?application=sharplauncher&version=" + Launcher.launcherVersion;
                try
                {
                    string msiList = "";
                    if (FirstCheck)
                    {
                        Launcher.ChangeUpdateStatus("Checking Launcher Version");
                        msiList = GetWebRequest(url);
                    }
                    if (msiList == "")
                    {
                        if (FirstCheck)
                        {
                            Launcher.ChangeUpdateStatus("Launcher is up to date");
                        }
                        FirstCheck = false;
                        url = string.Concat(new object[] { patchdb, "v2/?application=", Launcher.game, "&version=", Launcher.gameVersion });
                        Launcher.ChangeUpdateStatus("Checking " + Launcher.game + " Version");
                        msiList = GetWebRequest(url);
                        if (msiList == "")
                        {
                            if (this._GameConfig.strGameName != null)
                            {
                                string str = "Synced";
                                Launcher.ChangeUpdateStatus(str);
                            }
                            else
                            {
                                Launcher.ChangeUpdateStatus("Gioco non trovato nel registro.");
                            }
                        }
                        else if (Launcher.guestUser())
                        {
                            string text1 = "Una patch è disponibile per " + Launcher.game + ".  Ad ogni modo, assicurati di aver aperto il programma con privilegi di amministratore.";
                            MessageBox.Show("errmsg");
                        }
                        else
                        {
                            Launcher.PatchList = Launcher.getMsiIList(Launcher.game, msiList);
                            msiList = Launcher.PatchList[0].PatchFile;
                            strCurrentDownloadFile = msiList;
                            bool bFullReDownload = false;
                            if (msiList == fullString)
                            {
                                bFullReDownload = true;
                                string strGameName = this._GameConfig.strGameName;
                                if (strGameName == null)
                                {
                                    strGameName = "installer";
                                }
                                if (MessageBox.Show("Devi scaricare la versione completa di " + strGameName + " per poter continuare a giocare " + strGameName + ". Vuoi scaricare l'ultima versione di?", strGameName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Launcher.PatchList.RemoveAt(0);
                                    msiList = Launcher.PatchList[0].PatchFile;
                                    Launcher.PatchList.RemoveAt(0);
                                    Launcher.Download(msiList, false, bFullReDownload);
                                    Launcher.PatchList.Clear();
                                }
                            }
                            else
                            {
                                this.lastDownloaedPatchFileName = Launcher.PatchList[0].Game;
                                Launcher.PatchList.RemoveAt(0);
                                Launcher.Download(msiList, false, bFullReDownload);
                                string strMore = "&patchFile=" + msiList;
                                this.reportMetrics("firstlaunchStartPatchDownload", strMore);
                            }
                        }
                    }
                    else if (Launcher.guestUser())
                    {
                        MessageBox.Show("Una patch è disponibile per essere scaricata. Ad ogni modo, assicurati di aver aperto il programma con privilegi di amministratore.");
                    }
                    else
                    {
                        if (msiList.Contains("\n"))
                        {
                            int index = msiList.IndexOf("\n");
                            msiList = msiList.Substring(0, index);
                        }
                        Launcher.ChangeUpdateStatus("Sto aggiornando le componenti interne del programma.");
                        Launcher.Download(msiList, true, false);
                    }
                }
                catch (Exception exception)
                {
                    Launcher.ChangeUpdateStatus(exception.Message);
                }
            }
        }

        private static void TimerEventProcessor(object myObject, EventArgs myEventArgs)
        {
            string strGameopenURL = _ConfigManager.PrimeConfiguration.strGameopenURL;
            if (strGameopenURL != "")
            {
                HttpWebRequest request = WebRequest.Create(strGameopenURL + Launcher.game) as HttpWebRequest;
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string str3 = new StreamReader(response.GetResponseStream()).ReadToEnd();
                            Launcher.loginButton.Enabled = str3 == "1";
                        }
                    }
                }
                catch (Exception exception)
                {
                    Launcher.UpdateStatus.Text = "Il gioco non è ancora aperto. " + exception.Message;
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void updateGamesMenu()
        {
        }

        public TextBox ustats()
        {
            return this.UpdateStatus;
        }

        public GameConfig GameConfiguration
        {
            get
            {
                return this._GameConfig;
            }
            set
            {
                this._GameConfig = value;
            }
        }

        public static bool HasInternetConnection
        {
            get
            {
                return InternetGetConnectedState(ref uConnection, 0);
            }
        }

        public string TmSession
        {
            get
            {
                return this.tmSession;
            }
            set
            {
                this.tmSession = value;
            }
        }
    }
}

