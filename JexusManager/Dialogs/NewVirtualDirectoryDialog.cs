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
    using System.Runtime.InteropServices;

    public sealed partial class NewVirtualDirectoryDialog : DialogForm
    {
        public NewVirtualDirectoryDialog(IServiceProvider serviceProvider, VirtualDirectory existing, string pathToSite, Application application)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtSite.Text = application.Site.Name;
            txtPath.Text = pathToSite;
            btnBrowse.Visible = application.Server.IsLocalhost;
            VirtualDirectory = existing;
            Text = VirtualDirectory == null ? "Add Virtual Directory" : "Edit Virtual Directory";
            txtAlias.ReadOnly = VirtualDirectory != null;
            if (VirtualDirectory == null)
            {
                // TODO: test if IIS does this
            }
            else
            {
                txtAlias.Text = VirtualDirectory.Path.PathToName();
                txtPhysicalPath.Text = VirtualDirectory.PhysicalPath;
                RefreshButton();
            }

            var item = new ConnectAsItem(VirtualDirectory);

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPhysicalPath, application.GetActualExecutable());
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
                            ShowMessage("The application path cannot contain the following characters: \\, ?, ;, :, @, &, =, +, $, ,, |, \", <, >, *.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
                    {
                        if (txtAlias.Text.Contains(ch.ToString(CultureInfo.InvariantCulture)))
                        {
                            ShowMessage("The site name cannot contain the following characters: ' '.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    if (!application.Server.Verify(txtPhysicalPath.Text, application.GetActualExecutable()))
                    {
                        ShowMessage("The specified directory does not exist on the server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (VirtualDirectory == null)
                    {
                        string path = "/" + txtAlias.Text;
                        foreach (VirtualDirectory virtualDirectory in application.VirtualDirectories)
                        {
                            if (string.Equals(virtualDirectory.Path, path, StringComparison.OrdinalIgnoreCase))
                            {
                                ShowMessage("This virtual directory already exists.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }

                        var fullPath = $"{txtPath.Text}{path}";
                        foreach (Application app in application.Site.Applications)
                        {
                            if (string.Equals(fullPath, app.Path))
                            {
                                ShowMessage("An application with this virtual path already exists.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                        }

                        try
                        {
                            VirtualDirectory = new VirtualDirectory(null, application.VirtualDirectories)
                            {
                                Path = path
                            };
                        }
                        catch (COMException ex)
                        {
                            ShowError(ex, string.Empty, false);
                            return;
                        }

                        VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
                        VirtualDirectory.Parent.Add(VirtualDirectory);

                        item.Element = VirtualDirectory;
                        item.Apply();
                    }
                    else
                    {
                        VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
                    }

                    DialogResult = DialogResult.OK;
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
                    RefreshButton();
                    txtConnectAs.Text = string.IsNullOrEmpty(item.UserName)
                        ? "Pass-through authentication"
                        : $"connect as '{item.UserName}'";
                }));

            txtConnectAs.Text = string.IsNullOrEmpty(item.UserName)
                ? "Pass-through authentication"
                : $"connect as '{item.UserName}'";
        }

        private void RefreshButton()
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtAlias.Text) && !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
        }

        public VirtualDirectory VirtualDirectory { get; private set; }

        private void NewVirtualDirectoryDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210458");
        }
    }
}
