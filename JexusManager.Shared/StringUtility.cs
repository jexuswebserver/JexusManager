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
    }
}
