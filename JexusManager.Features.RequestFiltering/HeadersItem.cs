// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class HeadersItem : IItem<HeadersItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(HeadersItem other)
        {
            return other != null && other.Header == Header;
        }

        public HeadersItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                return;
            }

            Header = (string)element["header"];
            SizeLimit = (uint)element["sizeLimit"];
        }

        public uint SizeLimit { get; set; }

        public string Header { get; set; }

        public void Apply()
        {
            Element["header"] = Header;
            Element["sizeLimit"] = SizeLimit;
        }

        public string Flag { get; set; }

        public bool Equals(HeadersItem other)
        {
            return Match(other) && other.SizeLimit == SizeLimit;
        }
    }
}
