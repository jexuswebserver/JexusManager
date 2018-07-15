// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class SettingsDialog : DialogForm
    {
        public SettingsDialog(IServiceProvider serviceProvider, SiteTraceFailedRequestsLogging element, TraceFailedRequestsFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbEnabled.Checked = element.Enabled;
            txtDirectory.Text = element.Directory;
            txtNumber.Text = element.MaxLogFiles.ToString();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtDirectory, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtNumber, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(cbEnabled, "CheckedChanged"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtDirectory.Text)
                        && !string.IsNullOrWhiteSpace(txtNumber.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (uint.TryParse(txtNumber.Text, out uint number) && number > 0 && number <=10000)
                    {
                        element.MaxLogFiles = number;
                        element.Enabled = cbEnabled.Checked;
                        element.Directory = txtDirectory.Text;
                        DialogResult = DialogResult.OK;
                        return;
                    }

                    // TODO: can this come from validator?
                    ShowMessage(
                        "The 'Maximum Number of Trace Files' property is invalid. The value must be a valid integer between 1 and 10000.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtDirectory);
        }
    }
}
