// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class RequestCollection : ConfigurationElementCollectionBase<Request>
    {
        internal RequestCollection()
        { }

        protected override Request CreateNewElement(string elementTagName)
        {
            return null;
        }
    }
}
