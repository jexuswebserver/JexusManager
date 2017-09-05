// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
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

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (!_application.Server.Verify(txtPhysicalPath.Text))
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
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {

                    DialogHelper.ShowBrowseDialog(txtPhysicalPath);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {

                    var dialog = new SelectPoolDialog(txtPool.Text, _application.Server);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtPool.Text = dialog.Selected.Name;
                }));
        }

        private void EditSiteDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210531#Edit_Site");
        }
    }
}
