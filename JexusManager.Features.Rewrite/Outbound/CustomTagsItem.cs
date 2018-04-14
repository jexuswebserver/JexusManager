// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    public class CustomTagsItem : IItem<CustomTagsItem>
    {
        public ConfigurationElement Element { get; set; }

        public CustomTagsItem(ConfigurationElement element)
        {
            Element = element;
            Tags = new List<CustomTagItem>();
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            Name = (string)element["name"];
            var collection = element.GetCollection();
            foreach (ConfigurationElement item in collection)
            {
                Tags.Add(new CustomTagItem(item));
            }
        }

        public List<CustomTagItem> Tags { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public string TagString { get; private set; }

        public void Apply()
        {
            Element["name"] = Name;
            var collection = Element.GetCollection();
            collection.Clear();
            foreach (var item in Tags)
            {
                item.AppendTo(collection);
            }
        }

        public void Add(CustomTagItem newItem)
        {
            // TODO: add
        }

        public bool Match(CustomTagsItem other)
        {
            return other != null && Name == other.Name;
        }

        public bool Equals(CustomTagsItem other)
        {
            return Match(other) && TagString == other.TagString;
        }
    }
}
