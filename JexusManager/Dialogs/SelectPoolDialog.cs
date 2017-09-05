// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class SelectPoolDialog : Form
    {
        public SelectPoolDialog(string name, ServerManager server)
        {
            InitializeComponent();
            int selected = 0;
            foreach (ApplicationPool pool in server.ApplicationPools)
            {
                int index = cbPools.Items.Add(pool);
                if (pool.Name == name)
                {
                    selected = index;
                }
            }

            cbPools.SelectedIndex = selected;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbPools, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var item = cbPools.SelectedItem as ApplicationPool;
                    if (item == null)
                    {
                        return;
                    }

                    Selected = item;
                    txtVersion.Text = string.Format(".Net CLR Version: {0}", item.ManagedRuntimeVersion.RuntimeVersionToDisplay());
                    txtMode.Text = string.Format("Pipeline mode: {0}", item.ManagedPipelineMode);
                }));
        }

        private void SelectPoolDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210458");
        }

        internal ApplicationPool Selected { get; set; }
    }
}
