// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

using JexusManager.Services;

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;

using Application = Microsoft.Web.Administration.Application;
using Binding = Microsoft.Web.Administration.Binding;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using JexusManager.Features.HttpApi;
using Microsoft.Web.Management.Client;
using System.Linq;

namespace JexusManager.Dialogs
{
    public partial class NewSiteDialog : DialogForm
    {
        public NewSiteDialog(IServiceProvider serviceProvider, SiteCollection collection)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbType.SelectedIndex = 0;
            if (collection == null)
            {
                throw new InvalidOperationException("null collection");
            }

            if (collection.Parent == null)
            {
                throw new InvalidOperationException("null server for site collection");
            }
            
            btnBrowse.Visible = collection.Parent.IsLocalhost;
            txtPool.Text = collection.Parent.ApplicationDefaults.ApplicationPoolName;
            btnChoose.Enabled = collection.Parent.Mode != WorkingMode.Jexus;
            txtHost.Text = collection.Parent.Mode == WorkingMode.IisExpress ? "localhost" : string.Empty;
            DialogHelper.LoadAddresses(cbAddress);
            if (!collection.Parent.SupportsSni)
            {
                cbSniRequired.Enabled = false;
            }

            var item = new ConnectAsItem(NewSite?.Applications[0].VirtualDirectories[0]);

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbType, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtPort.Text = cbType.Text == "http" ? "80" : "443";
                    txtCertificates.Visible = cbType.SelectedIndex == 1;
                    cbSniRequired.Visible = cbType.SelectedIndex == 1;
                    cbCertificates.Visible = cbType.SelectedIndex == 1;
                    btnSelect.Visible = cbType.SelectedIndex == 1;
                    btnView.Visible = cbType.SelectedIndex == 1;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnConnect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using (var dialog = new ConnectAsDialog(ServiceProvider, item))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    item.Apply();
                    txtConnectAs.Text = string.IsNullOrEmpty(item.UserName)
                        ? "Pass-through authentication"
                        : $"connect as '{item.UserName}'";
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPath, null);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    foreach (var ch in SiteCollection.InvalidSiteNameCharacters())
                    {
                        if (txtName.Text.Contains(ch))
                        {
                            ShowMessage("The site name cannot contain the following characters: '\\, /, ?, ;, :, @, &, =, +, $, ,, |, \", <, >'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
                    {
                        if (txtName.Text.Contains(ch) || txtName.Text.StartsWith("~"))
                        {
                            ShowMessage("The site name cannot contain the following characters: '~,  '.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    if (!collection.Parent.Verify(txtPath.Text, null))
                    {
                        ShowMessage("The specified directory does not exist on the server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (!IPEndPointIsValid(out IPAddress address, out int port))
                    {
                        return;
                    }

                    var invalid = "\"/\\[]:|<>+=;,?*$%#@{}^`".ToCharArray();
                    foreach (var ch in invalid)
                    {
                        if (txtHost.Text.Contains(ch))
                        {
                            ShowMessage("The specified host name is incorrect. The host name must use a valid host name format and cannot contain the following characters: \"/\\[]:|<>+=;,?*$%#@{}^`. Example: www.contoso.com.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    if (collection.Parent.Mode == WorkingMode.IisExpress)
                    {
                        if (txtHost.Text != "localhost")
                        {
                            ShowMessage(
                                "The specific host name is not recommended for IIS Express. The host name should be localhost.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);
                        }
                    }

                    long largestId = 0;
                    foreach (Site site in collection)
                    {
                        if (site.Id > largestId)
                        {
                            largestId = site.Id;
                        }
                    }

                    largestId++;

                    NewSite = new Site(collection) { Name = txtName.Text, Id = largestId };
                    var host = txtHost.Text.DisplayToHost();
                    var info = cbType.Text == "https" ? (CertificateInfo)cbCertificates.SelectedItem : null;
                    var binding = new Binding(
                        cbType.Text,
                        string.Format("{0}:{1}:{2}", address.AddressToDisplay(), port, host.HostToDisplay()),
                        info?.Certificate.GetCertHash() ?? new byte[0],
                        info?.Store,
                        cbSniRequired.Checked ? SslFlags.Sni : SslFlags.None,
                        NewSite.Bindings);
                    if (collection.FindDuplicate(binding, null, null) != false)
                    {
                        var result = ShowMessage(string.Format("The binding '{0}' is assigned to another site. If you assign the same binding to this site, you will only be able to start one of the sites. Are you sure that you want to add this duplicate binding?", binding), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            collection.Remove(NewSite);
                            return;
                        }
                    }

                    if (collection.Parent.Mode == WorkingMode.IisExpress || collection.Parent.Mode == WorkingMode.Iis)
                    {
                        var (state, message)  = binding.FixCertificateMapping(info?.Certificate);
                        if (state != CertificateMappingState.RegistrationSucceeded)
                        {
                            if (state == CertificateMappingState.HostNameNotMatched)
                            {
                                var result = ShowMessage($"{message}. Do you still want to use this certificate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result == DialogResult.Yes)
                                {
                                    // IMPORTANT: force using the certificate.
                                    (state, message) = binding.FixCertificateMapping(info?.Certificate, true);
                                }
                            }

                            if (state != CertificateMappingState.RegistrationSucceeded)
                            {
                                collection.Remove(NewSite);
                                ShowMessage(string.Format("The binding '{0}' is invalid: {1}", binding, message), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }
                    }

                    var app = NewSite.Applications.Add(Application.RootPath, txtPath.Text);
                    app.Name = string.Empty;
                    app.ApplicationPoolName = txtPool.Text;
                    NewSite.Bindings.Add(binding);

                    item.Element = NewSite.Applications[0].VirtualDirectories[0];
                    item.Apply();

                    if (collection.Parent.Mode == WorkingMode.IisExpress)
                    {
                        if (binding.Host != "localhost")
                        {
                            ShowMessage(
                                "The specific host name is not recommended for IIS Express. The host name should be localhost.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);

                            var reservation = binding.ToUrlPrefix();
                            var feature = new ReservedUrlsFeature((Module)serviceProvider);
                            feature.Load();
                            if (feature.Items.All(item => item.UrlPrefix != reservation))
                            {
                                var message = BindingUtility.AddReservedUrl(reservation);
                                if (!string.IsNullOrEmpty(message))
                                {
                                    ShowMessage($"Reserved URL {reservation} cannot be added. {message}", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                    return;
                                }
                            }
                        }
                    }

                    DialogResult = DialogResult.OK;
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
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtPort, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(cbAddress, "TextChanged"))
                .Merge(certificatesSelected)
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (Helper.IsRunningOnMono())
                    {
                        return;
                    }

                    var toElevate = IPEndPointIsValid(out IPAddress address, out int port, false) ? BindingUtility.Verify(cbType.Text, cbAddress.Text, txtPort.Text, cbCertificates.SelectedItem as CertificateInfo) : false;
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
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnView, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var info = (CertificateInfo)cbCertificates.SelectedItem;
                    DialogHelper.DisplayCertificate(info.Certificate, this.Handle);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnChoose, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new SelectPoolDialog(txtPool.Text, collection.Parent);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtPool.Text = dialog.Selected.Name;
                }));
        }

        private bool IPEndPointIsValid(out IPAddress address, out int port, bool showDialog = true)
        {
            try
            {
                address = cbAddress.Text.ComboToAddress();
            }
            catch (Exception)
            {
                if (showDialog)
                {
                    ShowMessage("The specified IP address is invalid. Specify a valid IP address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }

                address = null;
                port = 0;
                return false;
            }

            return Binding.PortIsValid(txtPort.Text, out port, Text, showDialog);
        }

        public Site NewSite { get; set; }

        private void NewSiteDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210531#Add_Site");
        }

        private void NewSiteDialogLoad(object sender, EventArgs e)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            DialogHelper.LoadCertificates(cbCertificates, null, null, service);
        }
    }
}
