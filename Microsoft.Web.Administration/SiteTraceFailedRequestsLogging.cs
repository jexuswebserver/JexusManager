// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class SiteTraceFailedRequestsLogging : ConfigurationElement
    {
        internal SiteTraceFailedRequestsLogging(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "traceFailedRequestsLogging", null, parent, null, null)
        {
        }

        public string Directory
        {
            get { return (string)this["directory"]; }
            set { this["directory"] = value; }
        }

        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        public long MaxLogFiles
        {
            get { return Convert.ToInt64(this["maxLogFiles"]); }
            set { this["maxLogFiles"] = Convert.ToUInt32(value); }
        }
    }
}
