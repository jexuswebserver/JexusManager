// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Web;

namespace Microsoft.Web.Administration
{
    public static class WebConfigurationManager
    {
        public static ConfigurationSection GetSection(string sectionPath)
        {
            return null;
        }

#if !NET7_0
        public static ConfigurationSection GetSection(HttpContext context, string sectionPath)
        {
            return null;
        }
#endif
        public static ConfigurationSection GetSection(string siteName, string virtualPath, string sectionPath)
        {
            return null;
        }
#if !NET7_0
        public static ConfigurationSection GetSection(HttpContext context, string sectionPath, Type sectionType)
        {
            return null;
        }
#endif
        public static ConfigurationSection GetSection(string siteName, string virtualPath, string sectionPath, Type sectionType)
        {
            return null;
        }
    }
}
