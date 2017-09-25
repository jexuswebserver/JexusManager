// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Linq;

    public sealed partial class ApplicationPoolBasicSettingsDialog : DialogForm
    {
        public ApplicationPool Pool { get; private set; }

        public ApplicationPoolBasicSettingsDialog(IServiceProvider serviceProvider, ApplicationPool pool, ApplicationPoolDefaults defaults, ApplicationPoolCollection collection)
            : base(serviceProvider)
        {
            InitializeComponent();
            Pool = pool;
            var add = pool == null;
            if (pool == null)
            {
                Text = "Add Application Pool";
                cbStart.Checked = defaults.AutoStart;
                cbMode.SelectedIndex = (int)defaults.ManagedPipelineMode;
                SetRuntimeVersion(defaults.ManagedRuntimeVersion);
            }
            else
            {
                Text = "Edit Application Pool";
                txtName.Text = pool.Name;
                txtName.Enabled = false;
                cbStart.Checked = pool.AutoStart;
                cbMode.SelectedIndex = (int)pool.ManagedPipelineMode;
                SetRuntimeVersion(Pool.ManagedRuntimeVersion);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (Pool == null)
                    {
                        if (collection.Any(item => item.Name == txtName.Text))
                        {
                            ShowMessage("An application pool with this name already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            txtName.Focus();
                            return;
                        }

                        Pool = collection.Add(txtName.Text);
                    }
                    else
                    {
                        Pool.Name = txtName.Text;
                    }

                    if (cbVersion.SelectedIndex == 0)
                    {
                        Pool.ManagedRuntimeVersion = "v4.0";
                    }
                    else if (cbVersion.SelectedIndex == 1)
                    {
                        Pool.ManagedRuntimeVersion = "v2.0";
                    }
                    else
                    {
                        Pool.ManagedRuntimeVersion = string.Empty;
                    }

                    if (add && collection.Parent.Mode == WorkingMode.IisExpress)
                    {
                        Pool["CLRConfigFile"] = @"%IIS_USER_HOME%\config\aspnet.config";
                    }

                    Pool.AutoStart = cbStart.Checked;
                    Pool.ManagedPipelineMode = (ManagedPipelineMode)cbMode.SelectedIndex;
                    DialogResult = DialogResult.OK;
                }));
        }

        private void SetRuntimeVersion(string managedRuntimeVersion)
        {
            if (managedRuntimeVersion == "v4.0")
            {
                cbVersion.SelectedIndex = 0;
            }
            else if (managedRuntimeVersion == "v2.0")
            {
                cbVersion.SelectedIndex = 1;
            }
            else
            {
                cbVersion.SelectedIndex = 2;
            }
        }

        private void ApplicationPoolBasicSettingsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210456");
        }
    }
}
