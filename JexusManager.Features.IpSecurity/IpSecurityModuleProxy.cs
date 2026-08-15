// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.IpSecurity
{
    internal sealed class IpSecurityModuleProxy : ModuleServiceProxy
    {
        internal IpSecurityItem[] GetItems()
        {
            return (IpSecurityItem[])Invoke(nameof(GetItems));
        }

        internal void Add(IpSecurityItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Remove(IpSecurityItem item)
        {
            Invoke(nameof(Remove), item);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }

        internal IpSecuritySettings GetSettings()
        {
            return (IpSecuritySettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(IpSecuritySettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }

        internal bool IsDynamicIpSecurityAvailable()
        {
            return (bool)Invoke(nameof(IsDynamicIpSecurityAvailable));
        }

        internal DynamicIpSecuritySettings GetDynamicSettings()
        {
            return (DynamicIpSecuritySettings)Invoke(nameof(GetDynamicSettings));
        }

        internal void ApplyDynamicSettings(DynamicIpSecuritySettings settings)
        {
            Invoke(nameof(ApplyDynamicSettings), settings);
        }
    }
}
