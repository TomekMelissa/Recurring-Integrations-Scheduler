using Quartz;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Controls;
using RecurringIntegrationsScheduler.Properties;
using RecurringIntegrationsScheduler.Settings;
using System;
using System.Linq;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler
{
    internal class FormsHelper
    {
        public static int DropDownWidth(ComboBox myCombo)
        {
            int minWidth = 1;
            foreach (var item in myCombo.Items)
            {
                int temp = TextRenderer.MeasureText(item.ToString(), myCombo.Font).Width;
                if (temp > minWidth)
                {
                    minWidth = temp;
                }
            }
            return minWidth;
        }

        public static DateTimeOffset? GetScheduleForCron(string cronexpression, DateTimeOffset date)
        {
            try
            {
                var cron = new CronExpression(cronexpression);
                return cron.GetNextValidTimeAfter(date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Cron_expression_is_invalid);
                return DateTimeOffset.MinValue;
            }
        }

        public static void SetDropDownsWidth(Control parentCtrl)
        {
            parentCtrl.Controls
                .OfType<ComboBox>()
                .ToList()
                .ForEach(t => t.DropDownWidth = DropDownWidth(t));

            foreach (Control c in parentCtrl.Controls)
            {
                SetDropDownsWidth(c);
            }
        }

        public static void TrimTextBoxes(Control parentCtrl)
        {
            parentCtrl.Controls //Trim all textboxes
                .OfType<TextBox>()
                .ToList()
                .ForEach(t => t.Text = t.Text.Trim());

            foreach (Control c in parentCtrl.Controls)
            {
                TrimTextBoxes(c);
            }
        }

        public static SftpSettingsControl AddSftpTab(TabControl tabControl, SftpMode mode, string tabTitle = null)
        {
            if (tabControl == null)
            {
                throw new ArgumentNullException(nameof(tabControl));
            }

            if (Properties.Settings.Default.SftpServers == null)
            {
                Properties.Settings.Default.SftpServers = new SftpServers();
            }

            if (Properties.Settings.Default.SftpCredentials == null)
            {
                Properties.Settings.Default.SftpCredentials = new SftpCredentials();
            }

            var control = new SftpSettingsControl();
            control.BindData(Properties.Settings.Default.SftpServers, Properties.Settings.Default.SftpCredentials);
            control.SetMode(mode);
            control.Dock = DockStyle.Fill;
            var resolvedTitle = string.IsNullOrWhiteSpace(tabTitle) ? Resources.Sftp_TabTitle : tabTitle;
            var tabPage = new TabPage(resolvedTitle);
            tabPage.Controls.Add(control);
            tabControl.TabPages.Add(tabPage);
            return control;
        }

        public static SftpServer FindSftpServer(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return null;
            }

            var servers = Properties.Settings.Default.SftpServers;
            if (servers == null || servers.Count == 0)
            {
                return null;
            }

            var normalizedHost = host.Trim();
            var normalizedPort = port > 0 ? port : 22;

            return servers.FirstOrDefault(server =>
                server != null &&
                string.Equals(server.Host, normalizedHost, StringComparison.OrdinalIgnoreCase) &&
                (server.Port > 0 ? server.Port : 22) == normalizedPort);
        }

        public static SftpCredential FindSftpCredential(string username, bool usePrivateKey, string encryptedPassword, string keyPath, string encryptedPassphrase)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var credentials = Properties.Settings.Default.SftpCredentials;
            if (credentials == null || credentials.Count == 0)
            {
                return null;
            }

            var password = string.IsNullOrWhiteSpace(encryptedPassword) ? string.Empty : EncryptDecrypt.Decrypt(encryptedPassword);
            var passphrase = string.IsNullOrWhiteSpace(encryptedPassphrase) ? string.Empty : EncryptDecrypt.Decrypt(encryptedPassphrase);
            var normalizedKeyPath = keyPath?.Trim() ?? string.Empty;

            return credentials.FirstOrDefault(credential =>
            {
                if (credential == null || !string.Equals(credential.Username, username, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if (credential.UsePrivateKey != usePrivateKey)
                {
                    return false;
                }

                if (!usePrivateKey)
                {
                    var storedPassword = string.IsNullOrEmpty(credential.Password)
                        ? string.Empty
                        : EncryptDecrypt.Decrypt(credential.Password);
                    return string.Equals(storedPassword, password, StringComparison.Ordinal);
                }

                if (!string.Equals(credential.KeyPath ?? string.Empty, normalizedKeyPath, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                var storedPassphrase = string.IsNullOrEmpty(credential.KeyPassphrase)
                    ? string.Empty
                    : EncryptDecrypt.Decrypt(credential.KeyPassphrase);

                return string.Equals(storedPassphrase, passphrase, StringComparison.Ordinal);
            });
        }

        public static void ThrowSftpConfigurationError(string message)
        {
            MessageBox.Show(message, Resources.Validation_error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            throw new InvalidOperationException(message);
        }
    }
}
