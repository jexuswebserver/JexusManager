// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Handlers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewWildcardDialog : DialogForm
    {
        private readonly HandlersFeature _feature;

        public NewWildcardDialog(IServiceProvider serviceProvider, HandlersItem existing, HandlersFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add Wildcard Script Map" : "Edit Wildcard Script Map";
            txtName.ReadOnly = existing != null;
            _feature = feature;
            Item = existing ?? new HandlersItem(null);
            if (existing == null)
            {
                Item.Modules = "IsapiModule";
            }
            else
            {
                txtExecutable.Text = Item.ScriptProcessor;
                txtName.Text = Item.Name;
                txtPath.Text = Item.Path;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.ScriptProcessor = txtExecutable.Text;
                    Item.Name = txtName.Text;
                    Item.Path = txtPath.Text;
                    if (!txtName.ReadOnly && _feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A handler with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtExecutable, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                        && !string.IsNullOrWhiteSpace(txtExecutable.Text)
                        && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtExecutable, "(*.dll)|*.dll|(*.exe)|*.exe");
                }));
        }

        public HandlersItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210505#Add_Wildcard");
        }
    }
}
