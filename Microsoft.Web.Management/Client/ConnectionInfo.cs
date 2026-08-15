// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionInfo
    {
        private readonly IConnectionManager _connectionManager;

        public ConnectionInfo(string name, Uri url, bool isLocal, ManagementScope scope, ManagementScopePath scopePath, ConnectionCredential credentials, IConnectionManager connectionManager)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Url = url ?? throw new ArgumentNullException(nameof(url));
            IsLocal = isLocal;
            Scope = scope;
            ScopePath = scopePath ?? throw new ArgumentNullException(nameof(scopePath));
            Credentials = credentials;
            _connectionManager = connectionManager;
        }

        public override bool Equals(object obj)
        {
            return obj is ConnectionInfo other
                && IsLocal == other.IsLocal
                && Scope == other.Scope
                && Equals(ScopePath, other.ScopePath)
                && Equals(Url, other.Url)
                && Equals(Credentials, other.Credentials);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Url, IsLocal, Scope, ScopePath, Credentials);
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
