// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Jexus
{
    using System;
    using System.Diagnostics;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal class JexusFeature
    {
        public JexusFeature(Module module)
        {
            Module = module;
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
            var settings = ((JexusModule)Module).Proxy.GetSettings();
            IsFeatureEnabled = settings.IsAvailable;
            Contents = settings.Contents;
            OnJexusSettingsSaved();
        }

        public string Contents { get; set; }

        protected void OnJexusSettingsSaved()
        {
            JexusSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("https://server.jexusmanager.com/en/latest/tutorials/configuration.html");
            return false;
        }

        public JexusSettingsSavedEventHandler JexusSettingsUpdated { get; set; }
        public string Description { get; }

        public bool IsFeatureEnabled { get; private set; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }

        public string Name
        {
            get
            {
                return "Jexus Specific";
            }
        }

        public void CancelChanges()
        {
            Load();
        }

        public bool ApplyChanges()
        {
            ((JexusModule)Module).Proxy.Apply(Contents);
            return true;
        }
    }
}
