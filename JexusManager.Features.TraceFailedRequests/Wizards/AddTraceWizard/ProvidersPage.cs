// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    public partial class ProvidersPage : DefaultWizardPage
    {
        private bool _initialized;

        public ProvidersPage()
        {
            InitializeComponent();
            Caption = "Select Trace Providers";

            var container = new CompositeDisposable();
            Disposed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(clbProviders, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var data = (AddTraceWizardData)WizardData;
                    var provider = clbProviders.SelectedItem as Provider;
                    cbVerbosity.Enabled = provider != null;
                    if (provider != null)
                    {
                        cbVerbosity.SelectedIndex = provider.Verbosity;
                        clbAreas.Items.Clear();
                        foreach (var area in provider.Areas)
                        {
                            clbAreas.Items.Add(area, provider.SelectedAreas.Contains(area));
                        }
                    }
                }));

            container.Add(
                Observable.FromEventPattern<ItemCheckEventArgs>(clbProviders, "ItemCheck")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var provider = clbProviders.SelectedItem as Provider;
                    if (provider != null)
                    {
                        provider.Selected = evt.EventArgs.NewValue == CheckState.Checked;
                    }

                    VerifyFinish();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbVerbosity, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var provider = clbProviders.SelectedItem as Provider;
                    provider.Verbosity = cbVerbosity.SelectedIndex;
                    VerifyFinish();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(clbAreas, "ItemCheck")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var provider = clbProviders.SelectedItem as Provider;
                    provider.SelectedAreas.Clear();
                    foreach (string item in clbAreas.CheckedItems)
                    {
                        provider.SelectedAreas.Add(item);
                    }

                    VerifyFinish();
                }));
        }

        private void VerifyFinish()
        {
            var data = (AddTraceWizardData)WizardData;
            data.Providers.Clear();
            foreach (Provider provider in clbProviders.CheckedItems)
            {
                data.Providers.Add(provider);
            }

            UpdateWizard();
        }

        protected override void Activate()
        {
            base.Activate();
            _initialized = false;
            var data = (AddTraceWizardData)WizardData;
            foreach (var provider in data.Providers)
            {
                if (!data.Editing)
                {
                    // IMPORTANT: select all providers and areas for new rule.
                    provider.Selected = true;
                    provider.SelectedAreas.AddRange(provider.Areas);
                }

                clbProviders.Items.Add(provider, provider.Selected);
            }

            cbVerbosity.Enabled = false;
            _initialized = true;
        }
    }
}
