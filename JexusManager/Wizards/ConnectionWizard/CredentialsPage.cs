// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using JexusManager.Dialogs;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class CredentialsPage : WizardPage
    {
        public CredentialsPage()
        {
            InitializeComponent();
            Caption = "Provide Credentials";
        }

        protected internal override bool CanNavigateNext
        {
            get
            {
                return !string.IsNullOrWhiteSpace(cbUserName.Text)
                    && !string.IsNullOrWhiteSpace(txtPassword.Text)
                    && base.CanNavigateNext;
            }
        }

        private void CbUserNameTextChanged(object sender, EventArgs e)
        {
            var wizardData = ((ConnectionWizardData)WizardData);
            wizardData.UserName = cbUserName.Text;
            wizardData.Password = txtPassword.Text;
            UpdateWizard();
        }

        protected internal override void Activate()
        {
            base.Activate();
            var wizardData = (ConnectionWizardData)WizardData;
            txtConnect.Text = string.Format("Connecting to {0}.", wizardData.HostName.ExtractName());
            cbUserName.Focus();
        }

        internal void ShowProgress(bool visible)
        {
            cbUserName.Enabled = !visible;
            txtPassword.Enabled = !visible;
        }

        public override bool OnNext()
        {
            ShowProgress(true);
            var context = SynchronizationContext.Current;

            var result = AsyncHelper.RunSync(() => OpenConnection(context));
            ShowProgress(false);
            return result;
        }

        private async Task<bool> OpenConnection(SynchronizationContext context)
        {
            string accepted = null;
            var handler = ServicePointManager.ServerCertificateValidationCallback;
            ServicePointManager.ServerCertificateValidationCallback =
                (sender1, certificate, chain, sslPolicyErrors)
                =>
                {
                    var hash = certificate.GetCertHashString();
                    if (accepted == hash)
                    {
                        return true;
                    }

                    if (sslPolicyErrors == SslPolicyErrors.None)
                    {
                        accepted = hash;
                        return true;
                    }

                    var dialog = new CertificateErrorsDialog(certificate);
                    var result = dialog.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        return false;
                    }

                    accepted = hash;
                    return true;
                };

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));

            try
            {
                var data = (ConnectionWizardData)WizardData;
                data.Server = new ServerManager(data.HostName, data.UserName + "|" + data.Password);
                var version = await data.Server.GetVersionAsync();
                if (version == null)
                {
                    service.ShowMessage("Authentication failed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (version < JexusHelper.MinimumServerVersion)
                {
                    var toContinue =
                        service.ShowMessage(
                            string.Format(
                                "The server version is {0}, while minimum compatible version is {1}. Making changes might corrupt server configuration. Do you want to continue?",
                                version,
                                JexusHelper.MinimumServerVersion),
                            Text,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                    if (toContinue != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                var conflict = await data.Server.HelloAsync();
                if (Environment.MachineName != conflict)
                {
                    service.ShowMessage(string.Format("The server is also connected to {0}. Making changes on multiple clients might corrupt server configuration.", conflict), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                data.Server.Mode = WorkingMode.Jexus;
                data.CertificateHash = accepted;
                return true;
            }
            catch (Exception ex)
            {
                File.WriteAllText(DialogHelper.DebugLog, ex.ToString());
                var last = ex;
                while (last is AggregateException)
                {
                    last = last.InnerException;
                }

                var message = new StringBuilder();
                message.AppendLine("Could not connect to the specified computer.")
                                    .AppendLine()
                                    .AppendFormat("Details: {0}", last.Message);
                service.ShowMessage(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback = handler;
            }
        }
    }
}
