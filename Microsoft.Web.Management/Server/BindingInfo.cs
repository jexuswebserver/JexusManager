// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.ApplicationHost;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public class BindingInfo
    {
        public BindingInfo(
            ISiteBinding binding
            )
        { }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public string BindingInformation { get; }
        public string HostName { get; }
        public string IPAddress { get; }
        public string Port { get; }
        public string Protocol { get; }
        public SslFlags SslFlags { get; }
    }
}
