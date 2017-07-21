// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Jexus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Resources;
    using System.Text;

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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service.ServerManager.Mode != WorkingMode.Jexus)
            {
                IsFeatureEnabled = false;
                return;
            }

            IsFeatureEnabled = true;
            var settings = service.Server == null ? service.Application.GetExtra() : service.Server.GetExtra();
            var text = new StringBuilder();
            foreach (var key in settings.Keys)
            {
                foreach (var item in settings[key])
                {
                    text.AppendFormat("{0}={1}", key, item).AppendLine();
                }
            }

            Contents = text.ToString();
            OnJexusSettingsSaved();
        }

        public string Contents { get; set; }

        protected void OnJexusSettingsSaved()
        {
            JexusSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("https://jexus.codeplex.com/wikipage?title=Configuration%20Files");
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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service.Server == null)
            {
                service.Application.GetExtra().Clear();
            }
            else
            {
                service.Server.GetExtra().Clear();
            }

            var reader = new StringReader(Contents);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var index = line.IndexOf('=');
                if (index == -1)
                {
                    continue;
                }

                var key = line.Substring(0, index).Trim();
                if (key.Length == 0)
                {
                    continue;
                }

                var value = line.Substring(index + 1).Trim();
                if (service.Server == null)
                {
                    var extra = service.Application.GetExtra();
                    if (extra.ContainsKey(key))
                    {
                        extra[key].Add(value);
                    }
                    else
                    {
                        extra.Add(key, new List<string> { value });
                    }
                }
                else
                {
                    var extra = service.Server.GetExtra();
                    if (extra.ContainsKey(key))
                    {
                        extra[key].Add(value);
                    }
                    else
                    {
                        extra.Add(key, new List<string> { value });
                    }
                }
            }

            service.ServerManager.CommitChanges();
            return true;
        }
    }
}
