// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;
    using System.IO;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using System.Windows.Forms;

    public partial class BrowsePage : WizardPage
    {
        private bool _initialized;

        public BrowsePage()
        {
            InitializeComponent();
            Caption = "Specify a Configuration File";
        }

        protected internal override bool CanNavigateNext
        {
            get
            {
                return base.CanNavigateNext && File.Exists(txtName.Text);
            }
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            ((ConnectionWizardData)WizardData).FileName = txtName.Text;
            this.UpdateWizard();
        }

        public override bool OnNext()
        {
            var data = ((ConnectionWizardData)WizardData);
            if (data.FileName.EndsWith(".sln", StringComparison.OrdinalIgnoreCase))
            {
                var folder = Path.GetDirectoryName(data.FileName);
                var config = Path.Combine(folder, ".vs", "config", "applicationHost.config");
                if (File.Exists(config))
                {
                    data.Server = new IisExpressServerManager(config);
                    data.FileName = config;
                }
                else
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage("This solution does not contain IIS Express configuration file. Make sure you run the web project in Visual Studio 2015 once for the configuration file to be generated. If you are using other versions of Visual Studio, you might check the sites under IIS Express server node.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                data.Server = new IisExpressServerManager(data.FileName);
            }

            return true;
        }

        protected internal override void Activate()
        {
            base.Activate();
            _initialized = false;
            txtName.Text = ((ConnectionWizardData)this.WizardData).FileName;
            _initialized = true;
            txtName.Focus();
            txtName.SelectAll();
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            DialogHelper.ShowOpenFileDialog(txtName, "Common Files|*.config;*.sln|Config Files|*.config|Solution Files|*.sln|All Files|*.*");
        }
    }
}
