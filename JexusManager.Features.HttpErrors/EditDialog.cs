// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class EditDialog : DialogForm
    {
        public EditDialog(IServiceProvider serviceProvider, ConfigurationElement element)
            : base(serviceProvider)
        {
            InitializeComponent();
            var mode = (long)element["errorMode"];
            rbCustom.Checked = mode == 1;
            rbDetailed.Checked = mode == 2;
            rbRemote.Checked = mode == 0;

            var defaultMode = (long)element["defaultResponseMode"];
            cbType.SelectedIndex = (int)defaultMode;

            txtPath.Text = (string)element["defaultPath"];

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSelect, "Click")
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPath);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    if (rbCustom.Checked)
                    {
                        element["errorMode"] = 1L;
                    }
                    else if (rbDetailed.Checked)
                    {
                        element["errorMode"] = 2L;
                    }
                    else
                    {
                        element["errorMode"] = 0L;
                    }

                    element["defaultResponseMode"] = (long)cbType.SelectedIndex;
                    element["defaultPath"] = txtPath.Text;
                }));
        }

        private void EditDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210481");
        }
    }
}
