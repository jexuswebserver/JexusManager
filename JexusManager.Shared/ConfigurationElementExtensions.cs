// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;

namespace JexusManager
{
    public static class ConfigurationElementExtensions
    {
        public static string GetIsLocked(this ConfigurationElement element)
        {
            return element.IsLocked;
        }

        public static void SetIsLocked(this ConfigurationElement element, string value)
        {
            element.IsLocked = value;
        }
    }
}
