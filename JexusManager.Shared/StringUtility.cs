// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JexusManager
{
    public static class StringUtility
    {
        public static string Combine(this IEnumerable<string> item, string connector)
        {
            var preConditions = item.ToList();
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

        public static bool IsWildcard(this string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }

            if (host == "*")
            {
                return true;
            }
            
            var index = host.IndexOf('.');
            if (index != 1)
            {
                return false;
            }

            return host[0] == '*' && host.IndexOf('*', 1) == -1;
        }

        public static bool IsValidHost(this string host, bool supportsWildcard = false)
        {
            return supportsWildcard && host.IsWildcard() ? host.TrimStart('*', '.').IsValidBody() : host.IsValidBody();
        }

        private static bool IsValidBody(this string host)
        {
            var invalid = "\"/\\[]:|<>+=;,?*$%#@{}^`".ToCharArray();
            foreach (var ch in invalid)
            {
                if (host.Contains(ch))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
