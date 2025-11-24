using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Properties;
using RecurringIntegrationsScheduler.Settings;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler.Forms
{
    public partial class ValidateSftpConnection : Form
    {
        public ValidateSftpConnection()
        {
            InitializeComponent();
        }

        public SftpServers Servers { get; set; }

        public SftpCredentials Credentials { get; set; }

        public SftpServer SelectedServer { get; set; }

        public SftpCredential SelectedCredential { get; set; }

        private void ValidateSftpConnection_Load(object sender, EventArgs e)
        {
            Text = Resources.Sftp_ValidateTitle;
            validateGroupBox.Text = Resources.Sftp_ValidateTitle;
            serverLabel.Text = Resources.Sftp_SelectServer;
            credentialLabel.Text = Resources.Sftp_SelectCredential;
            validateButton.Text = Resources.Validate;

            serverComboBox.DisplayMember = nameof(SftpServer.Name);
            credentialComboBox.DisplayMember = nameof(SftpCredential.Name);

            if (Servers != null)
            {
                serverComboBox.Items.AddRange(Servers.ToArray());
            }

            if (Credentials != null)
            {
                credentialComboBox.Items.AddRange(Credentials.ToArray());
            }

            SetComboSelection(serverComboBox, SelectedServer, s => s?.Name);
            SetComboSelection(credentialComboBox, SelectedCredential, c => c?.Name);
        }

        private static void SetComboSelection<T>(ComboBox comboBox, T preferred, Func<T, string> keySelector)
        {
            if (comboBox.Items.Count == 0)
            {
                return;
            }

            if (preferred != null)
            {
                var preferredKey = keySelector(preferred);
                if (!string.IsNullOrWhiteSpace(preferredKey))
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        if (comboBox.Items[i] is T candidate && string.Equals(keySelector(candidate), preferredKey, StringComparison.OrdinalIgnoreCase))
                        {
                            comboBox.SelectedIndex = i;
                            return;
                        }
                    }
                }
            }

            comboBox.SelectedIndex = 0;
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            messagesTextBox.Clear();

            if (!(serverComboBox.SelectedItem is SftpServer server) || !(credentialComboBox.SelectedItem is SftpCredential credential))
            {
                messagesTextBox.Text = Resources.Sftp_ValidationMissingData;
                return;
            }

            try
            {
                var password = string.IsNullOrEmpty(credential.Password) ? string.Empty : EncryptDecrypt.Decrypt(credential.Password);
                var passphrase = string.IsNullOrEmpty(credential.KeyPassphrase) ? string.Empty : EncryptDecrypt.Decrypt(credential.KeyPassphrase);

                var configuration = new SftpConfiguration(
                    server.Host?.Trim(),
                    server.Port,
                    credential.Username?.Trim(),
                    password,
                    credential.UsePrivateKey,
                    credential.KeyPath?.Trim(),
                    passphrase,
                    "/",
                    "*");

                SftpTransferHelper.ValidateConnection(configuration);
                messagesTextBox.Text = Resources.Sftp_ValidationSuccess;
            }
            catch (Exception ex)
            {
                messagesTextBox.Text = string.Format(CultureInfo.InvariantCulture, Resources.Sftp_ValidationFailed, ex.Message);
            }
        }
    }
}
