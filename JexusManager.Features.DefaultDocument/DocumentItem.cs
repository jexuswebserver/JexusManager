// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DefaultDocument
{
    using Microsoft.Web.Administration;

    public class DocumentItem : IItem<DocumentItem>
    {
        public ConfigurationElement Element { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }

        public DocumentItem(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            if (element == null)
            {
                return;
            }

            Name = (string)element["value"];
        }

        private DocumentItem(DefaultDocumentEntry entry)
        {
            Name = entry.Name;
            Flag = entry.IsLocal ? "Local" : "Inherited";
        }

        internal static DocumentItem FromEntry(DefaultDocumentEntry entry)
        {
            return new DocumentItem(entry);
        }

        public bool Equals(DocumentItem other)
        {
            return Match(other);
        }

        public void Apply()
        {
            Element["value"] = Name;
        }

        public bool Match(DocumentItem other)
        {
            return other != null && other.Name == Name;
        }
    }
}
