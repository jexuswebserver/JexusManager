// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Host
{
    public interface IManagementHost
    {
        Object CreateBuilder(
            Type builderType
            );

        Module CreateHostModule();

        IManagementContext CreateManagementContext();

        string ApplicationName { get; }
        string Name { get; }
        string Title { get; }
        IDictionary UserData { get; }
        Type UserInterfaceTechnologyType { get; }
        Version Version { get; }
    }
}
