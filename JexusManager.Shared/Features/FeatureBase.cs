// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    public abstract class FeatureBase<T>
        where T : class, IItem<T>
    {
        private ListView? _listView;
        private ListViewItem? _lastSelectedItem = null;

        protected FeatureBase(Module module)
        {
            Module = module;
            Items = [];
        }

        public List<T> Items { get; set; }
        public T? SelectedItem { get; set; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public Module Module { get; }

        public bool CanMoveUp
        {
            get
            {
                return SelectedItem != null && Items.IndexOf(SelectedItem) > 0;
            }
        }
        public bool CanMoveDown
        {
            get
            {
                return SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1;
            }
        }

        protected object? GetService(Type type)
        {
            return (Module as IServiceProvider)?.GetService(type);
        }

        protected void DisplayErrorMessage(Exception ex, ResourceManager? resourceManager)
        {
            var service = GetService(typeof(IManagementUIService)) as IManagementUIService;
            service?.ShowError(ex, resourceManager?.GetString("General"), "", false);
        }

        protected abstract void OnSettingsSaved();

        #region Event handlers
        protected virtual void DoubleClick(T item)
        { }

        public void HandleMouseDoubleClick(ListView listView)
        {
            if (listView.SelectedItems.Count == 0)
            {
                return;
            }

            var item = (IFeatureListViewItem<T>)listView.SelectedItems[0];
            DoubleClick(item.Item);
        }

        public void HandleSelectedIndexChanged(ListView listView)
        {
            if (listView.SelectedItems.Count > 0)
            {
                var item = (IFeatureListViewItem<T>)listView.SelectedItems[0];
                SelectedItem = item.Item;
            }
            else
            {
                _lastSelectedItem = null;
                SelectedItem = null;
            }
        }

        public void InitializeMouseClick(ListView listView1, Action<T, string> updatePropertyAction, Func<string, bool> validateAction)
        {
            _listView = listView1;
            listView1.LabelEdit = true;
            listView1.AfterLabelEdit += (s, e) =>
            {
                if (string.IsNullOrEmpty(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }

                if (validateAction != null && !validateAction(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }

                if (SelectedItem == null)
                {
                    e.CancelEdit = true;
                    return;
                }

                updatePropertyAction(SelectedItem, e.Label);
            };

            listView1.MouseClick += (s, e) =>
            {
                var info = listView1.HitTest(e.Location);
                if (info?.Item == null)
                    return;

                if (_lastSelectedItem == info.Item)
                {
                    info.Item.BeginEdit();
                }

                _lastSelectedItem = info.Item;
            };
        }

        public void RenameInline(T item)
        {
            var index = Items.IndexOf(item);
            if (_listView == null)
            {
                return;
            }

            if (index >= 0 && index < _listView.Items.Count)
            {
                _listView.Items[index].BeginEdit();
            }
        }

        public bool FindDuplicate(Func<T, string> value, string text)
        {
            return Items.Where(item => item != SelectedItem)
                .Any(item => string.Equals(value(item), text, StringComparison.Ordinal));
        }

        public virtual string GetGroupKey(ListViewItem item, string selectedGroup)
        {
            return "Unknown";
        }

        public virtual void InitializeGrouping(ToolStripComboBox cbGroup)
        { }

        public void InitializeColumnClick(ListView listView)
        {
            var hook = new ColumnClickHook();
            hook.HandleColumnClick(listView);
        }
        #endregion
    }
}
