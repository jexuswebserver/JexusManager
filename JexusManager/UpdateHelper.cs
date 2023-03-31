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
        public static async Task FindUpdate()
        {
            string version = null;
            var previous = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                var client = new GitHubClient(new ProductHeaderValue("JexusManager"));
                var releases = await client.Repository.Release.GetAll("jexuswebserver", "JexusManager");
                if (releases.Count == 0)
                {
                    DialogHelper.MessageBoxShow("No update is found.");
                    return;
                }

                var recent = releases[0];
                version = recent.TagName.Substring(1);
            }
            catch (Exception)
            {
                DialogHelper.MessageBoxShow("Cannot connect to GitHub. Will open https://github.com/jexuswebserver/JexusManager/releases.");
                DialogHelper.ProcessStart("https://github.com/jexuswebserver/JexusManager/releases");
                return;
            }
            finally
            {
                ServicePointManager.SecurityProtocol = previous;
            }

            Version latest;
            if (!Version.TryParse(version, out latest))
            {
                DialogHelper.MessageBoxShow("No update is found.");
                return;
            }

            var current = Assembly.GetExecutingAssembly().GetName().Version;
            if (current >= latest)
            {
                DialogHelper.MessageBoxShow($"{current} is in use. No update is found, and {latest} is latest release.");
                return;
            }

            DialogHelper.MessageBoxShow($"{current} is in use. An update ({latest}) is available. Will open https://github.com/jexuswebserver/JexusManager/releases.");
            DialogHelper.ProcessStart("https://github.com/jexuswebserver/JexusManager/releases");
        }
    }
}
