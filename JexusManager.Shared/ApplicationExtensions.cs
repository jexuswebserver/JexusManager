// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using System.Collections.Generic;

namespace JexusManager
{
    public static class ApplicationExtensions
    {
        public static Site GetSite(this Application application)
        {
            return application.Site;
        }

        public static IDictionary<string, List<string>> GetExtra(this Application application)
        {
            return application.Extra;
        }
    }
}
