// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
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
        public SettingsDialog(IServiceProvider serviceProvider, ConfigurationElement element, IsapiCgiRestrictionFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbCgi.Checked = (bool)element["notListedCgisAllowed"];
            cbIsapi.Checked = (bool)element["notListedIsapisAllowed"];

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    element["notListedIsapisAllowed"] = cbIsapi.Checked;
                    element["notListedCgisAllowed"] = cbCgi.Checked;
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
