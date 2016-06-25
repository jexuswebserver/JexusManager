// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.ApplicationHost;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public class SiteInfo
    {
        public SiteInfo(
            IAppHostSite site
            )
        { }
        public string ApplicationPool { get; }
        public string BindingInformation { get; }
        public List<BindingInfo> Bindings { get; }
        public uint Id { get; }
        public string Name { get; }
        public string PhysicalPath { get; }

        public ObjectState RuntimeState { get; }
        public bool ServerAutoStart { get; }
        public ObjectState State { get; }
    }
}