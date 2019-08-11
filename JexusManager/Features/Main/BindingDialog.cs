// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using JexusManager.Features.HttpApi;
    using Microsoft.Web.Management.Client;

    public sealed partial class BindingDialog : DialogForm
    {
        public BindingDialog(IServiceProvider serviceProvider, Binding binding1, Site site)
            : base(serviceProvider)
        {
            InitializeComponent();
            Binding = binding1;
            Text = Binding == null ? "Create Site Binding" : "Edit Site Binding";
            DialogHelper.LoadAddresses(cbAddress);
            cbAddress.SelectedIndex = -1;
            txtPort.Text = "80";
            cbType.SelectedIndex = 0;
            if (!site.Server.SupportsSni)
            {
                cbSniRequired.Enabled = false;
            }

            if (Binding == null)
            {
                txtHost.Text = site.Server.Mode == WorkingMode.IisExpress ? "localhost" : string.Empty;
            }
            else
            {
                cbType.Text = Binding.Protocol;
                cbType.Enabled = false;
                cbAddress.Text = Binding.EndPoint?.Address.AddressToCombo();
                txtPort.Text = Binding.EndPoint?.Port.ToString();
                txtHost.Text = Binding.Host.HostToDisplay();
                if (Binding.EndPoint == null)
                {
                    var value = Binding.BindingInformation;
                    var last = value.LastIndexOf(':');
                    if (last > 0)
                    {
                        txtHost.Text = value.Substring(last + 1).HostToDisplay();
                        var next = value.LastIndexOf(':', last - 1);
                        txtPort.Text = value.Substring(next + 1, last - next - 1);
                        if (next > -1)
                        {
                            cbAddress.Text = value.Substring(0, next);
                        }
                    }
                }

                if (site.Server.SupportsSni)
                {
                    cbSniRequired.Checked = Binding.GetIsSni();
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    IPAddress address;
                    try
                    {
                        address = cbAddress.Text.ComboToAddress();
                    }
                    catch (Exception)
                    {
                        ShowMessage("The specified IP address is invalid. Specify a valid IP address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    int port;
                    try
                    {
                        port = int.Parse(txtPort.Text);
                    }
                    catch (Exception)
                    {
                        ShowMessage("The server port number must be a positive integer between 1 and 65535", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (port < 1 || port > 65535)
                    {
                        ShowMessage("The server port number must be a positive integer between 1 and 65535", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (!txtHost.Text.IsValidHost(site.Server.SupportsWildcard))
                    {
                        ShowMessage("The specified host name is incorrect. The host name must use a valid host name format and cannot contain the following characters: \"/\\[]:|<>+=;,?*$%#@{}^`. Example: www.contoso.com.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    var certificate = cbCertificates.SelectedItem as CertificateInfo;
                    var host = txtHost.Text.DisplayToHost();
                    var binding = new Binding(
                        cbType.Text,
                        $"{address.AddressToDisplay()}:{port}:{host.HostToDisplay()}",
                        cbType.Text == "https" ? certificate?.Certificate.GetCertHash() : new byte[0],
                        cbType.Text == "https" ? certificate?.Store : null,
                        cbSniRequired.Checked ? SslFlags.Sni : SslFlags.None,
                        site.Bindings);
                    var matched = site.Parent.FindDuplicate(binding, site, Binding);
                    if (matched == true)
                    {
                        var result = ShowMessage(
                            $"The binding '{binding}' is assigned to another site. If you assign the same binding to this site, you will only be able to start one of the sites. Are you sure that you want to add this duplicate binding?",
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
                        binding.Delete();
                        return;
                    }

                    var conflicts = binding.DetectConflicts();
                    if (conflicts)
                    {
                        var result = ShowMessage(
                            $"This binding is already being used. If you continue you might overwrite the existing certificate for this IP Address:Port or Host Name:Port combination. Do you want to use this binding anyway?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            return;
                        }
                    }

                    if (Binding == null)
                    {
                        Binding = binding;
                    }
                    else
                    {
                        Binding.Reinitialize(binding);
                    }

                    if (site.Server.Mode == WorkingMode.IisExpress || site.Server.Mode == WorkingMode.Iis)
                    {
                        var result = Binding.FixCertificateMapping(certificate?.Certificate);
                        if (!string.IsNullOrEmpty(result))
                        {
                            ShowMessage($"The binding '{Binding}' is invalid: {result}.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    if (site.Server.Mode == WorkingMode.IisExpress)
                    {
                        if (Binding.Host != "localhost")
                        {
                            ShowMessage(
                                "The specific host name is not recommended for IIS Express. The host name should be localhost.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);

                            var reservation = binding.ToUrlPrefix();
                            var feature = new ReservedUrlsFeature((Module)serviceProvider);
                            feature.Load();
                            if (feature.Items.All(item => item.UrlPrefix != reservation) && !BindingUtility.AddReservedUrl(reservation))
                            {
                                ShowMessage($"Reserved URL {reservation} cannot be added.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbType, "SelectedIndexChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(this, "Load"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (Binding == null)
                    {
                        txtPort.Text = cbType.Text == "http" ? "80" : "443";
                        cbAddress.SelectedIndex = 0;
                    }

                    txtCertificates.Visible = cbType.SelectedIndex == 1;
                    cbSniRequired.Visible = cbType.SelectedIndex == 1;
                    cbCertificates.Visible = cbType.SelectedIndex == 1;
                    btnSelect.Visible = cbType.SelectedIndex == 1;
                    btnView.Visible = cbType.SelectedIndex == 1;
                }));

            var certificatesSelected = Observable.FromEventPattern<EventArgs>(cbCertificates, "SelectedIndexChanged");
            container.Add(
                certificatesSelected
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnView.Enabled = cbCertificates.SelectedIndex > 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbAddress, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPort, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtHost, "TextChanged"))
                .Merge(certificatesSelected)
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
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
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnView, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.DisplayCertificate(((CertificateInfo)cbCertificates.SelectedItem).Certificate, Handle);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    // TODO:
                }));
        }

        internal Binding Binding { get; private set; }

        private void BindingDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210531#Site_Bindings");
        }

        private void BindingDialogLoad(object sender, EventArgs e)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            Binding?.RefreshCertificate();
            DialogHelper.LoadCertificates(cbCertificates, Binding?.CertificateHash, Binding?.CertificateStoreName, service);
        }
    }
}
