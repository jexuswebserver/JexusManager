// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Path}")]
    public sealed class SectionDefinition
    {
        internal SectionDefinition()
        { }

        internal SectionSchema Schema { get; set; }

        public string AllowDefinition { get; set; }
        public string AllowLocation { get; set; }
        public string Name { get; internal set; }
        public string OverrideModeDefault { get; set; }
        public bool RequirePermission { get; set; }
        public string Type { get; set; }
        internal string Path { get; set; }
        internal FileContext FileContext { get; set; }
        internal bool Ignore
        {
            get
            {
                return Type.StartsWith("System.Configuration.IgnoreSection, System.Configuration, Version=");
            }
        }
    }
}
