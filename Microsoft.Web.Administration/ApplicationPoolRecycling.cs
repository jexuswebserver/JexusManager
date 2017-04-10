// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolRecycling : ConfigurationElement
    {
        private ApplicationPoolPeriodicRestart _restart;

        internal ApplicationPoolRecycling(ConfigurationElement parent)
            : this(null, parent)
        {
        }

        internal ApplicationPoolRecycling(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "recycling", null, parent, null, null)
        {
        }

        public bool DisallowOverlappingRotation
        {
            get { return (bool)this["disallowOverlappingRotation"]; }
            set { this["disallowOverlappingRotation"] = value; }
        }

        public bool DisallowRotationOnConfigChange
        {
            get { return (bool)this["disallowRotationOnConfigChange"]; }
            set { this["disallowRotationOnConfigChange"] = value; }
        }

        public RecyclingLogEventOnRecycle LogEventOnRecycle
        {
            get { return (RecyclingLogEventOnRecycle)Enum.ToObject(typeof(RecyclingLogEventOnRecycle), this["logEventOnRecycle"]); }
            set { this["logEventOnRecycle"] = (long)value; }
        }

        public ApplicationPoolPeriodicRestart PeriodicRestart
        {
            get { return _restart ?? (_restart = new ApplicationPoolPeriodicRestart(ChildElements["periodicRestart"], this)); }
        }
    }
}
