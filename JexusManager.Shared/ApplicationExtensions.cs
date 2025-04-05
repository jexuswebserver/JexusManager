// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using System.Collections.Generic;
using System.Linq;

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

        public static string GetPoolName(this Application application)
        {
            // TODO: Windows Server 2025 seems to use v4.0 as default and override the schema.
            var pool = application.Server.ApplicationPools.First(item => item.Name == application.ApplicationPoolName);
            return $"{pool.Name} ({pool.ManagedRuntimeVersion})";
        }
    }
}
