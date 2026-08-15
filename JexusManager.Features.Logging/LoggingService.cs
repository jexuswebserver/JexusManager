// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Logging
{
    internal sealed class LoggingService : ModuleService
    {
        [ModuleServiceMethod]
        public LoggingSettings GetSettings()
        {
            var settings = new LoggingSettings();
            var section = GetSection("system.webServer/httpLogging");
            settings.Enabled = !(bool)section["dontLog"];

            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                var logSection = ManagementUnit.Configuration.GetSection("system.applicationHost/log");
                settings.Mode = (long)logSection.Attributes["centralLogFileMode"].Value;
                settings.Encoding = (bool)logSection.Attributes["logInUTF8"].Value ? 0 : 1;

                var sitesSection = ManagementUnit.Configuration.GetSection("system.applicationHost/sites");
                var element = sitesSection.ChildElements["siteDefaults"].ChildElements["logFile"];
                settings.LogFormat = (long)element.Attributes["logFormat"].Value;
                settings.Directory = element.Attributes["directory"].Value.ToString();
                settings.LogTargetW3C = element.Schema.AttributeSchemas["logTargetW3C"] != null ? (long)element.Attributes["logTargetW3C"].Value : -1;
                settings.LocalTimeRollover = (bool)element.Attributes["localTimeRollover"].Value;
                settings.TruncateSizeString = element.Attributes["truncateSize"].Value.ToString();
                settings.Period = (long)element.Attributes["period"].Value;
            }
            else
            {
                settings.Mode = 0;
                settings.Encoding = 0;
                settings.LogFormat = 0;
                settings.Directory = string.Empty;
                settings.LogTargetW3C = -1;
                settings.LocalTimeRollover = false;
                settings.TruncateSizeString = string.Empty;
                settings.Period = 0;
            }

            return settings;
        }

        [ModuleServiceMethod]
        public void Apply(LoggingSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection("system.webServer/httpLogging");
            section["dontLog"] = !settings.Enabled;

            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                var logSection = ManagementUnit.Configuration.GetSection("system.applicationHost/log");
                logSection.Attributes["centralLogFileMode"].Value = settings.Mode;
                logSection.Attributes["logInUTF8"].Value = settings.Encoding == 0;

                var sitesSection = ManagementUnit.Configuration.GetSection("system.applicationHost/sites");
                var element = sitesSection.ChildElements["siteDefaults"].ChildElements["logFile"];
                element.Attributes["logFormat"].Value = settings.LogFormat;
                element.Attributes["directory"].Value = settings.Directory;
                if (element.Schema.AttributeSchemas["logTargetW3C"] != null)
                {
                    element.Attributes["logTargetW3C"].Value = settings.LogTargetW3C;
                }

                element.Attributes["localTimeRollover"].Value = settings.LocalTimeRollover;
                element.Attributes["truncateSize"].Value = long.Parse(settings.TruncateSizeString);
                element.Attributes["period"].Value = settings.Period;
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void SetEnabled(bool enabled)
        {
            var section = GetSection("system.webServer/httpLogging");
            section["dontLog"] = !enabled;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection(string sectionPath)
        {
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                return ManagementUnit.Configuration.GetSection(sectionPath);
            }

            var locationPath = ManagementUnit.ConfigurationPath.GetEffectiveConfigurationPath(ManagementUnit.Scope);
            return ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(sectionPath, locationPath);
        }
    }
}