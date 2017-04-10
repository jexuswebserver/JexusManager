// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;
using System.Net.Sockets;

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
                    : string.Format("[{0}]", address);
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
    }
}
