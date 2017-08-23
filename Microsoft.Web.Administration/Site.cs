// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;

    public sealed class Site : ConfigurationElement
    {
        private const string Command = "/config:\"{0}\" /siteid:{1} /systray:false /trace:error";
        private ApplicationCollection _collection;
        private SiteLogFile _logFile;
        private SiteLimits _limits;
        private SiteTraceFailedRequestsLogging _trace;
        private VirtualDirectoryDefaults _virtualDefaults;
        private ObjectState? _state;

        internal Site(SiteCollection parent)
            : this(null, parent)
        { }

        internal Site(ConfigurationElement element, SiteCollection parent)
            : base(element, "site", null, parent, null, null)
        {
            ApplicationDefaults = ChildElements["applicationDefaults"] == null
                ? new ApplicationDefaults(parent.ChildElements["applicationDefaults"], this)
                : new ApplicationDefaults(ChildElements["applicationDefaults"], this);
            Parent = parent;
            Bindings = new BindingCollection(ChildElements["bindings"], this);
            if (element == null)
            {
                return;
            }

            foreach (ConfigurationElement node in (ConfigurationElementCollection)element)
            {
                var app = new Application(node, Applications);
                Applications.InternalAdd(app);
            }
        }

        public ApplicationDefaults ApplicationDefaults { get; }

        public ApplicationCollection Applications
        {
            get => _collection ?? (_collection = new ApplicationCollection(this));
            internal set => _collection = value;
        }

        public BindingCollection Bindings { get; internal set; }

        public long Id
        {
            get => (uint)this["id"];
            set => this["id"] = value;
        }

        public SiteLimits Limits => _limits ?? (_limits = new SiteLimits(ChildElements["limits"], this));

        public SiteLogFile LogFile => _logFile ?? (_logFile = new SiteLogFile(ChildElements["logFile"], this));

        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }

        public bool ServerAutoStart
        {
            get => (bool)this["serverAutoStart"];
            set => this["serverAutoStart"] = value;
        }

        public ObjectState State
        {
            get
            {
                if (_state == null)
                {
                    var result = GetState();
                    _state = result ? ObjectState.Started : ObjectState.Stopped;
                }

                return _state.Value;
            }

            internal set => _state = value;
        }

        internal string CommandLine => string.Format(Command, FileContext.FileName, Id);

        public SiteTraceFailedRequestsLogging TraceFailedRequestsLogging => _trace ?? (_trace = new SiteTraceFailedRequestsLogging(ChildElements["traceFailedRequestsLogging"], this));

        public VirtualDirectoryDefaults VirtualDirectoryDefaults => _virtualDefaults ?? (_virtualDefaults = new VirtualDirectoryDefaults(ChildElements["virtualDirectoryDefaults"], this));

        internal SiteCollection Parent { get; }

        public Configuration GetWebConfiguration()
        {
            foreach (Application app in Applications)
            {
                if (app.Path == Application.RootPath)
                {
                    return app.GetWebConfiguration();
                }
            }

            return new Configuration(new FileContext(Server, null, null, Name, true, true, true, (Entity as IXmlLineInfo).LineNumber));
        }

        public ObjectState Start()
        {
            // TODO: add timeout.
            State = ObjectState.Starting;
            Server.Start(this);
            return State;
        }

        public ObjectState Stop()
        {
            // TODO: add timeout.
            State = ObjectState.Stopping;
            Server.Stop(this);
            return State;
        }

        public override string ToString()
        {
            return Name;
        }

        internal void RemoveApplications()
        {
            foreach (Application application in Applications)
            {
                application.Remove();
            }

            Applications = new ApplicationCollection(this);
        }

        internal ServerManager Server => Parent.Parent;

        internal IEnumerable<DirectoryInfo> GetPhysicalDirectories()
        {
            if (Server.Mode != WorkingMode.Jexus)
            {
                var root = PhysicalPath.ExpandIisExpressEnvironmentVariables();
                if (Directory.Exists(root))
                {
                    var result = new DirectoryInfo(root).GetDirectories();
                    return result;
                }

                return new DirectoryInfo[0];
            }

            return null;
        }

        internal void Restart()
        {
            Server.Restart(this);
        }

        internal bool GetState()
        {
            return Server.GetSiteState(this);
        }

        internal void RemoveApplication(Application application)
        {
            Applications = application.Remove();
        }

        internal void Save()
        {
            foreach (Application application in Applications)
            {
                application.Save();
            }
        }

        internal string PhysicalPath
        {
            get
            {
                if (Applications.Count == 0)
                {
                    return string.Empty;
                }

                var path = Applications[0].PhysicalPath;
                return path;
            }
        }
    }
}
