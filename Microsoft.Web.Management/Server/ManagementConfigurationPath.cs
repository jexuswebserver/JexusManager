// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementConfigurationPath
    {
        private ManagementConfigurationPath(ConfigurationPathType pathType, string siteName, string applicationPath, string folderPath)
        {
            PathType = pathType;
            SiteName = siteName;
            ApplicationPath = applicationPath;
            FolderPath = folderPath;
        }

        public static ManagementConfigurationPath CreateApplicationConfigurationPath(string applicationPath)
        {
            return CreateApplicationConfigurationPath(null, applicationPath);
        }

        public static ManagementConfigurationPath CreateApplicationConfigurationPath(string siteName, string applicationPath)
        {
            return new ManagementConfigurationPath(ConfigurationPathType.Application, siteName, applicationPath, null);
        }

        public static ManagementConfigurationPath CreateFileConfigurationPath(string applicationPath, string filePath)
        {
            return CreateFileConfigurationPath(null, applicationPath, filePath);
        }

        public static ManagementConfigurationPath CreateFileConfigurationPath(string siteName, string applicationPath, string filePath)
        {
            return new ManagementConfigurationPath(ConfigurationPathType.File, siteName, applicationPath, filePath);
        }

        public static ManagementConfigurationPath CreateFolderConfigurationPath(string applicationPath, string folderPath)
        {
            return CreateFolderConfigurationPath(null, applicationPath, folderPath);
        }

        public static ManagementConfigurationPath CreateFolderConfigurationPath(string siteName, string applicationPath, string folderPath)
        {
            return new ManagementConfigurationPath(ConfigurationPathType.Folder, siteName, applicationPath, folderPath);
        }

        public static ManagementConfigurationPath CreateServerConfigurationPath()
        {
            return new ManagementConfigurationPath(ConfigurationPathType.Server, null, null, null);
        }

        public static ManagementConfigurationPath CreateSiteConfigurationPath(string siteName)
        {
            return new ManagementConfigurationPath(ConfigurationPathType.Site, siteName, null, null);
        }

        public ICollection<string> GetBindingProtocols(IServiceProvider serviceProvider)
        {
            return null;
        }

        public string GetEffectiveConfigurationPath(ManagementScope scope)
        {
            if (scope == ManagementScope.Server)
            {
                return string.Empty;
            }

            if (scope == ManagementScope.Site)
            {
                return SiteName ?? string.Empty;
            }

            var applicationPath = string.IsNullOrEmpty(ApplicationPath) ? "/" : ApplicationPath;
            return string.IsNullOrEmpty(SiteName) ? applicationPath : SiteName + applicationPath;
        }

        public ManagementFrameworkVersion GetFrameworkVersion(IServiceProvider serviceProvider)
        {
            return null;
        }

        public string GetState()
        {
            return $"{(int)PathType}|{SiteName}|{ApplicationPath}|{FolderPath}";
        }

        public bool IsEquivalentScope(ManagementScope scope)
        {
            return scope switch
            {
                ManagementScope.Server => PathType == ConfigurationPathType.Server,
                ManagementScope.Site => PathType == ConfigurationPathType.Site,
                ManagementScope.Application => PathType == ConfigurationPathType.Application
                    || PathType == ConfigurationPathType.Folder
                    || PathType == ConfigurationPathType.File,
                _ => false
            };
        }

        public string ApplicationPath { get; }
        public string FolderPath { get; }
        public ConfigurationPathType PathType { get; }
        public string SiteName { get; }
    }
}
