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
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPhysicalPath);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtAlias, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPhysicalPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtAlias.Text) && !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
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

                    if (!application.Server.Verify(txtPhysicalPath.Text))
                    {
                        ShowMessage("The specified directory does not exist on the server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (VirtualDirectory == null)
                    {
                        // TODO: fix this
                        try
                        {
                            VirtualDirectory = new VirtualDirectory(null, application.VirtualDirectories)
                            {
                                Path = "/" + txtAlias.Text
                            };
                        }
                        catch (COMException ex)
                        {
                            ShowError(ex, string.Empty, false);
                            return;
                        }

                        VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
                        VirtualDirectory.Parent.Add(VirtualDirectory);
                    }
                    else
                    {
                        VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        public VirtualDirectory VirtualDirectory { get; private set; }

        private void NewVirtualDirectoryDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210458");
        }
    }
}
