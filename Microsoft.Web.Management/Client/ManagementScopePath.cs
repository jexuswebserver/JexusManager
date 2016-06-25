// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class ManagementScopePath
    {
        private ManagementScopePath(string serverName, int port, string siteName, string applicationPath)
        {
            ServerName = serverName;
            Port = port;
            SiteName = siteName;
            ApplicationPath = applicationPath;
        }

        public static ManagementScopePath CreateApplicationPath(string managementServerName, int port, string siteName, string applicationPath)
        {
            return new ManagementScopePath(managementServerName, port, siteName, applicationPath);
        }

        public static ManagementScopePath CreateServerPath(string managementServerName, int port)
        {
            return new ManagementScopePath(managementServerName, port, null, null);
        }

        public static ManagementScopePath CreateServerPath(string managementServerName, int port, ManagementFrameworkVersion frameworkVersion)
        {
            var result = new ManagementScopePath(managementServerName, port, null, null);
            result.SetFrameworkVersion(frameworkVersion);
            return result;
        }

        public static ManagementScopePath CreateSitePath(string managementServerName, int port, string siteName)
        {
            return new ManagementScopePath(managementServerName, port, siteName, null);
        }

        public override bool Equals(object obj)
        {
            var path = obj as ManagementScopePath;
            return path != null && ServerName == path.ServerName && Port == path.Port && SiteName == path.SiteName &&
                   ApplicationPath == path.ApplicationPath;
        }

        public override int GetHashCode()
        {
            return ServerName.GetHashCode() + Port + SiteName.GetHashCode() + ApplicationPath.GetHashCode();
        }

        public void SetFrameworkVersion(ManagementFrameworkVersion frameworkVersion)
        {
            FrameworkVersion = frameworkVersion;
        }

        public string ApplicationPath { get; }
        public ManagementFrameworkVersion FrameworkVersion { get; set; }
        public int Port { get; }
        public string ServerName { get; }
        public string ServerReference { get; }
        public string SiteName { get; }
    }
}