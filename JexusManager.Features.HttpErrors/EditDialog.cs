// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class EditDialog : DialogForm
    {
        public EditDialog(IServiceProvider serviceProvider, HttpErrorsSettings settings, HttpErrorsFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            var mode = settings.ErrorMode;
            rbCustom.Checked = mode == 1;
            rbDetailed.Checked = mode == 2;
            rbRemote.Checked = mode == 0;

            var defaultMode = settings.DefaultResponseMode;
            cbType.SelectedIndex = (int)defaultMode;

            txtPath.Text = settings.DefaultPath;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtPath, string.Empty, null);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (rbCustom.Checked)
                    {
                        settings.ErrorMode = 1L;
                    }
                    else if (rbDetailed.Checked)
                    {
                        settings.ErrorMode = 2L;
                    }
                    else
                    {
                        settings.ErrorMode = 0L;
                    }

                    settings.DefaultResponseMode = cbType.SelectedIndex;
                    settings.DefaultPath = txtPath.Text;
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
