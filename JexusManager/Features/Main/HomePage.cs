// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class HomePage : ModulePage
    {
        private UpdateHelper.UpdateInfo _updateInfo;

        public HomePage()
        {
            InitializeComponent();
            LoadUpdateInfo();
        }

        private async void LoadUpdateInfo()
        {
            try
            {
                _updateInfo = await UpdateHelper.CheckForUpdate();
                UpdateVersionDisplay();
            }
            catch (Exception ex)
            {
                lblUpdateStatus.Text = $"Error checking for updates: {ex.Message}";
                lblUpdateStatus.ForeColor = Color.Red;
            }
        }

        private void UpdateVersionDisplay()
        {
            if (_updateInfo == null)
                return;

            // Always show current version
            lblCurrentVersion.Text = $"Current Version: {_updateInfo.CurrentVersion}";

            if (!string.IsNullOrEmpty(_updateInfo.ErrorMessage))
            {
                lblUpdateStatus.Text = _updateInfo.ErrorMessage;
                lblUpdateStatus.ForeColor = Color.Red;
                btnDownloadUpdate.Visible = false;
                return;
            }

            if (_updateInfo.UpdateAvailable)
            {
                lblUpdateStatus.Text = $"An update is available: {_updateInfo.LatestVersion}";
                lblUpdateStatus.ForeColor = Color.Green;
                btnDownloadUpdate.Visible = true;
            }
            else
            {
                lblUpdateStatus.Text = "You are using the latest version.";
                lblUpdateStatus.ForeColor = Color.Green;
                btnDownloadUpdate.Visible = false;
            }
        }

        private void txtHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://www.jexusmanager.com");
        }

        protected override bool ShowTaskList => false;

        private void txtStudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://halfblood.pro");
        }

        private void btnSponsor_Click(object sender, System.EventArgs e)
        {
            DialogHelper.ProcessStart("https://github.com/sponsors/lextm");
        }

        private void btnDownloadUpdate_Click(object sender, EventArgs e)
        {
            if (_updateInfo != null && !string.IsNullOrEmpty(_updateInfo.ReleaseUrl))
            {
                DialogHelper.ProcessStart(_updateInfo.ReleaseUrl);
            }
        }
    }
}
