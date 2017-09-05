// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class VirtualDirectoryCollection : ConfigurationElementCollectionBase<VirtualDirectory>
    {
        internal VirtualDirectoryCollection(Application application)
            : base(null, "application", null, application.ParentElement, application.InnerEntity, null)
        {
            Parent = application;
        }

        public VirtualDirectory Add(string path, string physicalPath)
        {
            var result = new VirtualDirectory(null, this) { Path = path, PhysicalPath = physicalPath };
            Add(result);
            return result;
        }

        protected override VirtualDirectory CreateNewElement(string elementTagName)
        {
            throw new NotImplementedException();
        }

        private static readonly char[] s_chars = { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' };

        public Application Parent { get; private set; }

        public static char[] InvalidVirtualDirectoryPathCharacters()
        {
            return s_chars;
        }
    }
}
