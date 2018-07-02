﻿// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        internal string Title
        {
            get
            {
                return string.IsNullOrEmpty(HostName)
                           ? (string.IsNullOrEmpty(Name) ? "UNKNOWN" : Name)
                           : HostName.ExtractName();
            }
        }

        public abstract bool SupportsSni { get; }

        public WorkingMode Mode { get; protected set; }

        private object locker = new object();

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

        internal virtual bool Verify(string path)
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
            if (Initialized)
            {
                return;
            }

            lock (locker)
            {
                Initialized = true;
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
                _web =
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
                    new FileContext(this, FileName, _web.FileContext, null, true, false, ReadOnly));

                LoadCache();

                var poolSection = _applicationHost.GetSection("system.applicationHost/applicationPools");
                _applicationPoolDefaults =
                    new ApplicationPoolDefaults(poolSection.GetChildElement("applicationPoolDefaults"), poolSection);
                ApplicationPoolCollection = new ApplicationPoolCollection(poolSection, this);
                var siteSection = _applicationHost.GetSection("system.applicationHost/sites");
                _siteDefaults = new SiteDefaults(siteSection.GetChildElement("siteDefaults"), siteSection);
                _applicationDefaults = new ApplicationDefaults(
                    siteSection.GetChildElement("applicationDefaults"),
                    siteSection);
                _virtualDirectoryDefaults =
                    new VirtualDirectoryDefaults(siteSection.GetChildElement("virtualDirectoryDefaults"), siteSection);
                SiteCollection = new SiteCollection(siteSection, this);

                PostInitialize();
            }
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
                    application.ClearCache();
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
                lock (locker)
                {
                    return _applicationDefaults;
                }
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
                lock (locker)
                {
                    return _applicationPoolDefaults;
                }
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
                lock (locker)
                {
                    return ApplicationPoolCollection;
                }
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
                lock (locker)
                {
                    return _siteDefaults;
                }
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
                lock (locker)
                {
                    return SiteCollection;
                }
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
                lock (locker)
                {
                    return _virtualDirectoryDefaults;
                }
            }
        }

        public WorkerProcessCollection WorkerProcesses
        {
            get
            {
                Initialize();
                lock (locker)
                {
                    return _workerProcessCollection;
                }
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
            var local = Path.Combine(
                Path.GetDirectoryName(typeof(FileContext).Assembly.Location),
                "schema");
            return Directory.Exists(local) ? Directory.GetFiles(local) : Enumerable.Empty<string>();
        }

        public Configuration GetApplicationHostConfiguration()
        {
            Initialize();
            lock (locker)
            {
                return _applicationHost;
            }
        }

        internal Configuration GetConfigurationCache()
        {
            Initialize();
            lock (locker)
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
