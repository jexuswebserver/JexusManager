// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementContentNavigator : ICloneable
    {
        public ManagementContentNavigator Clone()
        {
            return null;
        }

        public static ManagementContentNavigator Create(
            ManagementUnit managementUnit
            )
        {
            return null;
        }

        public ManagementContentNavigator[] GetChildren()
        {
            return null;
        }

        public int GetChildrenContainerCount()
        {
            return 0;
        }

        public bool MoveToChild(
            string name
            )
        {
            return false;
        }

        public bool MoveToParent()
        {
            return false;
        }

        public bool MoveToPath(
            string configurationPath
            )
        {
            return false;
        }

        public bool MoveToRoot()
        {
            return false;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool IsApplication { get; }
        public bool IsFile { get; }
        public bool IsFolder { get; }
        public bool IsServer { get; }
        public bool IsSite { get; }
        public bool IsValid { get; }
        public bool IsVirtualDirectory { get; }
        public DateTime LastModified { get; }
        public string Name { get; }
        public string NavigatorPath { get; }
        public string PhysicalPath { get; }
        public IDictionary<string, Object> Properties { get; }
        public long Size { get; }
        public int State { get; }
        public string VirtualPath { get; }
    }
}
