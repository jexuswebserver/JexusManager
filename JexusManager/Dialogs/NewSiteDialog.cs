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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

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

                    if (!collection.Parent.Verify(txtPath.Text, null))
                    {
                        MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                            MessageBox.Show("The specified host name is incorrect. The host name must use a valid host name format and cannot contain the following characters: \"/\\[]:|<>+=;,?*$%#@{}^`. Example: www.contoso.com.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    if (collection.Parent.Mode == WorkingMode.IisExpress)
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
                        var result = MessageBox.Show(string.Format("The binding '{0}' is assigned to another site. If you assign the same binding to this site, you will only be able to start one of the sites. Are you sure that you want to add this duplicate binding?", binding), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result != DialogResult.Yes)
                        {
                            collection.Remove(NewSite);
                            return;
                        }
                    }

                    if (collection.Parent.Mode == WorkingMode.IisExpress)
                    {
                        var result = binding.FixCertificateMapping(info?.Certificate);
                        if (!string.IsNullOrEmpty(result))
                        {
                            collection.Remove(NewSite);
                            MessageBox.Show(string.Format("The binding '{0}' is invalid: {1}", binding, result), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    var app = NewSite.Applications.Add(Application.RootPath, txtPath.Text);
                    app.Name = string.Empty;
                    app.ApplicationPoolName = txtPool.Text;
                    NewSite.Bindings.Add(binding);
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
                .Sample(TimeSpan.FromSeconds(1))
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
                    var dialog = new SelectPoolDialog(txtPool.Text, collection.Parent);
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
                    MessageBox.Show("The specified IP address is invalid. Specify a valid IP address.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
