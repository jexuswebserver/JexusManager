// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Client
{
    public class HierarchyCollectionEventArgs : EventArgs
    {
        public HierarchyCollectionEventArgs(
            HierarchyInfo hierarchyInfo,
            IEnumerable<HierarchyInfo> childrenAdded
            )
        {
            HierarchyInfo = hierarchyInfo;
            ChildrenAdded = childrenAdded;
        }

        public IEnumerable<HierarchyInfo> ChildrenAdded { get; }
        public HierarchyInfo HierarchyInfo { get; }
    }
}