// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class ContentPage : DefaultWizardPage
    {
        public ContentPage()
        {
            InitializeComponent();
            Caption = "Specify Content to Trace";

            var container = new CompositeDisposable();
            Disposed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbAll, "Click")
                .Merge(Observable.FromEventPattern<EventArgs>(rbAspNet, "Click"))
                .Merge(Observable.FromEventPattern<EventArgs>(rbAsp, "Click"))
                .Merge(Observable.FromEventPattern<EventArgs>(rbCustom, "Click"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtContent.Enabled = rbCustom.Checked;
                    UpdateWizard();
                }));
        }

        public override bool OnNext()
        {
            var data = (AddTraceWizardData)WizardData;
            if (rbAspNet.Checked)
            {
                data.FileName = "*.aspx";
            }
            else if (rbAsp.Checked)
            {
                data.FileName = "*.asp";
            }
            else if (rbAll.Checked)
            {
                data.FileName = "*";
            }
            else
            {
                data.FileName = txtContent.Text;
            }

            return base.OnNext();
        }

        protected override bool CanNavigateNext
        {
            get
            {
                var canNavigateNext = rbAll.Checked || rbAsp.Checked || rbAspNet.Checked || (rbCustom.Checked && !string.IsNullOrWhiteSpace(txtContent.Text));
                return base.CanNavigateNext && canNavigateNext;
            }
        }

        protected override void Activate()
        {
            var data = (AddTraceWizardData)WizardData;
            rbAll.Enabled = rbAspNet.Enabled = rbAsp.Enabled = rbCustom.Enabled = txtContent.Enabled = !data.Editing;
            if (data.FileName == "*.aspx")
            {
                rbAspNet.Checked = true;
            }
            else if (data.FileName == "*.asp")
            {
                rbAsp.Checked = true;
            }
            else if (data.FileName == "*")
            {
                rbAll.Checked = true;
            }
            else
            {
                rbCustom.Checked = true;
                txtContent.Text = data.FileName;
            }
        }
    }
}
