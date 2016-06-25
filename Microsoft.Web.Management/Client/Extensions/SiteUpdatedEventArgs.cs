// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client.Extensions
{
    public sealed class SiteUpdatedEventArgs : EventArgs
    {
        public SiteAction Action { get; }
        public string SiteName { get; }
    }
}