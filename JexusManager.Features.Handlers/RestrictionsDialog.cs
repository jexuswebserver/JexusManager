// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Handlers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class RestrictionsDialog : DialogForm
    {
        public RestrictionsDialog(IServiceProvider serviceProvider, HandlersItem item)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbInvoke.Checked = item.ResourceType != 3L;
            rbFile.Checked = item.ResourceType == 0L;
            rbFolder.Checked = item.ResourceType == 1L;
            rbEither.Checked = item.ResourceType == 2L;

            rbNone.Checked = item.RequireAccess == 0L;
            rbRead.Checked = item.RequireAccess == 1L;
            rbWrite.Checked = item.RequireAccess == 2L;
            rbScript.Checked = item.RequireAccess == 3L;
            rbExecute.Checked = item.RequireAccess == 4L;

            rbAllVerbs.Checked = item.Verb == "*";
            rbSelectedVerbs.Checked = !rbAllVerbs.Checked;
            if (rbSelectedVerbs.Checked)
            {
                txtVerbs.Text = item.Verb;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (!cbInvoke.Checked)
                    {
                        item.ResourceType = 3L;
                    }
                    else if (rbFile.Checked)
                    {
                        item.ResourceType = 0L;
                    }
                    else if (rbFolder.Checked)
                    {
                        item.ResourceType = 1L;
                    }
                    else if (rbEither.Checked)
                    {
                        item.ResourceType = 2L;
                    }

                    if (rbNone.Checked)
                    {
                        item.RequireAccess = 0L;
                    }
                    else if (rbRead.Checked)
                    {
                        item.RequireAccess = 1L;
                    }
                    else if (rbWrite.Checked)
                    {
                        item.RequireAccess = 2L;
                    }
                    else if (rbScript.Checked)
                    {
                        item.RequireAccess = 3L;
                    }
                    else if (rbExecute.Checked)
                    {
                        item.RequireAccess = 4L;
                    }

                    if (rbAllVerbs.Checked)
                    {
                        item.Verb = "*";
                    }
                    else if (rbSelectedVerbs.Checked)
                    {
                        item.Verb = txtVerbs.Text;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbInvoke, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    rbFile.Enabled = rbFolder.Enabled = rbEither.Enabled = cbInvoke.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbSelectedVerbs, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbAllVerbs, "CheckedChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtVerbs.Enabled = rbSelectedVerbs.Checked;
                }));
        }

        private void RestrictionsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210505#Request_Restrictions");
        }
    }
}
