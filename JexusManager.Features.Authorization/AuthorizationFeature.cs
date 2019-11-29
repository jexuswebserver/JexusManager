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

namespace JexusManager.Features.Authorization
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

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Authorization feature.
    /// </summary>
    internal class AuthorizationFeature : FeatureBase<AuthorizationRule>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly AuthorizationFeature _owner;

            public FeatureTaskList(AuthorizationFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("AddAllow", "Add Allow Rule...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("AddDeny", "Add Deny Rule...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Set", "Edit...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
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
                _owner.Edit();
            }
        }

        public AuthorizationFeature(Module module)
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

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/security/authorization");
            return section.GetCollection();
        }

        public void Load()
        {
            LoadItems();
        }

        public void AddAllow()
        {
            using var dialog = new NewRuleDialog(Module, null, true, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddDeny()
        {
            using var dialog = new NewRuleDialog(Module, null, false, this);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected authorization rule?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            using var dialog = new NewRuleDialog(Module, SelectedItem, false, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        protected override void OnSettingsSaved()
        {
            AuthorizationSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210462");
            return false;
        }

        public AuthorizationSettingsSavedEventHandler AuthorizationSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get { return "Authorization Rules"; }
        }
    }
}
