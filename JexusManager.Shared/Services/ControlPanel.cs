// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Microsoft.Web.Management.Client;

    public sealed class ControlPanel : IControlPanel
    {
        #region IControlPanel
        private readonly List<ModulePageInfo> _pages = new List<ModulePageInfo>();

        public ReadOnlyCollection<ControlPanelCategoryInfo> GetCategories(string categorization)
        {
            return new ReadOnlyCollection<ControlPanelCategoryInfo>(new List<ControlPanelCategoryInfo>());
        }

        public ControlPanelCategoryInfo GetCategory(string categorization, ModulePageInfo pageInfo)
        {
            return null;
        }

        public ModulePageInfo GetPage(Type pageType)
        {
            return null;
        }

        public ReadOnlyCollection<ModulePageInfo> GetPages(Module module)
        {
            return new ReadOnlyCollection<ModulePageInfo>(new List<ModulePageInfo>());
        }

        public ReadOnlyCollection<ModulePageInfo> GetPages(string categorization, string categoryName)
        {
            return new ReadOnlyCollection<ModulePageInfo>(new List<ModulePageInfo>());
        }

        public void RegisterCategory(ControlPanelCategoryInfo categoryInfo)
        {
        }

        public void RegisterHomepage(ModulePageInfo homepageInfo)
        {
        }

        public void RegisterPage(ModulePageInfo itemPageInfo)
        {
            _pages.Add(itemPageInfo);
        }

        public void RegisterPage(string categoryName, ModulePageInfo itemPageInfo)
        {
        }

        public ReadOnlyCollection<ControlPanelCategorization> Categorizations
        {
            get { return null; }
        }

        public ModulePageInfo ControlPanelPage
        {
            get { return null; }
        }

        public ReadOnlyCollection<ModulePageInfo> Pages
        {
            get { return new ReadOnlyCollection<ModulePageInfo>(_pages); }
        }
        #endregion
    }
}
