// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;
    using System.Drawing;

    using JexusManager.Properties;

    using Microsoft.Web.Management.Server;

    public static class ScopeExtensions
    {
        public static Image GetImage(this ManagementScope scope)
        {
            switch (scope)
            {
                case ManagementScope.Application:
                    return Resources.application_32;
                case ManagementScope.Server:
                    return Resources.title_32;
                case ManagementScope.Site:
                    return Resources.site_32;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
