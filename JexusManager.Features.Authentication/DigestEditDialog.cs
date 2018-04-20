// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    using Microsoft.Web.Management.Client.Extensions;

    public partial class DigestEditDialog : DialogForm
    {
        public DigestEditDialog(IServiceProvider serviceProvider, DigestItem item, AuthenticationFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtRealm.Text = item.Realm;
            txtRealm.SelectAll();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    item.Realm = txtRealm.Text;
                    item.Apply();
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }
    }
}
