// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

    using Microsoft.Web.Administration;

    internal class IdentityEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var editorService = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService != null)
            {
                ApplicationPoolProcessModel element = (ApplicationPoolProcessModel)value;
                using (IdentityDialog dialog = new IdentityDialog(null, element))
                {
                    editorService.ShowDialog(dialog);
                }
            }

            return value;
        }
    }
}
