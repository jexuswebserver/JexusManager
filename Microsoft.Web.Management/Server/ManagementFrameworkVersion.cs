// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementFrameworkVersion
    {
        public bool IsEqual(string versionIdentifier)
        {
            return false;
        }

        public bool CanManageFrameworkConfiguration { get; }
        public string FrameworkConfigurationPath { get; }
        public string Name { get; }
        public string Text { get; }
        public Version Version { get; }
    }
}