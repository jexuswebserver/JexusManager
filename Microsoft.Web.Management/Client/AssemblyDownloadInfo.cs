// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace Microsoft.Web.Management.Client
{
    public sealed class AssemblyDownloadInfo
    {
        public AssemblyDownloadInfo(
    string name,
    string displayName,
    string filePath,
    bool canIgnore,
    CultureInfo culture
)
        {
            Name = name;
            DisplayName = displayName;
            FilePath = filePath;
            CanIgnore = canIgnore;
            Culture = culture;
        }

        public bool CanIgnore { get; }
        public CultureInfo Culture { get; }
        public string DisplayName { get; }
        public string FilePath { get; }
        public string Name { get; }
    }
}
