// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class DenyStringsItem
    {
        public ConfigurationElement Child { get; set; }

        public DenyStringsItem(ConfigurationElement child)
        {
            this.Child = child;
            if (child == null)
            {
                return;
            }

            this.DenyString = (string)child["string"];
        }

        public string DenyString { get; set; }

        public void Apply()
        {
            Child["string"] = DenyString;
        }

        public void AppendTo(ConfigurationElementCollection denyStringsCollection)
        {
            if (Child == null)
            {
                Child = denyStringsCollection.CreateElement();
                denyStringsCollection.Add(Child);
            }

            Apply();
        }
    }
}