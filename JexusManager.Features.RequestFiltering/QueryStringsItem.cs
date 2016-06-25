// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class QueryStringsItem : IDuoItem<QueryStringsItem>
    {
        public bool Allowed { get; }

        public ConfigurationElement Element { get; set; }

        public bool Match(QueryStringsItem other)
        {
            return other != null && other.QueryString == QueryString;
        }

        public QueryStringsItem(ConfigurationElement element, bool allowed)
        {
            this.Allowed = allowed;
            this.Element = element;
            if (element == null)
            {
                return;
            }

            if (allowed)
            {
                QueryString = (string)element["queryString"];
            }
            else
            {
                QueryString = (string)element["sequence"];
            }
        }

        public string QueryString { get; set; }

        public void Apply()
        {
            if (Allowed)
            {
                Element["queryString"] = QueryString;
            }
            else
            {
                Element["sequence"] = QueryString;
            }
        }

        public string Flag { get; set; }

        public bool Equals(QueryStringsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
