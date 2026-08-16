// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class HiddenSegmentsItem : IItem<HiddenSegmentsItem>
    {
        public HiddenSegmentsItem()
        {
            Flag = "Local";
        }

        public string Segment { get; set; }
        public string Flag { get; set; }

        public bool Equals(HiddenSegmentsItem other)
        {
            return Match(other);
        }

        public bool Match(HiddenSegmentsItem other)
        {
            return other != null && other.Segment == Segment;
        }
    }
}
