// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Microsoft.Web.Administration
{
    internal static class StringExtensions
    {
        public const string AnyHostName = "*";

        public const string AllUnassigned = "All Unassigned";

        public static string HostToDisplay(this string text)
        {
            return text == AnyHostName ? string.Empty : text;
        }

        public static string DisplayToHost(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? AnyHostName : text;
        }

        public static string AddressToDisplay(this IPAddress address)
        {
            return address.Equals(IPAddress.Any)
                ? AnyHostName
                : address.AddressFamily == AddressFamily.InterNetwork
                    ? address.ToString()
                    : $"[{address}]";
        }

        public static IPAddress DisplayToAddress(this string text)
        {
            return text == AnyHostName || text == string.Empty ? IPAddress.Any : IPAddress.Parse(text);
        }

        public static string AddressToCombo(this IPAddress address)
        {
            return address.Equals(IPAddress.Any) ? AllUnassigned : address.ToString();
        }

        public static IPAddress ComboToAddress(this string text)
        {
            return text == AllUnassigned ? IPAddress.Any : IPAddress.Parse(text);
        }

        public static string ElementTagNameToName(this string tag)
        {
            return tag.Contains("/") ? tag.Substring(tag.LastIndexOf('/') + 1) : tag;
        }

        public static string Combine(IEnumerable<string> items, string connector)
        {
            var preConditions = items.ToList();
            if (preConditions.Count == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder(preConditions[0]);
            for (int index = 1; index < preConditions.Count; index++)
            {
                result.Append(connector).Append(preConditions[index]);
            }

            return result.ToString();
        }
    }
}
