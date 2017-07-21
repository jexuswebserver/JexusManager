// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRequestWizard
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Web.Management.Client.Win32;

    public partial class PropertiesPage : DefaultWizardPage
    {
        public PropertiesPage()
        {
            InitializeComponent();
            Caption = "Distinguished Name Properties";
        }

        protected override bool CanNavigateNext
        {
            get
            {
                return base.CanNavigateNext
                  && !string.IsNullOrWhiteSpace(txtCommonName.Text)
                  && !string.IsNullOrWhiteSpace(txtOrganization.Text)
                  && !string.IsNullOrWhiteSpace(txtUnit.Text)
                  && !string.IsNullOrWhiteSpace(txtCity.Text)
                  && !string.IsNullOrWhiteSpace(txtState.Text)
                  && !string.IsNullOrWhiteSpace(cbCountry.Text);
            }
        }

        private void txtCommonName_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.CommonName = txtCommonName.Text;
            UpdateWizard();
        }

        protected override void Activate()
        {
            base.Activate();
            txtCommonName.SelectAll();
            txtCommonName.Focus();
        }

        private void txtOrganization_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.Organization = txtOrganization.Text;
            UpdateWizard();
        }

        private void txtUnit_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.Unit = txtUnit.Text;
            UpdateWizard();
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.City = txtCity.Text;
            UpdateWizard();
        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.State = txtState.Text;
            UpdateWizard();
        }

        private void cbCountry_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((CertificateRequestWizardData)WizardData);
            wizardData.Country = cbCountry.Text;
            UpdateWizard();
        }

        private void PropertiesPage_Load(object sender, EventArgs e)
        {
            var result = new HashSet<string>();
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (CultureInfo cul in cinfo)
            {
                RegionInfo ri;
                try
                {
                    ri = new RegionInfo(cul.Name);
                    result.Add(ri.TwoLetterISORegionName);
                }
                catch
                {
                }
            }

            var codes = result.ToList();
            codes.Sort();

            foreach (var code in codes)
            {
                cbCountry.Items.Add(code);
            }

            cbCountry.Text = "US";
        }
    }
}
