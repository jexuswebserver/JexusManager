// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Web.Management.Client
{
    public interface IConnectionManager
    {
        Connection ActivateConnection(
            ConnectionInfo connection
            );

        void DeactivateConnection(
            Connection connection
            );

        ConnectionActiveState GetConnectionActiveState(
            ConnectionInfo connection
            );

        bool PerformLogin(
            ConnectionInfo connection
            );

        bool RefreshConnection(
            Connection connection
            );

        void Save();

        ReadOnlyCollection<Connection> Connections { get; }

        bool IsDirty { get; }
        ConnectionInfoCollection RegisteredConnections { get; }
        bool SupportsMultipleConnections { get; }

        event ConnectionEventHandler ConnectionActivated;
        event ConnectionEventHandler ConnectionDeactivated;
        event EventHandler ConnectionRefreshed;
        event ConnectionEventHandler ConnectionRefreshing;
        event EventHandler IsDirtyChanged;
    }
}
