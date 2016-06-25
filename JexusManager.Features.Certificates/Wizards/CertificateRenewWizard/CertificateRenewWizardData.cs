// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    internal class CertificateRenewWizardData
    {
        public string FileName { get; set; }

        public bool IsComplete
        {
            get { return !string.IsNullOrWhiteSpace(FileName); }
        }

        public bool UseIisStyle { get; set; }
    }
}
