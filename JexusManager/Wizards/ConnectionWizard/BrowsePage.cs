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
        private SolutionTypePage _solutionTypePage;
        private FinishPage _finishPage;

        public BrowsePage(SolutionTypePage solutionTypePage, FinishPage finishPage)
        {
            InitializeComponent();
            _solutionTypePage = solutionTypePage;
            _finishPage = finishPage;
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

            var data = (ConnectionWizardData)WizardData;
            if (data.UseVisualStudio)
            {
                data.SolutionFile = txtName.Text;
            }
            else
            {
                data.FileName = txtName.Text;
            }

            UpdateWizard();
        }

        public override bool OnNext()
        {
            if (!IisExpressServerManager.ServerInstalled)
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                service.ShowMessage("No IIS Express installation detected. Please install IIS Express before moving on.", Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var data = (ConnectionWizardData)WizardData;
            if (!data.UseVisualStudio)
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

                var name = Path.GetFileName(data.FileName);
                var badNames = new[] { "administration.config", "redirection.config" };
                foreach (var bad in badNames)
                {
                    if (string.Equals(name, bad, StringComparison.OrdinalIgnoreCase))
                    {
                        var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                        var result = service.ShowMessage(
                            $"This file '{data.FileName}' does not seem to be a valid IIS configuration file. Do you want to continue?",
                            Caption,
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);
                        if (result != DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                }

                if (name.EndsWith(".exe.config", StringComparison.OrdinalIgnoreCase))
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    var result = service.ShowMessage(
                        $"This file '{data.FileName}' does not seem to be a valid IIS configuration file. Do you want to continue?",
                        Caption,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);
                    if (result != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                data.Server = new IisExpressServerManager(data.FileName);
            }

            return true;
        }

        protected internal override void Activate()
        {
            base.Activate();
            _initialized = false;
            ConnectionWizardData wizardData = (ConnectionWizardData)WizardData;
            txtName.Text = wizardData.UseVisualStudio ? wizardData.SolutionFile : wizardData.FileName;
            Caption = wizardData.UseVisualStudio ? "Specify a Visual Studio Solution File" : "Specify a Configuration File";
            txtType.Text = wizardData.UseVisualStudio ? "Visual Studio solution file name:" : "Configuration file name:";
            if (wizardData.UseVisualStudio)
            {
                SetNextPage(_solutionTypePage);
            }
            else
            {
                SetNextPage(_finishPage);
            }

            _initialized = true;
            txtName.Focus();
            txtName.SelectAll();
        }

        private void BtnBrowseClick(object sender, EventArgs e)
        {
            ConnectionWizardData wizardData = (ConnectionWizardData)WizardData;
            DialogHelper.ShowOpenFileDialog(
                txtName,
                wizardData.UseVisualStudio
                    ? "Solution Files|*.sln|All Files|*.*"
                    : "Config Files|*.config|All Files|*.*");
        }
    }
}
