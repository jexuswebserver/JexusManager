// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using JexusManager.Services;

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
                return SelectedItem != null && Items.IndexOf(SelectedItem) > 0
                       && Items.All(item => item.Element.IsLocked == "false" && item.Element.LockAttributes.Count == 0);
            }
        }
        public bool CanMoveDown
        {
            get
            {
                return SelectedItem != null && Items.IndexOf(SelectedItem) < Items.Count - 1
                       && Items.All(item => item.Element.IsLocked == "false" && item.Element.LockAttributes.Count == 0);
            }
        }

        protected abstract ConfigurationElementCollection GetCollection(IConfigurationService service);

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

        public virtual void LoadItems()
        {
            Items.Clear();
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            ConfigurationElementCollection collection = GetCollection(service);
            foreach (ConfigurationElement addElement in collection)
            {
                var type = typeof(T);
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var constructorInfo = constructors[0];
                var item =
                    (T)
                    constructorInfo.Invoke(
                        constructorInfo.GetParameters().Length == 1
                            ? new object[] { addElement }
                            : [addElement, true]);
                Items.Add(item);
            }

            var secondary = GetSecondaryCollection(service);
            if (secondary != null)
            {
                foreach (ConfigurationElement addElement in secondary)
                {
                    var type = typeof(T);
                    var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    var constructorInfo = constructors[0];
                    var item =
                        (T)
                        constructorInfo.Invoke(
                            constructorInfo.GetParameters().Length == 1
                                ? new object[] { addElement }
                                : [addElement, false]);
                    Items.Add(item);
                }
            }

            OnSettingsSaved();
        }

        public virtual void AddItem(T item)
        {
            Items.Add(item);
            SelectedItem = item;
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service, item);
            if (collection == null)
            {
                throw new InvalidOperationException("ConfigurationElementCollection is not available.");
            }

            item.AppendTo(collection);
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        private ConfigurationElementCollection? GetCollection(IConfigurationService service, T item)
        {
            var duo = item as IDuoItem<T>;
            if (duo == null || duo.Allowed)
            {
                return GetCollection(service);
            }

            return GetSecondaryCollection(service);
        }

        public virtual void InsertItem(int index, T item)
        {
            Items.Insert(index, item);
            SelectedItem = item;
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service);
            item.Element = collection.CreateElement();
            item.Apply();
            collection.AddAt(index, item.Element);
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void EditItem(T newItem)
        {
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            if (newItem.Flag != "Local")
            {
                ConfigurationElementCollection collection = GetCollection(service);
                collection.Remove(newItem.Element);
                newItem.AppendTo(collection);
                newItem.Flag = "Local";
            }
            else
            {
                newItem.Apply();
            }

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void RemoveItem()
        {
            if (SelectedItem == null)
            {
                return;
            }

            Items.Remove(SelectedItem);
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service, SelectedItem);
            if (collection == null)
            {
                throw new InvalidOperationException("ConfigurationElementCollection is not available.");
            }

            try
            {
                collection.Remove(SelectedItem.Element);
            }
            catch (FileLoadException ex)
            {
                DisplayErrorMessage(ex, null);
                return;
            }

            service.ServerManager.CommitChanges();

            SelectedItem = default;
            OnSettingsSaved();
        }

        public virtual void MoveUpItem()
        {
            if (SelectedItem == null)
            {
                return;
            }

            int index = Items.IndexOf(SelectedItem);
            Items.Remove(SelectedItem);
            Items.Insert(index - 1, SelectedItem);

            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index - 1, element);

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void MoveDownItem()
        {
            if (SelectedItem == null)
            {
                return;
            }

            int index = Items.IndexOf(SelectedItem);
            Items.Remove(SelectedItem);
            Items.Insert(index + 1, SelectedItem);

            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index + 1, element);

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void RevertItems()
        {
            if (GetService(typeof(IConfigurationService)) is not IConfigurationService service)
            {
                throw new InvalidOperationException("IConfigurationService is not available.");
            }

            var collection = GetCollection(service);
            collection.Revert();

            service.ServerManager.CommitChanges();
            SelectedItem = null;
            LoadItems();
        }

        protected virtual ConfigurationElementCollection? GetSecondaryCollection(IConfigurationService service)
        {
            return null;
        }

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
            // TODO: seem to be duplicate to the Match pattern. 
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
