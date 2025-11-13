namespace RecurringIntegrationsScheduler.Forms
{
    partial class SftpCredentialForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.useKeyCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.keyPathLabel = new System.Windows.Forms.Label();
            this.keyPathPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.keyPathTextBox = new System.Windows.Forms.TextBox();
            this.keyBrowseButton = new System.Windows.Forms.Button();
            this.keyPassphraseLabel = new System.Windows.Forms.Label();
            this.keyPassphraseTextBox = new System.Windows.Forms.TextBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.layoutPanel.SuspendLayout();
            this.keyPathPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 2;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.nameLabel, 0, 0);
            this.layoutPanel.Controls.Add(this.nameTextBox, 1, 0);
            this.layoutPanel.Controls.Add(this.usernameLabel, 0, 1);
            this.layoutPanel.Controls.Add(this.usernameTextBox, 1, 1);
            this.layoutPanel.Controls.Add(this.useKeyCheckBox, 0, 2);
            this.layoutPanel.Controls.Add(this.passwordLabel, 0, 3);
            this.layoutPanel.Controls.Add(this.passwordTextBox, 1, 3);
            this.layoutPanel.Controls.Add(this.keyPathLabel, 0, 4);
            this.layoutPanel.Controls.Add(this.keyPathPanel, 1, 4);
            this.layoutPanel.Controls.Add(this.keyPassphraseLabel, 0, 5);
            this.layoutPanel.Controls.Add(this.keyPassphraseTextBox, 1, 5);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutPanel.Location = new System.Drawing.Point(10, 10);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 6;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.Size = new System.Drawing.Size(480, 265);
            this.layoutPanel.TabIndex = 0;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(134, 24);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameTextBox.Location = new System.Drawing.Point(143, 3);
            this.nameTextBox.MaxLength = 200;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(334, 26);
            this.nameTextBox.TabIndex = 1;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usernameLabel.Location = new System.Drawing.Point(3, 32);
            this.usernameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(134, 24);
            this.usernameLabel.TabIndex = 2;
            this.usernameLabel.Text = "Username:";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usernameTextBox.Location = new System.Drawing.Point(143, 35);
            this.usernameTextBox.MaxLength = 256;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(334, 26);
            this.usernameTextBox.TabIndex = 3;
            // 
            // useKeyCheckBox
            // 
            this.useKeyCheckBox.AutoSize = true;
            this.layoutPanel.SetColumnSpan(this.useKeyCheckBox, 2);
            this.useKeyCheckBox.Location = new System.Drawing.Point(3, 69);
            this.useKeyCheckBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 12);
            this.useKeyCheckBox.Name = "useKeyCheckBox";
            this.useKeyCheckBox.Size = new System.Drawing.Size(143, 24);
            this.useKeyCheckBox.TabIndex = 4;
            this.useKeyCheckBox.Text = global::RecurringIntegrationsScheduler.Properties.Resources.Sftp_UsePrivateKey;
            this.useKeyCheckBox.UseVisualStyleBackColor = true;
            this.useKeyCheckBox.CheckedChanged += new System.EventHandler(this.UseKeyCheckBox_CheckedChanged);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordLabel.Location = new System.Drawing.Point(3, 105);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(134, 24);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password:";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passwordTextBox.Location = new System.Drawing.Point(143, 108);
            this.passwordTextBox.MaxLength = 512;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(334, 26);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // keyPathLabel
            // 
            this.keyPathLabel.AutoSize = true;
            this.keyPathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyPathLabel.Location = new System.Drawing.Point(3, 137);
            this.keyPathLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.keyPathLabel.Name = "keyPathLabel";
            this.keyPathLabel.Size = new System.Drawing.Size(134, 76);
            this.keyPathLabel.TabIndex = 7;
            this.keyPathLabel.Text = "Key file:";
            this.keyPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // keyPathPanel
            // 
            this.keyPathPanel.AutoSize = true;
            this.keyPathPanel.Controls.Add(this.keyPathTextBox);
            this.keyPathPanel.Controls.Add(this.keyBrowseButton);
            this.keyPathPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyPathPanel.Location = new System.Drawing.Point(143, 140);
            this.keyPathPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.keyPathPanel.Name = "keyPathPanel";
            this.keyPathPanel.Size = new System.Drawing.Size(334, 73);
            this.keyPathPanel.TabIndex = 8;
            // 
            // keyPathTextBox
            // 
            this.keyPathTextBox.Location = new System.Drawing.Point(3, 3);
            this.keyPathTextBox.MaxLength = 1024;
            this.keyPathTextBox.Name = "keyPathTextBox";
            this.keyPathTextBox.Size = new System.Drawing.Size(246, 26);
            this.keyPathTextBox.TabIndex = 0;
            // 
            // keyBrowseButton
            // 
            this.keyBrowseButton.AutoSize = true;
            this.keyBrowseButton.Location = new System.Drawing.Point(3, 35);
            this.keyBrowseButton.Name = "keyBrowseButton";
            this.keyBrowseButton.Size = new System.Drawing.Size(84, 35);
            this.keyBrowseButton.TabIndex = 1;
            this.keyBrowseButton.Text = global::RecurringIntegrationsScheduler.Properties.Resources.Sftp_BrowseButton;
            this.keyBrowseButton.UseVisualStyleBackColor = true;
            this.keyBrowseButton.Click += new System.EventHandler(this.KeyBrowseButton_Click);
            // 
            // keyPassphraseLabel
            // 
            this.keyPassphraseLabel.AutoSize = true;
            this.keyPassphraseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyPassphraseLabel.Location = new System.Drawing.Point(3, 221);
            this.keyPassphraseLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.keyPassphraseLabel.Name = "keyPassphraseLabel";
            this.keyPassphraseLabel.Size = new System.Drawing.Size(134, 36);
            this.keyPassphraseLabel.TabIndex = 9;
            this.keyPassphraseLabel.Text = "Key passphrase:";
            this.keyPassphraseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // keyPassphraseTextBox
            // 
            this.keyPassphraseTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyPassphraseTextBox.Location = new System.Drawing.Point(143, 224);
            this.keyPassphraseTextBox.MaxLength = 512;
            this.keyPassphraseTextBox.Name = "keyPassphraseTextBox";
            this.keyPassphraseTextBox.Size = new System.Drawing.Size(334, 26);
            this.keyPassphraseTextBox.TabIndex = 10;
            this.keyPassphraseTextBox.UseSystemPasswordChar = true;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.AutoSize = true;
            this.buttonPanel.Controls.Add(this.okButton);
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(250, 284);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(240, 44);
            this.buttonPanel.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(121, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(116, 38);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(3, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 38);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = global::RecurringIntegrationsScheduler.Properties.Resources.Sftp_KeyFileDialogFilter;
            this.openFileDialog.Title = global::RecurringIntegrationsScheduler.Properties.Resources.Sftp_KeyFileDialogTitle;
            // 
            // SftpCredentialForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(500, 338);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.layoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SftpCredentialForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SFTP Credential";
            this.Load += new System.EventHandler(this.SftpCredentialForm_Load);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.keyPathPanel.ResumeLayout(false);
            this.keyPathPanel.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.CheckBox useKeyCheckBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label keyPathLabel;
        private System.Windows.Forms.FlowLayoutPanel keyPathPanel;
        private System.Windows.Forms.TextBox keyPathTextBox;
        private System.Windows.Forms.Button keyBrowseButton;
        private System.Windows.Forms.Label keyPassphraseLabel;
        private System.Windows.Forms.TextBox keyPassphraseTextBox;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
