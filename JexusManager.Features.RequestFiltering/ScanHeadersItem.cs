// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class ScanHeadersItem : IItem<ScanHeadersItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(ScanHeadersItem other)
        {
            return other != null && other.RequestHeader == RequestHeader;
        }

        public ScanHeadersItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                return;
            }

            this.RequestHeader = (string)element["requestHeader"];
        }

        public string RequestHeader { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            this.Element["requestHeader"] = RequestHeader;
        }

        public bool Equals(ScanHeadersItem other)
        {
            return Match(other);
        }
    }
}
