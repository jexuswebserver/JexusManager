// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Cgi
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal class CgiFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly CgiFeature _owner;

            public FeatureTaskList(CgiFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[0];
            }
        }

        public CgiFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        private TaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/cgi", null, false);
            PropertyGridObject = new CgiItem(section);
            OnCgiSettingsSaved();
        }

        public CgiItem PropertyGridObject { get; set; }

        protected void OnCgiSettingsSaved()
        {
            CgiSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210534");
            return false;
        }

        public CgiSettingsSavedEventHandler CgiSettingsUpdated { get; set; }
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
            get { return "Cgi"; }
        }

        public bool IsEnabled { get; set; }

        public void CancelChanges()
        {
            Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
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
