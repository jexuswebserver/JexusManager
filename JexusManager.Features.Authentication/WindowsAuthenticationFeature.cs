// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Extensions;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Win32;
    using Module = Microsoft.Web.Management.Client.Module;

    internal class WindowsAuthenticationFeature : AuthenticationFeature
    {
        private FeatureTaskList _taskList;

        private sealed class FeatureTaskList : TaskList
        {
            private readonly WindowsAuthenticationFeature _owner;

            public FeatureTaskList(WindowsAuthenticationFeature owner)
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
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Edit", "Advanced Settings...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Providers", "Providers...", string.Empty).SetUsage());
                }

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
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Providers()
            {
                _owner.Providers();
            }
        }

        public WindowsAuthenticationFeature(Module module) : base(module)
        {
        }

        public override TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public override void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var windowsSection = service.GetSection("system.webServer/security/authentication/windowsAuthentication", null, false);
            var windowsEnabled = (bool)windowsSection["enabled"];
            SetEnabled(windowsEnabled);
        }

        public void Enable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var windowsSection = service.GetSection("system.webServer/security/authentication/windowsAuthentication", null, false);
            windowsSection["enabled"] = true;
            service.ServerManager.CommitChanges();
            SetEnabled(true);
        }

        public void Disable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var windowsSection = service.GetSection("system.webServer/security/authentication/windowsAuthentication", null, false);
            windowsSection["enabled"] = false;
            service.ServerManager.CommitChanges();
            SetEnabled(false);
        }

        private void Edit()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var windowsSection = service.GetSection("system.webServer/security/authentication/windowsAuthentication", null, false);
            var dialog = new WindowsAdvancedDialog(Module, new WindowsItem(windowsSection), this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
            OnAuthenticationSettingsSaved();
        }

        private void Providers()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var windowsSection = service.GetSection("system.webServer/security/authentication/windowsAuthentication", null, false);
            var dialog = new ProvidersDialog(Module, new WindowsItem(windowsSection), this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
            OnAuthenticationSettingsSaved();
        }

        public override Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210461#Windows");
            return true;
        }

        public override bool IsFeatureEnabled
        {
            get
            {
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                if (service.ServerManager.Mode == Microsoft.Web.Administration.WorkingMode.IisExpress)
                {
                    return true;
                }

                if (service.ServerManager.Mode == Microsoft.Web.Administration.WorkingMode.Iis)
                {
                    var reg = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp\Components");
                    if (reg == null)
                    {
                        return false;
                    }

                    return (int)reg.GetValue("WindowsAuthentication", 0) == 1;
                }

                return false;
            }
        }

        public override AuthenticationType AuthenticationType
        {
            get { return AuthenticationType.ChallengeBase; }
        }

        public override string Name
        {
            get { return "Windows Authentication"; }
        }
    }
}
