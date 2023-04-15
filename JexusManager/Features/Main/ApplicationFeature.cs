// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace JexusManager.Features.Main
{
    using JexusManager.Dialogs;
    using JexusManager.Main.Properties;
    using JexusManager.Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class ApplicationFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly ApplicationFeature _owner;

            public FeatureTaskList(ApplicationFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16).SetUsage());
                result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(
                    new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                        Resources.basic_settings_16).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("VirtualDirectories", "View Virtual Directories", string.Empty).SetUsage());

                if (_owner.SiteBindings.Any(item => item.CanBrowse))
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    var manageGroup = new GroupTaskItem(string.Empty, "Manage Application", string.Empty, true);
                    result.Add(manageGroup);
                    manageGroup.Items.Add(new TextTaskItem("Browse Application", string.Empty, true));
                    foreach (Binding binding in _owner.SiteBindings)
                    {
                        if (binding.CanBrowse)
                        {
                            var uri = binding.ToUri();
                            manageGroup.Items.Add(
                                new MethodTaskItem("Browse", $"Browse {binding.ToShortString()}",
                                    string.Empty, string.Empty,
                                    Resources.browse_16, uri).SetUsage());
                        }
                    }

                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());
                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Troubleshooting", string.Empty, true));
                    manageGroup.Items.Add(new MethodTaskItem("FixKestrel", "ASP.NET Core Diagnostics", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("FixPhp", "PHP Diagnostics", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Explore()
            {
                _owner.Explore();
            }

            [Obfuscation(Exclude = true)]
            public void Permissions()
            {
                _owner.Permissions();
            }

            [Obfuscation(Exclude = true)]
            public void Basic()
            {
                _owner.Basic();
            }

            [Obfuscation(Exclude = true)]
            public void VirtualDirectories()
            {
                _owner.VirtualDirectories();
            }

            [Obfuscation(Exclude = true)]
            public void Browse(object uri)
            {
                _owner.Browse(uri);
            }

            [Obfuscation(Exclude = true)]
            public void Advanced()
            {
                _owner.Advanced();
            }

            [Obfuscation(Exclude = true)]
            public void FixPhp()
            {
                _owner.FixPhp();
            }

            [Obfuscation(Exclude = true)]
            public void FixKestrel()
            {
                _owner.FixKestrel();
            }
        }

        public ApplicationFeature(Module module)
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
            OnApplicationSettingsSaved();
        }

        protected void OnApplicationSettingsSaved()
        {
            ApplicationSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210463");
            return false;
        }

        private void Advanced()
        {
        }

        private void Browse(object uri)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));

            // IMPORTANT: help users launch IIS Express instance.
            var site = service.Application.Site;
            if (site.Server.Mode == WorkingMode.IisExpress && site.State != ObjectState.Started)
            {
                var message = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result = message.ShowMessage(
                    "This website is not yet running. Do you want to start it now?",
                    "Question",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                try
                {
                    DialogHelper.SiteStart(site);
                }
                catch (Exception ex)
                {
                    message.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DialogHelper.ProcessStart(uri + service.Application.Path);
        }

        private void VirtualDirectories()
        {
        }

        private void Basic()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using (var dialog = new NewApplicationDialog(Module, service.Application.Site,
                service.Application.Path.GetParentPath(),
                service.Application.ApplicationPoolName,
                service.Application))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            service.ServerManager.CommitChanges();
            service.Application.Save();
        }

        private void Permissions()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            NativeMethods.ShowFileProperties(service.Application.PhysicalPath.ExpandIisExpressEnvironmentVariables(service.Application.GetActualExecutable()));
        }

        private void Explore()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            DialogHelper.Explore(service.Application.PhysicalPath.ExpandIisExpressEnvironmentVariables(service.Application.GetActualExecutable()));
        }

        private void FixPhp()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new PhpDiagDialog(Module, service.Application.Server);
            dialog.ShowDialog();
        }

        private void FixKestrel()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new KestrelDiagDialog(Module, service.Application);
            dialog.ShowDialog();
        }

        public IEnumerable<Binding> SiteBindings
        {
            get
            {
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var site = service.Application.Site;
                return site.Bindings;
            }
        }

        public ApplicationSettingsSavedEventHandler ApplicationSettingsUpdated { get; set; }
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
        public string Name { get; }
    }

    public delegate void ApplicationSettingsSavedEventHandler();
}
