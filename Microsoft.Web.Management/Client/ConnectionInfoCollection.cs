// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionInfoCollection : ReadOnlyCollection<ConnectionInfo>
    {
        public ConnectionInfoCollection(
            IConnectionManager owner
            ) : base(owner.RegisteredConnections)
        { }

        public ConnectionInfo this[
            string name
            ]
        { get { throw new NotImplementedException(); } }
    }
}
