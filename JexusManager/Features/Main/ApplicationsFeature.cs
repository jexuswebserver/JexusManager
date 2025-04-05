// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Dialogs;
    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Application = Microsoft.Web.Administration.Application;
    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Feature for managing applications within a site.
    /// </summary>
    internal class ApplicationsFeature : FeatureBase<Application>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly ApplicationsFeature _owner;

            public FeatureTaskList(ApplicationsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (_owner._site == null)
                {
                    if (_owner.SelectedItem != null)
                    {
                        result.Add(new MethodTaskItem("Change", "Change Application Pool...", string.Empty).SetUsage());
                    }
                }
                else
                {
                    result.Add(new MethodTaskItem("Add", "Add Application...", string.Empty, string.Empty, Resources.application_new_16)
                            .SetUsage());
                    result.Add(new MethodTaskItem("Set", "Set Application Defaults...", string.Empty).SetUsage());

                    if (_owner.SelectedItem != null)
                    {
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());

                        result.Add(new TextTaskItem("Manage Application", string.Empty, true));
                        result.Add(
                            new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16)
                                .SetUsage());
                        result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());
                        result.Add(RemoveTaskItem);

                        var canBrowse = _owner.SelectedItem.Site.Bindings.Any(binding => binding.CanBrowse);
                        if (canBrowse)
                        {
                            result.Add(MethodTaskItem.CreateSeparator().SetUsage());

                            result.Add(new TextTaskItem("Browse Application", string.Empty, true));
                            foreach (Binding binding in _owner.SelectedItem.Site.Bindings)
                            {
                                if (binding.CanBrowse)
                                {
                                    var uri = binding.ToUri();
                                    result.Add(
                                        new MethodTaskItem("Browse", $"Browse {uri}", string.Empty, string.Empty,
                                            Resources.browse_16, uri).SetUsage());
                                }
                            }
                        }

                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new TextTaskItem("Edit Application", string.Empty, true));
                        result.Add(
                            new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                                Resources.basic_settings_16).SetUsage());
                        result.Add(
                            new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());

                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new MethodTaskItem("VirtualDirectories", "View Virtual Directories", string.Empty).SetUsage());
                    }
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Basic()
            {
                _owner.Basic();
            }

            [Obfuscation(Exclude = true)]
            public void Advanced()
            {
                _owner.Advanced();
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

            [Obfuscation(Exclude = true)]
            public void VirtualDirectories()
            {
                _owner.VirtualDirectories();
            }

            [Obfuscation(Exclude = true)]
            public void Change()
            {
                _owner.Change();
            }
        }

        public ApplicationsFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;
        private Site _site;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load(List<Application> applications, Site site)
        {
            _site = site;
            // Filter out the root application if needed
            Items = site == null ? applications : site.Applications.Where(app => app.Path != "/").ToList();
            OnApplicationsSettingsSaved();
        }

        protected void OnApplicationsSettingsSaved()
        {
            ApplicationsSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("https://go.microsoft.com/fwlink/?LinkId=210458");
            return false;
        }

        private void Change()
        {
            using var dialog = new NewApplicationDialog(Module, SelectedItem.Site, SelectedItem.Path, SelectedItem.ApplicationPoolName, SelectedItem);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SelectedItem.Apply();
            SelectedItem.Site.Server.CommitChanges();
            OnApplicationsSettingsSaved();
        }

        private void Add()
        {
            using (var dialog = new NewApplicationDialog(Module, _site, string.Empty, string.Empty, null))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Items.Add(dialog.Application);
                SelectedItem = dialog.Application;
                _site.Server.CommitChanges();
                Items = _site.Applications.Where(app => app.Path != "/").ToList();
            }
            
            OnApplicationsSettingsSaved();
        }

        internal void Remove()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result = service.ShowMessage("Are you sure that you want to remove the selected application?", 
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel, 
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var index = _site.Applications.IndexOf(SelectedItem);
            _site.Applications.Remove(SelectedItem);
            
            if (_site.Applications.Count <= 1) // Count is 1 means only root application
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = index > _site.Applications.Count - 1 
                    ? _site.Applications[_site.Applications.Count - 1] 
                    : _site.Applications[index];

                // Ensure we don't select the root application
                if (SelectedItem.Path == "/")
                {
                    SelectedItem = _site.Applications.FirstOrDefault(app => app.Path != "/");
                }
            }

            _site.Server.CommitChanges();
            Items = _site.Applications.Where(app => app.Path != "/").ToList();
            OnApplicationsSettingsSaved();
        }

        private void Basic()
        {
            if (SelectedItem == null)
            {
                return;
            }

            using (var dialog = new NewApplicationDialog(Module, _site, SelectedItem.Path, SelectedItem.ApplicationPoolName, SelectedItem))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            _site.Server.CommitChanges();
            OnApplicationsSettingsSaved();
        }

        private void Advanced()
        {
            // Implement Advanced Settings dialog if needed
        }

        private void VirtualDirectories()
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var mainForm = (MainForm)service.Form;
            
            // Create a new VirtualDirectoriesPage and initialize it with the application           
            var page = new VirtualDirectoriesPage();
            ((IModulePage)page).Initialize(Module, null, SelectedItem);
            
            // Load the page in the main form
            mainForm.LoadPage(page);
        }

        protected override void DoubleClick(Application item)
        {
            Basic();
        }

        private void Permissions()
        {
            var path = SelectedItem.PhysicalPath.ExpandIisExpressEnvironmentVariables(SelectedItem.GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                NativeMethods.ShowFileProperties(path);
            }
        }

        private void Explore()
        {
            var path = SelectedItem.PhysicalPath.ExpandIisExpressEnvironmentVariables(SelectedItem.GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                DialogHelper.Explore(path);
            }
        }

        private void Browse(object uri)
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            // Help users launch IIS Express instance if needed
            var site = _site;
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
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            DialogHelper.ProcessStart(uri + SelectedItem.Path);
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        protected override void OnSettingsSaved()
        {
            OnApplicationsSettingsSaved();
        }

        public ApplicationsSettingsSavedEventHandler ApplicationsSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name { get; }
    }

    public delegate void ApplicationsSettingsSavedEventHandler();
}
