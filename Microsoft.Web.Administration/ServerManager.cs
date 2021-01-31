// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Web.Administration
{
    public abstract class ServerManager : IFeatureDetection
    {
        private Configuration _applicationHost;

        private Configuration _web;

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

        internal string Title => string.IsNullOrEmpty(HostName)
            ? (string.IsNullOrEmpty(Name) ? "UNKNOWN" : Name)
            : HostName.ExtractName();

        public abstract bool SupportsSni { get; }
        
        public abstract bool SupportsWildcard { get; }

        public WorkingMode Mode { get; protected set; }

        private readonly object _locker = new object();

        protected ServerManager()
            : this(null, true)
        {
        }

        protected ServerManager(string hostName, bool local)
            : this(hostName, false, null)
        {
        }

        protected ServerManager(bool readOnly, string applicationHostConfigurationPath)
            : this("localhost", readOnly, applicationHostConfigurationPath)
        {
        }

        protected ServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        internal ServerManager(string hostName, bool readOnly, string fileName)
        {
            HostName = hostName;
            ReadOnly = readOnly;
            FileName = fileName;
        }

        internal virtual bool Verify(string path, string executable)
        {
            return Directory.Exists(path.ExpandIisExpressEnvironmentVariables(executable));
        }

        internal virtual void SetPassword(VirtualDirectory virtualDirectory, string password)
        {
        }

        internal virtual void SetPassword(ApplicationPoolProcessModel processModel, string password)
        {
        }

        private void Initialize()
        {
            lock (_locker)
            {
                if (Initialized)
                {
                    return;
                }

                Initialized = true;
                PreInitialize();
              
                var machine =
                    new Configuration(
                        new FileContext(
                            this,
                            Helper.FileNameMachineConfig,
                            null,
                            null,
                            false,
                            true,
                            true)
                        { IgnoreSchemaCheck = true });

                _web =
                    new Configuration(
                        new FileContext(
                            this,
                            Helper.FileNameWebConfig,
                            machine.FileContext,
                            null,
                            false,
                            true,
                            false));

                _applicationHost =
                    new Configuration(
                    new FileContext(this, FileName, _web.FileContext, null, true, false, ReadOnly));

                LoadCache();

                var poolSectionName = "system.applicationHost/applicationPools";
                var poolSection = _applicationHost.GetSection(poolSectionName);
                if (poolSection == null)
                {
                    throw new COMException($"Filename: \\\\?\\{FileName}\r\nError: The configuration section '{poolSectionName}' cannot be read because it is missing a section declaration\r\n\r\n");
                }

                _applicationPoolDefaults =
                    new ApplicationPoolDefaults(poolSection?.GetChildElement("applicationPoolDefaults"), poolSection);
                ApplicationPoolCollection = new ApplicationPoolCollection(poolSection, this);

                var siteSectionName = "system.applicationHost/sites";
                var siteSection = _applicationHost.GetSection(siteSectionName);
                if (siteSection == null)
                {
                    throw new COMException($"Filename: \\\\?\\{FileName}\r\nError: The configuration section '{siteSectionName}' cannot be read because it is missing a section declaration\r\n\r\n");
                }

                _siteDefaults = new SiteDefaults(siteSection?.GetChildElement("siteDefaults"), siteSection);
                _applicationDefaults = new ApplicationDefaults(
                    siteSection?.GetChildElement("applicationDefaults"),
                    siteSection);
                _virtualDirectoryDefaults =
                    new VirtualDirectoryDefaults(siteSection?.GetChildElement("virtualDirectoryDefaults"), siteSection);
                SiteCollection = new SiteCollection(siteSection, this);
            }

            // IMPORTANT: out of locking for Jexus web server.
            PostInitialize();
        }

        internal virtual void CleanSiteFile(string file)
        {
        }

        internal virtual string GetPhysicalPath(string root)
        {
            return root;
        }

        internal abstract void Stop(Site site);
        internal abstract void Start(Site site);

        internal virtual void Restart(Site site)
        {
            Stop(site);
            Start(site);
        }

        internal virtual void Recycle(ApplicationPool applicationPool)
        { }

        internal virtual void Start(ApplicationPool applicationPool)
        { }

        internal virtual void Stop(ApplicationPool applicationPool)
        { }

        protected virtual void PreInitialize()
        {
        }

        internal abstract bool GetPoolState(ApplicationPool pool);

        protected virtual void PostInitialize()
        {
        }

        private void LoadCache()
        {
            // IMPORTANT: force to reload clean elements from file.
            _cleanHost = null;
            _cleanHost =
                new Configuration(
                new FileContext(this, FileName, _applicationHost.FileContext.Parent, null, true, false, false));
        }

        public void Dispose()
        {
        }

        public void CommitChanges()
        {
            foreach (Site site in Sites)
            {
                foreach (Application application in site.Applications)
                {
                    application.Save();
                }
            }

            Save();

            PostCommitChanges();
        }

        protected virtual void PostCommitChanges()
        {
        }

        public void Save()
        {
            _web.FileContext.Save();
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

                Initialize();
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

                Initialize();
                return _applicationPoolDefaults;
            }
        }

        public ApplicationPoolCollection ApplicationPools
        {
            get
            {
                if (ApplicationPoolCollection != null)
                {
                    return ApplicationPoolCollection;
                }

                Initialize();
                return ApplicationPoolCollection;
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

                Initialize();
                return _siteDefaults;
            }
        }

        public SiteCollection Sites
        {
            get
            {
                if (SiteCollection != null)
                {
                    return SiteCollection;
                }

                Initialize();
                return SiteCollection;
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

                Initialize();
                return _virtualDirectoryDefaults;
            }
        }

        public WorkerProcessCollection WorkerProcesses
        {
            get
            {
                Initialize();
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

        internal abstract bool GetSiteState(Site site);

        public Configuration GetAdministrationConfiguration(WebConfigurationMap configMap, string configurationPath)
        {
            return null;
        }

        internal virtual IEnumerable<string> GetSchemaFiles()
        {
            // IMPORTANT: use local schema folder as fallback.
            var directoryName = Path.GetDirectoryName(typeof(FileContext).Assembly.Location);
            Debug.Assert(directoryName != null, nameof(directoryName) + " != null");
            var local = Path.Combine(
                directoryName,
                "schema");
            return Directory.Exists(local) ? Directory.GetFiles(local) : Enumerable.Empty<string>();
        }

        public Configuration GetApplicationHostConfiguration()
        {
            Initialize();
            lock (_locker)
            {
                return _applicationHost;
            }
        }

        internal Configuration GetConfigurationCache()
        {
            Initialize();
            lock (_locker)
            {
                return _cleanHost;
            }
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
            if (parts[0] != string.Empty && Sites != null && Sites.All(site => site.Name != parts[0]))
            {
                throw new FileNotFoundException(
                    $"Filename: \\\\?\\{FileName}\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/{parts[0]}'\r\n\r\n", FileName);
            }
        }
    }
}
