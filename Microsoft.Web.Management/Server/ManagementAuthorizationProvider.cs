// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Security.Principal;

namespace Microsoft.Web.Management.Server
{
    public abstract class ManagementAuthorizationProvider
    {
        public abstract ManagementAuthorizationInfoCollection GetAuthorizedUsers(
            string configurationPath,
            bool includeChildren,
            int itemIndex,
            int itemsPerPage
            );

        public abstract string[] GetConfigurationPaths(
            IPrincipal principal,
            string baseConfigurationPath
            );

        public abstract ManagementAuthorizationInfo Grant(
            string name,
            string configurationPath,
            bool isRole
            );

        public virtual void Initialize(
            IDictionary<string, string> args
            )
        { }

        public abstract bool IsAuthorized(
            IPrincipal principal,
            string configurationPath
            );

        public abstract void RenameConfigurationPath(
            string configurationPath,
            string newConfigurationPath
            );

        public abstract void Revoke(
            string name
            );

        public abstract void Revoke(
            string name,
            string configurationPath
            );

        public abstract void RevokeConfigurationPath(
            string configurationPath
            );
    }
}
