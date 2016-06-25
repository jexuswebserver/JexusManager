// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionInfo
    {
        public ConnectionInfo(string name, Uri url, bool isLocal, ManagementScope scope, ManagementScopePath scopePath, ConnectionCredential credentials, IConnectionManager connectionManager)
        { }

        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public ConnectionCredential Credentials { get; }
        public bool IsLocal { get; }
        public string Name { get; set; }
        public ManagementScope Scope { get; }
        public ManagementScopePath ScopePath { get; }
        public Uri Url { get; }

        public event EventHandler RenamedEventHandler;
    }
}