// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class IdentityDialog : DialogForm
    {
        private readonly ApplicationPoolProcessModel _element;

        public IdentityDialog(IServiceProvider serviceProvider, ApplicationPoolProcessModel element)
            : base(serviceProvider)
        {
            _element = element;
            InitializeComponent();
            rbBuiltin.Checked = _element.IdentityType != ProcessModelIdentityType.SpecificUser;
            rbCustom.Checked = !rbBuiltin.Checked;
            if (_element.IdentityType == ProcessModelIdentityType.SpecificUser)
            {
                txtCustom.Text = _element.UserName;
            }
            else
            {
                switch (_element.IdentityType)
                {
                    case ProcessModelIdentityType.LocalSystem:
                        cbBuiltin.SelectedIndex = 1;
                        break;
                    case ProcessModelIdentityType.LocalService:
                        cbBuiltin.SelectedIndex = 0;
                        break;
                    case ProcessModelIdentityType.NetworkService:
                        cbBuiltin.SelectedIndex = 2;
                        break;
                    case ProcessModelIdentityType.ApplicationPoolIdentity:
                        cbBuiltin.SelectedIndex = 3;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void IdentityDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210456#ApplicationPoolIdentity");
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (rbCustom.Checked)
            {
                _element.UserName = txtCustom.Text;
                _element.IdentityType = ProcessModelIdentityType.SpecificUser;
            }

            if (rbBuiltin.Checked)
            {
                _element.UserName = string.Empty;
                switch (cbBuiltin.SelectedIndex)
                {
                    case 0:
                        _element.IdentityType = ProcessModelIdentityType.LocalService;
                        break;
                    case 1:
                        _element.IdentityType = ProcessModelIdentityType.LocalSystem;
                        break;
                    case 2:
                        _element.IdentityType = ProcessModelIdentityType.NetworkService;
                        break;
                    case 3:
                        _element.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                        break;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void RbBuiltinCheckedChanged(object sender, EventArgs e)
        {
            txtCustom.Enabled = btnSet.Enabled = rbCustom.Checked;
            cbBuiltin.Enabled = rbBuiltin.Checked;
        }

        private void BtnSetClick(object sender, EventArgs e)
        {
            var dialog = new CredentialsDialog(null, txtCustom.Text);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtCustom.Text = dialog.UserName;
            _element.Password = dialog.Password;
        }
    }
}
