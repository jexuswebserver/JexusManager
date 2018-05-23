// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Asp
{
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    internal partial class AspPage : ModulePropertiesPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly AspPage _owner;

            public PageTaskList(AspPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private AspFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public AspPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new AspFeature(Module);
            _feature.AspSettingsUpdated = Refresh;
            _feature.Load();
            base.Initialize(navigationData);
        }

        protected override bool ShowHelp()
        {
            return _feature.ShowHelp();
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        protected override bool ApplyChanges()
        {
            try
            {
                if (!_feature.ApplyChanges())
                {
                    return false;
                }
            }
            catch (COMException ex)
            {
                ShowError(ex, false);
                return true;
            }

            ClearChanges();
            return true;
        }

        protected override void CancelChanges()
        {
            _initialized = false;
            _hasChanges = false;
            _feature.CancelChanges();
            ClearChanges();
        }

        protected override bool HasChanges
        {
            get { return _hasChanges; }
        }

        protected override bool CanApplyChanges
        {
            get { return true; }
        }

        private void InformChanges()
        {
            if (!_initialized)
            {
                return;
            }

            _hasChanges = true;
            Refresh();
        }

        private void ClearChanges()
        {
            _hasChanges = false;
            Refresh();
        }

        protected override TaskListCollection Tasks
        {
            get
            {
                if (_taskList == null)
                {
                    _taskList = new PageTaskList(this);
                }

                base.Tasks.Add(_feature.GetTaskList());
                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        protected override void OnRefresh()
        {
            if (!_hasChanges)
            {
                pgASP.SelectedObject = _feature.PropertyGridObject;
                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        private void PgAspPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            InformChanges();
        }

        protected override PropertyBag GetProperties()
        {
            return null;
        }

        protected override PropertyBag UpdateProperties(out bool updateSuccessful)
        {
            updateSuccessful = false;
            return null;
        }

        protected override void ProcessProperties(PropertyBag properties)
        {
        }
    }
}
