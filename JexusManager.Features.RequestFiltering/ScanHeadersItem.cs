// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class ScanHeadersItem : IItem<ScanHeadersItem>
    {
        public ScanHeadersItem()
        {
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(ScanHeadersItem other)
        {
            return other != null && other.RequestHeader == RequestHeader;
        }

        public string RequestHeader { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
        }

        public bool Equals(ScanHeadersItem other)
        {
            return Match(other);
        }
    }
}
