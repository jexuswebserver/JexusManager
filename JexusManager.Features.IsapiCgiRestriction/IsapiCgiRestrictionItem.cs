// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
{
    using Microsoft.Web.Administration;

    internal class IsapiCgiRestrictionItem : IItem<IsapiCgiRestrictionItem>
    {
        public IsapiCgiRestrictionItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                return;
            }

            Description = (string)element["description"];
            Path = (string)element["path"];
            Allowed = (bool)element["allowed"];
        }

        public bool Allowed { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool Equals(IsapiCgiRestrictionItem other)
        {
            // all properties
            return Match(other) && other.Description == Description;
        }

        public void Apply()
        {
            Element["description"] = Description;
            Element["path"] = Path;
            Element["allowed"] = Allowed;
        }

        public bool Match(IsapiCgiRestrictionItem other)
        {
            // match combined keys.
            return other != null && other.Path == Path;
        }
    }
}
