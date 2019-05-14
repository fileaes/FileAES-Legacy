using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FAES;

namespace FileAES
{
    public partial class FileAES_Encrypt : Form
    {
        Core core = new Core();
        FileAES_Update update = new FileAES_Update();

        private bool _inProgress = false;
        private bool _encryptSuccessful;
        private FAES_File _fileToEncrypt;
        private string _autoPassword;
        private decimal _progress = 0;

        public FileAES_Encrypt(string file, string password = null)
        {
            if (!String.IsNullOrEmpty(file)) _fileToEncrypt = new FAES_File(file);
            else throw new ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.GetVersionInfo();
            copyrightLabel.Text = core.getCopyrightInfo();
            populateCompressionModes();
            if (Program.doEncryptFile) fileName.Text = _fileToEncrypt.getFileName();
            else if (Program.doEncryptFolder) fileName.Text = _fileToEncrypt.getFileName().TrimEnd(Path.DirectorySeparatorChar);
            this.Focus();
            this.ActiveControl = passwordInput;
            _autoPassword = password;

            progressBar.CustomText = "";
            progressBar.VisualMode = ProgressBarDisplayMode.Percentage;
        }

        private void FileAES_Encrypt_Shown(object sender, EventArgs e)
        {
            update.CheckForUpdate();
        }

        private void FileAES_Encrypt_Load(object sender, EventArgs e)
        {
            if (_autoPassword != null && _autoPassword.Length > 3)
            {
                passwordInput.Text = _autoPassword;
                passwordInputConf.Text = _autoPassword;
                runtime_Tick(null, null);
                encryptButton_Click(null, null);
            }
        }

        private void SetNote(string note, int severity)
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

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (encryptButton.Enabled)
            {
                _progress = 0;
                if (_fileToEncrypt.isFileEncryptable() && !_inProgress && passwordInputConf.Text == passwordInput.Text) backgroundEncrypt.RunWorkerAsync();
                else if (passwordInputConf.Text != passwordInput.Text) SetNote("Passwords do not match!", 2);
                else if (_inProgress) SetNote("Encryption already in progress.", 1);
                else SetNote("Encryption Failed. Try again later.", 1);

                runtime_Tick(sender, e);
            }
        }

        private void Encrypt()
        {
            try
            {
                SetNote("Encrypting... Please wait.", 0);

                _inProgress = true;
                _encryptSuccessful = false;

                while (!backgroundEncrypt.CancellationPending)
                {
                    FAES.FileAES_Encrypt encrypt = new FAES.FileAES_Encrypt(_fileToEncrypt, passwordInput.Text, hintInput.Text);

                    encrypt.SetCompressionMode(FAES.Packaging.CompressionUtils.GetAllOptimiseModes()[compressMode.SelectedIndex]);

                    Thread eThread = new Thread(() =>
                    {
                        _encryptSuccessful = encrypt.encryptFile();
                    });
                    eThread.Start();

                    while (eThread.ThreadState == ThreadState.Running)
                    {
                        _progress = encrypt.GetEncryptionPercentComplete();
                    }

                    backgroundEncrypt.CancelAsync();
                }
            }
            catch (Exception e)
            {
                SetNote(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
            }
        }

        private void populateCompressionModes()
        {
            List<string> optimiseModes = FAES.Packaging.CompressionUtils.GetAllOptimiseModesAsStrings();

            compressMode.Items.Clear();
            
            foreach (string mode in optimiseModes)
            {
                compressMode.Items.Add(mode.Replace("_", " "));
            }

            compressMode.SelectedIndex = 0;
        }

        private void backgroundEncrypt_DoWork(object sender, DoWorkEventArgs e)
        {
            Encrypt();
        }

        private void backgroundEncrypt_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            _inProgress = false;

            if (_encryptSuccessful)
            {
                SetNote("Done!", 0);
                Application.Exit();
            }
        }

        private void runtime_Tick(object sender, EventArgs e)
        {
            if (_inProgress)
            {
                encryptButton.Enabled = false;
                passwordInput.Enabled = false;
                passwordInputConf.Enabled = false;
                hintInput.Enabled = false;
                compressMode.Enabled = false;

                if (_progress < 100)
                {
                    if (_progress > 0) progressBar.CustomText = "Encrypting";
                    else progressBar.CustomText = "Compressing";
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
            else if (_fileToEncrypt.isFileEncryptable() && passwordInput.Text.Length > 3 && passwordInputConf.Text.Length > 3 && !_inProgress)
            {
                encryptButton.Enabled = true;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;

            }
            else
            {
                encryptButton.Enabled = false;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;
            }
        }

        private void passwordBox_Focus(object sender, EventArgs e)
        {
            this.AcceptButton = encryptButton;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            if (e.CloseReason == CloseReason.ApplicationExitCall) return;

            if (_inProgress)
            {
                if (MessageBox.Show(this, "Are you sure you want to stop encrypting?", "Closing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundEncrypt.CancelAsync();
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
                update.Dispose();
            }
            else FileAES_Utilities.PurgeInstancedTempFolders();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }

        private void hintInput_TextChanged(object sender, EventArgs e)
        {
            hintInput.Text = hintInput.Text.Replace(Environment.NewLine, "");
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
