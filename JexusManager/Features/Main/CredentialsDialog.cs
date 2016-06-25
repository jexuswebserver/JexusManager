// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class CredentialsDialog : DialogForm
    {
        public CredentialsDialog(IServiceProvider serviceProvider, string name)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtName.Text = name;
        }

        private void CredentialsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210456#ApplicationPoolIdentity");
        }

        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                            && !string.IsNullOrWhiteSpace(txtPassword.Text)
                            && txtConfirm.Text == txtPassword.Text;
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            UserName = txtName.Text;
            Password = txtPassword.Text;
            // TODO: verify user
            // DialogResult = DialogResult.Cancel;
            DialogResult = DialogResult.OK;
        }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
