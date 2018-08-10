// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class TypePage : WizardPage
    {
        private readonly ServerPage _jexusPage;

        private readonly BrowsePage _iisExpressPage;

        public TypePage(ServerPage jexusPage, BrowsePage iisExpressPage)
        {
            _jexusPage = jexusPage;
            _iisExpressPage = iisExpressPage;
            InitializeComponent();
            Caption = "Choose Server Type";
            if (Helper.IsRunningOnMono())
            {
                rbIisExpress.Enabled = false;
                rbVisualStudio.Enabled = false;
                toolTip1.SetToolTip(rbIisExpress, "IIS Express is not supported when running on Mono.");
                toolTip1.SetToolTip(rbVisualStudio, "IIS Express is not supported when running on Mono.");
            }

            if (Helper.GetIisExpressVersion() == Version.Parse("0.0.0.0"))
            {
                rbIisExpress.Enabled = false;
                rbVisualStudio.Enabled = false;
                toolTip1.SetToolTip(rbIisExpress, "IIS Express is not installed to the default location.");
                toolTip1.SetToolTip(rbVisualStudio, "IIS Express is not installed to the default location.");
            }
        }

        private void RbJexusCheckedChanged(object sender, EventArgs e)
        {
            var wizardData = (ConnectionWizardData)WizardData;
            if (rbJexus.Checked)
            {
                wizardData.Mode = WorkingMode.Jexus;
            }
            else if (rbIisExpress.Checked)
            {
                wizardData.Mode = WorkingMode.IisExpress;
            }
            else if (rbVisualStudio.Checked)
            {
                wizardData.Mode = WorkingMode.IisExpress;
            }
            else
            {
                wizardData.Mode = WorkingMode.Iis;
            }

            wizardData.UseVisualStudio = rbVisualStudio.Checked;
            if (wizardData.Mode == WorkingMode.Jexus)
            {
                SetNextPage(_jexusPage);
            }
            else if (wizardData.Mode == WorkingMode.IisExpress)
            {
                SetNextPage(_iisExpressPage);
            }

            UpdateWizard();
        }
    }
}
