// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Forms;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    internal sealed class NavigationService : INavigationService
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("NavigationService");

        public NavigationService(MainForm host)
        {
            _host = host;
            _logger.LogInformation("NavigationService initialized");
        }
        #region INavigationService

        private readonly List<NavigationItem> _items = new List<NavigationItem>();
        private readonly MainForm _host;

        public bool Navigate(Connection connection, ManagementConfigurationPath configurationPath, Type pageType, object navigationData)
        {
            _logger.LogDebug("Navigate requested to page type: {PageType}", pageType.Name);
            var item = new NavigationItem(connection, configurationPath, pageType, navigationData);
            return NavigateToItem(item, true);
        }

        internal bool NavigateToItem(NavigationItem item, bool initializing)
        {
            _logger.LogDebug("NavigateToItem - item: {PageType}, initializing: {IsInitializing}",
                item.PageType.Name, initializing);

            var previousItem = this.CurrentItem;
            var previous = previousItem?.Page;
            if (previous != null && previous.HasChanges)
            {
                var basic = (ModulePage)previous;
                string msg = "The changes you have made will be lost. Do you want to save changes?";
                _logger.LogDebug("Page has unsaved changes: {PageName}", basic.Text);

                if (
                    _host.UIService.ShowMessage(
                        msg,
                        basic.Text,
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button3)
                        == DialogResult.Yes)
                {
                    _logger.LogDebug("User chose to save changes");
                    var dialog = previous as ModuleDialogPage;
                    if (dialog != null)
                    {
                        if (!dialog.ApplyChanges())
                        {
                            _logger.LogDebug("Failed to apply changes, cancelling navigation");
                            return false;
                        }
                    }
                }
                else if (_host.UIService.ShowMessage(msg, basic.Text, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3) == DialogResult.Cancel)
                {
                    // User cancelled navigation, so don't continue
                    _logger.LogDebug("User cancelled navigation");
                    return false;
                }
            }

            // The critical bug is here - we're manipulating the navigation history incorrectly
            // when navigating to a new item vs. back/forward navigation
            if (initializing)
            {
                // For new navigation, remove forward history and add the new item
                var current = _items.IndexOf(CurrentItem);
                if (current >= 0) // Only truncate if we have a current item
                {
                    _logger.LogDebug("Removing forward history from index {CurrentIndex} (current size: {HistorySize})",
                        current + 1, _items.Count);
                    _items.RemoveRange(current + 1, _items.Count - 1 - current);
                }
                _logger.LogDebug("Adding new navigation item to history: {PageType}", item.PageType.Name);
                _items.Add(item);
            }
            // For back/forward navigation, the item is already in the history,
            // so we just need to set the current item

            CurrentItem = item;
            _logger.LogDebug("Current item set to: {PageType}", item.PageType.Name);

            var page = CurrentItem.Page;
            if (typeof(IModuleChildPage).IsAssignableFrom(item.PageType))
            {
                var pageInfo = previous?.PageInfo;
                if (initializing)
                {
                    _logger.LogDebug("Initializing child page with parent: {ParentType}",
                        pageInfo?.GetType().Name ?? "null");
                    page.Initialize(pageInfo?.AssociatedModule, pageInfo, item.NavigationData);
                }

                var child = (IModuleChildPage)page;
                child.ParentPage = previous;
            }

            _host.LoadInner(page);
            OnNavigationPerformed(new NavigationEventArgs(this.CurrentItem, previousItem, initializing));

            // Log the current navigation stack
            LogNavigationHistory();

            return true;
        }

        public bool NavigateBack(int steps)
        {
            var current = _items.IndexOf(CurrentItem);
            if (steps > current)
            {
                _logger.LogWarning("Attempted to navigate back {Steps} steps but only {Available} available",
                    steps, current);
                throw new ArgumentOutOfRangeException();
            }

            _logger.LogDebug("Navigating back {Steps} steps from index {CurrentIndex}", steps, current);
            CurrentItem = _items[current - steps];
            return NavigateToItem(CurrentItem, false);
        }

        public bool NavigateForward()
        {
            var current = _items.IndexOf(CurrentItem);
            if (current == _items.Count - 1)
            {
                _logger.LogDebug("Attempted to navigate forward but already at the end of history");
                return false;
            }

            _logger.LogDebug("Navigating forward from index {CurrentIndex} to {NewIndex}",
                current, current + 1);
            CurrentItem = _items[current + 1];
            return NavigateToItem(CurrentItem, false);
        }

        public bool CanNavigateBack
        {
            get
            {
                var canNavigate = _items.IndexOf(CurrentItem) > 0;
                _logger.LogTrace("CanNavigateBack: {Result}", canNavigate);
                return canNavigate;
            }
        }

        public bool CanNavigateForward
        {
            get
            {
                var canNavigate = _items.IndexOf(CurrentItem) < _items.Count - 1;
                _logger.LogTrace("CanNavigateForward: {Result}", canNavigate);
                return canNavigate;
            }
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
            _logger.LogDebug("Navigation performed: {CurrentPageType} -> {PreviousPageType}, IsNew: {IsNew}",
                e.NewItem?.PageType.Name ?? "null",
                e.OldItem?.PageType.Name ?? "null",
                e.IsNew);

            this.NavigationPerformed?.Invoke(this, e);
        }

        private void LogNavigationHistory()
        {
            var currentIndex = _items.IndexOf(CurrentItem);
            var history = string.Join(" -> ",
                _items.Select((item, index) =>
                    $"{(index == currentIndex ? "[" : "")}" +
                    $"{item.PageType.Name}" +
                    $"{(index == currentIndex ? "]" : "")}"));

            _logger.LogDebug("Navigation history (current index: {CurrentIndex}): {History}",
                currentIndex, history);
        }
    }
}
