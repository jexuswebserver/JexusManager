// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public abstract class HierarchyProvider : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        protected HierarchyProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public abstract HierarchyInfo[] GetChildren(HierarchyInfo item);

        protected object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public virtual TaskList GetTasks(HierarchyInfo item)
        {
            return item.Tasks[0];
        }

        public virtual void SyncSelection(HierarchyInfoSyncSelectionEventArgs item)
        { }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetService(serviceType);
        }
    }
}
