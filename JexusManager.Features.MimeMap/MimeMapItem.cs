// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.MimeMap
{
    using Microsoft.Web.Administration;

    [System.Serializable]
    internal class MimeMapItem : IItem<MimeMapItem>
    {
        public MimeMapItem()
        {
            FileExtension = MimeType = string.Empty;
            Flag = "Local";
        }



        public string MimeType { get; set; }

        public string FileExtension { get; set; }

        internal string OriginalKey { get; set; }

        public string Flag { get; set; }

        public bool Equals(MimeMapItem other)
        {
            // all properties
            return this.Match(other) && other.MimeType == this.MimeType;
        }

        public bool Match(MimeMapItem other)
        {
            // match combined keys.
            return other != null && other.FileExtension == this.FileExtension;
        }
    }
}
