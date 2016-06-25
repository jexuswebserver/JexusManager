// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DirectoryBrowse
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class DirectoryBrowseFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly DirectoryBrowseFeature _owner;

            public FeatureTaskList(DirectoryBrowseFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (!_owner.IsEnabled)
                {
                    result.Add(new MethodTaskItem("Enable", "Enable", string.Empty).SetUsage());
                }

                if (_owner.IsEnabled)
                {
                    result.Add(new MethodTaskItem("Disable", "Disable", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Enable()
            {
                _owner.Enable();
            }

            [Obfuscation(Exclude = true)]
            public void Disable()
            {
                _owner.Disable();
            }
        }

        public DirectoryBrowseFeature(Module module)
        {
            this.Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        private TaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (this.Module as IServiceProvider).GetService(type);
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/directoryBrowse");
            var enabled = (bool)section["enabled"];
            var flags = (long)section["showFlags"];
            TimeEnabled = (flags & 4) == 4;
            SizeEnabled = (flags & 8) == 8;
            ExtensionEnabled = (flags & 16) == 16;
            DateEnabled = (flags & 2) == 2;
            LongDateEnabled = (flags & 32) == 32;
            this.SetEnabled(enabled);
        }

        private void Enable()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationSection section = service.GetSection("system.webServer/directoryBrowse");
            section["enabled"] = true;
            this.SetEnabled(true);
        }

        private void Disable()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationSection section = service.GetSection("system.webServer/directoryBrowse");
            section["enabled"] = false;
            this.SetEnabled(false);
        }

        public void SetEnabled(bool enabled)
        {
            this.IsEnabled = enabled;
            this.OnDirectoryBrowseSettingsSaved();
        }

        protected void OnDirectoryBrowseSettingsSaved()
        {
            this.DirectoryBrowseSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210534");
            return false;
        }

        public DirectoryBrowseSettingsSavedEventHandler DirectoryBrowseSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }

        public string Name
        {
            get { return "Directory Browsing"; }
        }

        public bool IsEnabled { get; set; }

        public void CancelChanges()
        {
            this.Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/directoryBrowse");
            section["enabled"] = this.IsEnabled;
            long flags = 0;
            if (DateEnabled)
            {
                flags |= 2;
            }

            if (TimeEnabled)
            {
                flags |= 4;
            }

            if (SizeEnabled)
            {
                flags |= 8;
            }

            if (ExtensionEnabled)
            {
                flags |= 16;
            }

            if (LongDateEnabled)
            {
                flags |= 32;
            }

            section["showFlags"] = flags;
            service.ServerManager.CommitChanges();
            return true;
        }

        public bool LongDateEnabled { get; set; }

        public bool ExtensionEnabled { get; set; }

        public bool SizeEnabled { get; set; }

        public bool TimeEnabled { get; set; }

        public bool DateEnabled { get; set; }
    }
}
