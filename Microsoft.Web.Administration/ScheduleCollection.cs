// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ScheduleCollection : ConfigurationElementCollectionBase<Schedule>
    {
        internal ScheduleCollection(ConfigurationElement parent)
            : this(null, parent)
        {
        }

        internal ScheduleCollection(ConfigurationElement element, ConfigurationElement parent)
            : base(element, "schedule", null, parent, null, null)
        {
            if (element != null)
            {
                foreach (ConfigurationElement node in (ConfigurationElementCollection)element)
                {
                    InternalAdd(new Schedule(node));
                }
            }
        }

        public Schedule Add(TimeSpan scheduleTime)
        {
            var result = new Schedule(this) { Time = scheduleTime };
            Add(result);
            return result;
        }

        protected override Schedule CreateNewElement(string elementTagName)
        {
            var result = new Schedule(this);
            Add(result);
            return result;
        }
    }
}
