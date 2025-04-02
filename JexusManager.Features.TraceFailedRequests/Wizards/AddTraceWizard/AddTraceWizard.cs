// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using Properties;

    internal partial class AddTraceWizard : WizardForm
    {
        private readonly TraceFailedRequestsItem _existing;
        private readonly ConfigurationElement _config;
        private readonly AddTraceWizardData _wizardData;
        private readonly TraceFailedRequestsFeature _feature;

        public AddTraceWizard(IServiceProvider serviceProvider, TraceFailedRequestsItem existing, ConfigurationElement config, TraceFailedRequestsFeature feature)
            : base(serviceProvider)
        {
            _existing = existing;
            _config = config;
            _feature = feature;
            _wizardData = new AddTraceWizardData(config, existing);
            InitializeComponent();
            TaskGlyph = Resources.trace_failed_requests_48;
            Text = existing == null ? "Add Failed Request Tracing Rule" : "Edit Failed Request Tracing Rule";
        }

        protected override object WizardData
        {
            get { return _wizardData; }
        }

        public TraceFailedRequestsItem Item { get; private set; }

        protected override void CompleteWizard()
        {
            Item = _existing == null ? new TraceFailedRequestsItem(null) : _existing;
            _wizardData.Apply(Item);
            if (_existing == null && _feature.Items.Any(item => item.Match(Item)))
            {
                ShowMessage(
                    "A failed request trace for this content already exists.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        protected override bool CanComplete
        {
            get
            {
                var data = (AddTraceWizardData)WizardData;
                return data.IsValid;
            }
        }

        protected override WizardPage[] GetWizardPages()
        {
            var contentPage = new ContentPage();
            contentPage.SetWizard(this);
            var conditionsPage = new ConditionsPage();
            conditionsPage.SetWizard(this);
            var providersPage = new ProvidersPage();
            providersPage.SetWizard(this);
            contentPage.SetNextPage(conditionsPage);
            conditionsPage.SetPreviousPage(contentPage);
            conditionsPage.SetNextPage(providersPage);
            providersPage.SetPreviousPage(conditionsPage);
            return [contentPage, conditionsPage, providersPage];
        }

        protected override void ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210482");
        }
    }
}
