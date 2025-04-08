// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public class ApplicationManagementUnit : ManagementUnit
    {
        protected override ManagementFrameworkVersion GetAssociatedFrameworkVersion()
        {
            throw new NotImplementedException();
        }

        public override Object[] GetTypeInformation(
            string baseTypeName,
            bool includeNonpublicTypes,
            Type generatorType
            )
        { throw new NotImplementedException(); }

        public string ApplicationPath { get; }
        public override ManagementScope Scope { get; }
        public string SiteName { get; }
    }
}
