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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;
    using System.Linq;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class PhysicalDirectoryFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly PhysicalDirectoryFeature _owner;

            public FeatureTaskList(PhysicalDirectoryFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16).SetUsage());
                result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());

                if (_owner.SiteBindings.Any(item => item.CanBrowse))
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    var manageGroup = new GroupTaskItem(string.Empty, "Manage Folder", string.Empty, true);
                    result.Add(manageGroup);
                    manageGroup.Items.Add(new TextTaskItem("Browse Folder", string.Empty, true));
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
            public void Browse(object uri)
            {
                _owner.Browse(uri);
            }
        }

        public PhysicalDirectoryFeature(Module module)
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
            OnPhysicalDirectorySettingsSaved();
        }

        protected void OnPhysicalDirectorySettingsSaved()
        {
            if (PhysicalDirectorySettingsUpdated != null)
            {
                PhysicalDirectorySettingsUpdated.Invoke();
            }
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210463");
            return false;
        }

        private void Browse(object uri)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));

            // IMPORTANT: help users launch IIS Express instance.
            var site = service.PhysicalDirectory.Application.Site;
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
                    site.Start();
                }
                catch (Exception ex)
                {
                    message.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Process.Start(uri + service.PhysicalDirectory.PathToSite);
        }

        private void Permissions()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            NativeMethods.ShowFileProperties(service.PhysicalDirectory.FullName);
        }

        private void Explore()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            DialogHelper.Explore(service.PhysicalDirectory.FullName.ExpandIisExpressEnvironmentVariables());
        }

        public IEnumerable<Binding> SiteBindings
        {
            get
            {
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var site = service.PhysicalDirectory.Application.Site;
                return site.Bindings;
            }
        }

        public PhysicalDirectorySettingsSavedEventHandler PhysicalDirectorySettingsUpdated { get; set; }
        public string Description { get; private set; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }
        public string Name { get; private set; }
    }

    public delegate void PhysicalDirectorySettingsSavedEventHandler();
}
