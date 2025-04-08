// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    public partial class ConditionsPage : DefaultWizardPage
    {
        private bool _initialized;

        public ConditionsPage()
        {
            InitializeComponent();
            Caption = "Define Trace Conditions";

            var container = new CompositeDisposable();
            Disposed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbCodes, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtCodes.Enabled = cbCodes.Checked;
                    UpdateWizard();
                }));
            container.Add(
                Observable.FromEventPattern<EventArgs>(cbTime, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtTime.Enabled = cbTime.Checked;
                    UpdateWizard();
                }));
            container.Add(
                Observable.FromEventPattern<EventArgs>(cbEventSeverity, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    cbSeverity.Enabled = cbEventSeverity.Checked;
                    UpdateWizard();
                }));
            container.Add(
                Observable.FromEventPattern<EventArgs>(txtCodes, "TextChanged")
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    UpdateWizard();
                }));
            container.Add(
                Observable.FromEventPattern<EventArgs>(txtTime, "TextChanged")
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    UpdateWizard();
                }));
        }

        protected override bool CanNavigateNext
        {
            get
            {
                var canNavigateNext = cbCodes.Checked && !string.IsNullOrWhiteSpace(txtCodes.Text)
                    && (!cbTime.Checked || (cbTime.Checked && !string.IsNullOrWhiteSpace(txtTime.Text)));
                return base.CanNavigateNext && canNavigateNext;
            }
        }

        public override bool OnNext()
        {
            if (cbCodes.Checked)
            {
                if (!IsValidCodes(txtCodes.Text))
                {
                    ShowMessage($"'{txtCodes.Text}' is an invalid status code. Status codes must be numbers in the form of 400 or 400.1. Status codes must be between 100 and 999. Sub-status codes must be between 1 and 999.", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }

            long time = 0;
            if (cbTime.Checked)
            {
                if (!long.TryParse(txtTime.Text, out time) || time < 0 || time > 922337203685)
                {
                    ShowMessage("The time interval must be a positive integer between 0 and 922,337,203,685", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }

            var data = (AddTraceWizardData)WizardData;
            data.Codes = txtCodes.Text;
            data.Time = time;
            if (cbEventSeverity.Checked)
            {
                switch (cbSeverity.SelectedIndex)
                {
                    case 0:
                        data.Verbosity = 2;
                        break;
                    case 1:
                        data.Verbosity = 1;
                        break;
                    case 2:
                        data.Verbosity = 3;
                        break;
                }
            }
            else
            {
                data.Verbosity = 0;
            }

            return true;
        }

        private static bool IsValidCodes(string text)
        {
            string[] sections = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var section in sections)
            {
                string[] bounds = section.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var bound in bounds)
                {
                    string[] codes = bound.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (codes.Length == 0)
                    {
                        return false;
                    }

                    if (!int.TryParse(codes[0], out int status) || status < 100 || status > 999)
                    {
                        return false;
                    }

                    if (codes.Length == 2)
                    {
                        if (!int.TryParse(codes[1], out int subStatus) || subStatus < 1 || subStatus > 999)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        protected override void Activate()
        {
            var data = (AddTraceWizardData)WizardData;
            cbCodes.Checked = !string.IsNullOrEmpty(data.Codes);
            txtCodes.Text = data.Codes;

            cbTime.Checked = data.Time > 0;
            txtTime.Text = cbTime.Checked ? data.Time.ToString() : string.Empty;

            cbEventSeverity.Checked = data.Verbosity > 0;
            cbSeverity.Enabled = cbEventSeverity.Checked;
            if (cbSeverity.Enabled)
            {
                switch (data.Verbosity)
                {
                    case 1:
                        cbSeverity.SelectedIndex = 1;
                        break;
                    case 2:
                        cbSeverity.SelectedIndex = 0;
                        break;
                    case 3:
                        cbSeverity.SelectedIndex = 2;
                        break;
                }
            }
        }
    }
}
