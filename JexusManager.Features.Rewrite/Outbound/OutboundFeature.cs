// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    public class OutboundFeature : FeatureBase<OutboundRule>
    {
        public OutboundFeature(Module module)
            : base(module)
        {
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            return section.GetCollection();
        }

        protected override void OnSettingsSaved()
        {
            RewriteSettingsUpdated?.Invoke();
        }

        public void Refresh()
        {
            OnSettingsSaved();
        }

        public RewriteSettingsSavedEventHandler RewriteSettingsUpdated { get; set; }

        public string Name
        {
            get
            {
                return "URL Rewrite";
            }
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var outSection = service.GetSection("system.webServer/rewrite/outboundRules");
            var preConditions = outSection.ChildElements["preConditions"];

            PreConditions = new List<PreConditionItem>();
            Tags = new List<CustomTagsItem>();
            foreach (ConfigurationElement condition in preConditions.GetCollection())
            {
                var item = new PreConditionItem(condition);
                PreConditions.Add(item);
            }

            var tags = outSection.ChildElements["customTags"];
            foreach (ConfigurationElement condition in tags.GetCollection())
            {
                var item = new CustomTagsItem(condition);
                Tags.Add(item);
            }

            LoadItems();
        }

        public List<CustomTagsItem> Tags { get; set; }

        public List<PreConditionItem> PreConditions { get; set; }

        public void Edit()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(OutboundRulePage), new Tuple<OutboundFeature, OutboundRule>(this, SelectedItem));
            OnSettingsSaved();
        }

        public void MoveUp()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveUpItem();
        }

        public void MoveDown()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveDownItem();
        }

        public void Disable()
        {
            SelectedItem.Enabled = false;
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            SelectedItem.Element["enabled"] = false;
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public void Enable()
        {
            SelectedItem.Enabled = true;
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            SelectedItem.Element["enabled"] = true;
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected rule?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }
    }
}
