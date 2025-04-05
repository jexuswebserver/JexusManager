// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Application = Microsoft.Web.Administration.Application;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public sealed partial class NewApplicationDialog : DialogForm
    {
        private readonly Site _site;
        private readonly string _parentPath;

        public NewApplicationDialog(IServiceProvider serviceProvider, Site site, string parentPath, string pool, Application existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtSite.Text = site.Name;
            txtPath.Text = parentPath;
            btnBrowse.Visible = site.Server.IsLocalhost;
            btnSelect.Enabled = site.Server.Mode != WorkingMode.Jexus;
            _site = site;
            _parentPath = parentPath;
            Application = existing;
            Text = Application == null ? "Add Application" : "Edit Application";
            txtAlias.ReadOnly = Application != null;
            if (Application == null)
            {
                // TODO: test if IIS does this
                txtPool.Text = pool;
            }
            else
            {
                txtAlias.Text = Application.Name ?? Application.Path.PathToName();
                txtPool.Text = Application.ApplicationPoolName;
                foreach (VirtualDirectory directory in Application.VirtualDirectories)
                {
                    if (directory.Path == "/")
                    {
                        txtPhysicalPath.Text = directory.PhysicalPath;
                    }
                }

                RefreshButton();
            }

            var item = new ConnectAsItem(Application?.VirtualDirectories[0]);

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPhysicalPath, null);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtAlias, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPhysicalPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    RefreshButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    foreach (var ch in ApplicationCollection.InvalidApplicationPathCharacters())
                    {
                        if (txtAlias.Text.Contains(ch.ToString(CultureInfo.InvariantCulture)))
                        {
                            MessageBox.Show("The application path cannot contain the following characters: \\, ?, ;, :, @, &, =, +, $, ,, |, \", <, >, *.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
                    {
                        if (txtAlias.Text.Contains(ch.ToString(CultureInfo.InvariantCulture)))
                        {
                            MessageBox.Show("The site name cannot contain the following characters: ' '.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    if (!_site.Server.Verify(txtPhysicalPath.Text, site.Applications[0].GetActualExecutable()))
                    {
                        MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (Application == null)
                    {
                        string path = string.Format("{0}/{1}", _parentPath.TrimEnd('/'), txtAlias.Text);
                        foreach (VirtualDirectory virtualDirectory in _site.Applications[0].VirtualDirectories)
                        {
                            if (string.Equals(virtualDirectory.Path, path, StringComparison.OrdinalIgnoreCase))
                            {
                                ShowMessage("This virtual directory already exists.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }

                        foreach (Application application in _site.Applications)
                        {
                            if (string.Equals(path, application.Path))
                            {
                                ShowMessage("An application with this virtual path already exists.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }

                        Application = _site.Applications.Add(path, txtPhysicalPath.Text);
                        Application.Name = txtAlias.Text;
                        Application.ApplicationPoolName = txtPool.Text;

                        item.Element = Application.VirtualDirectories[0];
                        item.Apply();
                    }
                    else
                    {
                        foreach (VirtualDirectory directory in Application.VirtualDirectories)
                        {
                            if (directory.Path == Application.Path)
                            {
                                directory.PhysicalPath = txtPhysicalPath.Text;
                            }
                        }
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new SelectPoolDialog(txtPool.Text, _site.Server);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtPool.Text = dialog.Selected.Name;
                    if (Application != null)
                    {
                        Application.ApplicationPoolName = dialog.Selected.Name;
                    }
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
                    RefreshButton();
                }));

            txtConnectAs.Text = string.IsNullOrEmpty(item.UserName)
                ? "Pass-through authentication"
                : $"connect as '{item.UserName}'";
        }

        private void RefreshButton()
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtAlias.Text) && !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
        }

        public Application Application { get; private set; }

        private void NewApplicationDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210458");
        }
    }
}
