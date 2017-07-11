// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.Web.Administration
{
    public sealed class Application : ConfigurationElement
    {
        private Configuration _configuration;
        private VirtualDirectoryCollection _collection;

        internal Application(ApplicationCollection parent)
            : this(null, parent)
        {
        }

        internal Application(ConfigurationElement element, ApplicationCollection parent)
            : base(element, "application", null, parent, null, null)
        {
            Parent = parent;

            // IMPORTANT: avoid duplicate application tag.
            ForceCreateEntity();
            if (string.IsNullOrWhiteSpace(Path))
            {
                // IMPORTANT: fix path attribute after initialization.
                Path = "/";
            }

            _collection = new VirtualDirectoryCollection(this);
            if (element == null)
            {
                return;
            }

            foreach (ConfigurationElement node in (ConfigurationElementCollection)element)
            {
                _collection.InternalAdd(new VirtualDirectory(node, _collection));
            }

            Location = this.Site.Name + this.Path;
            foreach (ApplicationPool pool in Server.ApplicationPools)
            {
                if (pool.Name == ApplicationPoolName)
                {
                    pool.ApplicationCount++;
                }
            }
        }

        internal string Location { get; set; }

        internal override void AddChild(ConfigurationElement child)
        {
            var application = child as VirtualDirectory;
            if (application != null)
            {
                _collection.Add(application);
            }
            else
            {
                base.AddChild(child);
            }
        }

        public Configuration GetWebConfiguration()
        {
            if (_configuration != null)
            {
                return _configuration;
            }

            string root = null;
            foreach (VirtualDirectory child in VirtualDirectories)
            {
                if (child.Path == VirtualDirectory.RootPath)
                {
                    root = child.PhysicalPath.ExpandIisExpressEnvironmentVariables();
                    break;
                }
            }

            if (this.IsRoot())
            {
                var server = Server.GetConfigurationCache().FileContext;
                if (root == null)
                {
                    var site = new Configuration(new FileContext(Server, null, server, Site.Name, false, false, Server.ReadOnly, (Entity as IXmlLineInfo).LineNumber));
                    return _configuration = site;
                }
                else
                {
                    var physicalPath = Server.GetPhysicalPath(root);
                    var siteFile = System.IO.Path.Combine(physicalPath,
                        "web.config").ExpandIisExpressEnvironmentVariables();
                    Server.CleanSiteFile(siteFile);

                    // TODO: test ACL to set ReadOnly.
                    var site = new Configuration(new FileContext(Server, siteFile, server, Site.Name, false, false, Server.ReadOnly));
                    return _configuration = site;
                }
            }

            string start = null;
            Configuration parent = null;
            while (parent == null)
            {
                var parentPath = (start ?? Path).GetParentPath();
                foreach (Application app in Site.Applications)
                {
                    if (app.Path != parentPath)
                    {
                        continue;
                    }

                    parent = app.GetWebConfiguration();
                    break;
                }

                if (start == parentPath)
                {
                    break;
                }

                start = parentPath;
            }

            if (root == null)
            {
                var app = new Configuration(new FileContext(Server, null, parent?.FileContext, Site.Name, false, false, Server.ReadOnly, (Entity as IXmlLineInfo).LineNumber));
                return _configuration = app;
            }

            var fullPath = Site.Name + Path;
            var appFile = System.IO.Path.Combine(root, "web.config");
            // TODO: test ACL to set ReadOnly.
            return _configuration = new Configuration(new FileContext(Server, appFile, parent?.FileContext, fullPath, false, false, Server.ReadOnly));
        }

        public override string ToString()
        {
            return Name;
        }

        public string ApplicationPoolName
        {
            get
            {
                var attribute = GetAttribute("applicationPool");
                return attribute.IsInheritedFromDefaultValue 
                    ? Server.ApplicationDefaults.ApplicationPoolName 
                    : (string)attribute.Value;
            }

            set
            {
                var attribute = GetAttribute("applicationPool");
                if (value == Server.ApplicationDefaults.ApplicationPoolName)
                {
                    attribute.Delete();
                    return;
                }

                attribute.Value = value;
            }
        }

        public string EnabledProtocols
        {
            get { return (string)this["enabledProtocols"]; }
            set { this["enabledProtocols"] = value; }
        }

        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        public VirtualDirectoryCollection VirtualDirectories
        {
            get { return _collection; }
            internal set { _collection = value; }
        }

        public VirtualDirectoryDefaults VirtualDirectoryDefaults
        {
            get { return (VirtualDirectoryDefaults)ChildElements["virtualDirectoryDefaults"]; }
        }

        internal SortedDictionary<string, List<string>> Extra { get; set; }
        internal ApplicationCollection Parent { get; private set; }

        internal ServerManager Server
        {
            get { return Parent.Parent.Parent.Parent; }
        }

        internal static readonly string RootPath = Helper.RootPath;

        internal string Name { get; set; }

        internal Site Site
        {
            get { return Parent.Parent; }
        }

        internal async Task<ApplicationCollection> RemoveAsync()
        {
            if (Path == RootPath)
            {
                throw new InvalidOperationException("Root application cannot be removed. Please remove the site.");
            }

            var newApps = new ApplicationCollection(Site);
            foreach (Application item in Parent)
            {
                if (item == this)
                {
                    item.Delete();
                    continue;
                }

                item.Parent = newApps;
                newApps.Add(item);
            }

            newApps.Parent.Applications = newApps;
            return newApps;
        }

        internal string ToFileName()
        {
            var merged = Parent.Parent.Name + Path;
            return merged.TrimEnd('/').Replace('/', '_');
        }

        internal void Save()
        {
            _configuration?.FileContext.Save();
            _configuration?.OnCacheInvalidated();
        }

        internal void Load(string path, string physicalPath)
        {
            VirtualDirectories.Add(
                new VirtualDirectory(null, VirtualDirectories) { Path = path, PhysicalPath = physicalPath });
        }
    }
}
