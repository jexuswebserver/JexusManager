// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Microsoft.Web.Management.Client;

    public sealed class ControlPanel : IControlPanel
    {
        #region IControlPanel
        private readonly List<ModulePageInfo> _pages = new List<ModulePageInfo>();
        private readonly HashSet<ModulePageInfo> _categoryPages = new HashSet<ModulePageInfo>();

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
            return _pages.FirstOrDefault(page => page.PageType == pageType);
        }

        public ReadOnlyCollection<ModulePageInfo> GetPages(Module module)
        {
            return new ReadOnlyCollection<ModulePageInfo>(_pages.Where(page => page.AssociatedModule == module).ToList());
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
            _pages.Add(itemPageInfo);
            _categoryPages.Add(itemPageInfo);
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
            get
            {
                // IIS Manager shows one feature icon per module. The module's home page is the
                // page registered through a category; otherwise the first registered page wins.
                var firsts = new List<ModulePageInfo>();
                var homes = new Dictionary<Module, ModulePageInfo>();
                var seen = new HashSet<Module>();
                foreach (var page in _pages)
                {
                    if (seen.Add(page.AssociatedModule))
                    {
                        firsts.Add(page);
                    }

                    if (_categoryPages.Contains(page))
                    {
                        homes[page.AssociatedModule] = page;
                    }
                }

                var result = firsts.Select(page => homes.TryGetValue(page.AssociatedModule, out var home) ? home : page).ToList();
                return new ReadOnlyCollection<ModulePageInfo>(result);
            }
        }
        #endregion
    }
}
