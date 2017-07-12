// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Extensions;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class FormsAuthenticationFeature : AuthenticationFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly FormsAuthenticationFeature _owner;

            public FeatureTaskList(FormsAuthenticationFeature owner)
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

                result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
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
        }

        private FeatureTaskList _taskList;

        public FormsAuthenticationFeature(Module module)
            : base(module)
        {
        }

        public override TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public override void Load()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.web/Authentication", null, false);
            var enabled = 3L == (long)section["mode"];
            this.SetEnabled(enabled);
        }

        private void Enable()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.web/Authentication", null, false);
            section["mode"] = "Forms";
            service.ServerManager.CommitChanges();
            this.SetEnabled(true);
        }

        private void Disable()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.web/Authentication", null, false);
            section["mode"] = "Windows";
            service.ServerManager.CommitChanges();
            this.SetEnabled(false);
        }

        private void Edit()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.web/Authentication", null, false);
            var dialog = new FormsEditDialog(this.Module, new FormsItem(section.GetChildElement("forms")));
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
            this.OnAuthenticationSettingsSaved();
        }

        public override Version MinimumFrameworkVersion
        {
            get { return FxVersion20; }
        }

        public override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461#Forms");
            return true;
        }

        public override bool IsFeatureEnabled
        {
            get { return true; }
        }

        public override AuthenticationType AuthenticationType
        {
            get { return AuthenticationType.LoginRedirectBased; }
        }

        public override string Name
        {
            get { return "Forms Authentication"; }
        }
    }
}
