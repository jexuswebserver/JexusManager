// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Principal;

namespace Microsoft.Web.Management.Server
{
    public interface IManagementContext
    {
        Version ClientClrVersion { get; }
        string ClientName { get; }
        string ClientUserInterfaceTechnology { get; }
        Version ClientVersion { get; }
        bool IsLocalConnection { get; }
        IPrincipal User { get; }
    }
}
