// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class MapRule : IItem<MapRule>
    {
        public MapRule()
        {
            Flag = "Local";
        }

        public bool Match(MapRule other)
        {
            return other != null && other.Original == Original;
        }

        public string Original { get; set; }

        internal string OriginalKey { get; set; }

        public string New { get; set; }

        public string Flag { get; set; }

        public bool Equals(MapRule other)
        {
            return Match(other) && other.New == New;
        }
    }
}
