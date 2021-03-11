// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using JexusManager.Features.Rewrite.Inbound;

    using Microsoft.Web.Management.Client.Win32;

    public partial class AddPreConditionDialog : DialogForm
    {
        public AddPreConditionDialog(IServiceProvider serviceProvider, PreConditionItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            Item = existing ?? new PreConditionItem(null);
            if (existing != null)
            {
                txtName.Text = Item.Name;
                cbUsing.SelectedIndex = (int)Item.PatternSyntax;
                cbGrouping.SelectedIndex = (int)Item.LogicalGrouping;
                foreach (var condition in Item.Conditions)
                {
                    lvConditions.Items.Add(new ConditionListViewItem(condition));
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnAdd, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new AddConditionDialog(ServiceProvider, null);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var newItem = dialog.Item;
                    Item.Conditions.Add(newItem);
                    var listViewItem = new ConditionListViewItem(newItem);
                    lvConditions.Items.Add(listViewItem);
                    listViewItem.Selected = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
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
                    Item.Conditions.Remove(item);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnEdit, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
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
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnMoveDown, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
                    var index = listViewItem.Index;

                    var item = listViewItem.Item;
                    Item.Conditions.RemoveAt(index);
                    listViewItem.Remove();

                    Item.Conditions.Insert(index + 1, item);
                    lvConditions.Items.Insert(index + 1, listViewItem);
                    listViewItem.Selected = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnMoveUp, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var listViewItem = ((ConditionListViewItem)lvConditions.SelectedItems[0]);
                    var index = listViewItem.Index;

                    var item = listViewItem.Item;
                    Item.Conditions.RemoveAt(index);
                    listViewItem.Remove();

                    Item.Conditions.Insert(index - 1, item);
                    lvConditions.Items.Insert(index - 1, listViewItem);
                    listViewItem.Selected = true;
                }));

            container.Add(
                Observable.FromEventPattern<ListViewItemSelectionChangedEventArgs>(lvConditions, "ItemSelectionChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var hasSelection = lvConditions.SelectedItems.Count > 0;
                    btnRemove.Enabled = btnEdit.Enabled = hasSelection;
                    btnMoveDown.Enabled = hasSelection && lvConditions.SelectedItems[0].Index < lvConditions.Items.Count - 1;
                    btnMoveUp.Enabled = hasSelection && lvConditions.SelectedItems[0].Index > 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Name = txtName.Text;
                    Item.PatternSyntax = cbUsing.SelectedIndex;
                    Item.LogicalGrouping = cbGrouping.SelectedIndex;
                    Item.Conditions.Clear();
                    foreach (ConditionListViewItem item in lvConditions.Items)
                    {
                        Item.Conditions.Add(item.Item);
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        public PreConditionItem Item { get; set; }

        private void AddPreconditionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=163111");
        }
    }
}
