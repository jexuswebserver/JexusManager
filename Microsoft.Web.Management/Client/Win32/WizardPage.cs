// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class WizardPage : UserControl
#else
    public abstract class WizardPage : UserControl
#endif
    {
        protected internal virtual void Activate()
        {
        }

        protected override object GetService(Type serviceType)
        {
            return Wizard?.GetService(serviceType) ?? base.GetService(serviceType);
        }

        public virtual bool OnNext()
        {
            return NextPage != null;
        }

        public virtual bool OnPrevious()
        {
            return PreviousPage != null;
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
        }

        internal void SetNextPage(WizardPage page)
        {
            NextPage = page;
        }

        internal void SetPreviousPage(WizardPage page)
        {
            PreviousPage = page;
        }

        internal void SetWizard(WizardForm wizard)
        {
            Wizard = wizard;
        }

        protected void ShowError(Exception exception, string message, bool isWarning)
        {
            var service = (IManagementUIService)Wizard.GetService(typeof(IManagementUIService));
            service.ShowError(exception, message, Caption, isWarning);
        }

        protected internal virtual void ShowHelp()
        {
        }

        protected void ShowMessage(string text)
        {
            var service = (IManagementUIService)Wizard.GetService(typeof(IManagementUIService));
            service.ShowMessage(text, Caption);
        }

        protected DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            var service = (IManagementUIService)Wizard.GetService(typeof(IManagementUIService));
            return service.ShowMessage(text, Caption, buttons, icon, defaultButton);
        }

        protected void UpdateWizard()
        {
            Wizard.UpdateWizard();
        }

        protected internal virtual bool CanNavigateNext
        {
            get { return NextPage != null; }
        }

        protected internal virtual bool CanNavigatePrevious
        {
            get { return PreviousPage != null; }
        }
        protected internal virtual bool CanShowHelp { get; }

        [Category("Appearance"), DefaultValue("")]
        public string Caption { get; set; }

        protected override CreateParams CreateParams
        {
            get { return base.CreateParams; }
        }

        [Category("Appearance"), DefaultValue("")]
        public string Description { get; set; }
        protected internal virtual WizardPage NextPage { get; private set; }

        protected IList Pages
        {
            get { return Wizard.Pages; }
        }

        protected internal virtual WizardPage PreviousPage { get; private set; }
        public bool RightToLeftLayout { get; set; }

        protected IServiceProvider ServiceProvider
        {
            get { return Wizard.ServiceProvider; }
        }

        protected WizardForm Wizard { get; private set; }

        protected object WizardData
        {
            get { return Wizard.WizardData; }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // WizardPage
            // 
            Name = "WizardPage";
            Size = new Size(593, 452);
            ResumeLayout(false);
        }
    }
}
