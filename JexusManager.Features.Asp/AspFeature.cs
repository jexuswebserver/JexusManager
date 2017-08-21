// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Asp
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal class AspFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly AspFeature _owner;

            public FeatureTaskList(AspFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[0];
            }
        }

        public AspFeature(Module module)
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
            var section = service.GetSection("system.webServer/asp");
            PropertyGridObject = new AspItem(section);
            this.OnAspSettingsSaved();
        }

        public AspItem PropertyGridObject { get; set; }

        protected void OnAspSettingsSaved()
        {
            this.AspSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210534");
            return false;
        }

        public AspSettingsSavedEventHandler AspSettingsUpdated { get; set; }
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
        public ServerManager Server { get; set; }
        public Application Application { get; set; }

        public string Name
        {
            get { return "Asp"; }
        }

        public bool IsEnabled { get; set; }

        public void CancelChanges()
        {
            this.Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            PropertyGridObject.Apply();
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
