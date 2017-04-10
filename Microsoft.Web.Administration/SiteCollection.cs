// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class SiteCollection : ConfigurationElementCollectionBase<Site>
    {
        internal SiteCollection(ServerManager parent)
            : this(null, parent)
        {
        }

        internal SiteCollection(ConfigurationElement element, ServerManager parent)
            : base(element, "sites", parent.SiteCollection?.Schema, null, parent.SiteCollection?.InnerEntity, null)
        {
            Parent = parent;
            Section = element?.Section;
            if (element == null)
            {
                return;
            }

            foreach (ConfigurationElement node in element.GetCollection())
            {
                if (Schema.CollectionSchema.ContainsAddElement(node.ElementTagName))
                {
                    InternalAdd(new Site(node, this));
                }
            }
        }

        public Site Add(string name, string physicalPath, int port)
        {
            throw new NotImplementedException();
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash)
        {
            throw new NotImplementedException();
        }

        public Site Add(string name, string bindingProtocol, string bindingInformation, string physicalPath)
        {
            throw new NotImplementedException();
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash, string certificateStore)
        {
            throw new NotImplementedException();
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash, string certificateStore, SslFlags sslFlags)
        {
            throw new NotImplementedException();
        }

        protected override Site CreateNewElement(string elementTagName)
        {
            throw new NotImplementedException();
        }

        private static readonly char[] s_chars = { '\\', '/', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>' };

        internal ServerManager Parent { get; private set; }

        public static char[] InvalidSiteNameCharacters()
        {
            return s_chars;
        }

        internal static char[] InvalidSiteNameCharactersJexus()
        {
            return new[] { ' ' };
        }

        internal bool? FindDuplicate(Binding binding, Site host, Binding existing)
        {
            foreach (Site site in this)
            {
                foreach (Binding item in site.Bindings)
                {
                    if (item.Protocol == binding.Protocol && item.ToString() == binding.ToString() && item != existing)
                    {
                        return site == host ? (bool?)null : true;
                    }
                }
            }

            return false;
        }
    }
}
