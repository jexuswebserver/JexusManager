// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Linq;
    using System.Reactive.Disposables;
    using Microsoft.Web.Management.Client.Extensions;

    public partial class BasicEditDialog : DialogForm
    {
        public BasicEditDialog(IServiceProvider serviceProvider, BasicItem existing, AuthenticationFeature feature)
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
