// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    public class CustomTagItem : IItem<CustomTagItem>
    {
        public CustomTagItem()
        {
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(CustomTagItem other)
        {
            return other != null && other.Name == Name;
        }

        public string Attribute { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
        }

        public bool Equals(CustomTagItem other)
        {
            return Match(other) && other.Attribute == Attribute;
        }
    }
}
