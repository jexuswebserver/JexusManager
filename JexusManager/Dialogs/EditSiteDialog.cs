// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Application = Microsoft.Web.Administration.Application;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class EditSiteDialog : DialogForm
    {
        private readonly Application _application;

        public EditSiteDialog(IServiceProvider serviceProvider, Application application)
            : base(serviceProvider)
        {
            InitializeComponent();
            _application = application;
            txtPool.Text = application.ApplicationPoolName;
            txtAlias.Text = application.Site.Name;
            txtPhysicalPath.Text = application.PhysicalPath;
            btnBrowse.Visible = application.Server.IsLocalhost;
            btnSelect.Enabled = application.Server.Mode != WorkingMode.Jexus;
            RefreshButton();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (!_application.Server.Verify(txtPhysicalPath.Text, _application.GetActualExecutable()))
                    {
                        MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    _application.PhysicalPath = txtPhysicalPath.Text;
                    _application.ApplicationPoolName = txtPool.Text;
                    _application.Server.CommitChanges();
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPhysicalPath, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPool, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    RefreshButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPhysicalPath, _application.GetActualExecutable());
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new SelectPoolDialog(txtPool.Text, _application.Server);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtPool.Text = dialog.Selected.Name;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnConnect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var item = new ConnectAsItem(_application.VirtualDirectories[0]);
                    using (var dialog = new ConnectAsDialog(ServiceProvider, item))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                    }

                    item.Apply();
                    txtConnectAs.Text = string.IsNullOrEmpty(application.VirtualDirectories[0].UserName)
                        ? "Pass-through authentication"
                        : $"connect as '{application.VirtualDirectories[0].UserName}'";
                    RefreshButton();
                }));

            txtConnectAs.Text = string.IsNullOrEmpty(application.VirtualDirectories[0].UserName)
                ? "Pass-through authentication"
                : $"connect as '{application.VirtualDirectories[0].UserName}'";
        }

        private void RefreshButton()
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
        }

        private void EditSiteDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210531#Edit_Site");
        }
    }
}
