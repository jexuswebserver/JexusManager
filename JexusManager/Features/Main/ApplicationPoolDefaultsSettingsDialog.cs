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

    public partial class ApplicationPoolDefaultsSettingsDialog : DialogForm
    {
        private ApplicationPoolDefaults _defaults;
        public ApplicationPoolDefaultsSettingsDialog(IServiceProvider serviceProvider, ApplicationPoolDefaults defaults)
            : base(serviceProvider)
        {
            InitializeComponent();
            _defaults = defaults;
            var settings = new ApplicationPoolDefaultsSettings(defaults);
            propertyGrid1.SelectedObject = settings;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ((ApplicationPoolDefaultsSettings)propertyGrid1.SelectedObject).Apply(_defaults);
            DialogResult = DialogResult.OK;
        }

        private void ApplicationPoolAdvancedSettingsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210456");
        }
    }
}
