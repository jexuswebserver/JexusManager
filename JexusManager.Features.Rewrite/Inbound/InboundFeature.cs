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
            Items.Clear();
            Items.AddRange(((RewriteModule)Module).Proxy.GetInboundRules());
            CanRevert = GetService(typeof(IConfigurationService)) is IConfigurationService service && service.GetSection("system.webServer/rewrite/rules").CanRevert();
            OnSettingsSaved();
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
            var newRule = new InboundRule();
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

            ((RewriteModule)Module).Proxy.MoveInboundRuleUp(SelectedItem);
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

            ((RewriteModule)Module).Proxy.MoveInboundRuleDown(SelectedItem);
            Load();
        }

        public void Disable()
        {
            var item = SelectedItem;
            item.Enabled = false;
            ((RewriteModule)Module).Proxy.SetInboundRuleEnabled(item, false);
            OnSettingsSaved();
        }

        public void Enable()
        {
            var item = SelectedItem;
            item.Enabled = true;
            ((RewriteModule)Module).Proxy.SetInboundRuleEnabled(item, true);
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

            ((RewriteModule)Module).Proxy.RevertInboundRules();
            Load();
        }

        public void AddItem(InboundRule item)
        {
            ((RewriteModule)Module).Proxy.AddInboundRule(item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void EditItem(InboundRule item)
        {
            var original = SelectedItem ?? throw new InvalidOperationException("No rule is selected.");
            ((RewriteModule)Module).Proxy.UpdateInboundRule(original, item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No rule is selected.");
            ((RewriteModule)Module).Proxy.RemoveInboundRule(item);
            SelectedItem = null;
            Load();
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        public void Rename(InboundRule item, string name)
        {
            if (item == null)
            {
                return;
            }

            item.Name = name;
            ((RewriteModule)Module).Proxy.UpdateInboundRule(item, item);
            Load();
        }

        protected override void DoubleClick(InboundRule item)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(InboundRulePage), new Tuple<InboundFeature, InboundRule>(this, item));
            OnSettingsSaved();
        }
    }
}
