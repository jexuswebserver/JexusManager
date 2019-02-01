// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    internal sealed class NavigationService : INavigationService
    {
        public NavigationService(MainForm host)
        {
            _host = host;
        }
        #region INavigationService

        private readonly List<NavigationItem> _items = new List<NavigationItem>();
        private readonly MainForm _host;

        public bool Navigate(Connection connection, ManagementConfigurationPath configurationPath, Type pageType, object navigationData)
        {
            var item = new NavigationItem(connection, configurationPath, pageType, navigationData);
            return NavigateToItem(item, true);
        }

        internal bool NavigateToItem(NavigationItem item, bool initializing)
        {
            var previousItem = this.CurrentItem;
            var previous = previousItem?.Page;
            if (previous != null && previous.HasChanges)
            {
                var basic = (ModulePage)previous;
                string msg = "The changes you have made will be lost. Do you want to save changes?";
                if (
                    _host.UIService.ShowMessage(
                        msg,
                        basic.Text,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button3)
                        == DialogResult.Yes)
                {
                    var dialog = previous as ModuleDialogPage;
                    if (dialog != null)
                    {
                        if (!dialog.ApplyChanges())
                        {
                            return false;
                        }
                    }
                }
            }

            var current = _items.IndexOf(CurrentItem);
            _items.RemoveRange(current + 1, _items.Count - 1 - current);
            _items.Add(item);
            CurrentItem = item;
            var page = CurrentItem.Page;
            if (typeof(IModuleChildPage).IsAssignableFrom(item.PageType))
            {
                var pageInfo = previous?.PageInfo;
                if (initializing)
                {
                    page.Initialize(pageInfo?.AssociatedModule, pageInfo, item.NavigationData);
                }

                var child = (IModuleChildPage)page;
                child.ParentPage = previous;
            }

            _host.LoadInner(page);
            OnNavigationPerformed(new NavigationEventArgs(this.CurrentItem, previousItem, initializing));
            return true;
        }

        public bool NavigateBack(int steps)
        {
            var current = _items.IndexOf(CurrentItem);
            if (steps > current)
            {
                throw new ArgumentOutOfRangeException();
            }

            CurrentItem = _items[current - steps];
            return NavigateToItem(CurrentItem, false);
        }

        public bool NavigateForward()
        {
            var current = _items.IndexOf(CurrentItem);
            if (current == _items.Count - 1)
            {
                return false;
            }

            CurrentItem = _items[current + 1];
            return NavigateToItem(CurrentItem, false);
        }

        public bool CanNavigateBack
        {
            get { return _items.IndexOf(CurrentItem) > 0; }
        }

        public bool CanNavigateForward
        {
            get { return _items.IndexOf(CurrentItem) < _items.Count - 1; }
        }

        public NavigationItem CurrentItem { get; private set; }

        public ReadOnlyCollection<NavigationItem> History
        {
            get { return new ReadOnlyCollection<NavigationItem>(_items); }
        }

        public event NavigationEventHandler NavigationPerformed;
        #endregion

        private void OnNavigationPerformed(NavigationEventArgs e)
        {
            this.NavigationPerformed?.Invoke(this, e);
        }
    }
}
