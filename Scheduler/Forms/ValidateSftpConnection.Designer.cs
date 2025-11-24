namespace RecurringIntegrationsScheduler.Forms
{
    partial class ValidateSftpConnection
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

        private void InitializeComponent()
        {
            this.validateGroupBox = new System.Windows.Forms.GroupBox();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverComboBox = new System.Windows.Forms.ComboBox();
            this.credentialLabel = new System.Windows.Forms.Label();
            this.credentialComboBox = new System.Windows.Forms.ComboBox();
            this.validateButton = new System.Windows.Forms.Button();
            this.messagesTextBox = new System.Windows.Forms.TextBox();
            this.validateGroupBox.SuspendLayout();
            this.layoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // validateGroupBox
            // 
            this.validateGroupBox.Controls.Add(this.layoutPanel);
            this.validateGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.validateGroupBox.Location = new System.Drawing.Point(0, 0);
            this.validateGroupBox.Name = "validateGroupBox";
            this.validateGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.validateGroupBox.Size = new System.Drawing.Size(684, 361);
            this.validateGroupBox.TabIndex = 0;
            this.validateGroupBox.TabStop = false;
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 2;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.serverLabel, 0, 0);
            this.layoutPanel.Controls.Add(this.serverComboBox, 1, 0);
            this.layoutPanel.Controls.Add(this.credentialLabel, 0, 1);
            this.layoutPanel.Controls.Add(this.credentialComboBox, 1, 1);
            this.layoutPanel.Controls.Add(this.validateButton, 1, 2);
            this.layoutPanel.Controls.Add(this.messagesTextBox, 0, 3);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(10, 29);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.layoutPanel.RowCount = 4;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Size = new System.Drawing.Size(664, 322);
            this.layoutPanel.TabIndex = 0;
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverLabel.Location = new System.Drawing.Point(3, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(174, 35);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Select SFTP server";
            this.serverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serverComboBox
            // 
            this.serverComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverComboBox.FormattingEnabled = true;
            this.serverComboBox.Location = new System.Drawing.Point(183, 3);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.Size = new System.Drawing.Size(478, 33);
            this.serverComboBox.TabIndex = 1;
            // 
            // credentialLabel
            // 
            this.credentialLabel.AutoSize = true;
            this.credentialLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.credentialLabel.Location = new System.Drawing.Point(3, 35);
            this.credentialLabel.Name = "credentialLabel";
            this.credentialLabel.Size = new System.Drawing.Size(174, 35);
            this.credentialLabel.TabIndex = 2;
            this.credentialLabel.Text = "Select credentials";
            this.credentialLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // credentialComboBox
            // 
            this.credentialComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.credentialComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.credentialComboBox.FormattingEnabled = true;
            this.credentialComboBox.Location = new System.Drawing.Point(183, 38);
            this.credentialComboBox.Name = "credentialComboBox";
            this.credentialComboBox.Size = new System.Drawing.Size(478, 33);
            this.credentialComboBox.TabIndex = 3;
            // 
            // validateButton
            // 
            this.validateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validateButton.Location = new System.Drawing.Point(526, 76);
            this.validateButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 8);
            this.validateButton.Name = "validateButton";
            this.validateButton.Size = new System.Drawing.Size(135, 36);
            this.validateButton.TabIndex = 4;
            this.validateButton.Text = "Validate";
            this.validateButton.UseVisualStyleBackColor = true;
            this.validateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // messagesTextBox
            // 
            this.layoutPanel.SetColumnSpan(this.messagesTextBox, 2);
            this.messagesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesTextBox.Location = new System.Drawing.Point(3, 118);
            this.messagesTextBox.Multiline = true;
            this.messagesTextBox.Name = "messagesTextBox";
            this.messagesTextBox.ReadOnly = true;
            this.messagesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messagesTextBox.Size = new System.Drawing.Size(658, 191);
            this.messagesTextBox.TabIndex = 5;
            // 
            // ValidateSftpConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.validateGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.AcceptButton = this.validateButton;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidateSftpConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Validate SFTP connection";
            this.Load += new System.EventHandler(this.ValidateSftpConnection_Load);
            this.validateGroupBox.ResumeLayout(false);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox validateGroupBox;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.ComboBox serverComboBox;
        private System.Windows.Forms.Label credentialLabel;
        private System.Windows.Forms.ComboBox credentialComboBox;
        private System.Windows.Forms.Button validateButton;
        private System.Windows.Forms.TextBox messagesTextBox;
    }
}
