// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationMethodSchema
    {
        internal ConfigurationMethodSchema()
        { }

        public ConfigurationElementSchema InputSchema { get; private set; }
        public string Name { get; private set; }
        public ConfigurationElementSchema OutputSchema { get; private set; }
    }
}
