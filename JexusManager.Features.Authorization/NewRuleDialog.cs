// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authorization
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Disposables;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewRuleDialog : DialogForm
    {
        public NewRuleDialog(IServiceProvider serviceProvider, AuthorizationRule existing, bool allowed, AuthorizationFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            var _allowed = existing == null ? allowed : existing.AccessType == 0L;
            Text = string.Format("{0} {1} Authorization Rule", existing == null ? "Add" : "Edit", _allowed ? "Allow" : "Deny");
            txtDescription.Text = _allowed
                ? "Allow access to this Web content to:"
                : "Deny access to this Web content to:";
            Item = existing ?? new AuthorizationRule(null);
            if (existing != null)
            {
                txtRoles.Text = Item.Roles;
                txtVerbs.Text = Item.Verbs;
                if (Item.Users == "*")
                {
                    rbAll.Checked = true;
                }
                else if (Item.Users == "?")
                {
                    rbAnonymous.Checked = true;
                }
                else
                {
                    rbUsers.Checked = true;
                    txtUsers.Text = Item.Users;
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.AccessType = _allowed ? 0L : 1L;
                    Item.Roles = string.Empty;
                    if (rbAll.Checked)
                    {
                        Item.Users = "*";
                    }
                    else if (rbAnonymous.Checked)
                    {
                        Item.Users = "?";
                    }
                    else if (rbUsers.Checked)
                    {
                        Item.Users = txtUsers.Text;
                    }

                    if (rbRoles.Checked)
                    {
                        Item.Roles = txtRoles.Text;
                        Item.Users = string.Empty;
                    }

                    if (cbVerbs.Checked)
                    {
                        Item.Verbs = txtVerbs.Text;
                    }

                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "This authorization rule already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbAll, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbRoles, "CheckedChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(rbAnonymous, "CheckedChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(rbUsers, "CheckedChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(cbVerbs, "CheckedChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtVerbs.Enabled = cbVerbs.Checked;
                    txtRoles.Enabled = rbRoles.Checked;
                    txtUsers.Enabled = rbUsers.Checked;
                    btnOK.Enabled = (rbRoles.Checked && !string.IsNullOrWhiteSpace(txtRoles.Text))
                        || (rbUsers.Checked && !string.IsNullOrWhiteSpace(txtUsers.Text))
                        || rbAll.Checked
                        || rbAnonymous.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtRoles, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtUsers, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtVerbs, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        public AuthorizationRule Item { get; private set; }
    }
}
