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

            this.Name = (string)element["name"];
            this.Image = (string)element["image"];
            this.Element = element;
        }

        public string Name { get; set; }

        public string Image { get; set; }
        public bool Loaded { get; set; }

        public bool Equals(GlobalModule other)
        {
            return Match(other) && other.Image == this.Image;
        }

        public string Flag { get; set; }

        public void Apply()
        {
            this.Element["name"] = this.Name;
            this.Element["image"] = this.Image;
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(GlobalModule other)
        {
            return other != null && other.Name == this.Name;
        }
    }
}
