// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolCpu : ConfigurationElement
    {
        internal ApplicationPoolCpu(ApplicationPool parent)
            : this(null, parent)
        { }

        internal ApplicationPoolCpu(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "cpu", null, parent, null, null)
        {
        }

        public ProcessorAction Action
        {
            get { return (ProcessorAction)Enum.ToObject(typeof(ProcessorAction), this["action"]); }
            set { this["action"] = (long)value; }
        }

        public long Limit
        {
            get { return Convert.ToInt64(this["limit"]); }
            set { this["limit"] = Convert.ToUInt32(value); }
        }

        public TimeSpan ResetInterval
        {
            get { return (TimeSpan)this["resetInterval"]; }
            set { this["resetInterval"] = value; }
        }

        public bool SmpAffinitized
        {
            get { return (bool)this["smpAffinitized"]; }
            set { this["smpAffinitized"] = value; }
        }

        public long SmpProcessorAffinityMask
        {
            get { return Convert.ToInt64(this["smpProcessorAffinityMask"]); }
            set { this["smpProcessorAffinityMask"] = Convert.ToUInt32(value); }
        }

        public long SmpProcessorAffinityMask2
        {
            get { return Convert.ToInt64(this["smpProcessorAffinityMask2"]); }
            set { this["smpProcessorAffinityMask2"] = Convert.ToUInt32(value); }
        }
    }
}
