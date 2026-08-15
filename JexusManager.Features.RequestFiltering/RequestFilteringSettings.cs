// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.RequestFiltering
{
    [Serializable]
    public sealed class RequestFilteringSettings
    {
        public bool FileExtensionsAllowUnlisted { get; set; }

        public bool VerbsAllowUnlisted { get; set; }

        public bool AllowHighBitCharacters { get; set; }

        public bool AllowDoubleEscaping { get; set; }

        public uint MaxAllowedContentLength { get; set; }

        public uint MaxUrl { get; set; }

        public uint MaxQueryString { get; set; }
    }
}
