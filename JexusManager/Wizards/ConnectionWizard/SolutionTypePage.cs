// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System.IO;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class SolutionTypePage : WizardPage
    {
        private string _vsFile;
        private string _riderFile;

        public SolutionTypePage()
        {
            InitializeComponent();
            Caption = "Choose Configuration Source";
            if (Helper.IsRunningOnMono())
            {
                rbVisualStudio.Enabled = false;
                rbRider.Enabled = false;
                toolTip1.SetToolTip(rbVisualStudio, "IIS Express is not supported when running on Mono.");
                toolTip1.SetToolTip(rbRider, "IIS Express is not supported when running on Mono.");
            }

            if (!IisExpressServerManager.ServerInstalled)
            {
                rbVisualStudio.Enabled = false;
                rbRider.Enabled = false;
                toolTip1.SetToolTip(rbVisualStudio, "IIS Express is not installed to the default location.");
                toolTip1.SetToolTip(rbRider, "IIS Express is not installed to the default location.");
            }
        }

        protected internal override void Activate()
        {
            base.Activate();
            ConnectionWizardData data = (ConnectionWizardData)WizardData;
            var folder = Path.GetDirectoryName(data.SolutionFile);
            var solutionName = Path.GetFileNameWithoutExtension(data.SolutionFile);
            _vsFile = Path.Combine(folder, ".vs", "config", "applicationHost.config");
            if (!File.Exists(_vsFile))
            {
                _vsFile = Path.Combine(folder, ".vs", solutionName, "config", "applicationHost.config");
            }

            _riderFile = Path.Combine(folder, ".idea", "config", "applicationHost.config");
            rbVisualStudio.Enabled = File.Exists(_vsFile);
            rbRider.Enabled = File.Exists(_riderFile);
            if (rbVisualStudio.Enabled)
            {
                rbVisualStudio.Checked = true;
            }
            else if (rbRider.Enabled)
            {
                rbRider.Checked = true;
            }
        }

        protected internal override bool CanNavigateNext => rbRider.Checked || rbVisualStudio.Checked;

        public override bool OnNext()
        {
            ConnectionWizardData data = (ConnectionWizardData)WizardData;
            data.FileName = rbRider.Checked ? _riderFile : _vsFile;
            data.Server = new IisExpressServerManager(data.FileName);
            return true;
        }
    }
}
