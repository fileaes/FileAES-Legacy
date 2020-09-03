using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FileAES
{
    public partial class FileAES_Update : Form
    {
        private UpdateStatus _appUpdateStatus;
        private string _latestVersion;
        private bool _updateThreadRunning = false;
        private bool _updateUI = false;
        private bool _showOnce = false;

        private static Core core = new Core();

        public FileAES_Update()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            InitializeComponent();
            CheckForUpdate();
            FaesVersion.Text = "FAES Version: " + FAES.FileAES_Utilities.GetVersion();
            copyrightLabel.Text = core.getCopyrightInfo();

            if (Program.GetDeveloperMode()) devToolStripMenuItem.Visible = true;
        }

        public enum UpdateStatus
        {
            ServerError,
            AppOutdated,
            AppLatest,
            AppNewer
        };

        private string GetLatestVersion()
        {
            try
            {
                WebClient client = new WebClient();
                byte[] html = client.DownloadData(String.Format("https://api.mullak99.co.uk/FAES/IsUpdate.php?app=faes_legacy&branch={0}&showver=true&version={1}", Program.GetBranch(), core.GetVersionInfo(false, true, false)));
                UTF8Encoding utf = new UTF8Encoding();
                if (String.IsNullOrEmpty(utf.GetString(html)) || utf.GetString(html) == "null")
                    return "0.0.0.0";
                else
                    return utf.GetString(html);
            }
            catch (Exception)
            {
                return "0.0.0.0";
            }
        }

        private UpdateStatus GetUpdateStatus(out string updateVersion)
        {
            try
            {
                string latestVer = GetLatestVersion();
                string currentVer = core.GetVersionInfo(false, true, true);
                updateVersion = latestVer;

                if (latestVer == currentVer)
                {
                    return UpdateStatus.AppLatest;
                }
                else if (latestVer != "0.0.0.0" && CheckServerConnection())
                {
                    string compareVersions = String.Format("https://api.mullak99.co.uk/FAES/CompareVersions.php?app=faes_legacy&branch={0}&version1={1}&version2={2}", "dev", currentVer, latestVer);

                    WebClient client = new WebClient();
                    byte[] html = client.DownloadData(compareVersions);
                    UTF8Encoding utf = new UTF8Encoding();
                    string result = utf.GetString(html).ToLower();

                    if (String.IsNullOrEmpty(result) || result == "null")
                        return UpdateStatus.ServerError;
                    else if (result.Contains("not exist in the database!") || result == "version1 is newer than version2")
                        return UpdateStatus.AppNewer;
                    else if (result == "version1 is older than version2")
                        return UpdateStatus.AppOutdated;
                    else if (result == "version1 is equal to version2")
                        return UpdateStatus.AppLatest;
                    else
                        return UpdateStatus.ServerError;
                }
                else
                {
                    return UpdateStatus.ServerError;
                }
            }
            catch
            {
                updateVersion = "0.0.0.0";
                return UpdateStatus.ServerError;
            }
        }

        public void CheckForUpdate()
        {
            if (!_updateThreadRunning)
            {
                _updateThreadRunning = true;

                Thread threaddedUpdateCheck = new Thread(() =>
                {
                    string updateVersion;
                    UpdateStatus updateInfo = GetUpdateStatus(out updateVersion);

                    _appUpdateStatus = updateInfo;
                    _latestVersion = updateVersion;
                    _updateUI = true;
                    _updateThreadRunning = false;
                });
                threaddedUpdateCheck.Start();
            }
        }

        public void UpdateUI()
        {
            currentVerLabel.Text = "Current Version: " + core.GetVersionInfo(false, true, true);

            if (_updateThreadRunning)
            {
                checkForUpdateButton.Enabled = false;
                checkForUpdateToolStripMenuItem.Enabled = false;
                updateButton.Visible = false;
                updateButton.Enabled = false;
                neverRemindButton.Visible = false;
                neverRemindButton.Enabled = false;
                downloadLatestButton.Visible = false;
                downloadLatestButton.Enabled = false;
                descLabel.Text = "Checking for updates...";
                latestVerLabel.Text = "Latest Version: Checking...";
            }
            else
            {
                if (_appUpdateStatus == UpdateStatus.AppLatest)
                {
                    string latestVersion = _latestVersion;
                    descLabel.Text = "You are on the latest version!";
                    updateButton.Visible = false;
                    updateButton.Enabled = false;
                    neverRemindButton.Visible = false;
                    neverRemindButton.Enabled = false;
                    downloadLatestButton.Visible = true;
                    downloadLatestButton.Enabled = true;
                    latestVerLabel.Text = "Latest Version: " + latestVersion;
                }
                else if (_appUpdateStatus == UpdateStatus.AppNewer)
                {
                    string latestVersion = _latestVersion;
                    descLabel.Text = "You are on a private build.";
                    updateButton.Visible = false;
                    updateButton.Enabled = false;
                    neverRemindButton.Visible = false;
                    neverRemindButton.Enabled = false;
                    downloadLatestButton.Visible = true;
                    downloadLatestButton.Enabled = true;
                    latestVerLabel.Text = "Latest Version: " + latestVersion;
                }
                else if (_appUpdateStatus == UpdateStatus.ServerError)
                {
                    descLabel.Text = "Unable to connect to the update server.";
                    updateButton.Visible = false;
                    updateButton.Enabled = false;
                    neverRemindButton.Visible = false;
                    neverRemindButton.Enabled = false;
                    downloadLatestButton.Visible = false;
                    downloadLatestButton.Enabled = false;
                    latestVerLabel.Text = "Latest Version: SERVER ERROR!";
                }
                else if (_appUpdateStatus == UpdateStatus.AppOutdated)
                {
                    string latestVersion = _latestVersion;
                    descLabel.Text = "An update is available!";
                    updateButton.Visible = true;
                    updateButton.Enabled = true;
                    neverRemindButton.Visible = true;
                    neverRemindButton.Enabled = true;
                    downloadLatestButton.Visible = false;
                    downloadLatestButton.Enabled = false;
                    latestVerLabel.Text = "Latest Version: " + latestVersion;

                    if (_appUpdateStatus == UpdateStatus.AppOutdated && !Program.GetSkipUpdate() && !_showOnce)
                    {
                        _showOnce = true;
                        this.Show();
                    }
                }
                _updateUI = false;
            }

            checkForUpdateButton.Enabled = true;
            checkForUpdateToolStripMenuItem.Enabled = true;
        }

        public static void UpdateSelf(bool doCleanUpdate = false)
        {
            string installDir = Directory.GetCurrentDirectory();

            if (CheckServerConnection())
                try
                {
                    if (File.Exists(Path.Combine(installDir, "FAES-Updater.exe")))
                        File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));
                    if (File.Exists(Path.Combine(installDir, "updater.pack")))
                        File.Delete(Path.Combine(installDir, "updater.pack"));

                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(new Uri(String.Format("https://api.mullak99.co.uk/FAES/GetDownload.php?app=faes_updater&ver=latest&branch={0}&redirect=true", Program.GetBranch())), Path.Combine(installDir, "updater.pack"));
                    ZipFile.ExtractToDirectory(Path.Combine(installDir, "updater.pack"), installDir);
                    File.Delete(Path.Combine(installDir, "updater.pack"));
                    Thread.Sleep(100);

                    string args = "";
                    if (doCleanUpdate) args += "--pure ";
                    if (Program.GetFullInstall()) args += "--full ";
                    if (Program.GetDeveloperMode()) args += "--verbose ";
                    else args += "--silent ";
                    args += "--branch " + Program.GetBranch() + " ";
                    args += "--tool faes_legacy ";
                    args += "--delay 10 ";
                    args += "--run ";
                    if (Program.GetFullInstall())
                    {
                        ProcessStartInfo proc = new ProcessStartInfo();
                        proc.UseShellExecute = true;
                        proc.Verb = "runas";
                    }
                    Process.Start(Path.Combine(installDir, "FAES-Updater.exe"), args);

                    Environment.Exit(0);
                }
                catch (UnauthorizedAccessException)
                {
                    if (MessageBox.Show("You are not running FileAES as an admin, by doing this you cannot update the application in admin protected directories.\n\nDo you want to launch as admin?", "Notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        RunAsAdmin();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error");
                    if (File.Exists(Path.Combine(installDir, "FAES-Updater.exe")))
                        File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));
                    if (File.Exists(Path.Combine(installDir, "updater.pack")))
                        File.Delete(Path.Combine(installDir, "updater.pack"));
                }
            else
            {
                if (File.Exists(Path.Combine(installDir, "FAES-Updater.exe")))
                    File.Delete(Path.Combine(installDir, "FAES-Updater.exe"));
                if (File.Exists(Path.Combine(installDir, "updater.pack")))
                    File.Delete(Path.Combine(installDir, "updater.pack"));
            }
        }

        public static bool CheckServerConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://api.mullak99.co.uk/"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static void RunAsAdmin()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Application.ExecutablePath;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    return;
                }
                Environment.Exit(0);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            Hide();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            UpdateSelf(Program.GetCleanUpdates());
        }

        private void downloadLatestButton_Click(object sender, EventArgs e)
        {
            UpdateSelf(Program.GetCleanUpdates());
        }

        private void downloadLatestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (downloadLatestButton.Enabled)
                downloadLatestButton_Click(sender, e);
            else if (updateButton.Enabled)
                updateButton_Click(sender, e);
        }

        private void neverRemindButton_Click(object sender, EventArgs e)
        {
            core.setIgnoreUpdate(true);
        }

        private void checkForUpdateButton_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkForUpdateButton_Click(sender, e);
        }

        private void spoofV110ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spoofV110ToolStripMenuItem.Checked = true;
            spoofV999999ToolStripMenuItem.Checked = false;
            spoofV132ToolStripMenuItem.Checked = false;
            core.SetSpoofVersion(true, "1.1.0.0");
        }

        private void spoofV999999ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spoofV110ToolStripMenuItem.Checked = false;
            spoofV999999ToolStripMenuItem.Checked = true;
            spoofV132ToolStripMenuItem.Checked = false;
            core.SetSpoofVersion(true, "99.99.99.0");
        }
        private void spoofV132ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spoofV110ToolStripMenuItem.Checked = false;
            spoofV999999ToolStripMenuItem.Checked = false;
            spoofV132ToolStripMenuItem.Checked = true;
            core.SetSpoofVersion(true, "1.3.2.0");
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spoofV110ToolStripMenuItem.Checked = false;
            spoofV999999ToolStripMenuItem.Checked = false;
            core.SetSpoofVersion(false);
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (_updateThreadRunning || _updateUI)
            {
                UpdateUI();
            }
        }
    }
}
