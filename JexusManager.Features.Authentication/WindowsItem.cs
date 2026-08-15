// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class WindowsItem
    {
        public List<ProviderItem> Providers { get; set; } = new List<ProviderItem>();

        public int TokenChecking { get; set; }
        public bool UseKernelMode { get; set; }

    }
}
