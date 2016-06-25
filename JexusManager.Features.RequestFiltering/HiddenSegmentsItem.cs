// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class HiddenSegmentsItem : IItem<HiddenSegmentsItem>
    {
        public HiddenSegmentsItem(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            if (element == null)
            {
                return;
            }

            Segment = (string)element["segment"];
        }

        public string Segment { get; set; }
        public string Flag { get; set; }
        public ConfigurationElement Element { get; set; }

        public bool Equals(HiddenSegmentsItem other)
        {
            return Match(other);
        }

        public bool Match(HiddenSegmentsItem other)
        {
            return other != null && other.Segment == Segment;
        }

        public void Apply()
        {
            Element["segment"] = Segment;
        }
    }
}
