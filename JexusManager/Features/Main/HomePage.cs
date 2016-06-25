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

        private void txtHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://jexus.codeplex.com");
        }

        private void txtHomepageChinese_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://linuxdot.net");
        }

        private void txtLeXtudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://lextudio.com");
        }

        protected override bool ShowTaskList
        {
            get { return false; }
        }
    }
}
