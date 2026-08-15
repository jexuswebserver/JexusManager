// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class SetRestrictionsDialog : DialogForm
    {
        public SetRestrictionsDialog(IServiceProvider serviceProvider, IpSecuritySettings settings, IpSecurityFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbDomain.Checked = settings.EnableReverseDns;
            cbAccess.SelectedIndex = settings.AllowUnlisted ? 0 : 1;
            cbProxy.Enabled = settings.EnableProxyMode != null;
            if (cbProxy.Enabled)
            {
                cbProxy.Checked = settings.EnableProxyMode.Value;
            }

            cbAction.Enabled = settings.DenyAction != null;
            if (cbAction.Enabled)
            {
                var action = settings.DenyAction.Value;
                if (action == 0L)
                {
                    cbAction.SelectedIndex = 0;
                }
                else if (action == 401L)
                {
                    cbAction.SelectedIndex = 1;
                }
                else if (action == 403L)
                {
                    cbAction.SelectedIndex = 2;
                }
                else
                {
                    cbAction.SelectedIndex = 3;
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    settings.EnableReverseDns = cbDomain.Checked;
                    settings.AllowUnlisted = cbAccess.SelectedIndex == 0;
                    if (cbProxy.Enabled)
                    {
                        settings.EnableProxyMode = cbProxy.Checked;
                    }

                    if (cbAction.Enabled)
                    {
                        if (cbAction.SelectedIndex == 0)
                        {
                            settings.DenyAction = 0L;
                        }
                        else if (cbAction.SelectedIndex == 1)
                        {
                            settings.DenyAction = 401L;
                        }
                        else if (cbAction.SelectedIndex == 2)
                        {
                            settings.DenyAction = 403L;
                        }
                        else
                        {
                            settings.DenyAction = 404L;
                        }
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }
    }
}
