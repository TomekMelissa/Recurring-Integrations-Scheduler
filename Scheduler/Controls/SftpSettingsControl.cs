using System;
using System.Drawing;
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
        private readonly TextBox _hostTextBox;
        private readonly NumericUpDown _portUpDown;
        private readonly TextBox _usernameTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly CheckBox _useKeyCheckBox;
        private readonly TextBox _keyPathTextBox;
        private readonly Button _browseKeyButton;
        private readonly TextBox _keyPassphraseTextBox;
        private readonly TextBox _remoteFolderTextBox;
        private readonly TextBox _fileMaskTextBox;

        public SftpSettingsControl()
        {
            Dock = DockStyle.Fill;

            _groupBox = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = "SFTP"
            };

            _layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(8)
            };
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            _enableCheckBox = new CheckBox { Text = "Enable SFTP integration" };
            _enableCheckBox.CheckedChanged += (_, __) => UpdateControlState();

            _hostTextBox = new TextBox();
            _portUpDown = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 65535,
                Value = 22
            };
            _usernameTextBox = new TextBox();
            _passwordTextBox = new TextBox { UseSystemPasswordChar = true };
            _useKeyCheckBox = new CheckBox { Text = "Use private key" };
            _useKeyCheckBox.CheckedChanged += (_, __) => UpdateControlState();

            _keyPathTextBox = new TextBox();
            _browseKeyButton = new Button { Text = "Browse...", AutoSize = true };
            _browseKeyButton.Click += BrowseKeyButtonOnClick;
            _keyPassphraseTextBox = new TextBox { UseSystemPasswordChar = true };
            _remoteFolderTextBox = new TextBox();
            _fileMaskTextBox = new TextBox { Text = "*.*" };

            AddRow(_enableCheckBox, span: 3);
            AddRow(new Label { Text = "Host:", AutoSize = true }, _hostTextBox);
            AddRow(new Label { Text = "Port:", AutoSize = true }, _portUpDown);
            AddRow(new Label { Text = "Username:", AutoSize = true }, _usernameTextBox);
            AddRow(new Label { Text = "Password:", AutoSize = true }, _passwordTextBox);
            AddRow(_useKeyCheckBox, span: 3);
            AddRow(new Label { Text = "Key file:", AutoSize = true }, _keyPathTextBox, _browseKeyButton);
            AddRow(new Label { Text = "Key passphrase:", AutoSize = true }, _keyPassphraseTextBox);
            AddRow(new Label { Text = "Remote folder:", AutoSize = true }, _remoteFolderTextBox);
            AddRow(new Label { Text = "File mask:", AutoSize = true }, _fileMaskTextBox);

            _groupBox.Controls.Add(_layout);
            Controls.Add(_groupBox);

            UpdateControlState();
        }

        private void AddRow(Control control, Control second = null, Control third = null, int span = 1)
        {
            var rowIndex = _layout.RowCount++;
            _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            if (span == 3)
            {
                _layout.Controls.Add(control, 0, rowIndex);
                _layout.SetColumnSpan(control, 3);
                return;
            }

            _layout.Controls.Add(control, 0, rowIndex);
            control.Margin = new Padding(0, 4, 8, 4);

            if (second != null)
            {
                _layout.Controls.Add(second, 1, rowIndex);
                second.Dock = DockStyle.Fill;
                second.Margin = new Padding(0, 4, 8, 4);
            }

            if (third != null)
            {
                _layout.Controls.Add(third, 2, rowIndex);
                third.Margin = new Padding(0, 4, 0, 4);
            }
        }

        private void BrowseKeyButtonOnClick(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Key files (*.ppk;*.pem)|*.ppk;*.pem|All files (*.*)|*.*",
                Title = "Select private key file"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _keyPathTextBox.Text = dialog.FileName;
            }
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

            _keyPathTextBox.Enabled = enabled && _useKeyCheckBox.Checked;
            _browseKeyButton.Enabled = enabled && _useKeyCheckBox.Checked;
            _keyPassphraseTextBox.Enabled = enabled && _useKeyCheckBox.Checked;
            _passwordTextBox.Enabled = enabled && !_useKeyCheckBox.Checked;
        }

        public bool SftpEnabled
        {
            get => _enableCheckBox.Checked;
            set => _enableCheckBox.Checked = value;
        }

        public string Host
        {
            get => _hostTextBox.Text;
            set => _hostTextBox.Text = value;
        }

        public int Port
        {
            get => (int)_portUpDown.Value;
            set
            {
                if (value >= _portUpDown.Minimum && value <= _portUpDown.Maximum)
                {
                    _portUpDown.Value = value;
                }
            }
        }

        public string Username
        {
            get => _usernameTextBox.Text;
            set => _usernameTextBox.Text = value;
        }

        public string Password
        {
            get => _passwordTextBox.Text;
            set => _passwordTextBox.Text = value;
        }

        public bool UsePrivateKey
        {
            get => _useKeyCheckBox.Checked;
            set => _useKeyCheckBox.Checked = value;
        }

        public string PrivateKeyPath
        {
            get => _keyPathTextBox.Text;
            set => _keyPathTextBox.Text = value;
        }

        public string PrivateKeyPassphrase
        {
            get => _keyPassphraseTextBox.Text;
            set => _keyPassphraseTextBox.Text = value;
        }

        public string RemoteFolder
        {
            get => _remoteFolderTextBox.Text;
            set => _remoteFolderTextBox.Text = value;
        }

        public string FileMask
        {
            get => _fileMaskTextBox.Text;
            set => _fileMaskTextBox.Text = value;
        }

        public void SetMode(SftpMode mode)
        {
            _groupBox.Text = mode == SftpMode.Inbound
                ? "Inbound SFTP (download files)"
                : "Outbound SFTP (upload files)";
        }
    }
}
