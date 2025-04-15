// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;

namespace JexusManager
{
    public interface IMainForm
    {
        IManagementUIService UIService { get; }

        void AddSiteNode(Site newSite);
        void EndProgress();
        void LoadInner(IModulePage page);
        void LoadPage(IModulePage page);
        void LoadPageAndSelectNode(IModulePage page, object navigationData); // TODO: delete this ultimately
        void LoadPools();
        void LoadSites();
        void RemoveSiteNode(Site selectedItem);
        void BeginProgress();
    }
}
