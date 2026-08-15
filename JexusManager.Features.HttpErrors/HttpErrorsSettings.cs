// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    internal sealed class HttpErrorsSettings
    {
        public long ErrorMode { get; set; }

        public long DefaultResponseMode { get; set; }

        public string DefaultPath { get; set; } = string.Empty;
    }
}
