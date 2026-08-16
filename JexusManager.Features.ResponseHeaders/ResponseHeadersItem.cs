// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using Microsoft.Web.Administration;

    [System.Serializable]
    internal class ResponseHeadersItem : IItem<ResponseHeadersItem>
    {
        public ResponseHeadersItem()
        {
            Name = Value = string.Empty;
            Flag = "Local";
        }
        public string Name { get; internal set; }
        public string Value { get; internal set; }
        public string Flag { get; set; }



        public bool Equals(ResponseHeadersItem other)
        {
            return Match(other);
        }

        public bool Match(ResponseHeadersItem other)
        {
            return other != null && other.Name == Name;
        }
    }
}
