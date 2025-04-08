// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementConfigurationPath
    {
        public static ManagementConfigurationPath CreateApplicationConfigurationPath(string applicationPath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateApplicationConfigurationPath(string siteName, string applicationPath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateFileConfigurationPath(string applicationPath, string filePath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateFileConfigurationPath(string siteName, string applicationPath, string filePath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateFolderConfigurationPath(string applicationPath, string folderPath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateFolderConfigurationPath(string siteName, string applicationPath, string folderPath)
        {
            return null;
        }

        public static ManagementConfigurationPath CreateServerConfigurationPath()
        {
            return null;
        }

        public static ManagementConfigurationPath CreateSiteConfigurationPath(string siteName)
        {
            return null;
        }

        public ICollection<string> GetBindingProtocols(IServiceProvider serviceProvider)
        {
            return null;
        }

        public string GetEffectiveConfigurationPath(ManagementScope scope)
        {
            return null;
        }

        public ManagementFrameworkVersion GetFrameworkVersion(IServiceProvider serviceProvider)
        {
            return null;
        }

        public string GetState()
        {
            return null;
        }

        public bool IsEquivalentScope(ManagementScope scope)
        {
            return false;
        }

        public string ApplicationPath { get; }
        public string FolderPath { get; }
        public ConfigurationPathType PathType { get; }
        public string SiteName { get; }
    }
}
