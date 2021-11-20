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

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class IpSecurityFeature : FeatureBase<IpSecurityItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly IpSecurityFeature _owner;

            public FeatureTaskList(IpSecurityFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("AddAllow", "Add Allow Entry...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("AddDeny", "Add Deny Entry...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RemoveTaskItem);
                }

                result.Add(MethodTaskItem.CreateSeparator().SetUsage()); // IMPORTANT: this is where IIS Manager was wrong
                result.Add(new MethodTaskItem("Set", "Edit Feature Settings...", string.Empty).SetUsage());
                if (_owner.CanRevert)
                {
                    result.Add(new MethodTaskItem("Revert", "Revert To Parent", string.Empty).SetUsage());
                }

                result.Add(new MethodTaskItem("View", "View Ordered List...", string.Empty).SetUsage());
                if (_owner.IsFeatureEnabled)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Dynamic", "Edit Dynamic Restriction Settings...", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddAllow()
            {
                _owner.AddAllow();
            }

            [Obfuscation(Exclude = true)]
            public void AddDeny()
            {
                _owner.AddDeny();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Revert();
            }

            [Obfuscation(Exclude = true)]
            public void View()
            {
                _owner.View();
            }
            [Obfuscation(Exclude = true)]
            public void Dynamic()
            {
                _owner.Dynamic();
            }
        }

        public IpSecurityFeature(Module module)
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
            LoadItems();
        }

        public void AddAllow()
        {
            using var dialog = new NewRestrictionDialog(Module, true, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddDeny()
        {
            using var dialog = new NewRestrictionDialog(Module, false, this);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected restriction?", Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
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

        public void Set()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/ipSecurity", null, false);
            using (var dialog = new SetRestrictionsDialog(Module, section, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            service.ServerManager.CommitChanges();
        }

        public void View()
        {
        }

        public void Dynamic()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/dynamicIpSecurity", null, false);
            using (var dialog = new DynamicDialog(Module, section, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            service.ServerManager.CommitChanges();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection section = service.GetSection("system.webServer/security/ipSecurity", null, false);
            return section.GetCollection();
        }

        protected override void OnSettingsSaved()
        {
            IpSecuritySettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210513");
            return false;
        }

        public bool CanRevert { get; private set; }

        public IpSecuritySettingsSavedEventHandler IpSecuritySettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion => FxVersionNotRequired;

        public string Name => "IP Address and Domain Restrictions";

        public override bool IsFeatureEnabled
        {
            get
            {
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var section = service.GetSection("system.webServer/security/dynamicIpSecurity", null, false);
                return section != null;
            }
        }
    }
}
