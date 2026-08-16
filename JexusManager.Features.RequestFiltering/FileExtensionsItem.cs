// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class FileExtensionsItem : IItem<FileExtensionsItem>
    {
        public FileExtensionsItem()
        {
            Flag = "Local";
        }

        public bool Match(FileExtensionsItem other)
        {
            return other != null && other.Extension == Extension;
        }

        public string Flag { get; set; }

        public bool Allowed { get; set; }

        public string Extension { get; set; }

        public bool Equals(FileExtensionsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
