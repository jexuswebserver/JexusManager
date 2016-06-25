// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class NativeModulesDialog : DialogForm
    {
        public NativeModulesDialog(IServiceProvider serviceProvider, ModulesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            if (feature.CanRevert)
            {
                btnRegister.Visible = btnEdit.Visible = btnRemove.Visible = false;
                lvModules.Width += 130;
            }

            foreach (var global in feature.GlobalModules)
            {
                if (feature.Items.Any(module => module.Name == global.Name))
                {
                    continue;
                }

                var item = lvModules.Items.Add(global.Name);
                item.Tag = global;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    Items = new List<ModulesItem>();
                    foreach (ListViewItem item in lvModules.Items)
                    {
                        if (!item.Checked)
                        {
                            continue;
                        }

                        var module = (GlobalModule)item.Tag;
                        module.Loaded = true;
                        Items.Add(new ModulesItem(null) { Name = module.Name }.Load(feature));
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(lvModules, "SelectedIndexChanged")
                .Subscribe(evt =>
                {
                    btnEdit.Enabled = btnRemove.Enabled = lvModules.SelectedItems.Count > 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRegister, "Click")
                .Subscribe(evt =>
                {
                    var dialog = new NewNativeDialog(ServiceProvider, null);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var item = lvModules.Items.Add(dialog.Item.Name);
                    item.Tag = dialog.Item;
                    feature.AddGlobal(dialog.Item);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnEdit, "Click")
                .Subscribe(evt =>
                {
                    GlobalModule item = (GlobalModule)lvModules.SelectedItems[0].Tag;
                    var dialog = new NewNativeDialog(ServiceProvider, item);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    dialog.Item.Apply();
                    lvModules.SelectedItems[0].Text = dialog.Item.Name;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .Subscribe(evt =>
                {
                    var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                    if (
                        dialog.ShowMessage("Are you sure that you want to remove the selected native module registration?", "Confirm Remove",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                        DialogResult.Yes)
                    {
                        return;
                    }

                    GlobalModule item = (GlobalModule)lvModules.SelectedItems[0].Tag;
                    feature.RemoveGlobal(item);
                    lvModules.SelectedItems[0].Remove();
                }));
        }

        public List<ModulesItem> Items { get; set; }

        private void NativeModulesDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210521");
        }
    }
}
