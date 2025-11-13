/* Copyright (c) Microsoft Corporation. All rights reserved.
   Licensed under the MIT License. */

using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Properties;
using RecurringIntegrationsScheduler.Settings;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler.Forms
{
    public partial class Parameters : Form
    {
        private DataGridView _sftpServersGrid;
        private ToolStripButton _sftpServersEditButton;
        private ToolStripButton _sftpServersDeleteButton;
        private DataGridView _sftpCredentialsGrid;
        private ToolStripButton _sftpCredentialsEditButton;
        private ToolStripButton _sftpCredentialsDeleteButton;

        public Parameters()
        {
            InitializeComponent();
            InitializeSftpManagementArea();
        }

        private void InitializeSftpManagementArea()
        {
            if (Properties.Settings.Default.SftpServers == null)
            {
                Properties.Settings.Default.SftpServers = new SftpServers();
            }

            if (Properties.Settings.Default.SftpCredentials == null)
            {
                Properties.Settings.Default.SftpCredentials = new SftpCredentials();
            }

            var sftpLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            sftpLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            sftpLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            miscSettingsGroupBox.Controls.Add(sftpLayoutPanel);

            var serversGroupBox = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = Resources.Sftp_ServersGroupTitle
            };
            serversGroupBox.Controls.Add(CreateSftpServersGrid());
            serversGroupBox.Controls.Add(CreateSftpServersToolStrip());
            sftpLayoutPanel.Controls.Add(serversGroupBox, 0, 0);

            var credentialsGroupBox = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = Resources.Sftp_CredentialsGroupTitle
            };
            credentialsGroupBox.Controls.Add(CreateSftpCredentialsGrid());
            credentialsGroupBox.Controls.Add(CreateSftpCredentialsToolStrip());
            sftpLayoutPanel.Controls.Add(credentialsGroupBox, 0, 1);
        }

        private ToolStrip CreateSftpServersToolStrip()
        {
            var toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(24, 24),
                Dock = DockStyle.Top
            };

            var addButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Add_server
            };
            addButton.Click += SftpServersAddButton_Click;

            _sftpServersEditButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Edit_server,
                Enabled = false
            };
            _sftpServersEditButton.Click += SftpServersEditButton_Click;

            _sftpServersDeleteButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Delete,
                Enabled = false
            };
            _sftpServersDeleteButton.Click += SftpServersDeleteButton_Click;

            toolStrip.Items.Add(addButton);
            toolStrip.Items.Add(_sftpServersEditButton);
            toolStrip.Items.Add(_sftpServersDeleteButton);

            return toolStrip;
        }

        private Control CreateSftpServersGrid()
        {
            _sftpServersGrid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Control,
                BorderStyle = BorderStyle.Fixed3D,
                ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
                Dock = DockStyle.Fill,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var nameColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpServer.Name),
                HeaderText = Resources.NameLabel,
                MinimumWidth = 100
            };
            var hostColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpServer.Host),
                HeaderText = Resources.Sftp_ServerHost,
                MinimumWidth = 100
            };
            var portColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpServer.Port),
                HeaderText = Resources.Sftp_ServerPort,
                MinimumWidth = 60
            };

            _sftpServersGrid.Columns.AddRange(nameColumn, hostColumn, portColumn);
            _sftpServersGrid.RowsRemoved += SftpServersGrid_RowsRemoved;
            _sftpServersGrid.RowStateChanged += SftpServersGrid_RowStateChanged;
            _sftpServersGrid.CellContentDoubleClick += SftpServersGrid_CellContentDoubleClick;

            return _sftpServersGrid;
        }

        private ToolStrip CreateSftpCredentialsToolStrip()
        {
            var toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(24, 24),
                Dock = DockStyle.Top
            };

            var addButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Add_credential
            };
            addButton.Click += SftpCredentialsAddButton_Click;

            _sftpCredentialsEditButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Edit_credential,
                Enabled = false
            };
            _sftpCredentialsEditButton.Click += SftpCredentialsEditButton_Click;

            _sftpCredentialsDeleteButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = Resources.Delete,
                Enabled = false
            };
            _sftpCredentialsDeleteButton.Click += SftpCredentialsDeleteButton_Click;

            toolStrip.Items.Add(addButton);
            toolStrip.Items.Add(_sftpCredentialsEditButton);
            toolStrip.Items.Add(_sftpCredentialsDeleteButton);

            return toolStrip;
        }

        private Control CreateSftpCredentialsGrid()
        {
            _sftpCredentialsGrid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Control,
                BorderStyle = BorderStyle.Fixed3D,
                ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
                Dock = DockStyle.Fill,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var nameColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpCredential.Name),
                HeaderText = Resources.NameLabel,
                MinimumWidth = 100
            };
            var userColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpCredential.Username),
                HeaderText = Resources.Sftp_CredentialUsername,
                MinimumWidth = 100
            };
            var authColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(SftpCredential.UsePrivateKey),
                HeaderText = Resources.Sftp_CredentialAuthType,
                MinimumWidth = 80
            };

            _sftpCredentialsGrid.Columns.AddRange(nameColumn, userColumn, authColumn);
            _sftpCredentialsGrid.CellFormatting += SftpCredentialsGrid_CellFormatting;
            _sftpCredentialsGrid.RowsRemoved += SftpCredentialsGrid_RowsRemoved;
            _sftpCredentialsGrid.RowStateChanged += SftpCredentialsGrid_RowStateChanged;
            _sftpCredentialsGrid.CellContentDoubleClick += SftpCredentialsGrid_CellContentDoubleClick;

            return _sftpCredentialsGrid;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.JobGroups = (JobGroups) jobGroupsGrid.DataSource;
            Properties.Settings.Default.Users = (Users) usersDataGrid.DataSource;
            if (_sftpServersGrid?.DataSource is SftpServers servers)
            {
                Properties.Settings.Default.SftpServers = servers;
            }
            if (_sftpCredentialsGrid?.DataSource is SftpCredentials creds)
            {
                Properties.Settings.Default.SftpCredentials = creds;
            }
            Properties.Settings.Default.Instances = (Instances) instancesGrid.DataSource;
            Properties.Settings.Default.DataJobs = (DataJobs) dataJobsGrid.DataSource;
            Properties.Settings.Default.AadApplications = (AadApplications) applicationsGrid.DataSource;
            Properties.Settings.Default.ProcessingErrorsFolder = processingErrorsFolder.Text;
            Properties.Settings.Default.ProcessingSuccessFolder = processingSuccessFolder.Text;
            Properties.Settings.Default.DownloadErrorsFolder = downloadErrorsFolder.Text;
            Properties.Settings.Default.UploadErrorsFolder = uploadErrorsFolder.Text;
            Properties.Settings.Default.UploadInputFolder = uploadInputFolder.Text;
            Properties.Settings.Default.UploadSuccessFolder = uploadSuccessFolder.Text;
            Properties.Settings.Default.Save();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UpdateSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpdateSettings = false;
                Properties.Settings.Default.Save();
            }
            jobGroupsGrid.DataSource = Properties.Settings.Default.JobGroups;
            dataJobsGrid.DataSource = Properties.Settings.Default.DataJobs;
            instancesGrid.DataSource = Properties.Settings.Default.Instances;
            applicationsGrid.DataSource = Properties.Settings.Default.AadApplications;
            usersDataGrid.DataSource = Properties.Settings.Default.Users;
            if (_sftpServersGrid != null)
            {
                _sftpServersGrid.DataSource = Properties.Settings.Default.SftpServers;
            }

            if (_sftpCredentialsGrid != null)
            {
                _sftpCredentialsGrid.DataSource = Properties.Settings.Default.SftpCredentials;
            }
            processingErrorsFolder.Text = Properties.Settings.Default.ProcessingErrorsFolder;
            processingSuccessFolder.Text = Properties.Settings.Default.ProcessingSuccessFolder;
            downloadErrorsFolder.Text = Properties.Settings.Default.DownloadErrorsFolder;
            uploadErrorsFolder.Text = Properties.Settings.Default.UploadErrorsFolder;
            uploadInputFolder.Text = Properties.Settings.Default.UploadInputFolder;
            uploadSuccessFolder.Text = Properties.Settings.Default.UploadSuccessFolder;
        }

        private void InstancesDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((instancesGrid.RowCount != 0) && (instancesGrid.SelectedRows.Count != 0)) return;
            instancesDeleteButton.Enabled = false;
            instancesValidateButton.Enabled = false;
            instancesEditButton.Enabled = false;
        }

        private void InstancesDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

            instancesDeleteButton.Enabled = true;
            instancesValidateButton.Enabled = true;
            instancesEditButton.Enabled = true;
        }

        private void AxInstancesAddButton_Click(object sender, EventArgs e)
        {
            var form = new InstanceForm();
            if (form.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.Instances.Add(form.Instance);
        }

        private void AxInstancesDeleteButton_Click(object sender, EventArgs e)
        {
            if (instancesGrid.SelectedRows.Count > 0)
                instancesGrid.Rows.RemoveAt(instancesGrid.SelectedRows[0].Index);
        }

        private void UsersDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((usersDataGrid.RowCount != 0) && (usersDataGrid.SelectedRows.Count != 0)) return;
            usersDeleteButton.Enabled = false;
            usersEditButton.Enabled = false;
        }

        private void UsersDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

            usersDeleteButton.Enabled = true;
            usersEditButton.Enabled = true;
        }

        private void UsersAddButton_Click(object sender, EventArgs e)
        {
            using UserForm form = new UserForm();
            if (form.ShowDialog() != DialogResult.OK) return;
            var axUser = form.User;
            if (axUser != null)
                axUser.Password = EncryptDecrypt.Encrypt(axUser.Password);
            Properties.Settings.Default.Users.Add(axUser);
        }

        private void UsersDeleteButton_Click(object sender, EventArgs e)
        {
            if (usersDataGrid.SelectedRows.Count > 0)
                usersDataGrid.Rows.RemoveAt(usersDataGrid.SelectedRows[0].Index);
        }

        private void DataJobsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((dataJobsGrid.RowCount != 0) && (dataJobsGrid.SelectedRows.Count != 0)) return;
            dataJobsDeleteButton.Enabled = false;
            dataJobsEditButton.Enabled = false;
        }

        private void DataJobsDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

            dataJobsDeleteButton.Enabled = true;
            dataJobsEditButton.Enabled = true;
        }

        private void DataJobsAddButton_Click(object sender, EventArgs e)
        {
            using DataJobForm form = new DataJobForm();
            if (form.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.DataJobs.Add(form.DataJob);
        }

        private void DataJobsDeleteButton_Click(object sender, EventArgs e)
        {
            if (dataJobsGrid.SelectedRows.Count > 0)
                dataJobsGrid.Rows.RemoveAt(dataJobsGrid.SelectedRows[0].Index);
        }

        private void JobGroupsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((jobGroupsGrid.RowCount != 0) && (jobGroupsGrid.SelectedRows.Count != 0)) return;
            jobGroupsDeleteButton.Enabled = false;
            jobGroupsEditButton.Enabled = false;
        }

        private void JobGroupsDataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

            jobGroupsDeleteButton.Enabled = true;
            jobGroupsEditButton.Enabled = true;
        }

        private void JobGroupsAddButton_Click(object sender, EventArgs e)
        {
            using JobGroupForm form = new JobGroupForm();
            if (form.ShowDialog() == DialogResult.OK)
                Properties.Settings.Default.JobGroups.Add(form.JobGroup);
        }

        private void JobGroupsDeleteButton_Click(object sender, EventArgs e)
        {
            if (jobGroupsGrid.SelectedRows.Count > 0)
                jobGroupsGrid.Rows.RemoveAt(jobGroupsGrid.SelectedRows[0].Index);
        }

        private void SftpServersAddButton_Click(object sender, EventArgs e)
        {
            using var form = new SftpServerForm
            {
                Text = Resources.Add_server
            };
            if (form.ShowDialog() != DialogResult.OK || form.Server == null)
            {
                return;
            }

            Properties.Settings.Default.SftpServers.Add(form.Server);
        }

        private void SftpServersEditButton_Click(object sender, EventArgs e)
        {
            EditSftpServer();
        }

        private void SftpServersDeleteButton_Click(object sender, EventArgs e)
        {
            if (_sftpServersGrid.SelectedRows.Count > 0)
            {
                _sftpServersGrid.Rows.RemoveAt(_sftpServersGrid.SelectedRows[0].Index);
            }
        }

        private void SftpServersGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((_sftpServersGrid.RowCount != 0) && (_sftpServersGrid.SelectedRows.Count != 0)) return;
            _sftpServersEditButton.Enabled = false;
            _sftpServersDeleteButton.Enabled = false;
        }

        private void SftpServersGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            _sftpServersEditButton.Enabled = true;
            _sftpServersDeleteButton.Enabled = true;
        }

        private void SftpServersGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            EditSftpServer();
        }

        private void EditSftpServer()
        {
            if (_sftpServersGrid.SelectedRows.Count == 0)
            {
                return;
            }

            var selected = _sftpServersGrid.SelectedRows[0].DataBoundItem as SftpServer;
            if (selected == null)
            {
                return;
            }

            var clone = new SftpServer
            {
                Name = selected.Name,
                Host = selected.Host,
                Port = selected.Port
            };

            using var form = new SftpServerForm
            {
                Server = clone,
                Text = Resources.Edit_server
            };

            if (form.ShowDialog() != DialogResult.OK || form.Server == null)
            {
                return;
            }

            var index = Properties.Settings.Default.SftpServers.IndexOf(selected);
            if (index < 0)
            {
                return;
            }

            Properties.Settings.Default.SftpServers.RemoveAt(index);
            Properties.Settings.Default.SftpServers.Insert(index, form.Server);
        }

        private void SftpCredentialsAddButton_Click(object sender, EventArgs e)
        {
            using var form = new SftpCredentialForm
            {
                Text = Resources.Add_credential
            };

            if (form.ShowDialog() != DialogResult.OK || form.Credential == null)
            {
                return;
            }

            var credentialToStore = PrepareCredentialForStorage(form.Credential);
            Properties.Settings.Default.SftpCredentials.Add(credentialToStore);
        }

        private void SftpCredentialsEditButton_Click(object sender, EventArgs e)
        {
            EditSftpCredential();
        }

        private void SftpCredentialsDeleteButton_Click(object sender, EventArgs e)
        {
            if (_sftpCredentialsGrid.SelectedRows.Count > 0)
            {
                _sftpCredentialsGrid.Rows.RemoveAt(_sftpCredentialsGrid.SelectedRows[0].Index);
            }
        }

        private void SftpCredentialsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_sftpCredentialsGrid.Columns[e.ColumnIndex].DataPropertyName != nameof(SftpCredential.UsePrivateKey))
            {
                return;
            }

            var credential = _sftpCredentialsGrid.Rows[e.RowIndex].DataBoundItem as SftpCredential;
            if (credential == null)
            {
                return;
            }

            e.Value = credential.UsePrivateKey ? Resources.Private_key : Resources.Password;
            e.FormattingApplied = true;
        }

        private void SftpCredentialsGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((_sftpCredentialsGrid.RowCount != 0) && (_sftpCredentialsGrid.SelectedRows.Count != 0)) return;
            _sftpCredentialsEditButton.Enabled = false;
            _sftpCredentialsDeleteButton.Enabled = false;
        }

        private void SftpCredentialsGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            _sftpCredentialsEditButton.Enabled = true;
            _sftpCredentialsDeleteButton.Enabled = true;
        }

        private void SftpCredentialsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            EditSftpCredential();
        }

        private void EditSftpCredential()
        {
            if (_sftpCredentialsGrid.SelectedRows.Count == 0)
            {
                return;
            }

            var selected = _sftpCredentialsGrid.SelectedRows[0].DataBoundItem as SftpCredential;
            if (selected == null)
            {
                return;
            }

            var clone = new SftpCredential
            {
                Name = selected.Name,
                Username = selected.Username,
                UsePrivateKey = selected.UsePrivateKey,
                Password = string.IsNullOrEmpty(selected.Password) ? string.Empty : EncryptDecrypt.Decrypt(selected.Password),
                KeyPath = selected.KeyPath,
                KeyPassphrase = string.IsNullOrEmpty(selected.KeyPassphrase) ? string.Empty : EncryptDecrypt.Decrypt(selected.KeyPassphrase)
            };

            using var form = new SftpCredentialForm
            {
                Credential = clone,
                Text = Resources.Edit_credential
            };

            if (form.ShowDialog() != DialogResult.OK || form.Credential == null)
            {
                return;
            }

            var newCredential = PrepareCredentialForStorage(form.Credential);
            var index = Properties.Settings.Default.SftpCredentials.IndexOf(selected);
            if (index < 0)
            {
                return;
            }

            Properties.Settings.Default.SftpCredentials.RemoveAt(index);
            Properties.Settings.Default.SftpCredentials.Insert(index, newCredential);
        }

        private static SftpCredential PrepareCredentialForStorage(SftpCredential credential)
        {
            var prepared = new SftpCredential
            {
                Name = credential.Name.Trim(),
                Username = credential.Username.Trim(),
                UsePrivateKey = credential.UsePrivateKey,
                KeyPath = credential.UsePrivateKey ? (credential.KeyPath?.Trim() ?? string.Empty) : string.Empty
            };

            prepared.Password = credential.UsePrivateKey || string.IsNullOrWhiteSpace(credential.Password)
                ? string.Empty
                : EncryptDecrypt.Encrypt(credential.Password);

            prepared.KeyPassphrase = credential.UsePrivateKey && !string.IsNullOrWhiteSpace(credential.KeyPassphrase)
                ? EncryptDecrypt.Encrypt(credential.KeyPassphrase)
                : string.Empty;

            return prepared;
        }

        private void InstancesValidateButton_Click(object sender, EventArgs e)
        {
            using ValidateConnection form = new ValidateConnection { Instance = (Instance)instancesGrid.SelectedRows[0].DataBoundItem };
            form.ShowDialog();
        }

        private void InstancesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (instancesGrid.SelectedRows.Count != 0) return;
            instancesDeleteButton.Enabled = false;
            instancesValidateButton.Enabled = false;
            instancesEditButton.Enabled = false;
        }

        private void ApplicationsAddButton_Click(object sender, EventArgs e)
        {
            using AadApplicationForm form = new AadApplicationForm();
            if (form.ShowDialog() != DialogResult.OK) return;

            var application = form.AadApplication;
            if ((application != null) && (application.Secret != string.Empty))
                application.Secret = EncryptDecrypt.Encrypt(application.Secret);

            Properties.Settings.Default.AadApplications.Add(application);
        }

        private void ApplicationsDeleteButton_Click(object sender, EventArgs e)
        {
            if (applicationsGrid.SelectedRows.Count > 0)
                applicationsGrid.Rows.RemoveAt(applicationsGrid.SelectedRows[0].Index);
        }

        private void ApplicationsGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if ((applicationsGrid.RowCount != 0) && (applicationsGrid.SelectedRows.Count != 0)) return;
            applicationsDeleteButton.Enabled = false;
            applicationsEditButton.Enabled = false;
        }

        private void ApplicationsGrid_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;

            applicationsDeleteButton.Enabled = true;
            applicationsEditButton.Enabled = true;
        }

        private void InstancesEdit()
        {
            using InstanceForm form = new InstanceForm { Instance = (Instance)instancesGrid.SelectedRows[0].DataBoundItem };
            var index = Properties.Settings.Default.Instances.IndexOf((Instance)instancesGrid.SelectedRows[0].DataBoundItem);

            if (form.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Instances.RemoveAt(index);
                Properties.Settings.Default.Instances.Insert(index, form.Instance);
            }
            instancesGrid.Rows[index].Selected = true;
        }

        private void DataJobsEdit()
        {
            using DataJobForm form = new DataJobForm { DataJob = (DataJob)dataJobsGrid.SelectedRows[0].DataBoundItem };
            var index = Properties.Settings.Default.DataJobs.IndexOf((DataJob)dataJobsGrid.SelectedRows[0].DataBoundItem);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.DataJobs.RemoveAt(index);
                Properties.Settings.Default.DataJobs.Insert(index, form.DataJob);
            }
            dataJobsGrid.Rows[index].Selected = true;
        }

        private void ApplicationsEdit()
        {
            var application = (AadApplication) applicationsGrid.SelectedRows[0].DataBoundItem;
            try
            {
                if (application.Secret != string.Empty)
                    application.Secret = EncryptDecrypt.Decrypt(application.Secret);
            }
            catch
            {
                application.Secret = string.Empty;
                MessageBox.Show(Resources.Existing_application_secret_could_not_be_decrypted);
            }

            using AadApplicationForm form = new AadApplicationForm { AadApplication = application };
            var index = Properties.Settings.Default.AadApplications.IndexOf((AadApplication)applicationsGrid.SelectedRows[0].DataBoundItem);

            if (form.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.AadApplications.RemoveAt(index);
                application = form.AadApplication;

                if ((application != null) && (application.Secret != string.Empty))
                    application.Secret = EncryptDecrypt.Encrypt(application.Secret);

                Properties.Settings.Default.AadApplications.Insert(index, application);
            }
            else
            {
                try
                {
                    application.Secret = EncryptDecrypt.Encrypt(application.Secret);
                }
                catch
                {
                    application.Secret = string.Empty;
                    MessageBox.Show(Resources.Existing_application_secret_could_not_be_encrypted);
                }
            }
            applicationsGrid.Rows[index].Selected = true;
        }

        private void UsersEdit()
        {
            var user = (User) usersDataGrid.SelectedRows[0].DataBoundItem;
            try
            {
                user.Password = EncryptDecrypt.Decrypt(user.Password);
            }
            catch
            {
                user.Password = string.Empty;
                MessageBox.Show(Resources.Existing_password_could_not_be_decrypted);
            }

            using UserForm form = new UserForm { User = user };
            var index = Properties.Settings.Default.Users.IndexOf((User)usersDataGrid.SelectedRows[0].DataBoundItem);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.Users.RemoveAt(index);
                user = form.User;
                if (user != null)
                    user.Password = EncryptDecrypt.Encrypt(user.Password);
                Properties.Settings.Default.Users.Insert(index, user);
            }
            else
            {
                try
                {
                    user.Password = EncryptDecrypt.Encrypt(user.Password);
                }
                catch
                {
                    user.Password = string.Empty;
                    MessageBox.Show(Resources.Existing_password_could_not_be_encrypted);
                }
            }
            usersDataGrid.Rows[index].Selected = true;
        }

        private void JobGroupsEdit()
        {
            using JobGroupForm form = new JobGroupForm { JobGroup = (JobGroup)jobGroupsGrid.SelectedRows[0].DataBoundItem };
            var index = Properties.Settings.Default.JobGroups.IndexOf((JobGroup)jobGroupsGrid.SelectedRows[0].DataBoundItem);
            if (form.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.JobGroups.RemoveAt(index);
                Properties.Settings.Default.JobGroups.Insert(index, form.JobGroup);
            }
            jobGroupsGrid.Rows[index].Selected = true;
        }

        private void InstancesEditButton_Click(object sender, EventArgs e)
        {
            InstancesEdit();
        }

        private void DataJobsEditButton_Click(object sender, EventArgs e)
        {
            DataJobsEdit();
        }

        private void ApplicationsEditButton_Click(object sender, EventArgs e)
        {
            ApplicationsEdit();
        }

        private void UsersEditButton_Click(object sender, EventArgs e)
        {
            UsersEdit();
        }

        private void JobGroupsEditButton_Click(object sender, EventArgs e)
        {
            JobGroupsEdit();
        }

        private void InstancesGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InstancesEdit();
        }

        private void DataJobsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataJobsEdit();
        }

        private void ApplicationsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ApplicationsEdit();
        }

        private void UsersDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UsersEdit();
        }

        private void JobGroupsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            JobGroupsEdit();
        }
    }
}
