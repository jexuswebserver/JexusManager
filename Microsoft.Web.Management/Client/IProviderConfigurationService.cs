// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client.Extensions;

namespace Microsoft.Web.Management.Client
{
    public interface IProviderConfigurationService
    {
        bool ConfigureProvider(
            ProviderFeature feature
            );
    }
}
