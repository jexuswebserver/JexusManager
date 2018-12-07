// Copyright (c) Jonathan Wood. All rights reserved.
// 
// Use of this article and any related source code or other files is governed by the terms and conditions of The Code Project Open License.
// http://www.blackbeltcoder.com/Articles/net/implementing-vbs-like-operator-in-c

using System.Collections.Generic;

namespace JexusManager
{
    static class StringCompareExtensions
    {
        /// <summary>
        /// Implement's VB's Like operator logic.
        /// </summary>
        public static bool IsLike(this string s, string pattern)
        {
            // Characters matched so far
            int matched = 0;

            // Loop through pattern string
            for (int i = 0; i < pattern.Length;)
            {
                // Check for end of string
                if (matched > s.Length)
                    return false;

                // Get next pattern character
                char c = pattern[i++];
                if (c == '[') // Character list
                {
                    // Test for exclude character
                    bool exclude = (i < pattern.Length && pattern[i] == '!');
                    if (exclude)
                        i++;
                    // Build character list
                    int j = pattern.IndexOf(']', i);
                    if (j < 0)
                        j = s.Length;
                    HashSet<char> charList = CharListToSet(pattern.Substring(i, j - i));
                    i = j + 1;

                    if (charList.Contains(s[matched]) == exclude)
                        return false;
                    matched++;
                }
                else if (c == '?') // Any single character
                {
                    matched++;
                }
                else if (c == '#') // Any single digit
                {
                    if (!char.IsDigit(s[matched]))
                        return false;
                    matched++;
                }
                else if (c == '*') // Zero or more characters
                {
                    if (i < pattern.Length)
                    {
                        // Matches all characters until
                        // next character in pattern
                        char next = pattern[i];
                        int j = s.IndexOf(next, matched);
                        if (j < 0)
                            return false;
                        matched = j;
                    }
                    else
                    {
                        // Matches all remaining characters
                        matched = s.Length;
                        break;
                    }
                }
                else // Exact character
                {
                    if (matched >= s.Length || c != s[matched])
                        return false;
                    matched++;
                }
            }
            // Return true if all characters matched
            return (matched == s.Length);
        }

        /// <summary>
        /// Converts a string of characters to a HashSet of characters. If the string
        /// contains character ranges, such as A-Z, all characters in the range are
        /// also added to the returned set of characters.
        /// </summary>
        /// <param name="charList">Character list string</param>
        private static HashSet<char> CharListToSet(string charList)
        {
            HashSet<char> set = new HashSet<char>();

            for (int i = 0; i < charList.Length; i++)
            {
                if ((i + 1) < charList.Length && charList[i + 1] == '-')
                {
                    // Character range
                    char startChar = charList[i++];
                    i++; // Hyphen
                    char endChar = (char)0;
                    if (i < charList.Length)
                        endChar = charList[i++];
                    for (int j = startChar; j <= endChar; j++)
                        set.Add((char)j);
                }
                else set.Add(charList[i]);
            }
            return set;
        }
    }
}
