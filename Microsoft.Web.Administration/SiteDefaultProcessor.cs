// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    internal class SiteDefaultProcessor<V>
        where V : ConfigurationElement
    {
        public SiteDefaultProcessor(V siteDefault)
        {
            SiteDefault = siteDefault;
        }

        public V SiteDefault { get; }

        public T Get<T>(string name, Func<T> defaultGetter, Func<ConfigurationAttribute, T> mainGetter, V instance)
        {
            var attribute = instance.GetAttribute(name);
            if (attribute.IsInheritedFromDefaultValue && SiteDefault != null)
            {
                return defaultGetter();
            }

            return mainGetter(attribute);
        }

        public void Set<T>(string name, T value, Func<T> defaultGetter, Action<ConfigurationAttribute> mainSetter, V instance)
        {
            var attribute = instance.GetAttribute(name);
            if (SiteDefault != null && value.Equals(defaultGetter()))
            {
                attribute.Delete();
                return;
            }

            mainSetter(attribute);
        }
    }
}
