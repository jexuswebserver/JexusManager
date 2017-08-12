// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
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
                    if (directory.Path == Application.Path)
                    {
                        txtPhysicalPath.Text = directory.PhysicalPath;
                    }
                }
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

                    if (!_site.Server.Verify(txtPhysicalPath.Text))
                    {
                        MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (Application == null)
                    {
                        Application = new Application(_site.Applications)
                        {
                            Name = txtAlias.Text,
                            ApplicationPoolName = txtPool.Text
                        };
                        Application.Path = string.Format("{0}/{1}", _parentPath.TrimEnd('/'), Application.Name);
                        Application.Load(Application.Path, txtPhysicalPath.Text);
                        Application.Parent.Add(Application);
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
                    var dialog = new SelectPoolDialog(txtPool.Text, _site.Server);
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
        }

        public Application Application { get; private set; }

        private void NewApplicationDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210458");
        }
    }
}
