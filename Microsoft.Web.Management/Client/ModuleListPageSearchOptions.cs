// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ModuleListPageSearchOptions
    {
        public ModuleListPageSearchOptions()
            : this(string.Empty)
        { }

        public ModuleListPageSearchOptions(
            string text
            ) : this(text, null)
        { }

        public ModuleListPageSearchOptions(
            string text,
            ModuleListPageSearchField field
            )
        {
            Text = text;
            Field = field;
        }

        public ModuleListPageSearchField Field { get; }
        public bool ShowAll { get; }
        public string Text { get; }
    }
}