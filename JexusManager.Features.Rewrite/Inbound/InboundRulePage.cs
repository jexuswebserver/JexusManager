// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;
    using JexusManager.Services;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class InboundRulePage : ModuleDialogPage, IModuleChildPage
    {
        private sealed class PageTaskList : DefaultTaskList
        {
            private readonly InboundRulePage _owner;

            public PageTaskList(InboundRulePage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                           {
                               GetBackTaskItem("Back", "Back to Rules"),
                               MethodTaskItem.CreateSeparator().SetUsage(),
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

        private class ServerVariableListViewItem : ListViewItem
        {
            private readonly ListViewSubItem _value;

            private readonly ListViewSubItem _replace;

            public ServerVariableListViewItem(ServerVariableItem variable)
                : base(variable.Name)
            {
                _value = new ListViewSubItem(this, variable.Value);
                SubItems.Add(_value);
                _replace = new ListViewSubItem(this, variable.Replace.ToString());
                SubItems.Add(_replace);
                Item = variable;
            }

            public ServerVariableItem Item { get; }

            public void Update()
            {
                Text = Item.Name;
                _value.Text = Item.Value;
                _replace.Text = Item.Replace.ToString();
            }
        }

        private bool _hasChanges;

        private bool _initialized;

        private TaskList _taskList;

        private InboundFeature _feature;

        public InboundRulePage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)ServiceProvider.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            var info = (Tuple<InboundFeature, InboundRule>)navigationData;
            _feature = info.Item1;
            Rule = info.Item2;
            txtName.ReadOnly = Rule != null;
            if (Rule != null)
            {
                // TODO: invoke RuleSettingsUpdate somewhere.
                Rule.RuleSettingsUpdated = Refresh;
            }

            if (Rule == null)
            {
                Rule = new InboundRule(null);
                Rule.Enabled = true;
            }

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
                var result = ShowMessage(
                    "The changes you have made will be lost. Do you want to save changes?",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    var applied = ApplyChanges();
                    if (!applied)
                    {
                        return;
                    }
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

        public InboundRule Rule { get; set; }

        private void BtnTestClick(object sender, EventArgs e)
        {
            using var dialog = new RegexTestDialog(Module, txtPattern.Text, cbIgnoreCase.Checked, false);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtPattern.Text = dialog.Pattern;
            cbIgnoreCase.Checked = dialog.IgnoreCase;
        }

        protected override bool ApplyChanges()
        {
            if (!CanApplyChanges)
            {
                ShowMessage("The data in the page is invalid. Correct the data and try again.");
                return false;
            }

            uint status = 0;
            uint substatus = 0;
            if (cbAction.SelectedIndex == 3)
            {
                if (!uint.TryParse(txtStatus.Text, out status) || status < 100 || status > 999
                    || (status > 299 && status < 308))
                {
                    ShowMessage(
                        string.Format(
                            "The specific code '{0}' is not valid. The status code must be between 100 and 999 and cannot contain values between 300 and 307.",
                            txtStatus.Text),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    return false;
                }

                if (!uint.TryParse(txtSubstatus.Text, out substatus) || substatus > 999)
                {
                    ShowMessage(
                        string.Format(
                            "The specified code '{0}' is not valid. Sub-status codes must be between 0 and 999.",
                            txtSubstatus.Text),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    return false;
                }
            }

            Rule.Name = txtName.Text;
            Rule.PatternSyntax = cbType.SelectedIndex;
            Rule.StopProcessing = cbStop.Checked;
            Rule.PatternUrl = txtPattern.Text;
            Rule.IgnoreCase = cbIgnoreCase.Checked;
            Rule.Negate = cbMatch.SelectedIndex == 1;
            Rule.Type = cbAction.SelectedIndex;
            Rule.ActionUrl = cbAction.SelectedIndex == 1 ? txtUrl.Text : txtRedirect.Text;
            Rule.AppendQueryString = cbAction.SelectedIndex == 1 ? cbAppend.Checked : cbAppendRedirect.Checked;
            Rule.LogRewrittenUrl = cbLog.Checked;
            Rule.RedirectType = cbRedirect.SelectedIndex;
            Rule.StatusCode = status;
            Rule.SubStatusCode = substatus;
            Rule.StatusReason = txtReason.Text;
            Rule.StatusDescription = txtError.Text;

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

        protected override void CancelChanges()
        {
            _initialized = false;
            _hasChanges = false;
            Rule.CancelChanges();
            ClearChanges();
        }

        protected override bool HasChanges
        {
            get
            {
                return _hasChanges;
            }
        }

        protected override bool CanApplyChanges
        {
            get
            {
                return !string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPattern.Text) &&
                    (
                        (cbAction.SelectedIndex == 1 && !string.IsNullOrWhiteSpace(txtUrl.Text)) ||
                        (cbAction.SelectedIndex == 2 && !string.IsNullOrWhiteSpace(txtRedirect.Text)) ||
                        (cbAction.SelectedIndex == 3 && !string.IsNullOrWhiteSpace(txtReason.Text) && !string.IsNullOrWhiteSpace(txtError.Text) && !string.IsNullOrWhiteSpace(txtStatus.Text) && !string.IsNullOrWhiteSpace(txtSubstatus.Text)));
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
                cbAction.SelectedIndex = 1;
                cbAppend.Checked = true;
                cbAppendRedirect.Checked = true;
                cbIgnoreCase.Checked = true;
                cbMatch.SelectedIndex = 0;
                cbStop.Checked = false;
                cbType.SelectedIndex = 0;
                cbLog.Checked = false;
                cbRedirect.SelectedIndex = 0;
                cbAny.SelectedIndex = 0;
                if (Rule != null)
                {
                    txtName.Text = Rule.Name;
                    cbAction.SelectedIndex = (int)Rule.Type;
                    cbAppend.Checked = cbAppendRedirect.Checked = Rule.AppendQueryString;
                    cbIgnoreCase.Checked = Rule.IgnoreCase;
                    txtRedirect.Text = txtUrl.Text = Rule.ActionUrl;
                    cbMatch.SelectedIndex = Rule.Negate ? 1 : 0;
                    cbStop.Checked = Rule.StopProcessing;
                    cbType.SelectedIndex = (int)Rule.PatternSyntax;
                    txtPattern.Text = Rule.PatternUrl;
                    cbLog.Checked = Rule.LogRewrittenUrl;
                    cbRedirect.SelectedIndex = Rule.RedirectType;
                    txtStatus.Text = Rule.StatusCode.ToString();
                    txtSubstatus.Text = Rule.SubStatusCode.ToString();
                    txtReason.Text = Rule.StatusReason;
                    txtError.Text = Rule.StatusDescription;

                    lvVariables.Items.Clear();
                    foreach (var variable in Rule.ServerVariables)
                    {
                        lvVariables.Items.Add(new ServerVariableListViewItem(variable));
                        gbVariables.IsExpanded = true;
                    }

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

            CbActionSelectedIndexChanged(null, null);
            cbStop.Visible = cbAction.SelectedIndex == 0 || cbAction.SelectedIndex == 1;
            UpdateVariablesButtons();
            UpdateConditionsButtons();

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        public IModulePage ParentPage { get; set; }

        private void CbActionSelectedIndexChanged(object sender, EventArgs e)
        {
            gbRewrite.Visible = cbAction.SelectedIndex == 1;
            gbRedirect.Visible = cbAction.SelectedIndex == 2;
            gbCustom.Visible = cbAction.SelectedIndex == 3;
        }

        private void CbAppendRedirectCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            using (var dialog = new AddConditionDialog(ServiceProvider, null))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                Rule.Conditions.Add(newItem);
                var listViewItem = new ConditionListViewItem(newItem);
                lvConditions.Items.Add(listViewItem);
                listViewItem.Selected = true;
            }
            InformChanges();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (dialog.ShowMessage(
                "Are you sure that you want to remove the selected condition?",
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) != DialogResult.Yes)
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
            using (var dialog = new AddConditionDialog(ServiceProvider, listViewItem.Item))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
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
            btnDown.Enabled = hasSelection
                                   && lvConditions.SelectedItems[0].Index < lvConditions.Items.Count - 1;
            btnUp.Enabled = hasSelection && lvConditions.SelectedItems[0].Index > 0;
        }

        private void BtnVarAddClick(object sender, EventArgs e)
        {
            using (var dialog = new AddServerVariableDialog(ServiceProvider, null))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                Rule.ServerVariables.Add(newItem);
                var listViewItem = new ServerVariableListViewItem(newItem);
                lvVariables.Items.Add(listViewItem);
                listViewItem.Selected = true;
            }
            InformChanges();
        }

        private void BtnVarRemoveClick(object sender, EventArgs e)
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (dialog.ShowMessage(
                "Are you sure that you want to remove the selected entry?",
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            {
                return;
            }

            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            listViewItem.Remove();
            var item = listViewItem.Item;
            Rule.ServerVariables.Remove(item);
            InformChanges();
        }

        private void BtnVarEditClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            using (var dialog = new AddServerVariableDialog(ServiceProvider, listViewItem.Item))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            listViewItem.Update();
            InformChanges();
        }

        private void BtnVarDownClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            Rule.ServerVariables.RemoveAt(index);
            listViewItem.Remove();

            Rule.ServerVariables.Insert(index + 1, item);
            lvVariables.Items.Insert(index + 1, listViewItem);
            listViewItem.Selected = true;
            InformChanges();
        }

        private void BtnVarUpClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            Rule.ServerVariables.RemoveAt(index);
            listViewItem.Remove();

            Rule.ServerVariables.Insert(index - 1, item);
            lvVariables.Items.Insert(index - 1, listViewItem);
            listViewItem.Selected = true;
            InformChanges();
        }

        private void UpdateVariablesButtons()
        {
            var hasSelection = lvVariables.SelectedItems.Count > 0;
            btnVarRemove.Enabled = btnVarEdit.Enabled = hasSelection;
            btnVarDown.Enabled = hasSelection
                                      && lvVariables.SelectedItems[0].Index < lvVariables.Items.Count - 1;
            btnVarUp.Enabled = hasSelection && lvVariables.SelectedItems[0].Index > 0;
        }

        private void lvConditions_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateConditionsButtons();
        }

        private void lvVariables_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateVariablesButtons();
        }
    }
}
