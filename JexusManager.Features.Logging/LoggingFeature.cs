// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class LoggingFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly LoggingFeature _owner;

            public FeatureTaskList(LoggingFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (!_owner.IsEnabled)
                {
                    result.Add(new MethodTaskItem("Enable", "Enable", string.Empty).SetUsage());
                }

                if (_owner.IsEnabled)
                {
                    result.Add(new MethodTaskItem("Disable", "Disable", string.Empty).SetUsage());
                }

                result.Add(new MethodTaskItem("View", "View Log Files...", string.Empty).SetUsage());
                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Enable()
            {
                _owner.Enable();
            }

            [Obfuscation(Exclude = true)]
            public void Disable()
            {
                _owner.Disable();
            }

            [Obfuscation(Exclude = true)]
            public void View()
            {
                _owner.View();
            }
        }

        public LoggingFeature(Module module)
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
            var section = service.GetSection("system.applicationHost/log", null, false);
            Mode = (long)section.Attributes["centralLogFileMode"].Value;
            Encoding = (bool)section.Attributes["logInUTF8"].Value ? 0 : 1;

            if (service.Server != null)
            {
                var section2 = service.GetSection("system.applicationHost/sites");
                var element = section2.ChildElements["siteDefaults"].ChildElements["logFile"];
                LogFormat = (long)element.Attributes["logFormat"].Value;
                Directory = element.Attributes["directory"].Value.ToString();
                if (element.Schema.AttributeSchemas["logTargetW3C"] != null)
                {
                    LogTargetW3C = (long)element.Attributes["logTargetW3C"].Value;
                }
                else
                {
                    LogTargetW3C = -1;
                }

                LocalTimeRollover = (bool)element.Attributes["localTimeRollover"].Value;
                TruncateSizeString = element.Attributes["truncateSize"].Value.ToString();
                Period = (long)element.Attributes["period"].Value;
            }
            else
            {
                var logFile = service.Application.GetSite().LogFile;
                LogFormat = (long)logFile.LogFormat;
                Directory = logFile.Directory;
                if (logFile.Schema.AttributeSchemas["logTargetW3C"] != null)
                {
                    LogTargetW3C = (long)logFile.LogTargetW3C;
                }
                else
                {
                    LogTargetW3C = -1;
                }

                LocalTimeRollover = logFile.LocalTimeRollover;
                TruncateSizeString = logFile.TruncateSize.ToString();
                Period = (long)logFile.Period;
            }

            CanBrowse = service.Application == null || service.Application.IsRoot();
            CanEncoding = service.Server != null;

            ConfigurationSection httpLoggingSection1 = service.GetSection("system.webServer/httpLogging", null, false);
            var dontLog = (bool)httpLoggingSection1["dontLog"];
            SetEnabled(!dontLog);
        }

        public long Period { get; set; }

        public string TruncateSizeString { get; set; }

        public bool LocalTimeRollover { get; set; }

        public long LogTargetW3C { get; set; }

        public bool CanBrowse { get; set; }

        public long LogFormat { get; set; }

        public string Directory { get; set; }

        public int Encoding { get; set; }

        public long Mode { get; set; }

        private void Enable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ConfigurationSection httpLoggingSection1 = service.GetSection("system.webServer/httpLogging", null, false);
            httpLoggingSection1["dontLog"] = false;
            SetEnabled(true);
        }

        private void Disable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ConfigurationSection httpLoggingSection1 = service.GetSection("system.webServer/httpLogging", null, false);
            httpLoggingSection1["dontLog"] = true;
            SetEnabled(false);
        }

        private void View()
        {
            var path = Directory.ExpandIisExpressEnvironmentVariables();
            if (System.IO.Directory.Exists(path))
            {
                DialogHelper.ProcessStart(path);
                return;
            }

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowMessage("The specific log directory is invalid.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal void SelectFields()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            SiteLogFile element;
            if (service.Server != null)
            {
                var section2 = service.GetSection("system.applicationHost/sites");
                var parent = section2.ChildElements["siteDefaults"];
                element = new SiteLogFile(parent.ChildElements["logFile"], parent);
            }
            else
            {
                element = service.Application.GetSite().LogFile;
            }

            var dialog = new FieldsDialog(Module, element);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
        }

        protected void OnLoggingSettingsSaved()
        {
            LoggingSettingsUpdated?.Invoke();
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
            OnLoggingSettingsSaved();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210517");
            return false;
        }

        public LoggingSettingsSavedEventHandler LoggingSettingsUpdated { get; set; }
        public string Description { get; }
        public bool IsEnabled { get; private set; }

        public virtual bool IsFeatureEnabled => true;

        public virtual Version MinimumFrameworkVersion => FxVersionNotRequired;

        public Module Module { get; }

        public string Name => "Logging";

        public bool CanEncoding { get; set; }

        public void CancelChanges()
        {
            Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service.Server != null)
            {
                var section = service.GetSection("system.applicationHost/log");
                section.Attributes["centralLogFileMode"].Value = Mode;
                section.Attributes["logInUTF8"].Value = Encoding == 0;

                var section2 = service.GetSection("system.applicationHost/sites");
                var element = section2.ChildElements["siteDefaults"].ChildElements["logFile"];
                element.Attributes["logFormat"].Value = LogFormat;
                element.Attributes["directory"].Value = Directory;
                if (element.Schema.AttributeSchemas["logTargetW3C"] != null)
                {
                    element.Attributes["logTargetW3C"].Value = LogTargetW3C;
                }

                element.Attributes["localTimeRollover"].Value = LocalTimeRollover;
                element.Attributes["truncateSize"].Value = Int64.Parse(TruncateSizeString);
                element.Attributes["period"].Value = Period;
            }
            else
            {
                var logFile = service.Application.GetSite().LogFile;
                logFile.LogFormat = (LogFormat)LogFormat;
                logFile.Directory = Directory;
                if (logFile.Schema.AttributeSchemas["logTargetW3C"] != null)
                {
                    logFile.LogTargetW3C = (LogTargetW3C)LogTargetW3C;
                }

                logFile.LocalTimeRollover = LocalTimeRollover;
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                long size;
                if (!long.TryParse(TruncateSizeString, out size))
                {
                    dialog.ShowMessage("The maximum file size must be a valid, positive integer.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (size < 1048576 || size > 4294967295)
                {
                    dialog.ShowMessage("The specified number is invalid. The valid range is between 1 MB and 4 GB.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                logFile.TruncateSize = size;
                logFile.Period = (LoggingRolloverPeriod)Period;
            }

            return true;
        }
    }
}
