// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.Logging
{
    [Serializable]
    public sealed class LoggingSettings
    {
        public bool Enabled { get; set; }

        public long Mode { get; set; }

        public int Encoding { get; set; }

        public long LogFormat { get; set; }

        public string Directory { get; set; } = string.Empty;

        public long LogTargetW3C { get; set; }

        public bool LocalTimeRollover { get; set; }

        public string TruncateSizeString { get; set; } = string.Empty;

        public long Period { get; set; }
    }
}
