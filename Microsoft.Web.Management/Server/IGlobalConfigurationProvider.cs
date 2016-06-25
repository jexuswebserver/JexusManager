// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;

namespace Microsoft.Web.Management.Server
{
    public interface IGlobalConfigurationProvider
    {
        string GetFrameworkConfigurationPath(
            ManagementFrameworkVersion version
            );

        ReadOnlyCollection<ManagementFrameworkVersion> FrameworkVersions { get; }
        string ServerConfigurationPath { get; }
    }
}