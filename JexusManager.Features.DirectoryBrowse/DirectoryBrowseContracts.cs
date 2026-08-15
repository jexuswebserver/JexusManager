// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.DirectoryBrowse
{
    [Serializable]
    public sealed class DirectoryBrowseSnapshot
    {
        public bool Enabled { get; set; }
        public bool LongDateEnabled { get; set; }
        public bool ExtensionEnabled { get; set; }
        public bool SizeEnabled { get; set; }
        public bool TimeEnabled { get; set; }
        public bool DateEnabled { get; set; }
    }
}
