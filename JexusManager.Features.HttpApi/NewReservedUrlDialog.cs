// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpApi
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewReservedUrlDialog : DialogForm
    {
        public NewReservedUrlDialog(IServiceProvider serviceProvider, ReservedUrlsFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtAddress, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = txtAddress.Text.Length > 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item = new ReservedUrlsItem(txtAddress.Text, "D:(A;;GX;;;WD)", feature);
                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A URL reservation already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        public ReservedUrlsItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("https://msdn.microsoft.com/en-us/library/windows/desktop/aa364698(v=vs.85).aspx");
        }
    }
}
