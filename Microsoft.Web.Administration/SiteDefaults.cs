// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class SiteDefaults : ConfigurationElement
    {
        private SiteLimits _limits;
        private SiteLogFile _logFile;
        private SiteTraceFailedRequestsLogging _trace;

        internal SiteDefaults(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "siteDefaults", null, parent, null, null)
        {
        }

        public SiteLimits Limits
        {
            get { return _limits ?? (_limits = new SiteLimits(ChildElements["limits"], this)); }
        }

        public SiteLogFile LogFile
        {
            get { return _logFile ?? (_logFile = new SiteLogFile(ChildElements["logFile"], this)); }
        }

        public bool ServerAutoStart
        {
            get { return (bool)this["serverAutoStart"]; }
            set { this["serverAutoStart"] = value; }
        }

        public SiteTraceFailedRequestsLogging TraceFailedRequestsLogging
        {
            get { return _trace ?? (_trace = new SiteTraceFailedRequestsLogging(ChildElements["traceFailedRequestsLogging"], this)); }
        }
    }
}
