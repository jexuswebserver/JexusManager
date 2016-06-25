// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using Microsoft.Web.Management.Client;

namespace Microsoft.Web.Management.Host
{
    public sealed class ConnectionManager : IConnectionManager,
        IDisposable
    {
        public ConnectionManager(
    IServiceProvider serviceProvider,
    bool supportsMultipleConnections
)
        { }

        ReadOnlyCollection<Connection> IConnectionManager.Connections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IConnectionManager.IsDirty
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ConnectionInfoCollection IConnectionManager.RegisteredConnections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IConnectionManager.SupportsMultipleConnections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        event ConnectionEventHandler IConnectionManager.ConnectionActivated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event ConnectionEventHandler IConnectionManager.ConnectionDeactivated
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IConnectionManager.ConnectionRefreshed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event ConnectionEventHandler IConnectionManager.ConnectionRefreshing
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler IConnectionManager.IsDirtyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        Connection IConnectionManager.ActivateConnection(ConnectionInfo connection)
        {
            throw new NotImplementedException();
        }

        void IConnectionManager.DeactivateConnection(Connection connection)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        ConnectionActiveState IConnectionManager.GetConnectionActiveState(ConnectionInfo connection)
        {
            throw new NotImplementedException();
        }

        bool IConnectionManager.PerformLogin(ConnectionInfo connection)
        {
            throw new NotImplementedException();
        }

        bool IConnectionManager.RefreshConnection(Connection connection)
        {
            throw new NotImplementedException();
        }

        void IConnectionManager.Save()
        {
            throw new NotImplementedException();
        }
    }
}
