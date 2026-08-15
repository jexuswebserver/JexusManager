// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;

    [Serializable]
    public class FormsItem
    {
        public long Mode { get; set; }

        public long ProtectedMode { get; set; }

        public bool RequireSsl { get; set; }

        public bool SlidinngExpiration { get; set; }

        public string Name { get; set; }

        public TimeSpan Timeout { get; set; }

        public string LoginUrl { get; set; }

    }
}
