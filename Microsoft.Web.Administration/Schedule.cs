// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class Schedule : ConfigurationElement
    {
        internal Schedule(ConfigurationElement parent)
            : this(null, parent)
        {
        }

        internal Schedule(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "add", null, parent, null, null)
        {
        }

        public TimeSpan Time
        {
            get { return (TimeSpan)this["time"]; }
            set { this["time"] = value; }
        }
    }
}
