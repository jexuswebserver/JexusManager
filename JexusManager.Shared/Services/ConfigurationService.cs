// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System.Windows.Forms;

    public class ConfigurationService : IConfigurationService
    {
        public ManagementScope Scope { get; set; }
        public ServerManager Server { get; set; }
        public Microsoft.Web.Administration.Application Application { get; set; }
        public Site Site { get; set; }
        public Form Form { get; set; }
        public VirtualDirectory VirtualDirectory { get; set; }
        public PhysicalDirectory PhysicalDirectory { get; set; }
        private readonly Configuration _config;

        private readonly string _location;

        public ConfigurationService(Form form, Configuration config, ManagementScope scope, ServerManager server, Site site, Microsoft.Web.Administration.Application application, VirtualDirectory virtualDirectory, PhysicalDirectory physicalDirectory, string location)
        {
            Scope = scope;
            Server = server;
            Application = application;
            Site = site;
            Form = form;
            VirtualDirectory = virtualDirectory;
            PhysicalDirectory = physicalDirectory;
            _config = config;
            _location = location;
        }

        public Configuration GetConfiguration()
        {
            return _config;
        }

        public ServerManager ServerManager
        {
            get
            {
                return Server ??
                       Site?.Server ??
                       Application?.Server ??
                       VirtualDirectory?.Application.Server ??
                       PhysicalDirectory?.Application.Server;
            }
        }

        public ConfigurationSection GetSection(string sectionPath, string locationPath = null, bool acceptNonLocallyStored = true)
        {
            var config = GetConfiguration();
            ConfigurationSection section;
            if (PhysicalDirectory != null)
            {
                section = config.GetSection(sectionPath, PhysicalDirectory.LocationPath);
            }
            else if (VirtualDirectory != null)
            {
                section = config.GetSection(sectionPath, VirtualDirectory.LocationPath());
            }
            else
            {
                section = locationPath == null ? config.GetSection(sectionPath) : config.GetSection(sectionPath, locationPath);
            }

            if (acceptNonLocallyStored)
            {
                return section;
            }

            return section.IsLocallyStored ? section : ServerManager.GetApplicationHostConfiguration().GetSection(sectionPath, _location);
        }
    }
}
