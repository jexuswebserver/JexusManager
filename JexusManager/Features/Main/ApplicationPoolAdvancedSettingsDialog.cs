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

    public partial class ApplicationPoolAdvancedSettingsDialog : DialogForm
    {
        private ApplicationPool _pool;
        public ApplicationPoolAdvancedSettingsDialog(IServiceProvider serviceProvider, ApplicationPool pool)
            : base(serviceProvider)
        {
            InitializeComponent();
            _pool = pool;
            var settings = new ApplicationPoolAdvancedSettings(pool);
            propertyGrid1.SelectedObject = settings;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ((ApplicationPoolAdvancedSettings)propertyGrid1.SelectedObject).Apply(_pool);
            DialogResult = DialogResult.OK;
        }

        private void ApplicationPoolAdvancedSettingsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210456");
        }
    }
}
