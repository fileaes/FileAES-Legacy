namespace FileAES
{
    partial class FileAES_Update
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileAES_Update));
            this.descLabel = new System.Windows.Forms.Label();
            this.currentVerLabel = new System.Windows.Forms.Label();
            this.latestVerLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.neverRemindButton = new System.Windows.Forms.Button();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.checkForUpdateButton = new System.Windows.Forms.Button();
            this.FaesVersion = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadLatestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spoofVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spoofV110ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spoofV999999ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runtime = new System.Windows.Forms.Timer(this.components);
            this.downloadLatestButton = new System.Windows.Forms.Button();
            this.spoofV132ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // descLabel
            // 
            this.descLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descLabel.Location = new System.Drawing.Point(12, 30);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(246, 49);
            this.descLabel.TabIndex = 0;
            this.descLabel.Text = "An update is available!";
            this.descLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // currentVerLabel
            // 
            this.currentVerLabel.Location = new System.Drawing.Point(13, 67);
            this.currentVerLabel.Name = "currentVerLabel";
            this.currentVerLabel.Size = new System.Drawing.Size(246, 15);
            this.currentVerLabel.TabIndex = 1;
            this.currentVerLabel.Text = "Current Version: v0.0.0.0";
            // 
            // latestVerLabel
            // 
            this.latestVerLabel.Location = new System.Drawing.Point(18, 87);
            this.latestVerLabel.Name = "latestVerLabel";
            this.latestVerLabel.Size = new System.Drawing.Size(240, 13);
            this.latestVerLabel.TabIndex = 2;
            this.latestVerLabel.Text = "Latest Version: v0.0.0.0";
            // 
            // updateButton
            // 
            this.updateButton.BackColor = System.Drawing.Color.Lime;
            this.updateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.updateButton.Location = new System.Drawing.Point(12, 128);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(119, 40);
            this.updateButton.TabIndex = 3;
            this.updateButton.Text = "Update Now";
            this.updateButton.UseVisualStyleBackColor = false;
            this.updateButton.Visible = false;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // neverRemindButton
            // 
            this.neverRemindButton.BackColor = System.Drawing.Color.Red;
            this.neverRemindButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.neverRemindButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.neverRemindButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.neverRemindButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.neverRemindButton.Location = new System.Drawing.Point(140, 128);
            this.neverRemindButton.Name = "neverRemindButton";
            this.neverRemindButton.Size = new System.Drawing.Size(119, 40);
            this.neverRemindButton.TabIndex = 5;
            this.neverRemindButton.Text = "Never Remind Me";
            this.neverRemindButton.UseVisualStyleBackColor = false;
            this.neverRemindButton.Visible = false;
            this.neverRemindButton.Click += new System.EventHandler(this.neverRemindButton_Click);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(184, 217);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(88, 13);
            this.copyrightLabel.TabIndex = 20;
            this.copyrightLabel.Text = "mullak99 © 2018";
            // 
            // checkForUpdateButton
            // 
            this.checkForUpdateButton.BackColor = System.Drawing.Color.Cyan;
            this.checkForUpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.checkForUpdateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkForUpdateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.checkForUpdateButton.Location = new System.Drawing.Point(12, 174);
            this.checkForUpdateButton.Name = "checkForUpdateButton";
            this.checkForUpdateButton.Size = new System.Drawing.Size(247, 40);
            this.checkForUpdateButton.TabIndex = 21;
            this.checkForUpdateButton.Text = "Check for Update";
            this.checkForUpdateButton.UseVisualStyleBackColor = false;
            this.checkForUpdateButton.Click += new System.EventHandler(this.checkForUpdateButton_Click);
            // 
            // FaesVersion
            // 
            this.FaesVersion.Location = new System.Drawing.Point(20, 107);
            this.FaesVersion.Name = "FaesVersion";
            this.FaesVersion.Size = new System.Drawing.Size(238, 13);
            this.FaesVersion.TabIndex = 22;
            this.FaesVersion.Text = "FAES Version: v0.0.0.0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.devToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(271, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdateToolStripMenuItem,
            this.downloadLatestToolStripMenuItem});
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // downloadLatestToolStripMenuItem
            // 
            this.downloadLatestToolStripMenuItem.Name = "downloadLatestToolStripMenuItem";
            this.downloadLatestToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downloadLatestToolStripMenuItem.Text = "Download Latest";
            this.downloadLatestToolStripMenuItem.Click += new System.EventHandler(this.downloadLatestToolStripMenuItem_Click);
            // 
            // devToolStripMenuItem
            // 
            this.devToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spoofVersionToolStripMenuItem});
            this.devToolStripMenuItem.Name = "devToolStripMenuItem";
            this.devToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.devToolStripMenuItem.Text = "Developer";
            this.devToolStripMenuItem.Visible = false;
            // 
            // spoofVersionToolStripMenuItem
            // 
            this.spoofVersionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spoofV110ToolStripMenuItem,
            this.spoofV999999ToolStripMenuItem,
            this.spoofV132ToolStripMenuItem,
            this.resetToolStripMenuItem});
            this.spoofVersionToolStripMenuItem.Name = "spoofVersionToolStripMenuItem";
            this.spoofVersionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.spoofVersionToolStripMenuItem.Text = "Spoof Version";
            // 
            // spoofV1100ToolStripMenuItem
            // 
            this.spoofV110ToolStripMenuItem.Name = "spoofV1100ToolStripMenuItem";
            this.spoofV110ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.spoofV110ToolStripMenuItem.Text = "Spoof v1.1.0";
            this.spoofV110ToolStripMenuItem.Click += new System.EventHandler(this.spoofV110ToolStripMenuItem_Click);
            // 
            // spoofV9999990ToolStripMenuItem
            // 
            this.spoofV999999ToolStripMenuItem.Name = "spoofV9999990ToolStripMenuItem";
            this.spoofV999999ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.spoofV999999ToolStripMenuItem.Text = "Spoof v99.99.99";
            this.spoofV999999ToolStripMenuItem.Click += new System.EventHandler(this.spoofV999999ToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // runtime
            // 
            this.runtime.Enabled = true;
            this.runtime.Tick += new System.EventHandler(this.runtime_Tick);
            // 
            // downloadLatestButton
            // 
            this.downloadLatestButton.BackColor = System.Drawing.Color.Gold;
            this.downloadLatestButton.Enabled = false;
            this.downloadLatestButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.downloadLatestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadLatestButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.downloadLatestButton.Location = new System.Drawing.Point(12, 128);
            this.downloadLatestButton.Name = "downloadLatestButton";
            this.downloadLatestButton.Size = new System.Drawing.Size(247, 40);
            this.downloadLatestButton.TabIndex = 24;
            this.downloadLatestButton.Text = "Download Latest Anyway";
            this.downloadLatestButton.UseVisualStyleBackColor = false;
            this.downloadLatestButton.Visible = false;
            this.downloadLatestButton.Click += new System.EventHandler(this.downloadLatestButton_Click);
            // 
            // spoofV132ToolStripMenuItem
            // 
            this.spoofV132ToolStripMenuItem.Name = "spoofV132ToolStripMenuItem";
            this.spoofV132ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.spoofV132ToolStripMenuItem.Text = "Spoof v1.3.2";
            this.spoofV132ToolStripMenuItem.Click += new System.EventHandler(this.spoofV132ToolStripMenuItem_Click);
            // 
            // FileAES_Update
            // 
            this.AcceptButton = this.updateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.neverRemindButton;
            this.ClientSize = new System.Drawing.Size(271, 231);
            this.Controls.Add(this.downloadLatestButton);
            this.Controls.Add(this.FaesVersion);
            this.Controls.Add(this.checkForUpdateButton);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.neverRemindButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.latestVerLabel);
            this.Controls.Add(this.currentVerLabel);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FileAES_Update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FileAES: Update";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label currentVerLabel;
        private System.Windows.Forms.Label latestVerLabel;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button neverRemindButton;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Button checkForUpdateButton;
        private System.Windows.Forms.Label FaesVersion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem devToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spoofVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spoofV110ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spoofV999999ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Timer runtime;
        private System.Windows.Forms.Button downloadLatestButton;
        private System.Windows.Forms.ToolStripMenuItem downloadLatestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spoofV132ToolStripMenuItem;
    }
}