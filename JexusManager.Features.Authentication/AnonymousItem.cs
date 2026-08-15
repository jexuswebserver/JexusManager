// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;

    [Serializable]
    public class AnonymousItem
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
