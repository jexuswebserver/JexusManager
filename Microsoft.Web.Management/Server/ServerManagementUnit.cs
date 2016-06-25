// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Microsoft.Web.Management.Server
{
    public sealed class ServerManagementUnit : ManagementUnit
    {
        public ApplicationManagementUnit GetApplication(
            string siteName,
            string applicationPath
            )
        {
            return null;
        }

        public SiteManagementUnit GetSite(
            string siteName
            )
        {
            return null;
        }

        protected override ManagementFrameworkVersion GetAssociatedFrameworkVersion()
        {
            throw new NotImplementedException();
        }

        public override Object[] GetTypeInformation(
            string baseTypeName,
            bool includeNonpublicTypes,
            Type generatorType
            )
        {
            return null;
        }

        public ICollection FrameworkVersions { get; }
        public string MachineName { get; }
        public override ManagementScope Scope { get; }
    }
}