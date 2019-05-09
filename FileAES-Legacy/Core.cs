using FAES;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

class Core
{
    private const bool _flagIsDevBuild = true;
    private const string _copyrightInfo = "mullak99 © 2019";

    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

    private string buildHash()
    {
        return (buildDate.Year % 100).ToString() + (buildDate.Month).ToString("00") + (buildDate.Day).ToString("00") + (buildDate.Hour).ToString("00") + (buildDate.Minute).ToString("00");
    }

    private bool isDebugBuild()
    {
        return this.GetType().Assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Select(da => da.IsJITTrackingEnabled).FirstOrDefault();
    }

    public string getVersionInfo(bool raw = false, bool formatted = false, bool trimRev = true)
    {
        string version = Application.ProductVersion;

        if (trimRev)
        {
            string[] verSplit = version.Split('.');
            version = verSplit[0] + "." + verSplit[1] + "." + verSplit[2];
        }
        if (formatted)
        {
            if (isDebugBuild() || _flagIsDevBuild) return "v" + version + "-DEV" + buildHash();
            else return "v" + version;
        }
        if (!raw)
        {
            if (isDebugBuild() || _flagIsDevBuild) return "v" + version + "-DEV" + buildHash();
            else return "v" + version;
        }
        else return Application.ProductVersion;
    }

    public string getCopyrightInfo()
    {
        return _copyrightInfo;
    }

    public void setIgnoreUpdate(bool state)
    {
        string path = null;
        if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"))) path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg");
        else if (File.Exists(@"Config\launchParams.cfg")) path = @"Config\launchParams.cfg";
        if (path != null)
        {
            if (state)
            {
                string[] param = File.ReadAllLines(path);

                for (int i = 0; i < param.Length; i++)
                {
                    if (param[i] == "--skipupdate") return;
                }
                File.AppendAllText(path, "--skipupdate");
            }
            else
            {
                string[] param = File.ReadAllLines(path);

                for (int i = 0; i < param.Length; i++)
                {
                    if (param[i] == "--skipupdate")
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