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
            var site = new Site(this);
            site.Name = name;
            site.Bindings.Add($"*:{port}:", "http");
            site.Applications.Add(Application.RootPath, physicalPath);
            return Add(site);
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash)
        {
            return Add(name, bindingInformation, physicalPath, certificateHash, "MY");
        }

        public Site Add(string name, string bindingProtocol, string bindingInformation, string physicalPath)
        {
            var site = new Site(this);
            site.Name = name;
            site.Bindings.Add(bindingInformation, bindingProtocol);
            site.Applications.Add(Application.RootPath, physicalPath);
            return Add(site);
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash, string certificateStore)
        {
            return Add(name, bindingInformation, physicalPath, certificateHash, certificateStore, SslFlags.None);
        }

        public Site Add(string name, string bindingInformation, string physicalPath, byte[] certificateHash, string certificateStore, SslFlags sslFlags)
        {
            var site = new Site(this);
            site.Name = name;
            site.Bindings.Add(bindingInformation, certificateHash, certificateStore, sslFlags);
            site.Applications.Add(Application.RootPath, physicalPath);
            return Add(site);
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

        /// <summary>
        /// Gets a site that has the specified name in the collection.
        /// </summary>
        /// <param name="name">The name of the site to retrieve from the site collection.</param>
        /// <returns>The <see cref="Site"/> object with the specified name in the <see cref="SiteCollection"/> object.</returns>
        /// <remarks>The <see cref="Site.Name"/> property of a <see cref="Site"/> object corresponds to the name parameter that is used to find a site in the <see cref="SiteCollection"/> object.</remarks>
        public new Site this[string name]
        {
            get
            {
                foreach (Site site in this)
                {
                    if (site.Name == name)
                    {
                        return site;
                    }
                }

                return null;
            }
        }
    }
}
