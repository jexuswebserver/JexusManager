// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpRedirect
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal class HttpRedirectFeature
    {
        public HttpRedirectFeature(Module module, ServerManager server, Application application)
        {
            this.Module = module;
            this.Server = server;
            this.Application = application;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (this.Module as IServiceProvider).GetService(type);
        }

        public void Load()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/httpRedirect");
            this.Enabled = (bool)section["enabled"];
            this.Link = (string)section["destination"];
            this.Exact = (bool)section["exactDestination"];
            this.OnlyRoot = !(bool)section["childOnly"];
            this.Mode = (long)section["httpResponseStatus"];

            SupportedModes = new List<long>();
            foreach (ConfigurationEnumValue item in section.Schema.AttributeSchemas["httpResponseStatus"].GetEnumValues())
            {
                SupportedModes.Add(item.Value);
            }

            this.OnHttpRedirectSettingsSaved();
        }

        public List<long> SupportedModes { get; set; }

        public string Directory { get; set; }

        protected void OnHttpRedirectSettingsSaved()
        {
            this.HttpRedirectSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210508");
            return false;
        }

        public HttpRedirectSettingsSavedEventHandler HttpRedirectSettingsUpdated { get; set; }
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
            get { return "HTTP Redirect"; }
        }


        public bool Enabled { get; set; }
        public string Link { get; set; }
        public bool Exact { get; set; }
        public bool OnlyRoot { get; set; }
        public long Mode { get; set; }

        public void CancelChanges()
        {
            this.Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/httpRedirect");
            section["enabled"] = this.Enabled;
            section["destination"] = this.Link;
            section["exactDestination"] = this.Exact;
            section["childOnly"] = !this.OnlyRoot;
            if (section.Schema.AttributeSchemas["httpResponseStatus"].GetEnumValues().GetName(this.Mode) == null)
            {
                section["httpResponseStatus"] = 301;
            }
            else
            {
                section["httpResponseStatus"] = this.Mode;
            }

            service.ServerManager.CommitChanges();
            return true;
        }
    }
}
