// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Web.Management.Client
{
    public interface ICertificateVerificationBuilder
    {
        bool VerifyCertificate(
            IServiceProvider serviceProvider,
            X509Certificate2 certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors
            );
    }
}
