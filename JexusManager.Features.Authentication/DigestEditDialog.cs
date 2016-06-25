// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    public partial class DigestEditDialog : DialogForm
    {
        public DigestEditDialog(IServiceProvider serviceProvider, DigestItem item)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtRealm.Text = item.Realm;
            txtRealm.SelectAll();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    item.Realm = txtRealm.Text;
                    item.Apply();
                    DialogResult = DialogResult.OK;
                }));
        }

        private void BasicEditDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461#Digest");
        }
    }
}
