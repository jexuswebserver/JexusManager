// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Certificates
{
    internal sealed class CertificatesModuleProxy : ModuleServiceProxy
    {
        internal CertificatesItem[] GetCertificates()
        {
            return (CertificatesItem[])Invoke(nameof(GetCertificates));
        }

        internal bool InstallFromFile(string fileName, string password, string friendlyName, string store)
        {
            return (bool)Invoke(nameof(InstallFromFile), fileName, password, friendlyName, store);
        }

        internal bool Delete(string thumbprint, string store)
        {
            return (bool)Invoke(nameof(Delete), thumbprint, store);
        }

        internal void Trust(string thumbprint, string store)
        {
            Invoke(nameof(Trust), thumbprint, store);
        }
    }
}
