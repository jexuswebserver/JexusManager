// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
{
    using Microsoft.Web.Administration;

    [System.Serializable]
    internal class IsapiCgiRestrictionItem : IItem<IsapiCgiRestrictionItem>
    {
        public IsapiCgiRestrictionItem() { Path = Description = string.Empty; Flag = "Local"; }


        public bool Allowed { get; set; }

        public string Path { get; set; }

        internal string OriginalKey { get; set; }

        public string Description { get; set; }

        public string Flag { get; set; }

        public bool Equals(IsapiCgiRestrictionItem other)
        {
            // all properties
            return Match(other) && other.Description == Description;
        }

        public bool Match(IsapiCgiRestrictionItem other)
        {
            // match combined keys.
            return other != null && other.Path == Path;
        }
    }
}
