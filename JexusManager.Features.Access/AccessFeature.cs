// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Access
{
    using System;
    using System.Diagnostics;
    using System.Resources;

    using Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal class AccessFeature
    {
        public AccessFeature(Module module, ServerManager server, Application application)
        {
            Module = module;
            Server = server;
            Application = application;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/access", null, false);
            SslFlags = (long)section["sslFlags"];

            OnAccessSettingsSaved();
        }

        public string Directory { get; set; }

        protected void OnAccessSettingsSaved()
        {
            AccessSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210534");
            return false;
        }

        public AccessSettingsSavedEventHandler AccessSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual bool IsFeatureEnabled => true;

        public virtual Version MinimumFrameworkVersion => FxVersionNotRequired;

        public Module Module { get; }
        public ServerManager Server { get; set; }
        public Application Application { get; set; }

        public string Name => "SSL Settings";


        public long SslFlags { get; set; }

        public void CancelChanges()
        {
            Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/access", null, false);
            section["sslFlags"] = SslFlags;
            service.ServerManager.CommitChanges();
            return true;
        }
    }
}
