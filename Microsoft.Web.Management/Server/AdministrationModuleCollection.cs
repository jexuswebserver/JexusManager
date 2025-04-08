// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class AdministrationModuleCollection : IEnumerable<AdministrationModule>
    {
        public void Add(string moduleName)
        { }

        public void Clear()
        { }

        public IEnumerator<AdministrationModule> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string moduleName)
        { throw new NotImplementedException(); }

        public int Count { get; }

        public AdministrationModule this[int index]
        { get { throw new NotImplementedException(); } }

        public AdministrationModule this[string name]
        { get { throw new NotImplementedException(); } }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
