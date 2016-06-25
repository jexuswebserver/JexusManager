// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.ComponentModel;

namespace Microsoft.Web.Management.Client
{
    public abstract class HierarchyInfo : IServiceProvider, IDisposable
    {
        private IServiceProvider _serviceProvider;

        protected HierarchyInfo(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static readonly string Application;
        public static readonly string ApplicationConnection;
        public static readonly string ApplicationPools;
        public static readonly string File;
        public static readonly string Folder;
        public static readonly string FtpSites;
        public static readonly string ServerConnection;
        public static readonly string Site;
        public static readonly string SiteConnection;
        public static readonly string Sites;
        public static readonly string Start;
        public static readonly string VirtualDirectory;

        public void Collapse()
        {
        }

        public bool Delete()
        {
            return false;
        }

        protected virtual void Dispose()
        {
            _serviceProvider = null;
        }

        public void Expand() { }

        protected virtual HierarchyInfo[] GetChildren()
        {
            return null;
        }

        protected object GetService(Type type)
        {
            return _serviceProvider.GetService(type);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        protected bool Navigate(Type pageType)
        {
            return Navigate(pageType, null);
        }

        protected bool Navigate(Type pageType, object navigationData)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            return service.Navigate(Connection, null, pageType, navigationData);
        }

        protected virtual void OnDeleting(CancelEventArgs e)
        {
        }

        protected virtual void OnRenamed(HierarchyRenameEventArgs e)
        { }

        protected virtual void OnRenaming(HierarchyRenameEventArgs e)
        { }

        protected virtual bool OnSelected()
        {
            return false;
        }

        public virtual void Refresh() { }


        public bool Select()
        {
            return false;
        }

        public void Update() { }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetService(serviceType);
        }

        protected virtual Connection Connection { get; }
        public virtual object Image { get; }
        public virtual bool IsExtendable { get; }
        public bool IsLoaded { get; }
        public abstract string NodeType { get; }
        public virtual string NodeTypeName { get; }
        public HierarchyInfo Parent { get; }
        public virtual HierarchyPriority Priority { get; }
        public virtual IDictionary Properties { get; }
        public virtual bool SupportsChildren { get; }
        protected virtual bool SupportsDelete { get; }
        protected virtual bool SupportsRename { get; }
        public virtual TaskListCollection Tasks { get; }
        public abstract string Text { get; }
        public virtual string ToolTip { get; }
        public virtual HierarchyVisibility Visibility { get; }
    }
}