// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public abstract class ModuleService
    {
        protected ModuleService CreateChildService(
            Type serviceType
            )
        { throw new NotImplementedException(); }

        protected void RaiseException(
            Exception ex
            )
        { }

        protected void RaiseException(
            string resourceName
            )
        { }

        protected void RaiseException(
            string resourceName,
            string errorMessage
            )
        { }

        public IManagementContext Context { get; }
        protected ManagementUnit ManagementUnit { get; }
        protected string ModuleName { get; }
    }
}