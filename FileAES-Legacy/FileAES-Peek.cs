using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FAES;

namespace FileAES
{
    public partial class FileAES_Peek : Form
    {
        private readonly Core core = new Core();
        private readonly FileAES_Update update = new FileAES_Update();

        private bool _inProgress = false;
        private bool _decryptSuccessful;
        private readonly FAES_File _fileToPeek;
        private readonly string _autoPassword;
        private string _pathOverride, _finalPath;
        private decimal _progress = 0;

        public FileAES_Peek(string file, string password = null)
        {
            if (!string.IsNullOrEmpty(file)) _fileToPeek = new FAES_File(file);
            else throw new ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.GetVersionInfo();
            copyrightLabel.Text = core.getCopyrightInfo();
            if (Program.doDecrypt) fileName.Text = _fileToPeek.GetFileName();
            Focus();
            ActiveControl = passwordInput;
            _autoPassword = password;

            progressBar.CustomText = "";
            progressBar.VisualMode = ProgressBarDisplayMode.Percentage;
        }

        private void FileAES_Decrypt_Shown(object sender, EventArgs e)
        {
            update.CheckForUpdate();
        }

        private void FileAES_Decrypt_Load(object sender, EventArgs e)
        {
            hintTextbox.Text = _fileToPeek.GetPasswordHint();

            if (_autoPassword != null && _autoPassword.Length > 3)
            {
                passwordInput.Text = _autoPassword;
                runtime_Tick(null, null);
                decryptButton_Click(null, null);
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            if (decryptButton.Enabled)
            {
                _progress = 0;

                if (_fileToPeek.IsFileDecryptable() && !_inProgress) backgroundDecrypt.RunWorkerAsync();
                else if (_inProgress) SetNote("Decryption already in progress.", 1);
                else SetNote("Decryption Failed. Try again later.", 1);

                runtime_Tick(sender, e);
            }
        }

        private void SetNote(string note, int severity)
        {
            try
            {
                if (!noteLabel.IsDisposed)
                {
                    if (note.Contains("ERROR:"))
                    {
                        note = note.Replace("ERROR:", "Error:");
                        noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = note; }));
                    }
                    else
                    {
                        switch (severity)
                        {
                            case 1:
                                noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Warning: " + note; }));
                                break;
                            case 2:
                                noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Important: " + note; }));
                                break;
                            case 3:
                                noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Error: " + note; }));
                                break;
                            default:
                                noteLabel.Invoke(new MethodInvoker(delegate { this.noteLabel.Text = "Note: " + note; }));
                                break;
                        }
                    }
                }
            }
            catch
            { }

        }

        private void Decrypt()
        {
            try
            {
                SetNote("Decrypting... Please wait.", 0);

                _inProgress = true;
                _decryptSuccessful = false;

                while (!backgroundDecrypt.CancellationPending)
                {
                    FAES.FileAES_Decrypt decrypt = new FAES.FileAES_Decrypt(_fileToPeek, passwordInput.Text, false, true);

                    _pathOverride = Path.Combine(Path.GetDirectoryName(_fileToPeek.GetPath()), "faesPeekFilePath_" + new Random().Next(), "peekFile" + FileAES_Utilities.ExtentionUFAES);
                    string dirName = Path.GetDirectoryName(_pathOverride);
                    _finalPath = Path.Combine(dirName, _fileToPeek.GetOriginalFileName());

                    DirectoryInfo di = Directory.CreateDirectory(dirName);
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                    Thread dThread = new Thread(() =>
                    {
                        try
                        {
                            _decryptSuccessful = decrypt.DecryptFile(_pathOverride);
                        }
                        catch (Exception e)
                        {
                            SetNote(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
                        }
                    });
                    dThread.Start();

                    while (dThread.ThreadState == ThreadState.Running)
                    {
                        _progress = decrypt.GetDecryptionPercentComplete();
                    }

                    backgroundDecrypt.CancelAsync();
                }
            }
            catch (Exception e)
            {
                SetNote(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
            }
        }

        private void backgroundDecrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            Decrypt();
        }

        private void backgroundDecrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;
            if (_decryptSuccessful)
            {
                SetNote("Decryption Complete", 0);
                progressBar.CustomText = "Done";
                progressBar.VisualMode = ProgressBarDisplayMode.TextAndPercentage;

                new PeekResult(File.ReadAllText(_finalPath)).ShowDialog(this);

                Delete(_pathOverride);
            }
            else if (!noteLabel.Text.ToLower().Contains("error"))
            {
                SetNote("Password Incorrect!", 3);
                progressBar.CustomText = "Password Incorrect!";
                progressBar.VisualMode = ProgressBarDisplayMode.TextAndPercentage;
            }
        }

        private void Delete(string pathToDelete)
        {
            if (Directory.Exists(Path.GetDirectoryName(pathToDelete)))
                Directory.Delete(Path.GetDirectoryName(pathToDelete), true);
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (_inProgress)
            {
                decryptButton.Enabled = false;
                passwordInput.Enabled = false;

                if (_progress < 100)
                {
                    progressBar.CustomText = _progress < 99 ? "Decrypting" : "Uncompressing";
                    progressBar.VisualMode = ProgressBarDisplayMode.TextAndPercentage;
                    progressBar.Value = Convert.ToInt32(Math.Ceiling(_progress));
                }
                else
                {
                    progressBar.CustomText = "Finishing";
                    progressBar.VisualMode = ProgressBarDisplayMode.TextAndPercentage;
                    progressBar.Value = 100;
                }
            }
            else if (_fileToPeek.IsFileDecryptable() && passwordInput.Text.Length > 3 && !_inProgress)
            {
                decryptButton.Enabled = true;
                passwordInput.Enabled = true;

            }
            else
            {
                decryptButton.Enabled = false;
                passwordInput.Enabled = true;
            }
        }

        private void passwordBox_Focus(object sender, EventArgs e)
        {
            AcceptButton = decryptButton;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            switch (e.CloseReason)
            {
                case CloseReason.WindowsShutDown:
                case CloseReason.ApplicationExitCall:
                    return;
            }

            if (_inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop decrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundDecrypt.CancelAsync();
                    try
                    {
                        FileAES_Utilities.PurgeInstancedTempFolders();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("This action is currently unsupported!", "Error");
                        e.Cancel = true;
                    }
                }
                else e.Cancel = true;
            }
            update.Dispose();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }

        private void noteLabel_MouseHover(object sender, EventArgs e)
        {
            slowToolTip.SetToolTip(noteLabel, noteLabel.Text);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
                Application.Exit();

            return base.ProcessDialogKey(keyData);
        }
    }
}
