// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewNativeDialog : DialogForm
    {
        public NewNativeDialog(IServiceProvider serviceProvider, GlobalModule existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            if (existing != null)
            {
                txtName.Text = existing.Name;
                txtPath.Text = existing.Image;
                Text = "Edit Native Module Registration";
                txtName.SelectAll();
                Item = existing;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtPath, "(*.dll)|*.dll|All Files (*.*)|*.*", null);
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
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (Item == null)
                    {
                        Item = new GlobalModule(null);
                    }

                    Item.Name = txtName.Text;
                    Item.Image = txtPath.Text;

                    try
                    {
                        var bit32Condition = "bitness32";
                        var bit64Condition = "bitness64";
                        var bit32 = DialogHelper.GetImageArchitecture(txtPath.Text);
                        if (bit32 && !Item.PreConditions.Contains(bit32Condition))
                        {
                            Item.PreConditions.Add(bit32Condition);
                        }
                        else if (!bit32 && !Item.PreConditions.Contains(bit64Condition))
                        {
                            Item.PreConditions.Add(bit64Condition);
                        }
                    }
                    catch (Exception)
                    {
                        ShowMessage(
                            "The specific module is invalid.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    ShowMessage("The module has been added successfully, but its section definition(s) might haven't yet been added to the applicationHost.config file. You can add them manually. Please refer to its documentation for more details.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1);
                    DialogResult = DialogResult.OK;
                }));
        }

        public GlobalModule Item { get; set; }
    }
}
