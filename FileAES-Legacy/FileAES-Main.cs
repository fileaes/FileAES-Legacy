using FAES;
using System;
using System.IO;
using System.Windows.Forms;

namespace FileAES
{
    public partial class FileAES_Main : Form
    {
        private readonly Core core = new Core();
        private readonly FileAES_Update update = new FileAES_Update();

        public FileAES_Main()
        {
            InitializeComponent();
            versionLabel.Text = core.GetVersionInfo();
            copyrightLabel.Text = core.getCopyrightInfo();
        }

        private void FileAES_Main_Shown(object sender, EventArgs e)
        {
            update.CheckForUpdate();
        }

        private void FileAES_Main_Load(object sender, EventArgs e)
        { }

        private void encryptFileDrop_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void decryptFileDrop_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.All : DragDropEffects.None;
        }

        private void encryptFileDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) MessageBox.Show("You may only encrypt a single file or folder at a time.", "Multiple Files Unsupported");
            else
            {
                if (Directory.Exists(FileList[0])) Program.doEncryptFolder = true;
                else if (File.Exists(FileList[0])) Program.doEncryptFile = true;
                using (FileAES_Encrypt encrypt = new FileAES_Encrypt(FileList[0].Replace(@"\\", @"\")))
                {
                    encrypt.StartPosition = FormStartPosition.CenterParent;
                    encrypt.ShowDialog();
                    encrypt.Focus();
                }
            }
        }

        private void encryptFileDrop_Click(object sender, EventArgs e)
        {
            string fileToEncrypt = "";
            if (openFileToEncrypt.ShowDialog() == DialogResult.OK)
            {
                fileToEncrypt = openFileToEncrypt.FileName;
                if (Directory.Exists(fileToEncrypt)) Program.doEncryptFolder = true;
                else if (File.Exists(fileToEncrypt)) Program.doEncryptFile = true;
                using (FileAES_Encrypt encrypt = new FileAES_Encrypt(fileToEncrypt.Replace(@"\\", @"\")))
                {
                    encrypt.StartPosition = FormStartPosition.CenterParent;
                    encrypt.ShowDialog();
                    encrypt.Focus();
                }
            }
        }

        private void decryptFileDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (FileList.Length > 1) MessageBox.Show("You may only decrypt a single file at a time.", "Multiple Files Unsupported");
            else if (!FileAES_Utilities.IsFileDecryptable(FileList[0])) MessageBox.Show("Please select a valid filetype.", "Invalid Filetype");
            else
            {
                Program.doDecrypt = true;
                using (FileAES_Decrypt decrypt = new FileAES_Decrypt(FileList[0].Replace(@"\\", @"\")))
                {
                    decrypt.StartPosition = FormStartPosition.CenterParent;
                    decrypt.ShowDialog();
                    decrypt.Focus();
                }
            }
        }

        private void decryptFileDrop_Click(object sender, EventArgs e)
        {
            string fileToDecrypt = "";
            if (openFileToDecrypt.ShowDialog() == DialogResult.OK)
            {
                if (!FileAES_Utilities.IsFileDecryptable(openFileToDecrypt.FileName)) MessageBox.Show("Please select a valid filetype.", "Invalid Filetype");
                else
                {
                    fileToDecrypt = openFileToDecrypt.FileName;
                    Program.doDecrypt = true;
                    using (FileAES_Decrypt decrypt = new FileAES_Decrypt(fileToDecrypt.Replace(@"\\", @"\")))
                    {
                        decrypt.StartPosition = FormStartPosition.CenterParent;
                        decrypt.ShowDialog();
                        decrypt.Focus();
                    }
                }
            }
        }

        private void clearTempToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear the FileAES Temp Folder?\n\nDoing this while encrypting or decrypting another file can result in data loss!", "Clear FileAES Temp", MessageBoxButtons.YesNo) == DialogResult.Yes)
                FileAES_Utilities.PurgeTempFolder();
        }

        private void versionLabel_Click(object sender, EventArgs e)
        {
            update.Show();
        }
    }
}
