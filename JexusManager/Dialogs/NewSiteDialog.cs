// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Application = Microsoft.Web.Administration.Application;
    using Binding = Microsoft.Web.Administration.Binding;
    using NativeMethods = JexusManager.NativeMethods;

    public partial class NewSiteDialog : DialogForm
    {
        private readonly SiteCollection _collection;

        public NewSiteDialog(IServiceProvider serviceProvider, SiteCollection collection)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbType.SelectedIndex = 0;
            _collection = collection;
            btnBrowse.Visible = collection.Parent.IsLocalhost;
            txtPool.Text = collection.Parent.ApplicationDefaults.ApplicationPoolName;
            btnChoose.Enabled = collection.Parent.Mode != WorkingMode.Jexus;
            txtHost.Text = collection.Parent.Mode == WorkingMode.IisExpress ? "localhost" : string.Empty;
            DialogHelper.LoadAddresses(cbAddress);
            if (Environment.OSVersion.Version < new Version(6, 2))
            {
                cbSniRequired.Enabled = false;
            }
        }

        private void CbTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            txtPort.Text = cbType.Text == "http" ? "80" : "443";
            txtCertificates.Visible = cbType.SelectedIndex == 1;
            cbSniRequired.Visible = cbType.SelectedIndex == 1;
            cbCertificates.Visible = cbType.SelectedIndex == 1;
            btnSelect.Visible = cbType.SelectedIndex == 1;
            btnView.Visible = cbType.SelectedIndex == 1;
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtPath);
        }

        private async void BtnOkClick(object sender, EventArgs e)
        {
            foreach (var ch in SiteCollection.InvalidSiteNameCharacters())
            {
                if (txtName.Text.Contains(ch))
                {
                    MessageBox.Show("The site name cannot contain the following characters: '\\, /, ?, ;, :, @, &, =, +, $, ,, |, \", <, >'.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
            {
                if (txtName.Text.Contains(ch) || txtName.Text.StartsWith("~"))
                {
                    MessageBox.Show("The site name cannot contain the following characters: '~,  '.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (!await _collection.Parent.VerifyAsync(txtPath.Text))
            {
                MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            IPAddress address;
            try
            {
                address = cbAddress.Text.ComboToAddress();
            }
            catch (Exception)
            {
                MessageBox.Show("The specified IP address is invalid. Specify a valid IP address.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int port;
            try
            {
                port = int.Parse(txtPort.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("The server port number must be a positive integer between 1 and 65535", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (port < 1 || port > 65535)
            {
                MessageBox.Show("The server port number must be a positive integer between 1 and 65535", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var invalid = "\"/\\[]:|<>+=;,?*$%#@{}^`".ToCharArray();
            foreach (var ch in invalid)
            {
                if (txtHost.Text.Contains(ch))
                {
                    MessageBox.Show("The specified host name is incorrect. The host name must use a valid host name format and cannot contain the following characters: \"/\\[]:|<>+=;,?*$%#@{}^`. Example: www.contoso.com.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (_collection.Parent.Mode == WorkingMode.IisExpress)
            {
                if (txtHost.Text != "localhost")
                {
                    MessageBox.Show(
                        "The specific host name is not recommended for IIS Express. The host name should be localhost.",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }

            long largestId = 0;
            foreach (Site site in _collection)
            {
                if (site.Id > largestId)
                {
                    largestId = site.Id;
                }
            }

            largestId++;

            NewSite = new Site(_collection) { Name = txtName.Text, Id = largestId };
            var host = txtHost.Text.DisplayToHost();
            var info = cbType.Text == "https" ? (CertificateInfo)cbCertificates.SelectedItem : null;
            var binding = new Binding(
                cbType.Text,
                string.Format("{0}:{1}:{2}", address.AddressToDisplay(), port, host.HostToDisplay()),
                info?.Certificate.GetCertHash() ?? new byte[0],
                info?.Store,
                cbSniRequired.Checked ? SslFlags.Sni : SslFlags.None,
                NewSite.Bindings);
            if (_collection.FindDuplicate(binding, null, null) != false)
            {
                var result = MessageBox.Show(string.Format("The binding '{0}' is assigned to another site. If you assign the same binding to this site, you will only be able to start one of the sites. Are you sure that you want to add this duplicate binding?", binding), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            if (_collection.Parent.Mode == WorkingMode.IisExpress)
            {
                var result = binding.FixCertificateMapping(info?.Certificate);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show(string.Format("The binding '{0}' is invalid: {1}", binding, result), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var app = new Application(NewSite.Applications)
            {
                Path = Application.RootPath,
                Name = string.Empty,
                ApplicationPoolName = txtPool.Text
            };
            app.Load(VirtualDirectory.RootPath, txtPath.Text);
            NewSite.Applications.Add(app);
            NewSite.Bindings.Add(binding);
            DialogResult = DialogResult.OK;
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            if (Helper.IsRunningOnMono())
            {
                return;
            }

            var toElevate = BindingExtensions.Verify(cbType.Text, cbAddress.Text, txtPort.Text, cbCertificates.SelectedItem as CertificateInfo);
            btnOK.Enabled = toElevate != null && !string.IsNullOrWhiteSpace(txtName.Text)
                            && !string.IsNullOrWhiteSpace(txtPath.Text);
            if (!toElevate.HasValue || !toElevate.Value)
            {
                NativeMethods.RemoveShieldFromButton(btnOK);
            }
            else
            {
                NativeMethods.TryAddShieldToButton(btnOK);
            }
        }

        public Site NewSite { get; set; }

        private void NewSiteDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210531#Add_Site");
        }

        private void NewSiteDialogLoad(object sender, EventArgs e)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            DialogHelper.LoadCertificates(cbCertificates, null, service);
        }

        private void CbCertificatesSelectedIndexChanged(object sender, EventArgs e)
        {
            btnView.Enabled = cbCertificates.SelectedIndex > 0;
            this.TxtNameTextChanged(null, null);
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            var info = (CertificateInfo)cbCertificates.SelectedItem;
            DialogHelper.DisplayCertificate(info.Certificate, this.Handle);
        }

        private void BtnChooseClick(object sender, EventArgs e)
        {
            var dialog = new SelectPoolDialog(txtPool.Text, _collection.Parent);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtPool.Text = dialog.Selected.Name;
        }
    }
}
