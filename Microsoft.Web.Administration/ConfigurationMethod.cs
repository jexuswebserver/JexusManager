// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationMethod
    {
        internal ConfigurationMethod()
        { }

        public ConfigurationMethodInstance CreateInstance()
        {
            return null;
        }
        public string Name { get; private set; }
        public ConfigurationMethodSchema Schema { get; private set; }
    }
}
