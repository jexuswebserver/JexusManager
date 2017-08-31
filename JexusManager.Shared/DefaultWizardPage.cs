// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client.Win32;

namespace JexusManager
{
#if DESIGN
    public class DefaultWizardPage : WizardPage
#else
    public abstract class DefaultWizardPage : WizardPage
#endif
    {
        public new void SetNextPage(WizardPage page)
        {
            base.SetNextPage(page);
        }

        public new void SetPreviousPage(WizardPage page)
        {
            base.SetPreviousPage(page);
        }

        public new void SetWizard(WizardForm wizard)
        {
            base.SetWizard(wizard);
        }
    }
}
