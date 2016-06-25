// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager
{
    internal static class StringFormatter
    {
        public static string RuntimeVersionToDisplay(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "No Managed Code" : text.TrimStart('v');
        }

        public static string RuntimeVersionToDisplay2(this string text)
        {
            return string.IsNullOrWhiteSpace(text) ? "No Managed Code" : text;
        }
    }
}
