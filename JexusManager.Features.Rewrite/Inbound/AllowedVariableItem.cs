// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class AllowedVariableItem : IItem<AllowedVariableItem>
    {
        public AllowedVariableItem()
        {
            Flag = "Local";
        }

        public bool Match(AllowedVariableItem other)
        {
            return other != null && other.Name == Name;
        }

        public string Name { get; internal set; }

        public string Flag { get; set; }

        public bool Equals(AllowedVariableItem other)
        {
            return Match(other);
        }
    }
}
