// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationPoolCollection : ConfigurationElementCollectionBase<ApplicationPool>
    {
        internal ApplicationPoolCollection(ServerManager parent)
            : this(null, parent)
        { }

        internal ApplicationPoolCollection(ConfigurationElement collection, ServerManager parent)
            : base(collection, "applicationPools", parent.ApplicationPoolCollection?.Schema, null, collection?.InnerEntity, null)
        {
            Parent = parent;
            Section = collection?.Section;
            if (collection == null)
            {
                return;
            }

            foreach (ConfigurationElement element in collection.GetCollection())
            {
                if (Schema.CollectionSchema.ContainsAddElement(element.ElementTagName))
                {
                    InternalAdd(new ApplicationPool(element, this));
                }
            }
        }

        public ApplicationPool Add(string name)
        {
            var result = new ApplicationPool(null, this) { Name = name };
            Add(result);
            return result;
        }

        protected override ApplicationPool CreateNewElement(string elementTagName)
        {
            var result = new ApplicationPool(null, this);
            Add(result);
            return result;
        }

        private static readonly char[] SChars = { '\\', '/', '"', '|', '<', '>', ':', '*', '?', ']', '[', '+', '=', ';', ',', '@', '&' };

        public static char[] InvalidApplicationPoolNameCharacters()
        {
            return SChars;
        }

        internal ServerManager Parent { get; }
    }
}
