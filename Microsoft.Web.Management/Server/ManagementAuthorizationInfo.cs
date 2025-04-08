// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public class ManagementAuthorizationInfo
    {
        public ManagementAuthorizationInfo(
            string name,
            string configurationPath,
            bool isRole
            )
        {
            Name = name;
            ConfigurationPath = configurationPath;
            IsRole = isRole;
        }

        public string ConfigurationPath { get; }
        public bool IsRole { get; }
        public string Name { get; }
    }
}
