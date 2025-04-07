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

    public partial class AddCustomTagDialog : DialogForm
    {
        public AddCustomTagDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtAttribute, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtAttribute.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item = new CustomTagItem(null);
                    Item.Name = txtName.Text;
                    Item.Attribute = txtAttribute.Text;
                    DialogResult = DialogResult.OK;
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomTagItem Item { get; set; }

        private void AddCustomTagDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=163112");
        }
    }
}
