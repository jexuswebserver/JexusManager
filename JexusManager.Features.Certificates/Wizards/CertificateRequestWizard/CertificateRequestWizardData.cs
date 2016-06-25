// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRequestWizard
{
    internal class CertificateRequestWizardData
    {
        public string CommonName;
        public string Organization { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Provider = 1;
        public int Length { get; set; }
        public string FileName { get; set; }

        public bool IsComplete
        {
            get { return !string.IsNullOrWhiteSpace(FileName); }
        }

        public bool UseIisStyle { get; set; }
    }
}
