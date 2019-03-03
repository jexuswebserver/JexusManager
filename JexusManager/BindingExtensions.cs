// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Windows.Forms;

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

            var value = binding.BindingInformation;
            var last = value.LastIndexOf(':');
            string host = null;
            string address = null;
            string port = null;
            if (last > 0)
            {
                host = value.Substring(last + 1);
                var next = value.LastIndexOf(':', last - 1);
                port = value.Substring(next + 1, last - next - 1);
                if (next > -1)
                {
                    address = value.Substring(0, next);
                }
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                address = "*";
            }

            return binding.EndPoint == null && string.IsNullOrWhiteSpace(port)
                ? $": ({binding.Protocol})"
                : string.IsNullOrEmpty(binding.Host)
                    ? $"{address}:{port} ({binding.Protocol})"
                    : $"{host} on {address}:{port} ({binding.Protocol})";
        }
#if !IIS
        public static ListViewItem ToListViewItem(this Binding binding)
        {
            string host = binding.Host.HostToDisplay();
            string port = binding.EndPoint?.Port.ToString(CultureInfo.InvariantCulture);
            string address = binding.EndPoint?.Address.AddressToDisplay();
            if (binding.EndPoint == null)
            {
                var value = binding.BindingInformation;
                var last = value.LastIndexOf(':');
                if (last > 0)
                {
                    host = value.Substring(last + 1);
                    var next = value.LastIndexOf(':', last - 1);
                    port = value.Substring(next + 1, last - next - 1);
                    if (next > -1)
                    {
                        address = value.Substring(0, next);
                    }
                }
            }

            return new ListViewItem(new[]
                {
                        binding.Protocol,
                        host,
                        port,
                        address,
                        binding.CanBrowse ? string.Empty : binding.BindingInformation
                })
                { Tag = binding };
        }
#endif
    }
}
