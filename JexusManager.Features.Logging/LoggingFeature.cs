// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
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
            var settings = ((LoggingModule)Module).Proxy.GetSettings();
            Mode = settings.Mode;
            Encoding = settings.Encoding;
            LogFormat = settings.LogFormat;
            Directory = settings.Directory;
            LogTargetW3C = settings.LogTargetW3C;
            LocalTimeRollover = settings.LocalTimeRollover;
            TruncateSizeString = settings.TruncateSizeString;
            Period = settings.Period;
            SetEnabled(settings.Enabled);

            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service != null)
            {
                CanBrowse = service.Application == null || service.Application.IsRoot();
                CanEncoding = service.Server != null;
            }
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

        public Fields Fields { get; set; }

        private void Enable()
        {
            ((LoggingModule)Module).Proxy.SetEnabled(true);
            SetEnabled(true);
        }

        private void Disable()
        {
            ((LoggingModule)Module).Proxy.SetEnabled(false);
            SetEnabled(false);
        }

        private void View()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var path = Directory.ExpandIisExpressEnvironmentVariables(
                service.Server != null ? null : service.Application.GetActualExecutable());
            if (System.IO.Directory.Exists(path))
            {
                DialogHelper.ProcessStart(path);
                return;
            }

            var ui = (IManagementUIService)GetService(typeof(IManagementUIService));
            ui.ShowMessage("The specific log directory is invalid.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal bool SelectFields()
        {
            using var dialog = new FieldsDialog(Module, Fields);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            return true;
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
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                return false;
            }

            var settings = new LoggingSettings
            {
                Enabled = IsEnabled,
                Mode = Mode,
                Encoding = Encoding,
                LogFormat = LogFormat,
                Directory = Directory,
                LogTargetW3C = LogTargetW3C,
                LocalTimeRollover = LocalTimeRollover,
                TruncateSizeString = TruncateSizeString,
                Period = Period
            };

            ((LoggingModule)Module).Proxy.Apply(settings);
            if (service.Server == null && Fields != null)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                if (!long.TryParse(TruncateSizeString, out var size))
                {
                    dialog.ShowMessage("The maximum file size must be a valid, positive integer.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (size < 1048576 || size > 4294967295)
                {
                    dialog.ShowMessage("The specified number is invalid. The valid range is between 1 MB and 4 GB.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                service.ServerManager.CommitChanges();
            }

            return true;
        }
    }
}
