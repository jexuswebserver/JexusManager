// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class VerbsItem : IItem<VerbsItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(VerbsItem other)
        {
            return other != null && other.Verb == Verb;
        }

        public VerbsItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                return;
            }

            Verb = (string)element["verb"];
            Allowed = (bool)element["allowed"];
        }

        public bool Allowed { get; set; }

        public string Verb { get; set; }

        public void Apply()
        {
            Element["verb"] = Verb;
            Element["allowed"] = Allowed;
        }

        public string Flag { get; set; }

        public bool Equals(VerbsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
