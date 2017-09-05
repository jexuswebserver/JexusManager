// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationCollection : ConfigurationElementCollectionBase<Application>
    {
        internal ApplicationCollection(Site site)
            : base(null, "site", null, site.ParentElement, site.InnerEntity, null)
        {
            Parent = site;
        }

        public Application Add(string path, string physicalPath)
        {
            throw new NotImplementedException();
        }

        protected override Application CreateNewElement(string elementTagName)
        {
            throw new NotImplementedException();
        }

        private static readonly char[] s_chars = { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' };

        public static char[] InvalidApplicationPathCharacters()
        {
            return s_chars;
        }

        internal Site Parent { get; set; }
    }
}
