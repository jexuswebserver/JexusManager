// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Linq;
    using System.Reactive.Disposables;
    public partial class CredentialsDialog : DialogForm
    {
        public CredentialsDialog(IServiceProvider serviceProvider, string name)
            : base(serviceProvider)
        {
            InitializeComponent();
            btnOK.Enabled = false;
            txtName.Text = name;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPassword, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtConfirm, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                    && !string.IsNullOrWhiteSpace(txtPassword.Text)
                    && txtConfirm.Text == txtPassword.Text;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    UserName = txtName.Text;
                    Password = txtPassword.Text;
                    // TODO: verify user
                    // DialogResult = DialogResult.Cancel;
                    DialogResult = DialogResult.OK;
                }));
        }

        private void CredentialsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210461#Anonymous");
        }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
