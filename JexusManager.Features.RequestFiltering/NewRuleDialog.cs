// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewRuleDialog : DialogForm
    {
        public NewRuleDialog(IServiceProvider serviceProvider, FilteringRulesItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add Filtering Rule" : "Edit Filtering Rule";
            txtName.ReadOnly = existing != null;
            if (existing != null)
            {
                txtName.Text = existing.Name;
                dgvHeaders.DataSource = existing.Headers;
                dgvExtensions.DataSource = existing.Extensions;
                dgvStrings.DataSource = existing.DenyStrings;
                cbUrl.Checked = existing.ScanUrl;
                cbQuery.Checked = existing.ScanQueryString;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item = new FilteringRulesItem(null) { Name = txtName.Text };
                    Item.ScanQueryString = cbQuery.Checked;
                    Item.ScanUrl = cbUrl.Checked;
                    Item.Headers.Clear();
                    foreach (DataGridViewRow row in dgvHeaders.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            continue;
                        }

                        var header = row.Cells[0].Value.ToString();
                        Item.Headers.Add(new ScanHeadersItem(null) { RequestHeader = header });
                    }

                    Item.Extensions.Clear();
                    foreach (DataGridViewRow row in dgvExtensions.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            continue;
                        }

                        var header = row.Cells[0].Value.ToString();
                        Item.Extensions.Add(new AppliesToItem(null) { FileExtension = header });
                    }

                    Item.DenyStrings.Clear();
                    foreach (DataGridViewRow row in dgvStrings.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            continue;
                        }

                        var header = row.Cells[0].Value.ToString();
                        Item.DenyStrings.Add(new DenyStringsItem(null) { DenyString = header });
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(dgvHeaders, "RowsAdded"))
                .Merge(Observable.FromEventPattern<EventArgs>(dgvExtensions, "RowsAdded"))
                .Merge(Observable.FromEventPattern<EventArgs>(dgvStrings, "RowsAdded"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
    && dgvHeaders.RowCount > 1
    && dgvExtensions.RowCount > 1
    && dgvStrings.RowCount > 1;
                }));
        }

        public FilteringRulesItem Item { get; set; }

        private void NewHiddenSegmentDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526#Add_Filtering");
        }
    }
}
