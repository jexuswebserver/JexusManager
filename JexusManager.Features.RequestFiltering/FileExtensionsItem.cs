// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Administration;

    internal class FileExtensionsItem : IItem<FileExtensionsItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(FileExtensionsItem other)
        {
            return other != null && other.Extension == Extension;
        }

        public FileExtensionsItem(ConfigurationElement element)
        {
            this.Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            Extension = (string)element["fileExtension"];
            Allowed = (bool)element["allowed"];
        }

        public string Flag { get; set; }

        public bool Allowed { get; set; }

        public string Extension { get; set; }

        public void Apply()
        {
            Element["fileExtension"] = Extension;
            Element["allowed"] = Allowed;
        }

        public bool Equals(FileExtensionsItem other)
        {
            return Match(other) && other.Allowed == Allowed;
        }
    }
}
