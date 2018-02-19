// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    internal partial class BindingsDialog : DialogForm
    {
        private readonly Site _site;

        public BindingsDialog(IServiceProvider serviceProvider, Site site)
            : base(serviceProvider)
        {
            InitializeComponent();
            _site = site;
            foreach (Binding binding in site.Bindings)
            {
                var node = new ListViewItem(new[]
                {
                    binding.Protocol,
                    binding.Host.HostToDisplay(),
                    binding.EndPoint?.Port.ToString(CultureInfo.InvariantCulture),
                    binding.EndPoint?.Address.AddressToDisplay(),
                    binding.CanBrowse ? string.Empty : binding.BindingInformation
                })
                { Tag = binding };
                listView1.Items.Add(node);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnClose, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Close();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(listView1, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var selected = listView1.SelectedItems.Count > 0;
                    btnEdit.Enabled = selected && ProtocolMatched(listView1.SelectedItems);
                    btnRemove.Enabled = selected && listView1.Items.Count > 1;
                    btnBrowse.Enabled = selected && ProtocolMatched(listView1.SelectedItems);

                    if (!btnRemove.Enabled)
                    {
                        return;
                    }

                    if (Helper.IsRunningOnMono())
                    {
                        return;
                    }

                    var binding = (Binding)listView1.SelectedItems[0].Tag;
                    var supportsSni = binding.Parent.Parent.Server.SupportsSni;
                    var toElevate = selected && (!supportsSni || (supportsSni && binding.GetIsSni()));
                    if (toElevate)
                    {
                        JexusManager.NativeMethods.TryAddShieldToButton(btnRemove);
                    }
                    else
                    {
                        JexusManager.NativeMethods.RemoveShieldFromButton(btnRemove);
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var result = MessageBox.Show("Are you sure you want to remove the selected binding?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result != DialogResult.Yes)
                    {
                        return;
                    }

                    var binding = (Binding)listView1.SelectedItems[0].Tag;
                    binding.CleanUpSni();
                    listView1.SelectedItems[0].Remove();
                    _site.Bindings.Remove(binding);
                    _site.Server.CommitChanges();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnEdit, "Click")
                .Merge(Observable.FromEventPattern<EventArgs>(listView1, "MouseDoubleClick"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (listView1.SelectedItems.Count == 0)
                    {
                        return;
                    }

                    var item = listView1.SelectedItems[0];
                    var binding = (Binding)item.Tag;
                    var dialog = new BindingDialog(ServiceProvider, binding, _site);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    item.SubItems[1].Text = binding.Host.HostToDisplay();
                    item.SubItems[2].Text = binding.EndPoint.Port.ToString(CultureInfo.InvariantCulture);
                    item.SubItems[3].Text = binding.EndPoint.Address.AddressToDisplay();
                    _site.Server.CommitChanges();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnAdd, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var dialog = new BindingDialog(ServiceProvider, null, _site);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    var binding = dialog.Binding;
                    if (binding == null)
                    {
                        return;
                    }

                    var node = new ListViewItem(new[]
                        {
                    binding.Protocol,
                    binding.Host.HostToDisplay(),
                    binding.EndPoint.Port.ToString(CultureInfo.InvariantCulture),
                    binding.EndPoint.Address.AddressToDisplay(),
                    string.Empty
                })
                    { Tag = binding };
                    listView1.Items.Add(node);
                    _site.Bindings.Add(binding);
                    _site.Server.CommitChanges();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var binding = (Binding)listView1.SelectedItems[0].Tag;
                    DialogHelper.ProcessStart(binding.ToUri());
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnLink, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    var dialog = new LinkDiagDialog(serviceProvider, site);
                    dialog.ShowDialog();
                }));
        }

        private static bool ProtocolMatched(ListView.SelectedListViewItemCollection collection)
        {
            return collection.Count != 0 && ((Binding)collection[0].Tag).CanBrowse;
        }

        private void BindingsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210531#Site_Bingings");
        }

        private void BindingsDialogFormClosing(object sender, FormClosingEventArgs e)
        {
            if (listView1.Items.Count > 1 && _site.Server.Mode == WorkingMode.Jexus)
            {
                MessageBox.Show("Only one binding is supported by Jexus.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
            }
        }
    }
}
