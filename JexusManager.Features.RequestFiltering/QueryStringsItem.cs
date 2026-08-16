// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class QueryStringsItem : IItem<QueryStringsItem>
    {
        public QueryStringsItem(bool allowed)
        {
            Allowed = allowed;
        }

        public bool Allowed { get; }

        public ConfigurationElement Element { get; set; }

        public bool Match(QueryStringsItem other)
        {
            return other != null && other.QueryString == QueryString;
        }

        public string QueryString { get; set; }

        public void Apply()
        {
        }

        public string Flag { get; set; }

        public bool Equals(QueryStringsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
