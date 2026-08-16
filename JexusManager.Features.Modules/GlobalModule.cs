// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class GlobalModule : IItem<GlobalModule>
    {
        public GlobalModule()
        {
            Name = string.Empty;
            Image = string.Empty;
            PreConditions = new List<string>(0);
        }

        public List<string> PreConditions { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public bool Loaded { get; set; }

        public bool Equals(GlobalModule other)
        {
            return Match(other) && other.Image == Image;
        }

        public string Flag { get; set; }

        public bool Match(GlobalModule other)
        {
            return other != null && other.Name == Name;
        }
    }
}
