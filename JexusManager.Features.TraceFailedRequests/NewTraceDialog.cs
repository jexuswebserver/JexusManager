// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewTraceDialog : DialogForm
    {
        public NewTraceDialog(IServiceProvider serviceProvider, TraceFailedRequestsItem existing, TraceFailedRequestsFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add ISAPI Filter" : "Edit ISAPI Filter";
            txtName.ReadOnly = existing != null;
            Item = existing ?? new TraceFailedRequestsItem(null);
            if (existing != null)
            {
                txtPath.Text = Item.Path;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Path = txtPath.Text;
                    if (!txtName.ReadOnly && feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A filter with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                                    && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPath, null);
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TraceFailedRequestsItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210482");
        }
    }
}
