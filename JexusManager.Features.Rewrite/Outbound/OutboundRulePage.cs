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
                this.SubItems.Add(_matchType);
                _pattern = new ListViewSubItem(this, condition.Pattern);
                this.SubItems.Add(_pattern);
                this.Item = condition;
            }

            public ConditionItem Item { get; }

            public void Update()
            {
                this.Text = this.Item.Input;
                _matchType.Text = GetText(this.Item.MatchType);
                _pattern.Text = this.Item.MatchType > 3 ? this.Item.Pattern : "N/A";
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
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            // TODO: pictureBox1.Image
            var info = (Tuple<OutboundFeature, OutboundRule>)navigationData;
            _feature = info.Item1;
            this.Rule = info.Item2;
            txtName.ReadOnly = this.Rule != null;
            if (this.Rule != null)
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
                this.Rule.RuleSettingsUpdated = this.Refresh;
            }

            cbPreCondition.Items.Add("<None>");
            cbPreCondition.Items.Add("<Create New Precondition...>");
            cbTags.Items.Add("<Create New Tags Collection...>");
            this.Refresh();
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
                    this.ApplyChanges();
                }
            }

            var service = (INavigationService)this.GetService(typeof(INavigationService));
            service?.NavigateBack(1);
            _feature.SelectedItem = this.Rule;
            _feature.Refresh();
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            this.InformChanges();
        }

        public OutboundRule Rule { get; set; }

        private void BtnTestClick(object sender, EventArgs e)
        {
            var dialog = new RegexTestDialog(this.Module, txtPattern.Text, cbIgnoreCase.Checked, false);
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
            if (this.Rule == null)
            {
                this.Rule = new OutboundRule(null);
            }

            this.Rule.Name = txtName.Text;
            this.Rule.PreCondition = cbPreCondition.Text == "<None>" ? string.Empty : cbPreCondition.Text;
            Rule.CustomTags = cbTags.Text;
            this.Rule.Enabled = true;
            this.Rule.Syntax = cbUsing.SelectedIndex;
            this.Rule.Stopping = cbStop.Checked;
            this.Rule.Filter = filter;
            this.Rule.ServerVariable = txtVariable.Text;
            this.Rule.Pattern = txtPattern.Text;
            this.Rule.Negate = cbContent.SelectedIndex == 1;
            this.Rule.IgnoreCase = cbIgnoreCase.Checked;
            this.Rule.TrackAllCaptures = cbTrack.Checked;
            this.Rule.LogicalGrouping = cbAny.SelectedIndex;
            this.Rule.Action = cbAction.SelectedIndex;
            Rule.Value = txtValue.Text;
            Rule.Replace = !string.IsNullOrWhiteSpace(txtValue.Text);

            if (!this.Rule.ApplyChanges())
            {
                return false;
            }

            this.ClearChanges();
            txtName.ReadOnly = true;
            _feature.AddItem(this.Rule);
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
            this.Rule.CancelChanges();
            this.ClearChanges();
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
            this.Refresh();
        }

        private void ClearChanges()
        {
            _hasChanges = false;
            _initialized = false;
            this.Refresh();
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
                txtName.ReadOnly = this.Rule != null;
                cbPreCondition.SelectedIndex = 0;
                cbAction.SelectedIndex = 0;
                cbIgnoreCase.Checked = true;
                cbScope.SelectedIndex = 0;
                cbStop.Checked = false;
                cbUsing.SelectedIndex = 0;
                cbContent.SelectedIndex = 0;
                if (this.Rule != null)
                {
                    SetFilter(this.Rule.Filter, cbMatch);
                    txtName.Text = this.Rule.Name;
                    cbPreCondition.Text = this.Rule.PreCondition == string.Empty ? "<None>" : this.Rule.PreCondition;
                    cbTags.Text = Rule.CustomTags;
                    cbAction.SelectedIndex = (int)this.Rule.Action;
                    cbIgnoreCase.Checked = this.Rule.IgnoreCase;
                    txtVariable.Text = this.Rule.ServerVariable;
                    cbScope.SelectedIndex = string.IsNullOrWhiteSpace(this.Rule.ServerVariable) ? 0 : 1;
                    cbStop.Checked = this.Rule.Stopping;
                    cbUsing.SelectedIndex = (int)this.Rule.Syntax;
                    cbContent.SelectedIndex = this.Rule.Negate ? 1 : 0;
                    txtPattern.Text = this.Rule.Pattern;
                    txtValue.Text = this.Rule.Value;

                    lvConditions.Items.Clear();
                    foreach (var condition in this.Rule.Conditions)
                    {
                        lvConditions.Items.Add(new ConditionListViewItem(condition));
                        gbConditions.IsExpanded = true;
                    }

                    cbAny.SelectedIndex = (int)this.Rule.LogicalGrouping;
                    cbTrack.Checked = this.Rule.TrackAllCaptures;
                }

                _initialized = true;
            }

            cbTags.Enabled = cbMatch.CheckBoxItems[12].Checked;
            this.CbActionSelectedIndexChanged(null, null);
            cbStop.Visible = cbAction.SelectedIndex == 0 || cbAction.SelectedIndex == 1;
            this.UpdateConditionsButtons();

            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        public IModulePage ParentPage { get; set; }

        private void CbActionSelectedIndexChanged(object sender, EventArgs e)
        {
            gbRewrite.Visible = cbAction.SelectedIndex == 1;
        }

        private void CbAppendRedirectCheckedChanged(object sender, EventArgs e)
        {
            this.InformChanges();
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            var dialog = new AddConditionDialog(this.ServiceProvider, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            this.Rule.Conditions.Add(newItem);
            var listViewItem = new ConditionListViewItem(newItem);
            lvConditions.Items.Add(listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
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
            this.Rule.Conditions.Remove(item);
            this.InformChanges();
        }

        private void BtnEditClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var dialog = new AddConditionDialog(this.ServiceProvider, listViewItem.Item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            listViewItem.Update();
            this.InformChanges();
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            this.Rule.Conditions.RemoveAt(index);
            listViewItem.Remove();

            this.Rule.Conditions.Insert(index + 1, item);
            lvConditions.Items.Insert(index + 1, listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            this.Rule.Conditions.RemoveAt(index);
            listViewItem.Remove();

            this.Rule.Conditions.Insert(index - 1, item);
            lvConditions.Items.Insert(index - 1, listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
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
                var dialog = new AddPreConditionDialog(this.ServiceProvider, null);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cbPreCondition.Items.Insert(0, dialog.Item.Name);
                    _feature.PreConditions.Add(dialog.Item);
                    this.Rule.PreCondition = dialog.Item.Name;
                    cbPreCondition.SelectedIndex = 0;
                }
            }

            btnEditPrecondition.Enabled = cbPreCondition.SelectedIndex + 2 < cbPreCondition.Items.Count;
        }

        private void BtnEditPreconditionClick(object sender, EventArgs e)
        {
            var dialog = new AddPreConditionDialog(this.ServiceProvider, _feature.PreConditions.FirstOrDefault(item => item.Name == cbPreCondition.Text));
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return;
            }
        }

        private void CbTagsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTags.SelectedIndex == cbTags.Items.Count - 1)
            {
                var dialog = new AddCustomTagsDialog(this.ServiceProvider);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    cbTags.Items.Insert(0, dialog.Item.Name);
                    _feature.Tags.Add(dialog.Item);
                    this.Rule.CustomTags = dialog.Item.Name;
                    cbPreCondition.SelectedIndex = 0;
                }
            }
        }
    }
}
