// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client.Win32;
using System;

namespace JexusManager
{
    public abstract class DefaultWizardForm : WizardForm
    {
        public DefaultWizardForm(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public new void UpdateWizard()
        {
            base.UpdateWizard();
        }
    }
}
