// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    public class CustomTagsItem
    {
        public ConfigurationElement Element { get; set; }

        public CustomTagsItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Tags = new List<CustomTagItem>();
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            this.Name = (string)element["name"];
            var collection = element.GetCollection();
            foreach (ConfigurationElement item in collection)
            {
                this.Tags.Add(new CustomTagItem(item));
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

        public void AppendTo(ConfigurationElementCollection rulesCollection)
        {
            Element = rulesCollection.CreateElement();
            Apply();
            rulesCollection.Add(Element);
        }
    }
}