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
        public HttpRedirectFeature(Module module)
        {
            this.Module = module;
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
            var settings = Proxy.GetSettings();
            this.Enabled = settings.Enabled; this.Link = settings.Destination; this.Exact = settings.ExactDestination;
            this.OnlyRoot = !settings.ChildOnly; this.Mode = settings.ResponseStatus;
            SupportedModes = new List<long>(settings.SupportedStatuses);

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
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210508");
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
            Proxy.Apply(new HttpRedirectSnapshot { Enabled = Enabled, Destination = Link, ExactDestination = Exact, ChildOnly = !OnlyRoot, ResponseStatus = Mode, SupportedStatuses = SupportedModes.ToArray() });
            return true;
        }

        private HttpRedirectModuleProxy Proxy => ((HttpRedirectModule)Module).Proxy;
    }
}
