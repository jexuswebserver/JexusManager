// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager
{
    using System;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client;

    public class ModulePageInfoListViewItem : ListViewItem
    {
        private readonly ModulePageInfo _info;

        public ModulePageInfoListViewItem(ModulePageInfo info)
        {
            _info = info;
            Text = info.Title;
            ToolTipText = info.Description;
        }

        public IModulePage Page
        {
            get
            {
                var page = (IModulePage)Activator.CreateInstance(_info.PageType);
                page.Initialize(_info.AssociatedModule, _info, null);
                return page;
            }
        }
    }
}
