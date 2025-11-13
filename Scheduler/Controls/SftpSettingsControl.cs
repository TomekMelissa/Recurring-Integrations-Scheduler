using RecurringIntegrationsScheduler.Properties;
using RecurringIntegrationsScheduler.Settings;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler.Controls
{
    public enum SftpMode
    {
        Inbound,
        Outbound
    }

    public class SftpSettingsControl : UserControl
    {
        private readonly GroupBox _groupBox;
        private readonly TableLayoutPanel _layout;
        private readonly CheckBox _enableCheckBox;
        private readonly ComboBox _serverComboBox;
        private readonly ComboBox _credentialComboBox;
        private readonly TextBox _remoteFolderTextBox;
        private readonly TextBox _fileMaskTextBox;

        private BindingSource _serverBindingSource;
        private BindingSource _credentialBindingSource;

        public SftpSettingsControl()
        {
            Dock = DockStyle.Fill;

            _groupBox = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = Resources.Sftp_GroupTitle
            };

            _layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(8)
            };
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _enableCheckBox = new CheckBox { Text = Resources.Sftp_EnableIntegration, AutoSize = true };
            _enableCheckBox.CheckedChanged += (_, __) => UpdateControlState();

            _serverComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _credentialComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _remoteFolderTextBox = new TextBox();
            _fileMaskTextBox = new TextBox { Text = Resources.Sftp_DefaultFileMask };

            AddRow(_enableCheckBox, span: 2);
            AddRow(new Label { Text = Resources.Sftp_ServerLabel, AutoSize = true, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, _serverComboBox);
            AddRow(new Label { Text = Resources.Sftp_CredentialLabel, AutoSize = true, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, _credentialComboBox);
            AddRow(new Label { Text = Resources.Sftp_RemoteFolderLabel, AutoSize = true, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, _remoteFolderTextBox);
            AddRow(new Label { Text = Resources.Sftp_FileMaskLabel, AutoSize = true, TextAlign = System.Drawing.ContentAlignment.MiddleRight }, _fileMaskTextBox);

            _groupBox.Controls.Add(_layout);
            Controls.Add(_groupBox);

            UpdateControlState();
        }

        private void AddRow(Control control, Control field = null, int span = 1)
        {
            var rowIndex = _layout.RowCount++;
            _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            if (field == null)
            {
                _layout.Controls.Add(control, 0, rowIndex);
                _layout.SetColumnSpan(control, span);
                control.Margin = new Padding(0, 4, 0, 4);
                return;
            }

            _layout.Controls.Add(control, 0, rowIndex);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 4, 8, 4);

            _layout.Controls.Add(field, 1, rowIndex);
            field.Dock = DockStyle.Fill;
            field.Margin = new Padding(0, 4, 0, 4);
        }

        public void BindData(SftpServers servers, SftpCredentials credentials)
        {
            _serverBindingSource = new BindingSource { DataSource = servers ?? new SftpServers() };
            _serverComboBox.DataSource = _serverBindingSource;
            _serverComboBox.DisplayMember = nameof(SftpServer.Name);
            _serverComboBox.SelectedIndex = -1;

            _credentialBindingSource = new BindingSource { DataSource = credentials ?? new SftpCredentials() };
            _credentialComboBox.DataSource = _credentialBindingSource;
            _credentialComboBox.DisplayMember = nameof(SftpCredential.Name);
            _credentialComboBox.SelectedIndex = -1;
        }

        private void UpdateControlState()
        {
            var enabled = _enableCheckBox.Checked;
            foreach (Control control in _layout.Controls)
            {
                if (!ReferenceEquals(control, _enableCheckBox))
                {
                    control.Enabled = enabled;
                }
            }
        }

        public bool SftpEnabled
        {
            get => _enableCheckBox.Checked;
            set
            {
                _enableCheckBox.Checked = value;
                UpdateControlState();
            }
        }

        public SftpServer SelectedServer => _serverComboBox.SelectedItem as SftpServer;

        public SftpCredential SelectedCredential => _credentialComboBox.SelectedItem as SftpCredential;

        public string SelectedServerName
        {
            get => SelectedServer?.Name;
            set => SelectItemByName(_serverComboBox, value);
        }

        public string SelectedCredentialName
        {
            get => SelectedCredential?.Name;
            set => SelectItemByName(_credentialComboBox, value);
        }

        private static void SelectItemByName(ComboBox comboBox, string name)
        {
            if (comboBox == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            for (var i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i] is SftpServer server && server.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }

                if (comboBox.Items[i] is SftpCredential credential && credential.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }

            comboBox.SelectedIndex = -1;
        }

        public string RemoteFolder
        {
            get => _remoteFolderTextBox.Text;
            set => _remoteFolderTextBox.Text = value;
        }

        public string FileMask
        {
            get => _fileMaskTextBox.Text;
            set => _fileMaskTextBox.Text = string.IsNullOrWhiteSpace(value) ? Resources.Sftp_DefaultFileMask : value;
        }

        public bool HasServerOptions => _serverBindingSource?.Count > 0;

        public bool HasCredentialOptions => _credentialBindingSource?.Count > 0;

        public void SetMode(SftpMode mode)
        {
            _groupBox.Text = mode == SftpMode.Inbound
                ? Resources.Sftp_InboundGroupTitle
                : Resources.Sftp_OutboundGroupTitle;
        }

        public void SelectServer(Func<SftpServer, bool> predicate)
        {
            if (predicate == null || _serverBindingSource == null)
            {
                return;
            }

            var server = _serverBindingSource.List.Cast<SftpServer>().FirstOrDefault(predicate);
            if (server == null)
            {
                return;
            }

            SelectItemByName(_serverComboBox, server.Name);
        }

        public void SelectCredential(Func<SftpCredential, bool> predicate)
        {
            if (predicate == null || _credentialBindingSource == null)
            {
                return;
            }

            var credential = _credentialBindingSource.List.Cast<SftpCredential>().FirstOrDefault(predicate);
            if (credential == null)
            {
                return;
            }

            SelectItemByName(_credentialComboBox, credential.Name);
        }
    }
}
