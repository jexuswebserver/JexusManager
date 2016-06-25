// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client.Extensions
{
    public abstract class ProviderFeature
    {
        public virtual string ConnectionStringAttributeName { get; }
        public virtual bool ConnectionStringRequired { get; }
        public abstract string FeatureName { get; }
        public abstract string ProviderBaseType { get; }
        public abstract string ProviderCollectionPropertyName { get; }
        public abstract string[] ProviderConfigurationSettingNames { get; }
        public abstract string SectionName { get; }
        public abstract string SelectedProvider { get; }
        public abstract string SelectedProviderPropertyName { get; }
        public abstract ProviderConfigurationSettings Settings { get; }
    }
}