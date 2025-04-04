// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Features.Rewrite.Inbound;
    using JexusManager.Features.Rewrite.Outbound;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    public class RewriteFeature
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly RewriteFeature _owner;

            public FeatureTaskList(RewriteFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add Rule(s)...", string.Empty).SetUsage());
                if (_owner.Inbound.CanRevert)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RevertTaskItem);
                }

                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new TextTaskItem("Manage Server Variables", string.Empty, true));
                result.Add(new MethodTaskItem("ViewServerVariables", "View Server Variables...", string.Empty).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new TextTaskItem("Manage Providers", string.Empty, true));
                result.Add(new MethodTaskItem("ViewMaps", "View Rewrite Maps...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("ViewProviders", "View Providers...", string.Empty).SetUsage());

                if (_owner.Inbound.SelectedItem != null)
                {
                    var groupTaskItem = new GroupTaskItem("Conditions", "Conditions", string.Empty, true);
                    groupTaskItem.Items.Add(new MethodTaskItem("AddConditions", "Add...", string.Empty).SetUsage());
                    result.Add(groupTaskItem);
                }

                var inboundGroup = new GroupTaskItem("InboundRules", "Inbound Rules", string.Empty, true);
                result.Add(inboundGroup);
                if (_owner.Inbound.SelectedItem != null)
                {
                    inboundGroup.Items.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                    inboundGroup.Items.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    inboundGroup.Items.Add(RemoveTaskItem);
                    if (!_owner.Inbound.SelectedItem.Enabled)
                    {
                        inboundGroup.Items.Add(new MethodTaskItem("Enable", "Enable Rule", string.Empty).SetUsage());
                    }

                    if (_owner.Inbound.SelectedItem.Enabled)
                    {
                        inboundGroup.Items.Add(new MethodTaskItem("Disable", "Disable Rule", string.Empty).SetUsage());
                    }

                    inboundGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    inboundGroup.Items.Add(GetMoveUpTaskItem(_owner.Inbound.CanMoveUp));
                    inboundGroup.Items.Add(GetMoveDownTaskItem(_owner.Inbound.CanMoveDown));
                    inboundGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                }

                inboundGroup.Items.Add(new MethodTaskItem("Import", "Import Rules...", string.Empty).SetUsage());

                var outboundGroup = new GroupTaskItem("OutboundRules", "Outbound Rules", string.Empty, true);
                result.Add(outboundGroup);
                if (_owner.Outbound.SelectedItem != null)
                {
                    outboundGroup.Items.Add(new MethodTaskItem("EditOut", "Edit...", string.Empty).SetUsage());
                    outboundGroup.Items.Add(new MethodTaskItem("RenameOut", "Rename", string.Empty).SetUsage());
                    outboundGroup.Items.Add(GetRemoveTaskItem("Remove2"));
                    if (!_owner.Outbound.SelectedItem.Enabled)
                    {
                        outboundGroup.Items.Add(new MethodTaskItem("EnableOut", "Enable Rule", string.Empty).SetUsage());
                    }

                    if (_owner.Outbound.SelectedItem.Enabled)
                    {
                        outboundGroup.Items.Add(new MethodTaskItem("DisableOut", "Disable Rule", string.Empty).SetUsage());
                    }

                    outboundGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    outboundGroup.Items.Add(GetMoveUpTaskItem("MoveUpOut", _owner.Outbound.CanMoveUp));
                    outboundGroup.Items.Add(GetMoveDownTaskItem("MoveDownOut", _owner.Outbound.CanMoveDown));
                    outboundGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                }

                outboundGroup.Items.Add(new MethodTaskItem("ViewPreconditions", "View Preconditions...", string.Empty).SetUsage());
                outboundGroup.Items.Add(new MethodTaskItem("ViewTags", "View Custom Tags...", string.Empty).SetUsage());
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
                _owner.Inbound.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Inbound.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.RenameIn();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Inbound.Revert();
            }

            [Obfuscation(Exclude = true)]
            public void Enable()
            {
                _owner.Inbound.Enable();
            }

            [Obfuscation(Exclude = true)]
            public void Disable()
            {
                _owner.Inbound.Disable();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveUp()
            {
                _owner.Inbound.MoveUp();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveDown()
            {
                _owner.Inbound.MoveDown();
            }

            [Obfuscation(Exclude = true)]
            public void Remove2()
            {
                _owner.Outbound.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void EditOut()
            {
                _owner.Outbound.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void RenameOut()
            {
                _owner.RenameOut();
            }

            [Obfuscation(Exclude = true)]
            public void EnableOut()
            {
                _owner.Outbound.Enable();
            }

            [Obfuscation(Exclude = true)]
            public void DisableOut()
            {
                _owner.Outbound.Disable();
            }

            [Obfuscation(Exclude = true)]
            public void MoveUpOut()
            {
                _owner.Outbound.MoveUp();
            }

            [Obfuscation(Exclude = true)]
            public void MoveDownOut()
            {
                _owner.Outbound.MoveDown();
            }

            [Obfuscation(Exclude = true)]
            public void Import()
            {
                _owner.Import();
            }

            [Obfuscation(Exclude = true)]
            public void ViewServerVariables()
            {
                _owner.ViewServerVariables();
            }

            [Obfuscation(Exclude = true)]
            public void ViewMaps()
            {
                _owner.ViewMaps();
            }

            [Obfuscation(Exclude = true)]
            public void ViewProviders()
            {
                _owner.ViewProviders();
            }

            [Obfuscation(Exclude = true)]
            public void AddConditions()
            {
                _owner.Inbound.AddConditions();
            }

            [Obfuscation(Exclude = true)]
            public void ViewPreconditions()
            {
                _owner.ViewPreconditions();
            }

            [Obfuscation(Exclude = true)]
            public void ViewTags()
            {
                _owner.ViewTags();
            }
        }

        public RewriteFeature(Module module)
        {
            Module = module;
            Inbound = new InboundFeature(module);
            Outbound = new OutboundFeature(module);
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), string.Empty, false);
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
            Inbound.Load();
            Outbound.Load();
        }

        public void Add()
        {
            using var dialog = new NewRewriteRuleDialog(Inbound.Module);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (dialog.SelectedIndex == 0)
            {
                var service = (INavigationService)GetService(typeof(INavigationService));
                service.Navigate(null, null, typeof(InboundRulePage), new Tuple<InboundFeature, InboundRule>(Inbound, null));
                Inbound.Refresh();
            }
            else if (dialog.SelectedIndex == 1)
            {
                using var rule = new NewRuleWithRewriteMapsDialog(Inbound.Module, Inbound);
                if (rule.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            else if (dialog.SelectedIndex == 2)
            {
                using var rule = new NewRuleBlockingDialog(Inbound.Module, Inbound);
                if (rule.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            else if (dialog.SelectedIndex == 4)
            {
                var service = (INavigationService)GetService(typeof(INavigationService));
                service.Navigate(null, null, typeof(OutboundRulePage), new Tuple<OutboundFeature, OutboundRule>(Outbound, null));
                Outbound.Refresh();
            }
            else if (dialog.SelectedIndex == 5)
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                if (
                    service.ShowMessage("Search engines treat Web sites that can be accessed by more than one URL, each differing only in letter casing, as if they are two different sites. This results in a reduced ranking for the Web site. Use this rule template to create a redirect rule that will enforce the use of lowercase letters in the URL." + Environment.NewLine + Environment.NewLine + "Do you want to create a rule?", "Add a rule that will enforce lowercase URLs",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                    DialogResult.Yes)
                {
                    return;
                }

                Inbound.Add();
            }
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130403&amp;clcid=0x409");
            return false;
        }

        private void ViewTags()
        {
        }

        private void ViewPreconditions()
        {
        }

        private void ViewProviders()
        {
        }

        private void ViewMaps()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(MapsPage), null);
        }

        private void ViewServerVariables()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(ServerVariablesPage), null);
        }

        private void Import()
        {
        }

        private void RenameIn()
        {
            Inbound.RenameInline(Inbound.SelectedItem);
        }

        private void RenameOut()
        {
            Outbound.RenameInline(Outbound.SelectedItem);
        }

        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }

        public string Name
        {
            get
            {
                return "URL Rewrite";
            }
        }

        public InboundFeature Inbound { get; }

        public OutboundFeature Outbound { get; }
    }
}
