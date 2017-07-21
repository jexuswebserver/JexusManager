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

    public sealed partial class NewVirtualDirectoryDialog : DialogForm
    {
        private readonly Application _application;

        public NewVirtualDirectoryDialog(IServiceProvider serviceProvider, VirtualDirectory existing, string pathToSite, Application application)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtSite.Text = application.Site.Name;
            txtPath.Text = pathToSite;
            btnBrowse.Visible = application.Server.IsLocalhost;
            _application = application;
            VirtualDirectory = existing;
            Text = VirtualDirectory == null ? "Add Virtual Directory" : "Edit Virtual Directory";
            txtAlias.ReadOnly = VirtualDirectory != null;
            if (VirtualDirectory == null)
            {
                // TODO: test if IIS does this
                return;
            }

            txtAlias.Text = VirtualDirectory.Path.PathToName();
            txtPhysicalPath.Text = VirtualDirectory.PhysicalPath;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtPhysicalPath);
        }

        private void txtAlias_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtAlias.Text) && !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
        }

        public VirtualDirectory VirtualDirectory { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
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

            if (!_application.Server.Verify(txtPhysicalPath.Text))
            {
                MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (VirtualDirectory == null)
            {
                // TODO: fix this
                VirtualDirectory = new VirtualDirectory(null, _application.VirtualDirectories)
                {
                    Path = "/" + txtAlias.Text
                };
                VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
                VirtualDirectory.Parent.Add(VirtualDirectory);
            }
            else
            {
                VirtualDirectory.PhysicalPath = txtPhysicalPath.Text;
            }

            DialogResult = DialogResult.OK;
        }

        private void NewVirtualDirectoryDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210458");
        }
    }
}
