// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Web.Management.Client
{
    public interface IControlPanel
    {
        ReadOnlyCollection<ControlPanelCategoryInfo> GetCategories(string categorization);

        ControlPanelCategoryInfo GetCategory(string categorization, ModulePageInfo pageInfo);

        ModulePageInfo GetPage(Type pageType);

        ReadOnlyCollection<ModulePageInfo> GetPages(Module module);

        ReadOnlyCollection<ModulePageInfo> GetPages(string categorization, string categoryName);

        void RegisterCategory(ControlPanelCategoryInfo categoryInfo);

        void RegisterHomepage(ModulePageInfo homepageInfo);

        void RegisterPage(ModulePageInfo itemPageInfo);

        void RegisterPage(string categoryName, ModulePageInfo itemPageInfo);

        ReadOnlyCollection<ControlPanelCategorization> Categorizations { get; }
        ModulePageInfo ControlPanelPage { get; }
        ReadOnlyCollection<ModulePageInfo> Pages { get; }
    }
}