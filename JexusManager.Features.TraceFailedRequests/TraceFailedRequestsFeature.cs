// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace JexusManager.Features.TraceFailedRequests
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Module = Microsoft.Web.Management.Client.Module;
    using JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard;

    /// <summary>
    /// Description of TraceFailedRequests feature.
    /// </summary>
    internal class TraceFailedRequestsFeature : FeatureBase<TraceFailedRequestsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly TraceFailedRequestsFeature _owner;

            public FeatureTaskList(TraceFailedRequestsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (_owner.IsInOrder)
                {
                    result.Add(GetMoveUpTaskItem(_owner.CanMoveUp));
                    result.Add(GetMoveDownTaskItem(_owner.CanMoveDown));
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Unorder", "View Unordered List...", string.Empty).SetUsage());
                }
                else
                {
                    result.Add(new MethodTaskItem("Add", "Add...", string.Empty).SetUsage());
                    if (_owner.SelectedItem != null)
                    {
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                        result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                        result.Add(RemoveTaskItem);
                    }

                    result.Add(MethodTaskItem.CreateSeparator().SetUsage(true, MethodTaskItemUsages.TaskList));
                    if (_owner.CanRevert)
                    {
                        result.Add(new MethodTaskItem("Revert", "Revert to Parent", string.Empty).SetUsage());
                    }

                    result.Add(new MethodTaskItem("InOrder", "View Ordered List...", string.Empty).SetUsage());

                    if (_owner.IsSite)
                    {
                        result.Add(new MethodTaskItem("ViewTraceLogs", "View Trace Logs...", string.Empty).SetUsage());
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new MethodTaskItem("Set", "Edit Site Tracing...", string.Empty).SetUsage());
                    }
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveUp()
            {
                _owner.MoveUp();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveDown()
            {
                _owner.MoveDown();
            }

            [Obfuscation(Exclude = true)]
            public void InOrder()
            {
                _owner.InOrder();
            }

            [Obfuscation(Exclude = true)]
            public void Unorder()
            {
                _owner.Unorder();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Revert();
            }

            [Obfuscation(Exclude = true)]
            public void ViewTraceLogs()
            {
                _owner.ViewTraceLogs();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }
        }

        public TraceFailedRequestsFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            CanRevert = service.Scope != ManagementScope.Server;
            IsInOrder = false;
            IsSite = service.Scope == ManagementScope.Site;
            LoadItems();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection section = service.GetSection("system.webServer/tracing/traceFailedRequests");
            return section.GetCollection();
        }

        public void Add()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = new AddTraceWizard(Module, null, service.GetSection("system.webServer/tracing/traceProviderDefinitions"), this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected faile request tracing rule?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = new AddTraceWizard(Module, SelectedItem, service.GetSection("system.webServer/tracing/traceProviderDefinitions"), this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public void Rename()
        {
        }

        public void MoveUp()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveUpItem();
        }

        public void MoveDown()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveDownItem();
        }

        public void InOrder()
        {
            IsInOrder = true;
            OnSettingsSaved();
        }

        public void Unorder()
        {
            IsInOrder = false;
            OnSettingsSaved();
        }

        public void Revert()
        {
            if (!CanRevert)
            {
                throw new InvalidOperationException("Revert operation cannot be done at server level");
            }

            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result =
                dialog.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            RevertItems();
        }

        public void ViewTraceLogs()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.Site.TraceFailedRequestsLogging;
            var path = section.Directory.ExpandIisExpressEnvironmentVariables(
                service.Server != null ? null : service.Application.GetActualExecutable());
            if (System.IO.Directory.Exists(path))
            {
                DialogHelper.ProcessStart(path);
                return;
            }

            var ui = (IManagementUIService)GetService(typeof(IManagementUIService));
            ui.ShowMessage("The specific log directory is invalid.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void Set()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.Site.TraceFailedRequestsLogging;
            var dialog = new SettingsDialog(Module, section, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        protected override void OnSettingsSaved()
        {
            TraceFailedRequestsSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210482");
            return false;
        }

        public bool IsInOrder { get; private set; }

        public bool CanRevert { get; private set; }

        public bool IsSite { get; private set; }

        public TraceFailedRequestsSettingsSavedEventHandler TraceFailedRequestsSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get { return "Failed Request Tracing Rules"; }
        }
    }
}
