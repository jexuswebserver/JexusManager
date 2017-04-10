// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Microsoft.Web.Administration
{
    public sealed class BindingCollection : ConfigurationElementCollectionBase<Binding>
    {
        internal BindingCollection(Site parent)
            : this(null, parent)
        { }

        internal BindingCollection(ConfigurationElement element, Site parent)
            : base(element, "bindings", parent.Schema.ChildElementSchemas["bindings"], parent, element?.InnerEntity, null)
        {
            Parent = parent;
            if (element != null)
            {
                foreach (ConfigurationElement child in (ICollection)element)
                {
                    InternalAdd(new Binding(child, this));
                }
            }
        }

        public Binding Add(string bindingInformation, string bindingProtocol)
        {
            throw new NotImplementedException();
        }

        public Binding Add(string bindingInformation, byte[] certificateHash, string certificateStoreName)
        {
            throw new NotImplementedException();
        }

        public Binding Add(string bindingInformation, byte[] certificateHash, string certificateStoreName, SslFlags sslFlags)
        {
            throw new NotImplementedException();
        }

        protected override Binding CreateNewElement(string elementTagName)
        {
            throw new NotImplementedException();
        }

        public void Remove(Binding element, bool removeConfigOnly)
        {
            throw new NotImplementedException();
        }

        internal Site Parent { get; set; }
    }
}
