// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class HeadersItem : IItem<HeadersItem>
    {
        public HeadersItem()
        {
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(HeadersItem other)
        {
            return other != null && other.Header == Header;
        }

        public uint SizeLimit { get; set; }

        public string Header { get; set; }

        public void Apply()
        {
        }

        public string Flag { get; set; }

        public bool Equals(HeadersItem other)
        {
            return Match(other) && other.SizeLimit == SizeLimit;
        }
    }
}
