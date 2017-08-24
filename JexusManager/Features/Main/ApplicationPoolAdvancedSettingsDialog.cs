// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class ApplicationPoolAdvancedSettingsDialog : DialogForm
    {
         public ApplicationPoolAdvancedSettingsDialog(IServiceProvider serviceProvider, ApplicationPool pool)
            : base(serviceProvider)
        {
            InitializeComponent();
            var settings = new ApplicationPoolAdvancedSettings(pool);
            propertyGrid1.SelectedObject = settings;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    ((ApplicationPoolAdvancedSettings)propertyGrid1.SelectedObject).Apply(pool);
                    DialogResult = DialogResult.OK;
                }));
        }

        private void ApplicationPoolAdvancedSettingsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210456");
        }
    }
}
