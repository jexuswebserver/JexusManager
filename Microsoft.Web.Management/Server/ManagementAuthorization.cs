// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Principal;

namespace Microsoft.Web.Management.Server
{
    public static class ManagementAuthorization
    {
        public static ManagementAuthorizationInfoCollection GetAuthorizedUsers(
            string configurationPath,
            bool includeChildren,
            int itemIndex,
            int itemsPerPage
            )
        {
            return Provider.GetAuthorizedUsers(configurationPath, includeChildren, itemIndex, itemsPerPage);
        }

        public static string[] GetConfigurationPaths(
            IPrincipal principal,
            string baseConfigurationPath
            )
        {
            return Provider.GetConfigurationPaths(principal, baseConfigurationPath);
        }

        public static ManagementAuthorizationInfo Grant(
            string name,
            string configurationPath,
            bool isRole
            )
        {
            return Provider.Grant(name, configurationPath, isRole);
        }

        public static bool IsAuthorized(
            IPrincipal principal,
            string configurationPath
            )
        {
            return Provider.IsAuthorized(principal, configurationPath);
        }

        public static void RenameConfigurationPath(
            string configurationPath,
            string newConfigurationPath
            )
        {
            Provider.RenameConfigurationPath(configurationPath, newConfigurationPath);
        }

        public static void Revoke(
            string name
            )
        {
            Provider.Revoke(name);
        }

        public static void Revoke(
            string name,
            string configurationPath
            )
        { Provider.Revoke(name, configurationPath); }

        public static void RevokeConfigurationPath(
            string configurationPath
            )
        { Provider.RevokeConfigurationPath(configurationPath); }

        public static ManagementAuthorizationProvider Provider { get; }
    }
}