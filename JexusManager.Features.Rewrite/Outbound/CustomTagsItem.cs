// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    public class CustomTagsItem : IItem<CustomTagsItem>
    {
        public CustomTagsItem()
        {
            Tags = new List<CustomTagItem>();
            Flag = "Local";
        }

        public List<CustomTagItem> Tags { get; set; }

        public string Name { get; set; }

        internal string OriginalKey { get; set; }

        public string Flag { get; set; }

        public string TagString { get; private set; }

        public void Add(CustomTagItem newItem)
        {
            Tags.Add(newItem);
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
