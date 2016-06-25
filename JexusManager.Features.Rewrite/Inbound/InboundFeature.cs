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
            this.RewriteSettingsUpdated?.Invoke();
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
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/rules");
            this.CanRevert = section.CanRevert();
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
            while (this.Items.All(item => item.Name != name));
            var newRule = new InboundRule(null);
            newRule.Name = name;
            newRule.Input = "URL Path";
            newRule.PatternSyntax = 0L;
            newRule.PatternUrl = "[A-Z]";
            newRule.Type = 2L;
            newRule.ActionUrl = "{ToLower:{URL}}";
            newRule.IgnoreCase = false;
            newRule.RedirectType = 301;

            this.AddItem(newRule);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected rule?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            this.RemoveItem();
        }

        public void AddConditions()
        {
            var dialog = new AddConditionDialog(this.Module, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            this.SelectedItem.Conditions.Add(newItem);
        }

        public void MoveUp()
        {
            if (this.Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        this.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            this.MoveUpItem();
        }

        public void MoveDown()
        {
            if (this.Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        this.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            this.MoveDownItem();
        }

        public void Disable()
        {
            this.SelectedItem.Enabled = false;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            this.SelectedItem.Element["enabled"] = false;
            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public void Enable()
        {
            this.SelectedItem.Enabled = true;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            this.SelectedItem.Element["enabled"] = true;
            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public void Revert()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            var result =
                dialog.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    this.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            this.RevertItems();
        }

        public void Edit()
        {
            var service = (INavigationService)this.GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(InboundRulePage), new Tuple<InboundFeature, InboundRule>(this, this.SelectedItem));
            this.OnSettingsSaved();
        }
    }
}
