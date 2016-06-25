// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public class HierarchyInfoSyncSelectionEventArgs : EventArgs
    {
        public HierarchyInfoSyncSelectionEventArgs(Connection connection, HierarchyInfo hierarchyInfo, Type pageType)
        {
            Connection = connection;
            HierarchyInfo = hierarchyInfo;
            PageType = pageType;
        }

        public Connection Connection { get; }

        public HierarchyInfo HierarchyInfo { get; set; }

        public Type PageType { get; }
    }
}