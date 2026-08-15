// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class UrlsItem : IDuoItem<UrlsItem>
    {
        public UrlsItem(bool allowed)
        {
            Allowed = allowed;
        }

        public bool Allowed { get; }

        public ConfigurationElement Element { get; set; }

        public bool Match(UrlsItem other)
        {
            return other != null && other.Url == Url;
        }

        public string Url { get; set; }

        public void Apply()
        {
        }

        public string Flag { get; set; }

        public bool Equals(UrlsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
