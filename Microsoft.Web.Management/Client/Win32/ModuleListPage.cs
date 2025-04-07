// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class ModuleListPage : ModulePage
#else
    public abstract class ModuleListPage : ModulePage
#endif
    {
        protected ModuleListPage()
        { }

        protected ModuleListPage(ListPageListView listView)
        {
            ListView = listView;
        }

        protected static readonly ModuleListPageGrouping EmptyGrouping;
        private object _pageHeader;

        protected void ClearFilter()
        {
        }

        protected override void Dispose(bool disposing)
        {
            ListView = null;
            ListViewContainer = null;
        }

        protected virtual ListViewGroup[] GetGroups(ModuleListPageGrouping grouping)
        {
            return null;
        }

        protected virtual IComparer GetItemComparer(ColumnHeader sortColumn, SortOrder sortOrder)
        {
            return null;
        }

        protected void Group(ModuleListPageGrouping grouping)
        {
            ListViewGroup[] groups = null;
            if (grouping != null)
            {
                groups = GetGroups(grouping);
            }

            ListView.BeginUpdate();
            if ((grouping == null || grouping.Equals(EmptyGrouping))
                || (groups == null || groups.Length == 0))
            {
                ListView.ShowGroups = false;
            }
            else
            {
                ListView.ShowGroups = true;
                for (int i = ListView.Groups.Count - 1; i >= 0; i--)
                {
                    ListView.Groups.RemoveAt(i);
                }

                ListView.Groups.AddRange(groups);
                OnGroup(grouping);
            }

            ListView.EndUpdate();
            SelectedGrouping = grouping;
            //_pageHeader.UpdateGroupingCommands();
        }
#if DESIGN
        protected virtual void InitializeListPage()
        {
            throw new NotImplementedException();
        }
#else
        protected abstract void InitializeListPage();
#endif
        protected override void LoadPreferences(PreferencesStore store)
        { }

        protected override void OnActivated(bool initialActivation)
        { }

        protected virtual void OnClearFilter()
        { }

        protected virtual void OnGroup(ModuleListPageGrouping grouping)
        {
            throw new InvalidOperationException();
        }

        protected virtual void OnSearch(ModuleListPageSearchOptions options)
        { }

        protected virtual void OnSetView() { }

        protected void RefreshFilter() { }

        protected void RefreshSearchPanel() { }

        protected override void SavePreferences(PreferencesStore store)
        { }

        protected void SetView(ModuleListPageViewModes mode)
        {
            switch (mode)
            {
                case ModuleListPageViewModes.Details:
                    this.ListView.View = View.Details;
                    break;
                case ModuleListPageViewModes.Icons:
                    this.ListView.View = View.LargeIcon;
                    break;
                case ModuleListPageViewModes.Tiles:
                    this.ListView.View = View.Tile;
                    break;
                case ModuleListPageViewModes.List:
                    this.ListView.View = View.List;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            this.OnSetView();
        }

        protected void Sort()
        {
            this.ListView.Sort();
        }

        public void Sort(ColumnHeader column, SortOrder sortOrder)
        {
            var comparer = GetItemComparer(column, sortOrder);
            this.ListView.ListViewItemSorter = comparer;
            this.ListView.Sort();
        }

        protected virtual bool CanInstantSearch { get; }
        protected virtual bool CanSearch { get; }
        protected virtual ModuleListPageGrouping DefaultGrouping { get; }
        protected virtual ModuleListPageViewModes DefaultViewMode { get; }
        protected virtual ModuleListPageFilter Filter { get; }

        public virtual ModuleListPageGrouping[] Groupings
        {
            get
            {
                return null;
            }
        }

        protected ImageList LargeImageList
        {
            get
            {
                return this.ListView.LargeImageList;
            }
        }

        protected ListView ListView { get; private set; }
        protected Control ListViewContainer { get; private set; }
        protected virtual ModuleListPageSearchField[] SearchFields { get; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModuleListPageGrouping SelectedGrouping { get; set; }

        protected ImageList SmallImageList
        {
            get
            {
                return this.ListView.SmallImageList;
            }
        }

        public ColumnHeader SortColumn
        {
            get
            {
                // TODO:
                return this.ListView.Columns[0];
            }
        }

        public SortOrder SortOrder
        {
            get
            {
                return this.ListView.Sorting;
            }
        }

        public virtual ModuleListPageViewModes ViewModes { get; }
    }
}
