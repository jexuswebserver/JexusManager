// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Resources;
using Microsoft.Web.Management.Client.Win32;

namespace Microsoft.Web.Management.Client.Extensions
{
    public abstract class AuthenticationFeature
    {
        protected AuthenticationFeature(Module module)
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

        public abstract TaskList GetTaskList();

        public abstract void Load();

        protected void OnAuthenticationSettingsSaved()
        {
            AuthenticationSettingsUpdated?.Invoke();
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
            OnAuthenticationSettingsSaved();
        }

        public virtual bool ShowHelp()
        {
            return false;
        }

        public AuthenticationSettingsSavedEventHandler AuthenticationSettingsUpdated { get; set; }
        public abstract AuthenticationType AuthenticationType { get; }
        public virtual string Description { get; }
        public bool IsEnabled { get; private set; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }
        public abstract string Name { get; }
    }
}
