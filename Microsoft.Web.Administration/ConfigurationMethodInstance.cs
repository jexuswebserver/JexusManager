// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationMethodInstance
    {
        internal ConfigurationMethodInstance()
        { }

        public void Execute()
        { }

        public object GetMetadata(string metadataType)
        {
            return null;
        }

        public void SetMetadata(string metadataType, object value)
        {
        }

        public ConfigurationElement Input { get; private set; }
        public ConfigurationElement Output { get; private set; }
    }
}
