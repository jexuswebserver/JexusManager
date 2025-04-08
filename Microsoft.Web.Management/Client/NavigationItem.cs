// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class NavigationItem : IDisposable
    {
        private IModulePage _page;

        public NavigationItem(Connection connection, ManagementConfigurationPath configurationPath, Type pageType, object navigationData)
        {
            Connection = connection;
            ConfigurationPath = configurationPath;
            PageType = pageType;
            NavigationData = navigationData;
        }

        public ManagementConfigurationPath ConfigurationPath { get; }
        public Connection Connection { get; private set; }

        public bool IsPageCreated
        {
            get { return _page != null; }
        }

        public object NavigationData { get; }

        public IModulePage Page
        {
            get { return _page ?? (_page = (IModulePage)Activator.CreateInstance(PageType)); }
            internal set { _page = value; }
        }

        public Type PageType { get; }
        public IDictionary UserData { get; }

        void IDisposable.Dispose()
        {
            Connection = null;
            Page = null;
        }
    }
}
