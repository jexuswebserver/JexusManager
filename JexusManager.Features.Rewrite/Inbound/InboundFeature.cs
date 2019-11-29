// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    public class InboundFeature : FeatureBase<InboundRule>
    {
        public InboundFeature(Module module)
            : base(module)
        {
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var outSection = service.GetSection("system.webServer/rewrite/rules");
            return outSection.GetCollection();
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
            get { return "URL Rewrite"; }
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/rules");
            CanRevert = section.CanRevert();
            LoadItems();
        }

        public bool CanRevert { get; set; }

        public void Add()
        {
            int index = 0;
            string name;
            do
            {
                index++;
                name = string.Format("LowerCaseRule{0}", index);
            }
            while (Items.All(item => item.Name != name));
            var newRule = new InboundRule(null);
            newRule.Name = name;
            newRule.Input = "URL Path";
            newRule.PatternSyntax = 0L;
            newRule.PatternUrl = "[A-Z]";
            newRule.Type = 2L;
            newRule.ActionUrl = "{ToLower:{URL}}";
            newRule.IgnoreCase = false;
            newRule.RedirectType = 301;

            AddItem(newRule);
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

        public void AddConditions()
        {
            using var dialog = new AddConditionDialog(Module, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            SelectedItem.Conditions.Add(newItem);
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

            RevertItems();
        }

        public void Edit()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(InboundRulePage), new Tuple<InboundFeature, InboundRule>(this, SelectedItem));
            OnSettingsSaved();
        }
    }
}
