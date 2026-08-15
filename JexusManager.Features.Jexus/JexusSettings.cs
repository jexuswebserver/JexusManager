// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.Jexus
{
    [Serializable]
    public sealed class JexusSettings
    {
        public bool IsAvailable { get; set; }
        public string Contents { get; set; }
    }
}