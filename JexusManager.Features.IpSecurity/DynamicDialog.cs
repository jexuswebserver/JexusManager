// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class DynamicDialog : DialogForm
    {
        public DynamicDialog(IServiceProvider serviceProvider, DynamicIpSecuritySettings settings, IpSecurityFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbConcurrent.Checked = settings.EnableConcurrentDenial;
            txtConcurrent.Text = settings.MaxConcurrentRequests.ToString();
            cbInterval.Checked = settings.EnableRateDenial;
            txtNumer.Text = settings.MaxRequests.ToString();
            txtPeriod.Text = settings.RequestIntervalInMilliseconds.ToString();
            cbLogging.Checked = settings.EnableLoggingOnlyMode;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            var load = Observable.FromEventPattern<EventArgs>(this, "Load");

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbInterval, "CheckedChanged")
                .Merge(load)
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtPeriod.Enabled = txtNumer.Enabled = cbInterval.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbConcurrent, "CheckedChanged")
                .Merge(load)
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtConcurrent.Enabled = cbConcurrent.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    uint result;
                    if (!uint.TryParse(txtConcurrent.Text, out result))
                    {
                        ShowMessage(
                            "Input string was not in a correct format",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    settings.EnableConcurrentDenial = cbConcurrent.Checked;
                    settings.MaxConcurrentRequests = result;
                    settings.EnableRateDenial = cbInterval.Checked;
                    if (!uint.TryParse(txtNumer.Text, out result))
                    {
                        // TODO: show validator error.
                        ShowMessage(
                            "Input string was not in a correct format",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    settings.MaxRequests = result;
                    if (!uint.TryParse(txtNumer.Text, out result))
                    {
                        // TODO: show validator error.
                        ShowMessage(
                            "Input string was not in a correct format",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    settings.RequestIntervalInMilliseconds = result;
                    settings.EnableLoggingOnlyMode = cbLogging.Checked;
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
