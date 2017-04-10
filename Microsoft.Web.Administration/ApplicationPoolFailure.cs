// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolFailure : ConfigurationElement
    {
        internal ApplicationPoolFailure(ApplicationPool parent)
            : this(null, parent)
        { }

        internal ApplicationPoolFailure(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "failure", null, parent, null, null)
        {
        }

        public string AutoShutdownExe
        {
            get { return this["autoShutdownExe"].ToString(); }
            set { this["autoShutdownExe"] = value; }
        }

        public string AutoShutdownParams
        {
            get { return this["autoShutdownParams"].ToString(); }
            set { this["autoShutdownParams"] = value; }
        }

        public LoadBalancerCapabilities LoadBalancerCapabilities
        {
            get { return (LoadBalancerCapabilities)Enum.ToObject(typeof(LoadBalancerCapabilities), this["loadBalancerCapabilities"]); }
            set { this["loadBalancerCapabilities"] = (long)value; }
        }

        public string OrphanActionExe
        {
            get { return (string)this["orphanActionExe"]; }
            set { this["orphanActionExe"] = value; }
        }

        public string OrphanActionParams
        {
            get { return (string)this["orphanActionParams"]; }
            set { this["orphanActionParams"] = value; }
        }

        public bool OrphanWorkerProcess
        {
            get { return (bool)this["orphanWorkerProcess"]; }
            set { this["orphanWorkerProcess"] = value; }
        }

        public bool RapidFailProtection
        {
            get { return (bool)this["rapidFailProtection"]; }
            set { this["rapidFailProtection"] = value; }
        }

        public TimeSpan RapidFailProtectionInterval
        {
            get { return (TimeSpan)this["rapidFailProtectionInterval"]; }
            set { this["rapidFailProtectionInterval"] = value; }
        }

        public long RapidFailProtectionMaxCrashes
        {
            get { return Convert.ToInt64(this["rapidFailProtectionMaxCrashes"]); }
            set { this["rapidFailProtectionMaxCrashes"] = Convert.ToUInt32(value); }
        }
    }
}
