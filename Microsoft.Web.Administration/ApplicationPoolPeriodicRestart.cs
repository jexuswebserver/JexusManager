// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolPeriodicRestart : ConfigurationElement
    {
        internal ApplicationPoolPeriodicRestart(ConfigurationElement parent)
            : this(null, parent)
        {
        }

        internal ApplicationPoolPeriodicRestart(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "periodicRestart", null, parent, null, null)
        {
        }

        public long Memory
        {
            get { return Convert.ToInt64((uint)this["memory"]); }
            set { this["memory"] = Convert.ToUInt32(value); }
        }

        public long PrivateMemory
        {
            get { return Convert.ToInt64((uint)this["privateMemory"]); }
            set { this["privateMemory"] = Convert.ToUInt32(value); }
        }

        public long Requests
        {
            get { return Convert.ToInt64((uint)this["requests"]); }
            set { this["requests"] = Convert.ToUInt32(value); }
        }

        public ScheduleCollection Schedule
        {
            get { return (ScheduleCollection)ChildElements[0]; }
        }

        public TimeSpan Time
        {
            get { return (TimeSpan)this["time"]; }
            set { this["time"] = value; }
        }
    }
}
