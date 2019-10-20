// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;
    using System.IO;
    using System.Net.Security;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    using JexusManager.Dialogs;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using Rollbar;

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
            txtConnect.Text = $"Connecting to {wizardData.HostName.ExtractName()}.";
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

            var result = OpenConnection(context);
            ShowProgress(false);
            return result;
        }

        private bool OpenConnection(SynchronizationContext context)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));

            try
            {
                var data = (ConnectionWizardData)WizardData;
                var server = new JexusServerManager(data.HostName, data.UserName + "|" + data.Password);
                data.Server = server;
                server.ServerCertificateValidationCallback =
                    (sender1, certificate, chain, sslPolicyErrors) =>
                    {
                        var hash = certificate.GetCertHashString();
                        if (server.AcceptedHash == hash)
                        {
                            return true;
                        }

                        if (sslPolicyErrors == SslPolicyErrors.None)
                        {
                            server.AcceptedHash = hash;
                            return true;
                        }

                        var dialog = new CertificateErrorsDialog(certificate);
                        var result = dialog.ShowDialog();
                        if (result != DialogResult.OK)
                        {
                            return false;
                        }

                        server.AcceptedHash = hash;
                        return true;
                    };
                var version = AsyncHelper.RunSync(() => server.GetVersionAsync());
                if (version == null)
                {
                    service.ShowMessage("Authentication failed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (version < JexusServerManager.MinimumServerVersion)
                {
                    var toContinue =
                        service?.ShowMessage(
                            string.Format(
                                "The server version is {0}, while minimum compatible version is {1}. Making changes might corrupt server configuration. Do you want to continue?",
                                version,
                                JexusServerManager.MinimumServerVersion),
                            Text,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                    if (toContinue != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                var conflict = AsyncHelper.RunSync(() => server.HelloAsync());
                if (Environment.MachineName != conflict)
                {
                    service?.ShowMessage($"The server is also connected to {conflict}. Making changes on multiple clients might corrupt server configuration.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                data.CertificateHash = server.AcceptedHash;
                return true;
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
                File.WriteAllText(DialogHelper.DebugLog, ex.ToString());
                var last = ex;
                while (last is AggregateException)
                {
                    last = last.InnerException;
                }

                var message = new StringBuilder();
                message.AppendLine("Could not connect to the specified computer.")
                                    .AppendLine()
                                    .AppendFormat("Details: {0}", last?.Message);
                service?.ShowMessage(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
