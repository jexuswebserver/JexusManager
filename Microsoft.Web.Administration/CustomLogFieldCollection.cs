// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class CustomLogFieldCollection : ConfigurationElementCollectionBase<CustomLogField>
    {
        public CustomLogFieldCollection(ConfigurationElement element, ConfigurationElement parent)
            : base(element, null, null, parent, null, null)
        {
            if (element != null)
            {
                foreach (ConfigurationElement node in (ConfigurationElementCollection)element)
                {
                    InternalAdd(new CustomLogField(node, this));
                }
            }
        }

        public CustomLogField Add(string logFieldName, string sourceName, CustomLogFieldSourceType sourceType)
        {
            return null;
        }

        protected override CustomLogField CreateNewElement(string elementTagName)
        {
            return new CustomLogField(null, this);
        }
    }
}
