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
            var result = new Application(this) {Path = path};
            result.Load(VirtualDirectory.RootPath, physicalPath);
            Add(result);
            return result;
        }

        protected override Application CreateNewElement(string elementTagName)
        {
            throw new NotImplementedException();
        }

        private static readonly char[] SChars = { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' };

        public static char[] InvalidApplicationPathCharacters()
        {
            return SChars;
        }

        internal Site Parent { get; set; }
    }
}
