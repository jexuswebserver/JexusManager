// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
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

    using Binding = Microsoft.Web.Administration.Binding;

    public sealed partial class BindingDialog : DialogForm
    {
        private readonly Site _site;

        public BindingDialog(IServiceProvider serviceProvider, Binding binding, Site site)
            : base(serviceProvider)
        {
            InitializeComponent();
            Binding = binding;
            _site = site;
            Text = Binding == null ? "Create Site Binding" : "Edit Site Binding";
            DialogHelper.LoadAddresses(cbAddress);
            txtPort.Text = "80";
            cbType.SelectedIndex = 0;
            if (!Binding.Parent.Parent.Server.SupportsSni)
            {
                cbSniRequired.Enabled = false;
            }

            if (Binding == null)
            {
                txtHost.Text = site.Server.Mode == WorkingMode.IisExpress ? "localhost" : string.Empty;
                return;
            }

            cbType.Text = Binding.Protocol;
            cbType.Enabled = Binding == null;
            cbAddress.Text = Binding.EndPoint.Address.AddressToCombo();
            txtPort.Text = Binding.EndPoint.Port.ToString();
            txtHost.Text = Binding.Host.HostToDisplay();
            if (Binding.Parent.Parent.Server.SupportsSni)
            {
                cbSniRequired.Checked = Binding.GetIsSni();
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

        private void BtnOkClick(object sender, EventArgs e)
        {
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

            if (_site.Server.Mode == WorkingMode.IisExpress)
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

            var certificate = cbCertificates.SelectedItem as CertificateInfo;
            var host = txtHost.Text.DisplayToHost();
            var binding = new Binding(
                cbType.Text,
                $"{address.AddressToDisplay()}:{port}:{host.HostToDisplay()}",
                cbType.Text == "https" ? certificate?.Certificate.GetCertHash() : new byte[0],
                cbType.Text == "https" ? certificate?.Store : null,
                cbSniRequired.Checked ? SslFlags.Sni : SslFlags.None,
                _site.Bindings);
            var matched = _site.Parent.FindDuplicate(binding, _site, Binding);
            if (matched == true)
            {
                var result = ShowMessage(
                    $"The binding '{Binding}' is assigned to another site. If you assign the same binding to this site, you will only be able to start one of the sites. Are you sure that you want to add this duplicate binding?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            if (matched == null)
            {
                ShowMessage(
                    "The specific port is being used by a different binding.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            if (Binding == null)
            {
                Binding = binding;
            }
            else
            {
                Binding.Reinitialize(binding);
            }

            if (_site.Server.Mode == WorkingMode.IisExpress)
            {
                var result = Binding.FixCertificateMapping(certificate?.Certificate);
                if (!string.IsNullOrEmpty(result))
                {
                    MessageBox.Show($"The binding '{Binding}' is invalid: {result}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        internal Binding Binding { get; private set; }

        private void CbAddressTextChanged(object sender, EventArgs e)
        {
            if (Helper.IsRunningOnMono())
            {
                return;
            }

            var toElevate = BindingUtility.Verify(cbType.Text, cbAddress.Text, txtPort.Text, cbCertificates.SelectedItem as CertificateInfo);
            btnOK.Enabled = toElevate != null;
            if (!toElevate.HasValue || !toElevate.Value)
            {
                JexusManager.NativeMethods.RemoveShieldFromButton(btnOK);
            }
            else
            {
                JexusManager.NativeMethods.TryAddShieldToButton(btnOK);
            }
        }

        private void BindingDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210531#Site_Bingings");
        }

        private void BindingDialogLoad(object sender, EventArgs e)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            DialogHelper.LoadCertificates(cbCertificates, Binding?.CertificateHash, service);
        }

        private void CbCertificatesSelectedIndexChanged(object sender, EventArgs e)
        {
            btnView.Enabled = cbCertificates.SelectedIndex > 0;
            this.CbAddressTextChanged(null, null);
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            DialogHelper.DisplayCertificate(((CertificateInfo)cbCertificates.SelectedItem).Certificate, this.Handle);
        }

        private void BtnSelectClick(object sender, EventArgs e)
        {
            // TODO:
        }
    }
}
