// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    [Serializable]
    internal class DenyStringsItem
    {
        public DenyStringsItem()
        {
        }

        public string DenyString { get; set; }
    }
}
