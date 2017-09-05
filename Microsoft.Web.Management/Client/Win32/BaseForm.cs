// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class BaseForm : Form
#else
    public abstract class BaseForm : Form
#endif
    {
#if DESIGN
        public BaseForm() { }
#endif
        protected BaseForm(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected virtual void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)ServiceProvider.GetService(typeof(IManagementUIService));
            service.ShowError(ex, string.Empty, Text, false);
        }

        protected override void Dispose(bool disposing)
        {
            ServiceProvider = null;
        }

        protected internal new object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            base.OnHelpRequested(hevent);
        }

        protected virtual void OnInitialActivated(EventArgs e)
        {
        }

        protected void ShowError(Exception exception, string message, bool isWarning)
        {
            var service = (IManagementUIService)ServiceProvider.GetService(typeof(IManagementUIService));
            service.ShowError(exception, message, Text, isWarning);
        }

        protected virtual void ShowHelp()
        {
        }

        protected void ShowMessage(string message)
        {
            var service = (IManagementUIService)ServiceProvider.GetService(typeof(IManagementUIService));
            service.ShowMessage(message, Text);
        }

        protected DialogResult ShowMessage(string message, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            var service = (IManagementUIService)ServiceProvider.GetService(typeof(IManagementUIService));
            return service.ShowMessage(message, Text, buttons, icon, defaultButton);
        }

        protected new virtual void Update()
        {
            base.Update();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        protected virtual bool CanShowHelp { get; } = true;

        protected internal IServiceProvider ServiceProvider { get; private set; }
    }
}
