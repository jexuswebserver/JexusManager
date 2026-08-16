// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DefaultDocument
{
    using Microsoft.Web.Administration;

    public class DocumentItem : IItem<DocumentItem>
    {
        public DocumentItem()
        {
            Name = string.Empty;
            Flag = "Local";
        }

        public string Name { get; set; }
        public string Flag { get; set; }



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

        public bool Match(DocumentItem other)
        {
            return other != null && other.Name == Name;
        }
    }
}
