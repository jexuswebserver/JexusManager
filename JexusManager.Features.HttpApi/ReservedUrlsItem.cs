// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.HttpApi
{
    using Microsoft.Web.Administration;

    [Serializable]
    public class ReservedUrlsItem : IItem<ReservedUrlsItem>
    {
        public ReservedUrlsItem()
        {
        }

        public string UrlPrefix { get; set; }
        public string SecurityDescriptor { get; set; }

        public string Flag { get; set; }
        public ConfigurationElement Element { get; set; }

        public void Apply()
        { }

        public bool Equals(ReservedUrlsItem other)
        {
            return Match(other) && other.SecurityDescriptor == SecurityDescriptor;
        }

        public bool Match(ReservedUrlsItem other)
        {
            return other != null && other.UrlPrefix == UrlPrefix;
        }
    }
}
