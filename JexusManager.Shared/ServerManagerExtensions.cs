// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using System.Collections.Generic;

namespace JexusManager
{
    public static class ServerManagerExtensions
    {
        public static IDictionary<string, List<string>> GetExtra(this ServerManager server)
        {
            return server.Extra;
        }
    }
}
