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

    using Microsoft.Web.Management.Client.Win32;

    public partial class AddCustomTagsDialog : DialogForm
    {
        public AddCustomTagsDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    foreach (DataGridViewRow row in dgvTags.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dgvTags.Rows.Remove(row);
                        }
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item = new CustomTagsItem(null);
                    Item.Name = txtName.Text;
                    foreach (DataGridViewRow row in dgvTags.Rows)
                    {
                        var tag = new CustomTagItem(null);
                        tag.Name = row.Cells[0].Value.ToString();
                        tag.Attribute = row.Cells[1].Value.ToString();
                        Item.Add(tag);
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<DataGridViewCellEventArgs>(dgvTags, "RowEnter")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnRemove.Enabled = !dgvTags.SelectedRows[0].IsNewRow;
                }));
        }

        public CustomTagsItem Item { get; set; }

        private void AddCustomTagsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=163112");
        }
    }
}
