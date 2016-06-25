// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Resources;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
    using System.Text;

#if DESIGN
    public class ModulePage : UserControl, IModulePage
#else
    public abstract class ModulePage : ContainerControl,
        IModulePage
#endif
    {
        private TaskListCollection _tasks;

        protected ModulePage()
        {
            SetScrollState(1, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected ModuleServiceProxy CreateProxy(Type proxyType)
        {
            return null;
        }

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
        }

        protected void DisplayErrorMessage(string errorText, string errorMessage)
        {
        }

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager, string caption)
        {
        }

        protected void DisplayErrorMessage(string errorText, string errorMessage, string caption)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }

        protected bool EditProperties(
            IPropertyEditingUser user,
            Object targetObject,
            string titleText,
            string helpText)
        {
            return false;
        }

        protected bool EditProperties(
            IPropertyEditingUser user,
            Object targetObject,
            string titleText,
            string helpText,
            EventHandler showHelp)
        {
            return false;
        }

        protected Type GetPageType(string moduleName)
        {
            return null;
        }

        protected static string GetScopeStatusSummary(Connection connection, string configurationPath, string locationSubPath)
        {
            return null;
        }

        protected new Object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        protected virtual void Initialize(Object navigationData)
        {
        }

        protected virtual void LoadPreferences(PreferencesStore store)
        {
        }

        protected bool Navigate(Type pageType)
        {
            return Navigate(pageType, null);
        }

        protected bool Navigate(Type pageType, object navigationData)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, pageType, navigationData);
            return false;
        }

        protected virtual void OnActivated(bool initialActivation)
        {
        }

        protected virtual void OnDeactivating(CancelEventArgs e)
        {
        }

        protected new virtual void Refresh()
        {
            base.Refresh();
        }

        protected virtual void SavePreferences(PreferencesStore store)
        {
        }

        protected DialogResult ShowDialog(DialogForm form)
        {
            return form.ShowDialog();
        }

        protected void ShowError(Exception exception, bool isWarning)
        {
            ShowError(exception, null, isWarning);
        }

        protected void ShowError(string message, bool isWarning)
        {
            ShowError(null, message, isWarning);
        }

        protected void ShowError(Exception exception, string message, bool isWarning)
        {
            ShowError(exception, message, isWarning, Text);
        }

        protected void ShowError(Exception exception, string message, bool isWarning, string caption)
        {
            var display = new StringBuilder(message ?? "There was an error while performing this operation.");
            if (exception != null)
            {
                display.AppendLine().AppendLine().AppendLine("Details:").AppendLine().Append(exception.Message);
            }

            ShowMessage(
                display.ToString(),
                MessageBoxButtons.OK,
                isWarning ? MessageBoxIcon.Warning : MessageBoxIcon.Error,
                caption);
        }

        protected virtual bool ShowHelp()
        {
            return false;
        }

        protected void ShowMessage(string text)
        {
            ShowMessage(text, Text);
        }

        protected void ShowMessage(string text, string caption)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            dialog.ShowMessage(text, caption);
        }

        protected DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return ShowMessage(text, buttons, icon, Text);
        }

        protected DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon, string caption)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            return dialog.ShowMessage(text, caption, buttons, icon);
        }

        protected DialogResult ShowMessage(string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return ShowMessage(text, buttons, icon, defaultButton, Text);
        }

        protected DialogResult ShowMessage(
            string text,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton,
            string caption)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            return dialog.ShowMessage(text, caption, buttons, icon, defaultButton);
        }

        protected virtual bool ShowOnlineHelp()
        {
            return false;
        }

        protected void StartAsyncTask(
            string progressText,
            DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler)
        {
        }

        protected void StartAsyncTask(
            string progressText,
            DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler,
            MethodInvoker cancelTaskHandler)
        {
        }

        protected void StartAsyncTask(string progressText,
            DoWorkEventHandler doWorkHandler,
            RunWorkerCompletedEventHandler workCompletedHandler,
            MethodInvoker cancelTaskHandler,
            Object argument)
        {
        }

        protected void StartProgress(string text, MethodInvoker cancelMethod)
        {
        }

        protected void StopProgress()
        {
        }

        protected new void Update()
        {
        }

        void IModulePage.Initialize(Module module, ModulePageInfo pageInfo, object navigationData)
        {
            Module = module;
            PageInfo = pageInfo;
            Text = this.PageInfo?.Title;
            try
            {
                Initialize(navigationData);
            }
            catch (Exception ex)
            {
                ShowError(ex, false);
            }
        }

        void IModulePage.OnActivated(bool initialActivation)
        {
            OnActivated(initialActivation);
        }

        void IModulePage.OnDeactivating(CancelEventArgs e)
        {
            OnDeactivating(e);
        }

        void IModulePage.Refresh()
        {
            Refresh();
        }

        bool IModulePage.ShowHelp()
        {
            return ShowHelp();
        }

        protected virtual bool CanRefresh
        {
            get { return false; }
        }

        protected Connection Connection { get; }
        public override ContextMenuStrip ContextMenuStrip { get; set; }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 65536;
                createParams.ExStyle &= -513;
                createParams.Style &= -8388609;
                createParams.Style |= 8388608;
                return createParams;
            }
        }

        protected virtual bool HasChanges { get; }
        public bool InProgress { get; }
        public virtual new Padding Margin { get; }
        protected Module Module { get; private set; }
        protected ModulePageInfo PageInfo { get; private set; }
        protected virtual Guid PreferenceKey { get; }
        protected virtual bool ReadOnly { get; }
        protected virtual string ReadOnlyDescription { get; }
        public bool RightToLeftLayout { get; set; }
        public virtual string ScopeStatusSummary { get; }

        protected IServiceProvider ServiceProvider
        {
            get { return Module; }
        }

        protected virtual bool ShowTaskList
        {
            get { return true; }
        }

        protected virtual TaskListCollection Tasks
        {
            get { return _tasks ?? (_tasks = new TaskListCollection()); }
        }

        bool IModulePage.CanRefresh
        {
            get { return CanRefresh; }
        }

        bool IModulePage.HasChanges
        {
            get { return HasChanges; }
        }

        ModulePageInfo IModulePage.PageInfo
        {
            get { return PageInfo; }
        }

        TaskListCollection IModulePage.Tasks
        {
            get { return Tasks; }
        }
    }
}
