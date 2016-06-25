// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public interface IConnectionBuilder
    {
        ConnectionInfo BuildConnection(
            IServiceProvider serviceProvider,
            ManagementScope scope
            );

        ConnectionInfo BuildConnection(
            IServiceProvider serviceProvider,
            ConnectionInfo existingConnectionInfo,
            Exception connectionException
            );

        ConnectionInfo BuildFavorite(
            IServiceProvider serviceProvider,
            ManagementScope scope
            );
    }
}