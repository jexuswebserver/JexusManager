// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System.Windows.Forms;

    public interface IConfigurationService
    {
        Configuration GetConfiguration();
        ManagementScope Scope { get; set; }
        Microsoft.Web.Administration.Application Application { get; set; }
        ServerManager Server { get; set; }
        Site Site { get; set; }
        Form Form { get; set; }
        VirtualDirectory VirtualDirectory { get; set; }
        PhysicalDirectory PhysicalDirectory { get; }

        ServerManager ServerManager { get; }

        ConfigurationSection GetSection(string systemWebserverSecurityAccess, string location = null, bool acceptNonLocallyStored = true);
    }
}
