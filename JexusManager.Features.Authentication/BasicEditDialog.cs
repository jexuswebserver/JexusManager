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
    public partial class BasicEditDialog : DialogForm
    {
        public BasicEditDialog(IServiceProvider serviceProvider, BasicItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            txtDomain.Text = existing.Domain;
            txtRealm.Text = existing.Realm;

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    existing.Domain = txtDomain.Text;
                    existing.Realm = txtRealm.Text;
                    existing.Apply();
                    DialogResult = DialogResult.OK;
                }));
        }

        private void BasicEditDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461#Basic");
        }
    }
}
