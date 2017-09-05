// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace JexusManager.Dialogs
{
    using System;
    using System.Diagnostics;
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
            string version = null;
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("JexusManager"));
                var releases = await client.Repository.Release.GetAll("jexuswebserver", "JexusManager");
                if (releases.Count == 0)
                {
                    MessageBox.Show("No update is found", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                var recent = releases[0];
                version = recent.TagName.Substring(1);
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot connect to GitHub. Will open https://github.com/jexuswebserver/JexusManager/releases.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogHelper.ProcessStart("https://github.com/jexuswebserver/JexusManager/releases");
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
                MessageBox.Show($"{current} is in use. No update is found, and {latest} is latest release", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
                return;
            }

            var result = MessageBox.Show($"{current} is in use. An update ({latest}) is available. Do you want to download it now?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                Close();
                return;
            }

            DialogHelper.ProcessStart("https://github.com/jexuswebserver/JexusManager/releases");
            Close();
        }
    }
}
