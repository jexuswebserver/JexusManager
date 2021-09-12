// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class HomePage : ModulePage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void txtHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://www.jexusmanager.com");
        }

        protected override bool ShowTaskList => false;

        private void txtStudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://blog.lextudio.com");
        }

        private void btnSponsor_Click(object sender, System.EventArgs e)
        {
            DialogHelper.ProcessStart("https://github.com/sponsors/lextm");
        }
    }
}
