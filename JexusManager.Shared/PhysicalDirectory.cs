// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager
{
    using System.IO;

    using Microsoft.Web.Administration;

    public class PhysicalDirectory
    {
        public PhysicalDirectory(DirectoryInfo physicalDirectory, string path, Application application)
        {
            Name = physicalDirectory.Name;
            FullName = physicalDirectory.FullName;
            Application = application;
            PathToSite = Application.IsRoot() ? '/' + path : Application.Path + '/' + path;
            LocationPath = Application.Site.Name + PathToSite;
        }

        public Application Application { get; }
        public string FullName { get; private set; }
        public string Name { get; private set; }
        public string PathToSite { get; }
        public string LocationPath { get; private set; }
    }
}