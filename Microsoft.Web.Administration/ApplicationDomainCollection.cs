// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationDomainCollection : ConfigurationElementCollectionBase<ApplicationDomain>
    {
        internal ApplicationDomainCollection()
        { }

        protected override ApplicationDomain CreateNewElement(string elementTagName)
        {
            return null;
        }
    }
}
