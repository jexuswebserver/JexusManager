// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using Microsoft.Web.Administration;

    internal class GlobalModule : IItem<GlobalModule>
    {
        public GlobalModule(ConfigurationElement element)
        {
            if (element == null)
            {
                return;
            }

            Name = (string)element["name"];
            Image = (string)element["image"];
            Element = element;
        }

        public string Name { get; set; }

        public string Image { get; set; }
        public bool Loaded { get; set; }

        public bool Equals(GlobalModule other)
        {
            return Match(other) && other.Image == Image;
        }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["name"] = Name;
            Element["image"] = Image;
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(GlobalModule other)
        {
            return other != null && other.Name == Name;
        }
    }
}
