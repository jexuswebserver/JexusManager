// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace Microsoft.Web.Management.Client
{
    public abstract class ProviderConfigurationSettings
    {
        public IDictionary GetSettings()
        {
            return Settings;
        }

        public void LoadSettings(
            string[] parameters
            )
        { }

        public abstract bool Validate(
            out string message
            );

        public IList ProviderSpecificSettings { get; }

        protected abstract IDictionary Settings { get; }
    }
}
