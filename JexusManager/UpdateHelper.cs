// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Octokit;

namespace JexusManager
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;

    internal static class UpdateHelper
    {
        public enum UpdateErrorType
        {
            None,
            ConnectionError,
            NoReleaseFound,
            Other
        }

        public class UpdateInfo
        {
            public bool UpdateAvailable { get; set; }
            public Version CurrentVersion { get; set; }
            public Version LatestVersion { get; set; }
            public string ReleaseUrl { get; set; }
            public string ErrorMessage { get; set; }
            public UpdateErrorType ErrorType { get; set; } = UpdateErrorType.None;
        }

        public static async Task<UpdateInfo> CheckForUpdate()
        {
            var updateInfo = new UpdateInfo
            {
                ReleaseUrl = "https://github.com/jexuswebserver/JexusManager/releases",
                CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version
            };

            string version = null;
            var previous = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                var client = new GitHubClient(new ProductHeaderValue("JexusManager"));
                var releases = await client.Repository.Release.GetAll("jexuswebserver", "JexusManager");
                if (releases.Count == 0)
                {
                    updateInfo.ErrorMessage = "No update is found.";
                    updateInfo.ErrorType = UpdateErrorType.NoReleaseFound;
                    return updateInfo;
                }

                var recent = releases[0];
                version = recent.TagName.Substring(1);
            }
            catch (Exception)
            {
                updateInfo.ErrorMessage = "Cannot connect to GitHub.";
                updateInfo.ErrorType = UpdateErrorType.ConnectionError;
                return updateInfo;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = previous;
            }

            if (!Version.TryParse(version, out Version latest))
            {
                updateInfo.ErrorMessage = "No update is found.";
                updateInfo.ErrorType = UpdateErrorType.NoReleaseFound;
                return updateInfo;
            }

            updateInfo.LatestVersion = latest;
            updateInfo.UpdateAvailable = updateInfo.CurrentVersion < latest;
            
            return updateInfo;
        }
    }
}
