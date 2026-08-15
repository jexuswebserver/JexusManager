// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public abstract class ModuleService
    {
        internal void Initialize(ManagementUnit managementUnit, string moduleName)
        {
            ManagementUnit = managementUnit ?? throw new ArgumentNullException(nameof(managementUnit));
            ModuleName = string.IsNullOrWhiteSpace(moduleName)
                ? throw new ArgumentException("A module name is required.", nameof(moduleName))
                : moduleName;
        }

        protected ModuleService CreateChildService(
            Type serviceType
            )
        { throw new NotImplementedException(); }

        protected void RaiseException(
            Exception ex
            )
        {
            throw new ModuleServiceException(ex?.Message, null, ex);
        }

        protected void RaiseException(
            string resourceName
            )
        {
            throw new ModuleServiceException(resourceName, resourceName);
        }

        protected void RaiseException(
            string resourceName,
            string errorMessage
            )
        {
            throw new ModuleServiceException(errorMessage ?? resourceName, resourceName);
        }

        public IManagementContext Context { get; }
        protected ManagementUnit ManagementUnit { get; private set; }
        protected string ModuleName { get; private set; }
    }
}
