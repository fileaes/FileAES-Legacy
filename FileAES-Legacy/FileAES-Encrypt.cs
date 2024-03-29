﻿using System;
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
        private readonly Core core = new Core();
        private readonly FileAES_Update update = new FileAES_Update();

        private bool _inProgress = false;
        private bool _encryptSuccessful;
        private readonly FAES_File _fileToEncrypt;
        private readonly string _autoPassword;
        private decimal _progress = 0;
        private bool _deleteOriginal, _overwriteDuplicate;
        private FAES.Packaging.Optimise _compressionMode;

        public FileAES_Encrypt(string file, string password = null)
        {
            if (!string.IsNullOrEmpty(file)) _fileToEncrypt = new FAES_File(file);
            else throw new ArgumentException("Parameter cannot be null", "file");
            InitializeComponent();
            versionLabel.Text = core.GetVersionInfo();
            copyrightLabel.Text = core.getCopyrightInfo();
            populateCompressionModes();
            if (Program.doEncryptFile) fileName.Text = _fileToEncrypt.GetFileName();
            else if (Program.doEncryptFolder) fileName.Text = _fileToEncrypt.GetFileName().TrimEnd(Path.DirectorySeparatorChar);
            Focus();
            ActiveControl = passwordInput;
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
            catch { }
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (encryptButton.Enabled)
            {
                _progress = 0;

                _compressionMode = FAES.Packaging.CompressionUtils.GetAllOptimiseModes()[compressMode.SelectedIndex];
                _deleteOriginal = deleteOriginal.Checked;
                _overwriteDuplicate = forceOverwrite.Checked;

                if (_fileToEncrypt.IsFileEncryptable() && !_inProgress && passwordInputConf.Text == passwordInput.Text) backgroundEncrypt.RunWorkerAsync();
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
                    FileAES_Utilities.LocalEncrypt = Program.UseLocalEncrypt();
                    FAES.FileAES_Encrypt encrypt = new FAES.FileAES_Encrypt(_fileToEncrypt, passwordInput.Text, hintInput.Text);

                    encrypt.SetCompressionMode(_compressionMode);
                    encrypt.SetDeleteAfterEncrypt(_deleteOriginal);
                    encrypt.SetOverwriteDuplicate(_overwriteDuplicate);

                    Thread eThread = new Thread(() =>
                    {
                        try
                        {
                            _encryptSuccessful = encrypt.EncryptFile();
                        }
                        catch (Exception e)
                        {
                            SetNote(FileAES_Utilities.FAES_ExceptionHandling(e), 3);
                        }
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
                deleteOriginal.Enabled = false;
                forceOverwrite.Enabled = false;

                if (_progress < 100)
                {
                    progressBar.CustomText = _progress > 0 ? "Encrypting" : "Compressing";
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
            else if (_fileToEncrypt.IsFileEncryptable() && passwordInput.Text.Length > 3 && passwordInputConf.Text.Length > 3 && !_inProgress)
            {
                encryptButton.Enabled = true;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;
                deleteOriginal.Enabled = true;
                forceOverwrite.Enabled = true;

            }
            else
            {
                encryptButton.Enabled = false;
                passwordInput.Enabled = true;
                passwordInputConf.Enabled = true;
                hintInput.Enabled = true;
                compressMode.Enabled = true;
                deleteOriginal.Enabled = true;
                forceOverwrite.Enabled = true;
            }
        }

        private void passwordBox_Focus(object sender, EventArgs e)
        {
            AcceptButton = encryptButton;
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
