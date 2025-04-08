// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public class HierarchyInfoEventArgs : EventArgs
    {
        public HierarchyInfoEventArgs(HierarchyInfo hierarchyInfo)
        {
            HierarchyInfo = hierarchyInfo;
        }

        public HierarchyInfo HierarchyInfo { get; }
    }
}
