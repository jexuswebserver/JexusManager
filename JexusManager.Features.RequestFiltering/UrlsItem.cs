// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class UrlsItem : IDuoItem<UrlsItem>
    {
        public bool Allowed { get; }

        public ConfigurationElement Element { get; set; }

        public bool Match(UrlsItem other)
        {
            return other != null && other.Url == Url;
        }

        public UrlsItem(ConfigurationElement element, bool allowed)
        {
            this.Allowed = allowed;
            this.Element = element;
            if (element == null)
            {
                return;
            }

            if (allowed)
            {
                Url = (string)element["url"];
            }
            else
            {
                Url = (string)element["sequence"];
            }
        }

        public string Url { get; set; }

        public void Apply()
        {
            if (Allowed)
            {
                Element["url"] = Url;
            }
            else
            {
                Element["sequence"] = Url;
            }
        }

        public string Flag { get; set; }

        public bool Equals(UrlsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
