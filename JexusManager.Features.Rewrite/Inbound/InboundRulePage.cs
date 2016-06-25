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
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;

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

        private class ServerVariableListViewItem : ListViewItem
        {
            private readonly ListViewSubItem _value;

            private readonly ListViewSubItem _replace;

            public ServerVariableListViewItem(ServerVariableItem variable)
                : base(variable.Name)
            {
                _value = new ListViewSubItem(this, variable.Value);
                this.SubItems.Add(_value);
                _replace = new ListViewSubItem(this, variable.Replace.ToString());
                this.SubItems.Add(_replace);
                this.Item = variable;
            }

            public ServerVariableItem Item { get; }

            public void Update()
            {
                this.Text = this.Item.Name;
                _value.Text = this.Item.Value;
                _replace.Text = this.Item.Replace.ToString();
            }
        }

        private bool _hasChanges;

        private bool _initialized;

        private TaskList _taskList;

        private InboundFeature _feature;

        public InboundRulePage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            // TODO: pictureBox1.Image
            var info = (Tuple<InboundFeature, InboundRule>)navigationData;
            _feature = info.Item1;
            this.Rule = info.Item2;
            txtName.ReadOnly = this.Rule != null;
            if (this.Rule != null)
            {
                // TODO: invoke RuleSettingsUpdate somewhere.
                this.Rule.RuleSettingsUpdated = this.Refresh;
            }

            if (this.Rule == null)
            {
                this.Rule = new InboundRule(null);
                Rule.Enabled = true;
            }

            this.Refresh();
        }

        protected override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
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

        public InboundRule Rule { get; set; }

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

            if (!this.Rule.ApplyChanges())
            {
                return false;
            }

            this.ClearChanges();
            txtName.ReadOnly = true;
            _feature.AddItem(this.Rule);
            return true;
        }

        protected override void CancelChanges()
        {
            this.Rule.CancelChanges();
            this.ClearChanges();
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
                return !string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPattern.Text)
                       && !string.IsNullOrWhiteSpace(txtUrl.Text);
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
                if (this.Rule != null)
                {
                    txtName.Text = this.Rule.Name;
                    cbAction.SelectedIndex = (int)this.Rule.Type;
                    cbAppend.Checked = cbAppendRedirect.Checked = this.Rule.AppendQueryString;
                    cbIgnoreCase.Checked = this.Rule.IgnoreCase;
                    txtRedirect.Text = txtUrl.Text = this.Rule.ActionUrl;
                    cbMatch.SelectedIndex = this.Rule.Negate ? 1 : 0;
                    cbStop.Checked = this.Rule.StopProcessing;
                    cbType.SelectedIndex = (int)this.Rule.PatternSyntax;
                    txtPattern.Text = this.Rule.PatternUrl;
                    cbLog.Checked = this.Rule.LogRewrittenUrl;
                    cbRedirect.SelectedIndex = this.Rule.RedirectType;
                    txtStatus.Text = this.Rule.StatusCode.ToString();
                    txtSubstatus.Text = this.Rule.SubStatusCode.ToString();
                    txtReason.Text = this.Rule.StatusReason;
                    txtError.Text = this.Rule.StatusDescription;

                    lvVariables.Items.Clear();
                    foreach (var variable in this.Rule.ServerVariables)
                    {
                        lvVariables.Items.Add(new ServerVariableListViewItem(variable));
                        gbVariables.IsExpanded = true;
                    }

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

            this.CbActionSelectedIndexChanged(null, null);
            cbStop.Visible = cbAction.SelectedIndex == 0 || cbAction.SelectedIndex == 1;
            this.UpdateVariablesButtons();
            this.UpdateConditionsButtons();

            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
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
            btnDown.Enabled = hasSelection
                                   && lvConditions.SelectedItems[0].Index < lvConditions.Items.Count - 1;
            btnUp.Enabled = hasSelection && lvConditions.SelectedItems[0].Index > 0;
        }

        private void BtnVarAddClick(object sender, EventArgs e)
        {
            var dialog = new AddServerVariableDialog(this.ServiceProvider, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            this.Rule.ServerVariables.Add(newItem);
            var listViewItem = new ServerVariableListViewItem(newItem);
            lvVariables.Items.Add(listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
        }

        private void BtnVarRemoveClick(object sender, EventArgs e)
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
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
            this.Rule.ServerVariables.Remove(item);
            this.InformChanges();
        }

        private void BtnVarEditClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            var dialog = new AddServerVariableDialog(this.ServiceProvider, listViewItem.Item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            listViewItem.Update();
            this.InformChanges();
        }

        private void BtnVarDownClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            this.Rule.ServerVariables.RemoveAt(index);
            listViewItem.Remove();

            this.Rule.ServerVariables.Insert(index + 1, item);
            lvVariables.Items.Insert(index + 1, listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
        }

        private void BtnVarUpClick(object sender, EventArgs e)
        {
            var listViewItem = ((ServerVariableListViewItem)lvVariables.SelectedItems[0]);
            var index = listViewItem.Index;

            var item = listViewItem.Item;
            this.Rule.ServerVariables.RemoveAt(index);
            listViewItem.Remove();

            this.Rule.ServerVariables.Insert(index - 1, item);
            lvVariables.Items.Insert(index - 1, listViewItem);
            listViewItem.Selected = true;
            this.InformChanges();
        }

        private void UpdateVariablesButtons()
        {
            var hasSelection = lvVariables.SelectedItems.Count > 0;
            btnVarRemove.Enabled = btnVarEdit.Enabled = hasSelection;
            btnVarDown.Enabled = hasSelection
                                      && lvVariables.SelectedItems[0].Index < lvVariables.Items.Count - 1;
            btnVarUp.Enabled = hasSelection && lvVariables.SelectedItems[0].Index > 0;
        }
    }
}
