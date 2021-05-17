using FAES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FileAES
{
    static class Program
    {
        private const string _fullInstallPath = @"mullak99\FileAES\config\FileAES_Legacy-launchParams.cfg";
        private const string _localInstallPath = @"config\launchParams.cfg";

        public static string fileName = "";
        public static string tempPathInstance = "";
        public static bool doEncryptFile = false;
        public static bool doEncryptFolder = false;
        public static bool doDecrypt = false;
        private static bool _skipUpdate = false;
        private static bool _fullInstall = false;
        private static bool _cleanUpdates = false;
        private static bool _purgeTemp = false;
        private static bool _debugMode = false;
        private static string _branch = "";
        private static string _autoPassword = null;
        private static bool _doFilePeek = false;
        private static bool _associateFileTypes = false;
        private static bool _startMenuShortcuts = false;
        private static bool _contextMenus = false;

        private static readonly List<string> _supportedPeekFiles = new List<string> { ".TXT", ".MD", ".LOG" };

        [STAThread]
        static void Main(string[] args)
        {
            tempPathInstance = Path.Combine(Path.GetTempPath(), "FileAES");
            List<string> arguments = new List<string>();
            arguments.AddRange(args);
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _fullInstallPath))) arguments.AddRange(ReadLaunchParams());
            if (File.Exists(_localInstallPath)) arguments.AddRange(ReadLaunchParams(true));

            string[] param = arguments.ToArray();

            for (int i = 0; i < param.Length; i++)
            {
                param[i].ToLower();

                if (File.Exists(param[i]) && FileAES_Utilities.isFileDecryptable(param[i]) && !doEncryptFile && !doEncryptFolder)
                {
                    doDecrypt = true;
                    fileName = param[i];
                }
                else if (Directory.Exists(param[i]) && !doDecrypt && !doEncryptFile)
                {
                    doEncryptFolder = true;
                    fileName = param[i];
                }
                else if (File.Exists(param[i]) && !doDecrypt && !doEncryptFolder)
                {
                    doEncryptFile = true;
                    fileName = param[i];
                }

                if (param[i].Equals("-fullinstall") || param[i].Equals("--fullinstall") || param[i].Equals("-f") || param[i].Equals("--f")) _fullInstall = true;
                else if (param[i] == "--associatefiletypes") _associateFileTypes = true;
                else if (param[i] == "--startmenushortcuts") _startMenuShortcuts = true;
                else if (param[i] == "--contextmenus") _contextMenus = true;
                else if (param[i] == "--dev") _branch = "dev";
                else if (param[i] == "--beta") _branch = "beta";
                else if (param[i] == "--stable") _branch = "stable";
                else if (param[i] == "--skipupdate" || param[i] == "-skipupdate") _skipUpdate = true;
                else if (param[i].Equals("-cleanupdates") || param[i].Equals("--cleanupdates") || param[i].Equals("-c") || param[i].Equals("--c")) _cleanUpdates = true;
                else if (param[i].Equals("-update") || param[i].Equals("--update") || param[i].Equals("-u") || param[i].Equals("--u")) FileAES_Update.UpdateSelf(_cleanUpdates);
                else if (param[i].Equals("-password") || param[i].Equals("--password") || param[i].Equals("-p") || param[i].Equals("--p") && !String.IsNullOrEmpty(param[i + 1])) _autoPassword = param[i + 1];
                else if (param[i].Equals("-purgetemp") || param[i].Equals("--purgetemp") || param[i].Equals("-deletetemp") || param[i].Equals("--deletetemp")) _purgeTemp = true;
                else if (param[i].Equals("-debug") || param[i].Equals("--debug") || param[i].Equals("-developer") || param[i].Equals("--developer")) _debugMode = true;
                else if (param[i].Equals("-peek") || param[i].Equals("--peek") || param[i].Equals("-filepeek") || param[i].Equals("--filepeek")) _doFilePeek = true;
            }
            if (String.IsNullOrEmpty(_branch)) _branch = "stable";

            if (_purgeTemp) FileAES_Utilities.PurgeTempFolder();
            if (File.Exists("FAES-Updater.exe")) File.Delete("FAES-Updater.exe");

            Core core = new Core();
            if (_branch == "stable" && (core.IsBetaBuild() || core.IsDevBuild())) _branch = core.GetBuild();
            else if (_branch == "beta" && core.IsDevBuild()) _branch = core.GetBuild();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (doEncryptFile || doEncryptFolder) Application.Run(new FileAES_Encrypt(fileName, _autoPassword));
            else if (doDecrypt)
            {
                if (_doFilePeek && IsFileValidForPeek(new FAES_File(fileName)))
                    Application.Run(new FileAES_Peek(fileName, _autoPassword));
                else
                    Application.Run(new FileAES_Decrypt(fileName, _autoPassword));
            }
            else Application.Run(new FileAES_Main());

        }

        public static bool IsFileValidForPeek(FAES_File faesFile)
        {
            if (faesFile.isFileDecryptable())
                return _supportedPeekFiles.Contains(Path.GetExtension(faesFile.GetOriginalFileName()).ToUpper());

            return false;
        }

        public static string[] ReadLaunchParams(bool local = false)
        {
            string dir;
            if (local)
                dir = Path.Combine(Directory.GetCurrentDirectory(), _localInstallPath);
            else
                dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _fullInstallPath);

            if (File.Exists(dir)) return File.ReadAllLines(dir);
            else return null;
        }

        public static string[] DumpInstallerOptions()
        {
            List<string> options = new List<string>();

            if (_associateFileTypes)
                options.Add("--associatefiletypes");
            if (_startMenuShortcuts)
                options.Add("--startmenushortcuts");
            if (_contextMenus)
                options.Add("--contextmenus");

            return options.ToArray();
        }

        public static string GetBranch()
        {
            return _branch;
        }

        public static bool GetCleanUpdates()
        {
            return _cleanUpdates;
        }

        public static bool GetFullInstall()
        {
            return _fullInstall;
        }

        public static bool GetSkipUpdate()
        {
            return _skipUpdate;
        }

        public static bool GetDeveloperMode()
        {
            return _debugMode;
        }
    }
}
