// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Web;

namespace Microsoft.Web.Management.Server
{
#if !NET6_0
    public sealed class WebManagementServiceHandler : IHttpHandler
    {
        bool IHttpHandler.IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
#endif
}
