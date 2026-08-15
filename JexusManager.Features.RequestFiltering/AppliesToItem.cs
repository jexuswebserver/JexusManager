// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    [Serializable]
    internal class AppliesToItem
    {
        public AppliesToItem()
        {
        }

        public string FileExtension { get; set; }
    }
}
