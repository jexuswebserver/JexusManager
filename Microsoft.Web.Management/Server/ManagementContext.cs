// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Principal;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementContext : IManagementContext
    {
        public ManagementContext(bool isLocalConnection, IPrincipal user = null, string clientName = "Jexus Manager")
        {
            IsLocalConnection = isLocalConnection;
            User = user ?? new GenericPrincipal(new GenericIdentity(Environment.UserName), Array.Empty<string>());
            ClientName = clientName;
            ClientClrVersion = Environment.Version;
            ClientVersion = typeof(ManagementContext).Assembly.GetName().Version;
            ClientUserInterfaceTechnology = "WinForms";
        }

        public Version ClientClrVersion { get; }
        public string ClientName { get; }
        public string ClientUserInterfaceTechnology { get; }
        public Version ClientVersion { get; }
        public bool IsLocalConnection { get; }
        public IPrincipal User { get; }
    }
}
