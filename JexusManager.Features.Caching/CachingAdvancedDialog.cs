// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class CachingAdvancedDialog : DialogForm
    {
        public CachingAdvancedDialog(IServiceProvider serviceProvider, CachingItem item, CachingFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbString.Checked = !string.IsNullOrWhiteSpace(item.VaryByQueryString);
            txtString.Text = item.VaryByQueryString;

            cbHeaders.Checked = !string.IsNullOrWhiteSpace(item.VaryByHeaders);
            txtHeaders.Text = item.VaryByHeaders;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    item.VaryByHeaders = txtHeaders.Text;
                    item.VaryByQueryString = txtString.Text;
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbString, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtString.Enabled = cbString.Checked;
                    if (!cbString.Checked)
                    {
                        txtString.Text = string.Empty;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbHeaders, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtHeaders.Enabled = cbHeaders.Checked;
                    if (!cbHeaders.Checked)
                    {
                        txtHeaders.Text = string.Empty;
                    }
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
