using RecurringIntegrationsScheduler.Settings;
using System;
using System.Text;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler.Forms
{
    public partial class SftpServerForm : Form
    {
        public SftpServerForm()
        {
            InitializeComponent();
        }

        public SftpServer Server { get; set; }

        private void SftpServerForm_Load(object sender, EventArgs e)
        {
            if (Server == null)
            {
                portNumericUpDown.Value = 22;
                Text = Properties.Resources.Add_server;
                return;
            }

            Text = Properties.Resources.Edit_server;
            nameTextBox.Text = Server.Name;
            hostTextBox.Text = Server.Host;
            if (Server.Port >= portNumericUpDown.Minimum && Server.Port <= portNumericUpDown.Maximum)
            {
                portNumericUpDown.Value = Server.Port;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                DialogResult = DialogResult.None;
                return;
            }

            Server = new SftpServer
            {
                Name = nameTextBox.Text.Trim(),
                Host = hostTextBox.Text.Trim(),
                Port = (int)portNumericUpDown.Value
            };
        }

        private bool ValidateInput()
        {
            var builder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                builder.AppendLine(Properties.Resources.Name_is_required);
            }

            if (string.IsNullOrWhiteSpace(hostTextBox.Text))
            {
                builder.AppendLine(Properties.Resources.Host_is_required);
            }

            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString(), Properties.Resources.Validation_error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
