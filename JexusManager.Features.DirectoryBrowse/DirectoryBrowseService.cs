// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.DirectoryBrowse
{
    internal sealed class DirectoryBrowseService : ModuleService
    {
        private const string SectionPath = "system.webServer/directoryBrowse";

        [ModuleServiceMethod]
        public DirectoryBrowseSnapshot GetSettings()
        {
            var section = GetSection();
            var flags = (long)section["showFlags"];
            return new DirectoryBrowseSnapshot
            {
                Enabled = (bool)section["enabled"],
                DateEnabled = (flags & 2) == 2,
                TimeEnabled = (flags & 4) == 4,
                SizeEnabled = (flags & 8) == 8,
                ExtensionEnabled = (flags & 16) == 16,
                LongDateEnabled = (flags & 32) == 32
            };
        }

        [ModuleServiceMethod]
        public void SetEnabled(bool enabled)
        {
            GetSection()["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Apply(DirectoryBrowseSnapshot settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection();
            section["enabled"] = settings.Enabled;
            section["showFlags"] = CreateFlags(settings);
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private static long CreateFlags(DirectoryBrowseSnapshot settings)
        {
            long flags = 0;
            if (settings.DateEnabled)
            {
                flags |= 2;
            }

            if (settings.TimeEnabled)
            {
                flags |= 4;
            }

            if (settings.SizeEnabled)
            {
                flags |= 8;
            }

            if (settings.ExtensionEnabled)
            {
                flags |= 16;
            }

            if (settings.LongDateEnabled)
            {
                flags |= 32;
            }

            return flags;
        }
    }
}