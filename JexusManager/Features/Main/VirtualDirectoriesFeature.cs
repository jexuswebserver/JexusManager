// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using JexusManager.Dialogs;
    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Feature for managing virtual directories.
    /// </summary>
    internal class VirtualDirectoriesFeature : FeatureBase<VirtualDirectory>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly VirtualDirectoriesFeature _owner;

            public FeatureTaskList(VirtualDirectoriesFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList
                {
                    new MethodTaskItem("Add", "Add Virtual Directory...", string.Empty, string.Empty, Resources.virtual_directory_new_16)
                        .SetUsage(),
                    new MethodTaskItem("Set", "Set Virtual Directory Defauls...", string.Empty).SetUsage(),
                };

                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new TextTaskItem("Manage Virtual Directory", string.Empty, true));

                    result.Add(
                        new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16)
                            .SetUsage());
                    result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);

                    if (_owner.SiteBindings.Any(item => item.CanBrowse))
                    {
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new TextTaskItem("Browse Virtual Directory", string.Empty, true));
                        foreach (Binding binding in _owner.SiteBindings)
                        {
                            if (binding.CanBrowse)
                            {
                                var uri = binding.ToUri();
                                result.Add(
                                    new MethodTaskItem("Browse", $"Browse {binding.ToShortString()}",
                                        string.Empty, string.Empty,
                                        Resources.browse_16, uri).SetUsage());
                            }
                        }
                    }

                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new TextTaskItem("Edit Virtual Directory", string.Empty, true));
                    result.Add(
                        new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                            Resources.basic_settings_16).SetUsage());
                    result.Add(
                        new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());

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
                _owner.Edit();
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
            public void Advanced()
            {
                _owner.Advanced();
            }
        }

        public VirtualDirectoriesFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;
        private Microsoft.Web.Administration.Application _application;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load(Microsoft.Web.Administration.Application application)
        {
            _application = application;
            Items = GetFiltered(application.VirtualDirectories);
            OnVirtualDirectoriesSettingsSaved();
        }

        private List<VirtualDirectory> GetFiltered(VirtualDirectoryCollection collection)
        {
            return collection.Where(item => item.Path != "/").ToList();
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

        protected void OnVirtualDirectoriesSettingsSaved()
        {
            VirtualDirectoriesSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("https://go.microsoft.com/fwlink/?LinkId=210537");
            return false;
        }

        private void Add()
        {
            // Create and show dialog for adding a new virtual directory
            // This would use a dialog similar to the ones for Sites and Application Pools
            using (var dialog = new NewVirtualDirectoryDialog(Module, null, _application.Path, _application))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Items.Add(dialog.VirtualDirectory);
                SelectedItem = dialog.VirtualDirectory;
                _application.Server.CommitChanges();
                Items = GetFiltered(_application.VirtualDirectories);
            }
            OnVirtualDirectoriesSettingsSaved();
        }

        internal void Remove()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result = service.ShowMessage("Are you sure that you want to remove the selected virtual directory?",
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var index = _application.VirtualDirectories.IndexOf(SelectedItem);
            _application.VirtualDirectories.Remove(SelectedItem);

            if (_application.VirtualDirectories.Count == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = index > _application.VirtualDirectories.Count - 1
                    ? _application.VirtualDirectories[_application.VirtualDirectories.Count - 1]
                    : _application.VirtualDirectories[index];
            }

            _application.Server.CommitChanges();
            Items = _application.VirtualDirectories.ToList();
            OnVirtualDirectoriesSettingsSaved();
        }

        internal void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(VirtualDirectory item)
        {
            // Edit the selected virtual directory
            using (var dialog = new NewVirtualDirectoryDialog(Module, item, _application.Path, _application))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _application.Server.CommitChanges();
                }
            }

            OnVirtualDirectoriesSettingsSaved();
        }

        private void Permissions()
        {
            var path = SelectedItem.PhysicalPath.ExpandIisExpressEnvironmentVariables(SelectedItem.Application.GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                NativeMethods.ShowFileProperties(path);
            }
        }

        private void Explore()
        {
            var path = SelectedItem.PhysicalPath.ExpandIisExpressEnvironmentVariables(SelectedItem.Application.GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                DialogHelper.Explore(path);
            }
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
                    service.Form.BeginProgress();
                    DialogHelper.SiteStart(site);
                }
                catch (Exception ex)
                {
                    message.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    service.Form.EndProgress();
                }
            }

            DialogHelper.ProcessStart(uri + SelectedItem.PathToSite());
        }

        private void Advanced()
        {
            // Show advanced settings dialog
            // This would be implemented similar to the other features
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        protected override void OnSettingsSaved()
        {
            OnVirtualDirectoriesSettingsSaved();
        }

        public VirtualDirectoriesSettingsSavedEventHandler VirtualDirectoriesSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name { get; }
    }

    public delegate void VirtualDirectoriesSettingsSavedEventHandler();
}
