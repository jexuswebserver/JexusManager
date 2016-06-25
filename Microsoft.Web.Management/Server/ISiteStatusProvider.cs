// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public interface ISiteStatusProvider
    {
        ObjectState GetSiteStatus(
            Site site,
            out string errorMessage
            );

        IEnumerable<string> SupportedProtocols { get; }
    }
}