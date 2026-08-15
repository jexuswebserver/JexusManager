// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Access
{
    internal sealed class AccessService : ModuleService
    {
        private const string SectionPath = "system.webServer/security/access";

        [ModuleServiceMethod]
        public AccessSnapshot GetSettings()
        {
            return new AccessSnapshot
            {
                SslFlags = (long)GetSection()["sslFlags"]
            };
        }

        [ModuleServiceMethod]
        public void Apply(AccessSnapshot settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            GetSection()["sslFlags"] = settings.SslFlags;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }
    }
}