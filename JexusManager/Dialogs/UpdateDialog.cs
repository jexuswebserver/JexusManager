// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace JexusManager.Dialogs
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using System.Windows.Forms;

    public partial class UpdateDialog : Form
    {
        public UpdateDialog()
        {
            InitializeComponent();
        }

        private async void UpdateDialog_Load(object sender, EventArgs e)
        {
            txtStep.Text = "Checking update...";
            string version;
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("JexusManager"));
                var releases = await client.Repository.Release.GetAll("jexuswebserver", "JexusManager");
                var recent = releases[0];
                version = recent.Name;
            }
            catch (Exception)
            {
                MessageBox.Show("No update is found", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            Version latest;
            if (!Version.TryParse(version, out latest))
            {
                MessageBox.Show("No update is found", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            var current = Assembly.GetExecutingAssembly().GetName().Version;
            if (current >= latest)
            {
                MessageBox.Show("No update is found", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            var result = MessageBox.Show(string.Format("An update ({0}) is available. Do you want to download it now?", latest), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                Close();
                return;
            }

            Process.Start("https://jexus.codeplex.com/releases");
            Close();
        }
    }
}
