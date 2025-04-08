// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Client
{
    public abstract class HierarchyService
    {
        public abstract void AddChildren(HierarchyInfo hierarchyInfo, IEnumerable<HierarchyInfo> childrenInfo);

        public abstract void Collapse(HierarchyInfo hierarchyInfo);

        public abstract void Expand(HierarchyInfo hierarchyInfo);

        public abstract IEnumerable<HierarchyInfo> GetChildren(HierarchyInfo hierarchyInfo);

        public abstract TaskListCollection GetTasks(HierarchyInfo hierarchyInfo);

        public abstract void Refresh(HierarchyInfo hierarchyInfo);

        public abstract void Remove(HierarchyInfo hierarchyInfo);

        public abstract bool Select(HierarchyInfo hierarchyInfo);

        public abstract void SyncSelection(HierarchyInfoSyncSelectionEventArgs item);

        public abstract void Update(HierarchyInfo hierarchyInfo);

        public abstract HierarchyInfo SelectedInfo { get; }

        public abstract event HierarchyCollectionEventHandler ChildrenAdded;
        public abstract event HierarchyInfoEventHandler InfoCollapsed;
        public abstract event HierarchyInfoEventHandler InfoExpanded;
        public abstract event HierarchyInfoEventHandler InfoRefreshed;
        public abstract event HierarchyInfoEventHandler InfoRemoved;
        public abstract event HierarchyInfoEventHandler InfoUpdated;
        public abstract event EventHandler SelectedInfoChanged;
    }
}
