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
            PreConditions = new List<PreConditionItem>(((RewriteModule)Module).Proxy.GetPreConditions());
            Tags = new List<CustomTagsItem>(((RewriteModule)Module).Proxy.GetCustomTags());
            Items.Clear();
            Items.AddRange(((RewriteModule)Module).Proxy.GetOutboundRules());
            OnSettingsSaved();
        }

        public List<CustomTagsItem> Tags { get; set; }

        public List<PreConditionItem> PreConditions { get; set; }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        public void Rename(OutboundRule item, string name)
        {
            if (item == null)
            {
                return;
            }

            item.Name = name;
            ((RewriteModule)Module).Proxy.UpdateOutboundRule(item, item);
            Load();
        }

        protected override void DoubleClick(OutboundRule item)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(OutboundRulePage), new Tuple<OutboundFeature, OutboundRule>(this, item));
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

            ((RewriteModule)Module).Proxy.MoveOutboundRuleUp(SelectedItem);
            Load();
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

            ((RewriteModule)Module).Proxy.MoveOutboundRuleDown(SelectedItem);
            Load();
        }

        public void Disable()
        {
            var item = SelectedItem;
            item.Enabled = false;
            ((RewriteModule)Module).Proxy.SetOutboundRuleEnabled(item, false);
            OnSettingsSaved();
        }

        public void Enable()
        {
            var item = SelectedItem;
            item.Enabled = true;
            ((RewriteModule)Module).Proxy.SetOutboundRuleEnabled(item, true);
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

        public void AddItem(OutboundRule item)
        {
            ((RewriteModule)Module).Proxy.AddOutboundRule(item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void EditItem(OutboundRule item)
        {
            var original = SelectedItem ?? throw new InvalidOperationException("No rule is selected.");
            ((RewriteModule)Module).Proxy.UpdateOutboundRule(original, item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No rule is selected.");
            ((RewriteModule)Module).Proxy.RemoveOutboundRule(item);
            SelectedItem = null;
            Load();
        }

        public void Revert()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result =
                dialog.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            ((RewriteModule)Module).Proxy.RevertOutboundRules();
            Load();
        }
    }
}
