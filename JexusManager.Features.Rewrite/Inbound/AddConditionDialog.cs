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
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddConditionDialog : DialogForm
    {
        public AddConditionDialog(IServiceProvider serviceProvider, ConditionItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            Item = existing ?? new ConditionItem(null);
            txtInput.Text = Item.Input;
            txtPattern.Text = Item.Pattern;
            cbIgnore.Checked = Item.IgnoreCase;
            cbCheck.SelectedIndex = Item.MatchType;
            txtInput.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var source = new AutoCompleteStringCollection();
            foreach (var item in DialogHelper.Conditions)
            {
                source.Add($"{{{item}}}");
            }

            txtInput.AutoCompleteCustomSource = source;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            var check = Observable.FromEventPattern<EventArgs>(cbCheck, "SelectedIndexChanged");
            container.Add(
                check.ObserveOn(System.Threading.SynchronizationContext.Current)
                .Merge(Observable.FromEventPattern<EventArgs>(this, "Load"))
                .Subscribe(evt =>
                {
                    txtPattern.Enabled = btnTest.Enabled = cbIgnore.Enabled = cbCheck.SelectedIndex > 3;
                    if (!txtPattern.Enabled)
                    {
                        txtPattern.Text = string.Empty;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPattern, "TextChanged")
                .Merge(check)
                .Merge(Observable.FromEventPattern<EventArgs>(txtInput, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtInput.Text) && (cbCheck.SelectedIndex < 4 || !string.IsNullOrWhiteSpace(txtPattern.Text));
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Input = txtInput.Text;
                    Item.Pattern = txtPattern.Text;
                    Item.IgnoreCase = cbIgnore.Checked;
                    Item.MatchType = cbCheck.SelectedIndex;
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnTest, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new RegexTestDialog(ServiceProvider, txtPattern.Text, cbIgnore.Checked, true);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtPattern.Text = dialog.Pattern;
                    cbIgnore.Checked = dialog.IgnoreCase;
                }));
        }

        public ConditionItem Item { get; private set; }

        private void AddConditionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130403&amp;clcid=0x409");
        }
    }
}
