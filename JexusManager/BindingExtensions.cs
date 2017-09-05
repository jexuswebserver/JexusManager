// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public static class BindingExtensions
    {
        public static string ToShortString(this Binding binding)
        {
            if (binding.Protocol != "http" && binding.Protocol != "https")
            {
                return $"{binding.BindingInformation} ({binding.Protocol})";
            }

            return string.IsNullOrEmpty(binding.Host)
                ? $"{binding.EndPoint.Address.AddressToDisplay()}:{binding.EndPoint.Port} ({binding.Protocol})"
                : $"{binding.Host} on {binding.EndPoint.Address.AddressToDisplay()}:{binding.EndPoint.Port} ({binding.Protocol})";
        }
    }
}
