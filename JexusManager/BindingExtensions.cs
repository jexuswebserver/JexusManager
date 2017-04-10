// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public static class BindingExtensions
    {
        public static string ToShortString(this Binding binding)
        {
            return string.IsNullOrEmpty(binding.Host)
                ? string.Format("{0}:{1} ({2})",
                    binding.EndPoint.Address.AddressToDisplay(),
                    binding.EndPoint.Port,
                    binding.Protocol)
                : string.Format("{3} on {0}:{1} ({2})",
                    binding.EndPoint.Address.AddressToDisplay(),
                    binding.EndPoint.Port,
                    binding.Protocol,
                    binding.Host);
        }
    }
}
