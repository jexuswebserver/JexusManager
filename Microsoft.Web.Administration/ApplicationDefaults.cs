// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class ApplicationDefaults : ConfigurationElement
    {
        internal ApplicationDefaults(ConfigurationElement element, ConfigurationElement parent)
            : base(element, null, null, parent, null, null)
        {
        }

        public string ApplicationPoolName
        {
            get { return (string)this["applicationPool"]; }
            set { this["applicationPool"] = value; }
        }

        public string EnabledProtocols
        {
            get { return (string)this["enabledProtocols"]; }
            set { this["enabledProtocols"] = value; }
        }
    }
}
