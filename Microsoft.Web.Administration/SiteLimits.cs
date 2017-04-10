// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class SiteLimits : ConfigurationElement
    {
        internal SiteLimits(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "limits", null, parent, null, null)
        {
        }

        public TimeSpan ConnectionTimeout
        {
            get { return (TimeSpan)this["connectionTimeout"]; }
            set { this["connectionTimeout"] = value; }
        }

        public long MaxBandwidth
        {
            get { return Convert.ToInt64(this["maxBandwidth"]); }
            set { this["maxBandwidth"] = Convert.ToUInt32(value); }
        }

        public long MaxConnections
        {
            get { return Convert.ToInt64(this["maxConnections"]); }
            set { this["maxConnections"] = Convert.ToUInt32(value); }
        }

        public long MaxUrlSegments
        {
            get { return Convert.ToInt64(this["maxUrlSegments"]); }
            set { this["maxUrlSegments"] = Convert.ToUInt32(value); }
        }
    }
}
