using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace FileAES
{
    internal class Core
    {
        private const bool _flagIsDevBuild = false;
        private const bool _flagIsBetaBuild = true;
        private const string _betaBuildTag = "RC3";
        private const string _copyrightInfo = "mullak99 © 2022";

        private bool _versionSpoof = false;
        private string _spoofedVersion = "1.1.0.0";

        private static DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

        public static DateTime GetBuildDate()
        {
            return buildDate;
        }

        public static string GetBuildDateFormatted()
        {
            return GetBuildDate().ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        private string GetBuildHash()
        {
            return (buildDate.Year % 100).ToString() + (buildDate.Month).ToString("00") +
                   (buildDate.Day).ToString("00") + (buildDate.Hour).ToString("00") + (buildDate.Minute).ToString("00");
        }

        public string GetVersionInfo(bool raw = false, bool formatted = false, bool trimRev = true)
        {
            string version = _versionSpoof ? _spoofedVersion : Application.ProductVersion;

            if (trimRev)
            {
                string[] verSplit = version.Split('.');
                version = verSplit[0] + "." + verSplit[1] + "." + verSplit[2];
            }

            if (formatted)
            {
                if (_flagIsDevBuild && !_versionSpoof) return "v" + version + "-DEV" + GetBuildHash();
                if (_flagIsBetaBuild && !string.IsNullOrWhiteSpace(_betaBuildTag) && !_versionSpoof)
                    return "v" + version + "-" + _betaBuildTag;
                return "v" + version;
            }

            if (!raw)
            {
                if (_flagIsDevBuild && !_versionSpoof) return "v" + version + "-DEV" + GetBuildHash();
                if (_flagIsBetaBuild && !string.IsNullOrWhiteSpace(_betaBuildTag) && !_versionSpoof)
                    return "v" + version + "-" + _betaBuildTag;
                return "v" + version;
            }

            return version;
        }

        public bool IsStableBuild()
        {
            return (!_flagIsDevBuild && !_flagIsBetaBuild);
        }

        public bool IsBetaBuild()
        {
            return (!_flagIsDevBuild && _flagIsBetaBuild);
        }

        public bool IsDevBuild()
        {
            return _flagIsDevBuild;
        }

        public string GetBuild()
        {
            if (IsDevBuild()) return "dev";
            if (IsBetaBuild()) return "beta";
            return "stable";
        }

        public bool SetSpoofVersion(bool spoofVersion, string version = "1.0.0.0")
        {
            _versionSpoof = spoofVersion;

            if (_versionSpoof) _spoofedVersion = version;
            return _versionSpoof;
        }

        public string getCopyrightInfo()
        {
            return _copyrightInfo;
        }

        public void setIgnoreUpdate(bool state)
        {
            string path = null;
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    @"mullak99\FileAES-Legacy\config\launchParams.cfg")))
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    @"mullak99\FileAES-Legacy\config\launchParams.cfg");
            else if (File.Exists(@"Config\launchParams.cfg")) path = @"Config\launchParams.cfg";
            if (path != null)
            {
                if (state)
                {
                    string[] paramArray = File.ReadAllLines(path);

                    foreach (var param in paramArray)
                    {
                        if (param == "--skipupdate") return;
                    }

                    File.AppendAllText(path, "--skipupdate");
                }
                else
                {
                    string[] paramArray = File.ReadAllLines(path);

                    foreach (var param in paramArray)
                    {
                        if (param == "--skipupdate")
                        {
                            string text = File.ReadAllText(path);
                            text = text.Replace("--skipupdate", "");
                            File.WriteAllText(path, text);
                        }
                    }
                }
            }
        }
    }
}