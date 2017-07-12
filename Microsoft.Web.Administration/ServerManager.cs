﻿// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Web.Administration
{
    public abstract class ServerManager
    {
        private Configuration _applicationHost;

        private Configuration _cleanHost;

        internal bool Initialized;

        private SiteDefaults _siteDefaults;

        internal SiteCollection SiteCollection;

        private VirtualDirectoryDefaults _virtualDirectoryDefaults;

        private WorkerProcessCollection _workerProcessCollection;

        private ApplicationDefaults _applicationDefaults;

        private ApplicationPoolDefaults _applicationPoolDefaults;

        internal ApplicationPoolCollection ApplicationPoolCollection;

        internal string HostName { get; set; }

        internal string Name { get; set; }

        internal string Title
        {
            get
            {
                return string.IsNullOrEmpty(HostName)
                           ? (string.IsNullOrEmpty(Name) ? "UNKNOWN" : Name)
                           : HostName.ExtractName();
            }
        }

        public WorkingMode Mode { get; protected set; }

        public ServerManager()
            : this(null, true)
        {
        }

        public ServerManager(string hostName, bool local)
            : this(hostName)
        {
        }

        public ServerManager(bool readOnly, string applicationHostConfigurationPath)
            : this("localhost", readOnly, applicationHostConfigurationPath)
        {
        }

        internal virtual async Task<bool> VerifyAsync(string path)
        {
            return Directory.Exists(path.ExpandIisExpressEnvironmentVariables());
        }

        public ServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        internal ServerManager(string hostName, bool readOnly, string fileName)
        {
            HostName = hostName;
            ReadOnly = readOnly;
            FileName = fileName;
        }

        private void Initialize()
        {
            if (this.Initialized)
            {
                return;
            }

            this.Initialized = true;
            PreInitialize();
            var machineConfig = Helper.IsRunningOnMono()
                ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/machine.config"
                : Path.Combine(
                                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                                    "Microsoft.NET",
                                    IntPtr.Size == 2 ? "Framework" : "Framework64",
                                    "v4.0.30319",
                                    "config",
                                    "machine.config");
            var machine =
                new Configuration(
                    new FileContext(
                        this,
                        machineConfig,
                        null,
                        null,
                        false,
                        true,
                        true));
            var webConfig = Helper.IsRunningOnMono()
                ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/web.config"
                : Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                                "Microsoft.NET",
                                IntPtr.Size == 2 ? "Framework" : "Framework64",
                                "v4.0.30319",
                                "config",
                                "web.config");
            var web =
                new Configuration(
                    new FileContext(
                        this,
                        webConfig,
                        machine.FileContext,
                        null,
                        false,
                        true,
                        false));

            _applicationHost =
                new Configuration(
                new FileContext(this, this.FileName, web.FileContext, null, true, false, this.ReadOnly));

            this.LoadCache();

            var poolSection = _applicationHost.GetSection("system.applicationHost/applicationPools");
            _applicationPoolDefaults =
                new ApplicationPoolDefaults(poolSection.GetChildElement("applicationPoolDefaults"), poolSection);
            this.ApplicationPoolCollection = new ApplicationPoolCollection(poolSection, this);
            var siteSection = _applicationHost.GetSection("system.applicationHost/sites");
            _siteDefaults = new SiteDefaults(siteSection.GetChildElement("siteDefaults"), siteSection);
            _applicationDefaults = new ApplicationDefaults(
                siteSection.GetChildElement("applicationDefaults"),
                siteSection);
            _virtualDirectoryDefaults =
                new VirtualDirectoryDefaults(siteSection.GetChildElement("virtualDirectoryDefaults"), siteSection);
            this.SiteCollection = new SiteCollection(siteSection, this);

            PostInitialize();
        }

        internal virtual void CleanSiteFile(string file)
        {
        }

        internal virtual string GetPhysicalPath(string root)
        {
            return root;
        }

        internal abstract Task StopAsync(Site site);
        internal abstract Task StartAsync(Site site);

        protected virtual void PreInitialize()
        {
        }

        internal abstract Task<bool> GetPoolStateAsync(ApplicationPool pool);

        protected virtual void PostInitialize()
        {
        }

        private void LoadCache()
        {
            // IMPORTANT: force to reload clean elements from file.
            _cleanHost = null;
            _cleanHost =
                new Configuration(
                new FileContext(this, this.FileName, _applicationHost.FileContext.Parent, null, true, false, false));
        }

        public void Dispose()
        {
        }

        public void CommitChanges()
        {
            AsyncHelper.RunSync(CommitChangesAsync);
        }

        public async Task CommitChangesAsync()
        {
            foreach (Site site in Sites)
            {
                foreach (Application application in site.Applications)
                {
                    application.Save();
                }
            }

            Save();

            await PostCommitChangesAsync();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task PostCommitChangesAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

        public void Save()
        {
            _applicationHost.FileContext.Save();
            _applicationHost.OnCacheInvalidated();

            LoadCache();
        }

        public ApplicationDefaults ApplicationDefaults
        {
            get
            {
                if (_applicationDefaults != null)
                {
                    return _applicationDefaults;
                }

                this.Initialize();
                return _applicationDefaults;
            }
        }

        public ApplicationPoolDefaults ApplicationPoolDefaults
        {
            get
            {
                if (_applicationPoolDefaults != null)
                {
                    return _applicationPoolDefaults;
                }

                this.Initialize();
                return _applicationPoolDefaults;
            }
        }

        public ApplicationPoolCollection ApplicationPools
        {
            get
            {
                if (this.ApplicationPoolCollection != null)
                {
                    return this.ApplicationPoolCollection;
                }

                this.Initialize();
                return this.ApplicationPoolCollection;
            }
        }

        public SiteDefaults SiteDefaults
        {
            get
            {
                if (_siteDefaults != null)
                {
                    return _siteDefaults;
                }

                this.Initialize();
                return _siteDefaults;
            }
        }

        public SiteCollection Sites
        {
            get
            {
                if (this.SiteCollection != null)
                {
                    return this.SiteCollection;
                }

                this.Initialize();
                return this.SiteCollection;
            }
        }

        public VirtualDirectoryDefaults VirtualDirectoryDefaults
        {
            get
            {
                if (_virtualDirectoryDefaults != null)
                {
                    return _virtualDirectoryDefaults;
                }

                this.Initialize();
                return _virtualDirectoryDefaults;
            }
        }

        public WorkerProcessCollection WorkerProcesses
        {
            get
            {
                this.Initialize();
                return _workerProcessCollection;
            }
        }

        public object Status { get; internal set; }

        internal bool IsLocalhost { get; set; }

        internal bool ReadOnly { get; set; }

        internal string FileName { get; set; }

        internal SortedDictionary<string, List<string>> Extra { get; set; }

        internal string SiteFolder { get; set; }

        public Configuration GetAdministrationConfiguration()
        {
            return null;
        }

        internal abstract Task<bool> GetSiteStateAsync(Site site);

        public Configuration GetAdministrationConfiguration(WebConfigurationMap configMap, string configurationPath)
        {
            return null;
        }

        internal virtual IEnumerable<string> GetSchemaFiles()
        {
            var local = Path.Combine(
                Path.GetDirectoryName(typeof(FileContext).Assembly.Location),
                "schema");
            return Directory.Exists(local) ? Directory.GetFiles(local) : Enumerable.Empty<string>();
        }

        public Configuration GetApplicationHostConfiguration()
        {
            this.Initialize();
            return _applicationHost;
        }

        internal Configuration GetConfigurationCache()
        {
            this.Initialize();
            return _cleanHost;
        }

        public object GetMetadata(string metadataType)
        {
            return null;
        }

        public Configuration GetRedirectionConfiguration()
        {
            return null;
        }

        public Configuration GetWebConfiguration(string siteName)
        {
            return null;
        }

        public Configuration GetWebConfiguration(string siteName, string virtualPath)
        {
            return null;
        }

        public Configuration GetWebConfiguration(WebConfigurationMap configMap, string configurationPath)
        {
            return null;
        }

        public static IisServerManager OpenRemote(string serverName)
        {
            return new IisServerManager(serverName, false);
        }

        public void SetMetadata(string metadataType, object value)
        {
        }

        internal void VerifyLocation(string locationPath)
        {
            if (locationPath == null)
            {
                return;
            }

            // TODO: add deeper level check
            var parts = locationPath.Split('/');
            if (parts[0] != string.Empty && Sites.All(site => site.Name != parts[0]))
            {
                throw new FileNotFoundException(
                    string.Format(
                        "Filename: \r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/{0}'\r\n\r\n",
                        parts[0]));
            }
        }

        internal string Type { get; private set; }
    }
}
