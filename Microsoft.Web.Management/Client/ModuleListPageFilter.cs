// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ModuleListPageFilter
    {
        public ModuleListPageFilter(string description)
            : this(description, false)
        { }

        public ModuleListPageFilter(string description, bool canRemove)
        {
            Description = description;
            CanRemove = canRemove;
        }

        public bool CanRemove { get; }
        public string Description { get; }
    }
}
