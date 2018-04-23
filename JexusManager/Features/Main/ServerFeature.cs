// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class ServerFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly ServerFeature _owner;

            public FeatureTaskList(ServerFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new TextTaskItem("Manage Server", string.Empty, true));
                result.Add(
                    new MethodTaskItem("Restart", "Restart", string.Empty, string.Empty, Resources.restart_16).SetUsage(
                        !_owner.IsBusy));
                result.Add(
                    new MethodTaskItem("Start", "Start", string.Empty, string.Empty, Resources.start_16).SetUsage(
                        !_owner.IsBusy && !_owner.IsStarted));
                result.Add(
                    new MethodTaskItem("Stop", "Stop", string.Empty, string.Empty, Resources.stop_16).SetUsage(
                        !_owner.IsBusy && _owner.IsStarted));
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("ApplicationPools", "View Application Pools", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("Sites", "View Sites", string.Empty).SetUsage());
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("FxVersion", "Change .NET CLR Version", string.Empty).SetUsage());
                result.Add(new TextTaskItem("Troubleshooting", string.Empty, true));
                result.Add(
                    new MethodTaskItem("SslDiag", "SSL Diagnostics", string.Empty).SetUsage(_owner.SupportSslDiag));

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void ApplicationPools()
            {
                _owner.ApplicationPools();
            }

            [Obfuscation(Exclude = true)]
            public void Sites()
            {
                _owner.Sites();
            }

            [Obfuscation(Exclude = true)]
            public void Restart()
            {
                _owner.Restart();
            }

            [Obfuscation(Exclude = true)]
            public void Start()
            {
                _owner.Start();
            }

            [Obfuscation(Exclude = true)]
            public void Stop()
            {
                _owner.Stop();
            }

            [Obfuscation(Exclude = true)]
            public void FxVersion()
            {
                _owner.FxVersion();
            }

            [Obfuscation(Exclude = true)]
            public void SslDiag()
            {
                _owner.SslDiag();
            }
        }

        public ServerFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

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
            if (service.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)service.ServerManager;
                this.IsStarted = AsyncHelper.RunSync(() => server.GetStatusAsync());
            }

            SupportSslDiag = service.ServerManager.Mode != WorkingMode.Jexus;
            IsBusy = service.ServerManager.Mode != WorkingMode.Jexus; // TODO: how to start/stop IIS?
            OnServerSettingsSaved();
        }

        protected void OnServerSettingsSaved()
        {
            ServerSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210463");
            return false;
        }

        private void FxVersion()
        {
        }

        private void SslDiag()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = new SslDiagDialog(Module, service.ServerManager);
            dialog.ShowDialog();
        }

        private void Stop()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            IsBusy = true;
            OnServerSettingsSaved();
            if (service.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)service.ServerManager;
                AsyncHelper.RunSync(() => server.StopAsync());
                this.IsStarted = AsyncHelper.RunSync(() => server.GetStatusAsync());
            }
            else
            {
                IsStarted = false;
            }

            IsBusy = false;
            OnServerSettingsSaved();
        }

        private void Start()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            IsBusy = true;
            OnServerSettingsSaved();
            if (service.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)service.ServerManager;
                AsyncHelper.RunSync(() => server.StartAsync());
                this.IsStarted = AsyncHelper.RunSync(() => server.GetStatusAsync());
            }
            else
            {
                IsStarted = true;
            }

            IsBusy = false;
            OnServerSettingsSaved();
        }

        private void Restart()
        {
            IsBusy = true;
            OnServerSettingsSaved();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)service.ServerManager;
                AsyncHelper.RunSync(() => server.StopAsync());
                AsyncHelper.RunSync(() => server.StartAsync());
                this.IsStarted = AsyncHelper.RunSync(() => server.GetStatusAsync());
            }
            else
            {
                IsStarted = true;
            }

            IsBusy = false;
            OnServerSettingsSaved();
        }

        private void Sites()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ((MainForm)service.Form).LoadSites();
        }

        private void ApplicationPools()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ((MainForm)service.Form).LoadPools();
        }

        public bool IsStarted { get; set; }

        public bool SupportSslDiag { get; set; }

        public bool IsBusy { get; set; }

        public ServerSettingsSavedEventHandler ServerSettingsUpdated { get; set; }
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
        public string Name { get; }
    }
}
