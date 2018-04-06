// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InboundRulePage.cs" company="LeXtudio">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Features.Rewrite.Inbound;
    using JexusManager.Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using PresentationControls;

    public partial class OutboundRulePage : ModuleDialogPage, IModuleChildPage
    {
        private sealed class PageTaskList : DefaultTaskList
        {
            private readonly OutboundRulePage _owner;

            public PageTaskList(OutboundRulePage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                {
                    GetBackTaskItem("Back", "Back to Rules"),
                    new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage(),
                    HelpTaskItem
                };
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }

            [Obfuscation(Exclude = true)]
            public void Back()
            {
                _owner.Back();
            }
        }

        private class ConditionListViewItem : ListViewItem
        {
            private readonly ListViewSubItem _matchType;

            private readonly ListViewSubItem _pattern;

            public ConditionListViewItem(ConditionItem condition)
                : base(condition.Input)
            {
                _matchType = new ListViewSubItem(this, GetText(condition.MatchType));
                SubItems.Add(_matchType);
                _pattern = new ListViewSubItem(this, condition.Pattern);
                SubItems.Add(_pattern);
                Item = condition;
            }

            public ConditionItem Item { get; }

            public void Update()
            {
                Text = Item.Input;
                _matchType.Text = GetText(Item.MatchType);
                _pattern.Text = Item.MatchType > 3 ? Item.Pattern : "N/A";
            }

            private static string GetText(int matchType)
            {
                switch (matchType)
                {
                    case 0:
                        return "Is File";
                    case 1:
                        return "Is Not File";
                    case 2:
                        return "Is Directory";
                    case 3:
                        return "Is Not Directory";
                    case 4:
                        return "Matches the Pattern";
                    case 5:
                        return "Does Not Match the Pattern";
                }

                return string.Empty;
            }
        }

        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;
        private OutboundFeature _feature;

        public OutboundRulePage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            // TODO: pictureBox1.Image
            var info = (Tuple<OutboundFeature, OutboundRule>)navigationData;
            _feature = info.Item1;
            Rule = info.Item2;
            txtName.ReadOnly = Rule != null;
            if (Rule != null)
            {
                foreach (var preCondition in _feature.PreConditions)
                {
                    cbPreCondition.Items.Add(preCondition.Name);
                }

                foreach (var customTags in _feature.Tags)
                {
                    cbTags.Items.Add(customTags.Name);
                }

                // TODO: invoke RuleSettingsUpdate somewhere.
                Rule.RuleSettingsUpdated = Refresh;
            }

            cbPreCondition.Items.Add("<None>");
            cbPreCondition.Items.Add("<Create New Precondition...>");
            cbTags.Items.Add("<Create New Tags Collection...>");
            Refresh();
        }

        protected override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void Back()
        {
            if (_hasChanges)
            {
                var result = ShowMessage("The changes you have made will be lost. Do you want to save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    ApplyChanges();
                }
            }

            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(1);
            _feature.SelectedItem = Rule;
            _feature.Refresh();
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            InformChanges();
        }

        public OutboundRule Rule { get; set; }

        private void BtnTestClick(object sender, EventArgs e)
        {
            var dialog = new RegexTestDialog(Module, txtPattern.Text, cbIgnoreCase.Checked, false);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtPattern.Text = dialog.Pattern;
            cbIgnoreCase.Checked = dialog.IgnoreCase;
        }

        protected override bool ApplyChanges()
        {
            var filter = GetFilter(cbMatch);
            if (Rule == null)
            {
                Rule = new OutboundRule(null);
            }

            Rule.Name = txtName.Text;
            Rule.PreCondition = cbPreCondition.Text == "<None>" ? string.Empty : cbPreCondition.Text;
            Rule.CustomTags = cbTags.Text;
            Rule.Enabled = true;
            Rule.Syntax = cbUsing.SelectedIndex;
            Rule.Stopping = cbStop.Checked;
            Rule.Filter = filter;
            Rule.ServerVariable = txtVariable.Text;
            Rule.Pattern = txtPattern.Text;
            Rule.Negate = cbContent.SelectedIndex == 1;
            Rule.IgnoreCase = cbIgnoreCase.Checked;
            Rule.TrackAllCaptures = cbTrack.Checked;
            Rule.LogicalGrouping = cbAny.SelectedIndex;
            Rule.Action = cbAction.SelectedIndex;
            Rule.Value = txtValue.Text;
            Rule.Replace = !string.IsNullOrWhiteSpace(txtValue.Text);

            if (txtName.ReadOnly)
            {
                _feature.EditItem(Rule);
            }
            else
            {
                txtName.ReadOnly = true;
                _feature.AddItem(Rule);
            }

            if (!Rule.ApplyChanges())
            {
                return false;
            }

            ClearChanges();
            return true;
        }

        public static long GetFilter(CheckBoxComboBox box)
        {
            long result = 0L;
            for (int index = 0; index < box.CheckBoxItems.Count - 1; index++)
            {
                if (box.CheckBoxItems[index].Checked)
                {
                    result |= 1 << index;
                }
            }

            return box.CheckBoxItems[box.CheckBoxItems.Count - 1].Checked ? result | 32768 : result;
        }

        public static void SetFilter(long filter, CheckBoxComboBox box)
        {
            box.CheckBoxItems[box.CheckBoxItems.Count - 1].Checked = (filter & 32768) != 0;
            for (int index = 0; index < box.CheckBoxItems.Count - 1; index++)
            {
                box.CheckBoxItems[index].Checked = (filter & 1 << index) != 0;
            }
        }

        protected override void CancelChanges()
        {
            _initialized = false;
            _hasChanges = false;
            Rule?.CancelChanges();
            ClearChanges();
        }

        protected override bool HasChanges
        {
            get { return _hasChanges; }
        }

        protected override bool CanApplyChanges
        {
            get
            {
                return !string.IsNullOrWhiteSpace(txtName.Text) &&
                       !string.IsNullOrWhiteSpace(txtPattern.Text) &&
                       !string.IsNullOrWhiteSpace(txtValue.Text);
            }
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
            _initialized = false;
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

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        protected override void OnRefresh()
        {
            if (!_hasChanges)
            {
                txtName.ReadOnly = Rule != null;
                cbPreCondition.SelectedIndex = 0;
                cbAction.SelectedIndex = 0;
                cbIgnoreCase.Checked = true;
                cbScope.SelectedIndex = 0;
                cbStop.Checked = false;
                cbUsing.SelectedIndex = 0;
                cbContent.SelectedIndex = 0;
                if (Rule != null)
                {
                    SetFilter(Rule.Filter, cbMatch);
                    txtName.Text = Rule.Name;
                    cbPreCondition.Text = Rule.PreCondition == string.Empty ? "<None>" : Rule.PreCondition;
                    cbTags.Text = Rule.CustomTags;
                    cbAction.SelectedIndex = (int)Rule.Action;
                    cbIgnoreCase.Checked = Rule.IgnoreCase;
                    txtVariable.Text = Rule.ServerVariable;
                    cbScope.SelectedIndex = string.IsNullOrWhiteSpace(Rule.ServerVariable) ? 0 : 1;
                    cbStop.Checked = Rule.Stopping;
                    cbUsing.SelectedIndex = (int)Rule.Syntax;
                    cbContent.SelectedIndex = Rule.Negate ? 1 : 0;
                    txtPattern.Text = Rule.Pattern;
                    txtValue.Text = Rule.Value;

                    lvConditions.Items.Clear();
                    foreach (var condition in Rule.Conditions)
                    {
                        lvConditions.Items.Add(new ConditionListViewItem(condition));
                        gbConditions.IsExpanded = true;
                    }

                    cbAny.SelectedIndex = (int)Rule.LogicalGrouping;
                    cbTrack.Checked = Rule.TrackAllCaptures;
                }

                _initialized = true;
            }

            cbTags.Enabled = cbMatch.CheckBoxItems[12].Checked;
            CbActionSelectedIndexChanged(null, null);
            cbStop.Visible = cbAction.SelectedIndex == 0 || cbAction.SelectedIndex == 1;
            UpdateConditionsButtons();

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        public IModulePage ParentPage { get; set; }

        private void CbActionSelectedIndexChanged(object sender, EventArgs e)
        {
            gbRewrite.Visible = cbAction.SelectedIndex == 1;
        }

        private void CbAppendRedirectCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            var dialog = new AddConditionDialog(ServiceProvider, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            Rule.Conditions.Add(newItem);
            var listViewItem = new ConditionListViewItem(newItem);
            lvConditions.Items.Add(listViewItem);
            listViewItem.Selected = true;
            InformChanges();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected condition?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            listViewItem.Remove();
            var item = listViewItem.Item;
            Rule.Conditions.Remove(item);
            InformChanges();
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var dialog = new AddConditionDialog(ServiceProvider, listViewItem.Item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            listViewItem.Update();
            InformChanges();
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            Rule.Conditions.RemoveAt(index);
            listViewItem.Remove();

            Rule.Conditions.Insert(index + 1, item);
            lvConditions.Items.Insert(index + 1, listViewItem);
            listViewItem.Selected = true;
            InformChanges();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            Rule.Conditions.RemoveAt(index);
            listViewItem.Remove();

            Rule.Conditions.Insert(index - 1, item);
            lvConditions.Items.Insert(index - 1, listViewItem);
            listViewItem.Selected = true;
            InformChanges();
        }

        private void UpdateConditionsButtons()
        {
            var hasSelection = lvConditions.SelectedItems.Count > 0;
            btnRemove.Enabled = btnEdit.Enabled = hasSelection;
            btnDown.Enabled = hasSelection && lvConditions.SelectedItems[0].Index < lvConditions.Items.Count - 1;
            btnUp.Enabled = hasSelection && lvConditions.SelectedItems[0].Index > 0;
        }

        private void CbScopeSelectedIndexChanged(object sender, EventArgs e)
        {
            lblTagMatch.Visible = lblCustomTags.Visible = cbTags.Visible = cbMatch.Visible = cbScope.SelectedIndex == 0;
            lblContent.Text = cbScope.SelectedIndex == 0 ? "Content:" : "Variable value:";
            lblVariable.Visible = txtVariable.Visible = cbScope.SelectedIndex == 1;
        }

        private void CbPreConditionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPreCondition.SelectedIndex == cbPreCondition.Items.Count - 1)
            {
                var dialog = new AddPreConditionDialog(ServiceProvider, null);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cbPreCondition.Items.Insert(0, dialog.Item.Name);
                    _feature.PreConditions.Add(dialog.Item);
                    Rule.PreCondition = dialog.Item.Name;
                    cbPreCondition.SelectedIndex = 0;
                }
            }

            btnEditPrecondition.Enabled = cbPreCondition.SelectedIndex + 2 < cbPreCondition.Items.Count;
        }

        private void BtnEditPreconditionClick(object sender, EventArgs e)
        {
            var dialog = new AddPreConditionDialog(ServiceProvider, _feature.PreConditions.FirstOrDefault(item => item.Name == cbPreCondition.Text));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return;
            }
        }

        private void CbTagsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTags.SelectedIndex == cbTags.Items.Count - 1)
            {
                var dialog = new AddCustomTagsDialog(ServiceProvider);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cbTags.Items.Insert(0, dialog.Item.Name);
                    _feature.Tags.Add(dialog.Item);
                    Rule.CustomTags = dialog.Item.Name;
                    cbPreCondition.SelectedIndex = 0;
                }
            }
        }
    }
}
