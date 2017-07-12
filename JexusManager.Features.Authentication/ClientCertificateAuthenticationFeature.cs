// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Extensions;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class ClientCertificateAuthenticationFeature : AuthenticationFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly ClientCertificateAuthenticationFeature _owner;

            public FeatureTaskList(ClientCertificateAuthenticationFeature owner)
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
        }

        private FeatureTaskList _taskList;

        public ClientCertificateAuthenticationFeature(Module module) : base(module)
        {
        }

        public override TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public override void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var anonymousSection = service.GetSection("system.webServer/security/authentication/clientCertificateMappingAuthentication", null, false);
            var anonymousEnabled = (bool)anonymousSection["enabled"];
            SetEnabled(anonymousEnabled);
        }

        private void Enable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var anonymousSection = service.GetSection("system.webServer/security/authentication/clientCertificateMappingAuthentication", null, false);
            anonymousSection["enabled"] = true;
            service.ServerManager.CommitChanges();
            SetEnabled(true);
        }

        private void Disable()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var anonymousSection = service.GetSection("system.webServer/security/authentication/clientCertificateMappingAuthentication", null, false);
            anonymousSection["enabled"] = false;
            service.ServerManager.CommitChanges();
            SetEnabled(false);
        }

        public override Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461#Active_Directory");
            return true;
        }

        public override bool IsFeatureEnabled
        {
            get { return true; }
        }

        public override AuthenticationType AuthenticationType
        {
            get { return AuthenticationType.ClientCertificateBased; }
        }

        public override string Name
        {
            get { return "Active Directory Client Certificate Authentication"; }
        }
    }
}