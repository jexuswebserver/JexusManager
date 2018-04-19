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
            if (!IisExpressServerManager.ServerInstalled)
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                service.ShowMessage("No IIS Express installation detected. Please install IIS Express before moving on.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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
                    service.ShowMessage("This solution does not contain IIS Express configuration file. Make sure you run the web project in Visual Studio 2015/2017 once for the configuration file to be generated. If you are using other versions of Visual Studio, you might check the sites under IIS Express server node.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                var iisExpress = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express", "AppServer", "applicationhost.config");
                var iisExpressX86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express", "AppServer", "applicationhost.config");
                var iisExpress2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express", "config", "templates", "PersonalWebServer", "applicationhost.config");
                var iisExpress2X86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express", "config", "templates", "PersonalWebServer", "applicationhost.config");
                if (string.Equals(iisExpress, data.FileName, StringComparison.OrdinalIgnoreCase) 
                    || string.Equals(iisExpressX86, data.FileName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(iisExpress2, data.FileName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(iisExpress2X86, data.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage("This file coming from IIS Express installation cannot be used. Please select another file.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var iis = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "config", "applicationHost.config");
                if (string.Equals(iis, data.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage("This file coming from IIS installation cannot be used. Please select another file.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

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
