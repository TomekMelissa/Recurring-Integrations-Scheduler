using RecurringIntegrationsScheduler.Properties;
using RecurringIntegrationsScheduler.Settings;
using System;
using System.Text;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler.Forms
{
    public partial class SftpCredentialForm : Form
    {
        public SftpCredentialForm()
        {
            InitializeComponent();
        }

        public SftpCredential Credential { get; set; }

        private void SftpCredentialForm_Load(object sender, EventArgs e)
        {
            if (Credential == null)
            {
                UpdateInputState();
                return;
            }

            Text = Resources.Edit_credential;
            nameTextBox.Text = Credential.Name;
            usernameTextBox.Text = Credential.Username;
            useKeyCheckBox.Checked = Credential.UsePrivateKey;
            passwordTextBox.Text = Credential.Password;
            keyPathTextBox.Text = Credential.KeyPath;
            keyPassphraseTextBox.Text = Credential.KeyPassphrase;

            UpdateInputState();
        }

        private void UseKeyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInputState();
        }

        private void UpdateInputState()
        {
            var useKey = useKeyCheckBox.Checked;
            passwordTextBox.Enabled = !useKey;
            keyPathTextBox.Enabled = useKey;
            keyBrowseButton.Enabled = useKey;
            keyPassphraseTextBox.Enabled = useKey;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                DialogResult = DialogResult.None;
                return;
            }

            Credential = new SftpCredential
            {
                Name = nameTextBox.Text.Trim(),
                Username = usernameTextBox.Text.Trim(),
                UsePrivateKey = useKeyCheckBox.Checked,
                Password = passwordTextBox.Text,
                KeyPath = keyPathTextBox.Text.Trim(),
                KeyPassphrase = keyPassphraseTextBox.Text
            };
        }

        private bool ValidateInput()
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                builder.AppendLine(Resources.Name_is_required);
            }

            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                builder.AppendLine(Resources.Username_is_required);
            }

            if (!useKeyCheckBox.Checked)
            {
                if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
                {
                    builder.AppendLine(Resources.Password_is_required);
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(keyPathTextBox.Text))
                {
                    builder.AppendLine(Resources.Key_path_is_required);
                }
            }

            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(), Resources.Validation_error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void KeyBrowseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                keyPathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
