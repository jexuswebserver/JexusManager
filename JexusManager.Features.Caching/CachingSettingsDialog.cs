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

    internal partial class CachingSettingsDialog : DialogForm
    {
        public CachingSettingsDialog(IServiceProvider serviceProvider, CachingSettings settings, CachingFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbUser.Checked = settings.Enabled;
            cbKernel.Checked = settings.EnableKernelCache;
            txtSize.Text = settings.MaxResponseSize.ToString();
            var limit = settings.MaxCacheSize;
            cbLimit.Checked = limit != 0;
            if (cbLimit.Checked)
            {
                txtLimit.Text = limit.ToString();
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    settings.EnableKernelCache = cbKernel.Checked;
                    settings.Enabled = cbUser.Checked;
                    settings.MaxResponseSize = uint.Parse(txtSize.Text);
                    settings.MaxCacheSize = cbLimit.Checked ? uint.Parse(txtLimit.Text) : 0U;

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
