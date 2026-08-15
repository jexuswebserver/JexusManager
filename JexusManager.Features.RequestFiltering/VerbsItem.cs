// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class VerbsItem : IItem<VerbsItem>
    {
        public VerbsItem()
        {
        }

        public ConfigurationElement Element { get; set; }

        public bool Match(VerbsItem other)
        {
            return other != null && other.Verb == Verb;
        }

        public bool Allowed { get; set; }

        public string Verb { get; set; }

        public void Apply()
        {
        }

        public string Flag { get; set; }

        public bool Equals(VerbsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
