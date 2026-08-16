// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    public class ServerVariableItem : IItem<ServerVariableItem>
    {
        public ServerVariableItem()
        {
            Replace = true;
        }

        public bool Replace { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public bool Match(ServerVariableItem other)
        {
            return other != null && Name == other.Name;
        }

        public bool Equals(ServerVariableItem other)
        {
            return Match(other) && Value == other.Value && Replace == other.Replace;
        }
    }
}
