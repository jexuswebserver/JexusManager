// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.DefaultDocument
{
    [Serializable]
    public sealed class DefaultDocumentEntry
    {
        public string Name { get; set; }
        public bool IsLocal { get; set; }
        public bool IsLocked { get; set; }
    }

    [Serializable]
    public sealed class DefaultDocumentSnapshot
    {
        public bool Enabled { get; set; }
        public bool CanRevert { get; set; }
        public DefaultDocumentEntry[] Entries { get; set; } = Array.Empty<DefaultDocumentEntry>();
    }
}
