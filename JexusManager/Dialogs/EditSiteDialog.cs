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
            txtPhysicalPath.Text = application.VirtualDirectories[0].PhysicalPath;
            btnBrowse.Visible = application.Server.IsLocalhost;
            btnSelect.Enabled = application.Server.Mode != WorkingMode.Jexus;
        }

        private void EditSiteDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210531#Edit_Site");
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            if (!await _application.Server.VerifyAsync(txtPhysicalPath.Text))
            {
                MessageBox.Show("The specified directory does not exist on the server.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _application.VirtualDirectories[0].PhysicalPath = txtPhysicalPath.Text;
            _application.ApplicationPoolName = txtPool.Text;
            _application.Server.CommitChanges();
            DialogResult = DialogResult.OK;
        }

        private void txtPhysicalPath_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPhysicalPath.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtPhysicalPath);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var dialog = new SelectPoolDialog(txtPool.Text, _application.Server);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtPool.Text = dialog.Selected.Name;
        }
    }
}
