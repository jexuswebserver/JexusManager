// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ModuleListPageSearchField
    {
        public ModuleListPageSearchField(
            string name,
            string text
            )
        {
            Name = name;
            Text = text;
        }

        public override string ToString()
        {
            return null;
        }

        public string Name { get; }
        public string Text { get; }
    }
}
