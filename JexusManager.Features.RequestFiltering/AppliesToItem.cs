// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class AppliesToItem
    {
        public ConfigurationElement Child { get; set; }

        public AppliesToItem(ConfigurationElement child)
        {
            this.Child = child;
            if (child == null)
            {
                return;
            }

            this.FileExtension = (string)child["fileExtension"];
        }

        public string FileExtension { get; set; }

        public void Apply()
        {
            Child["fileExtension"] = FileExtension;
        }

        public void AppendTo(ConfigurationElementCollection appliesToCollection)
        {
            if (Child == null)
            {
                Child = appliesToCollection.CreateElement();
                appliesToCollection.Add(Child);
            }

            Apply();
        }
    }
}
