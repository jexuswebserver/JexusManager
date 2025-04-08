// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class ModulePropertiesPage : ModuleDialogPage
#else
    public abstract class ModulePropertiesPage : ModuleDialogPage
#endif
    {
        protected internal override bool ApplyChanges()
        {
            return false;
        }

        protected internal override void CancelChanges() { }

        protected void ClearDirty() { }
#if DESIGN
        protected virtual PropertyBag GetProperties()
        {
            throw new NotImplementedException();
        }
#else
        protected abstract PropertyBag GetProperties();
#endif
        protected void GetSettings() { }

        protected override void LoadPreferences(PreferencesStore store)
        { }

        protected override void OnActivated(bool initialActivation)
        { }

        protected virtual void OnException(Exception ex)
        { }

        protected override void OnRefresh() { }
#if DESIGN
        protected virtual void ProcessProperties(PropertyBag properties)
        {
            throw new NotImplementedException();
        }
#else
        protected abstract void ProcessProperties(PropertyBag properties);
#endif

        protected override void SavePreferences(PreferencesStore store)
        { }

#if DESIGN
        protected virtual PropertyBag UpdateProperties(out bool updateSuccessful)
        {
            throw new NotImplementedException();
        }
#else
        protected abstract PropertyBag UpdateProperties(out bool updateSuccessful);
#endif
        protected virtual bool ConfigNamesPresent { get; }
        protected override bool HasChanges { get; }
        protected object TargetObject { get; set; }

        protected override TaskListCollection Tasks
        {
            get
            {
                return base.Tasks;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ModulePropertiesPage
            // 
            this.Name = "ModulePropertiesPage";
            this.Size = new System.Drawing.Size(570, 527);
            this.ResumeLayout(false);
        }
    }
}
