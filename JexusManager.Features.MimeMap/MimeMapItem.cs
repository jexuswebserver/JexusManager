// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.MimeMap
{
    using Microsoft.Web.Administration;

    internal class MimeMapItem : IItem<MimeMapItem>
    {
        public MimeMapItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                return;
            }

            this.FileExtension = (string)element["fileExtension"];
            this.MimeType = (string)element["mimeType"];
        }

        public string MimeType { get; set; }

        public string FileExtension { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool Equals(MimeMapItem other)
        {
            // all properties
            return this.Match(other) && other.MimeType == this.MimeType;
        }

        public void Apply()
        {
            this.Element["fileExtension"] = this.FileExtension;
            this.Element["mimeType"] = this.MimeType;
        }

        public bool Match(MimeMapItem other)
        {
            // match combined keys.
            return other != null && other.FileExtension == this.FileExtension;
        }
    }
}
