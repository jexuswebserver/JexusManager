// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
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
        private TextBox _editBox;
        private ListView _listView;
        private ListViewItem? _lastSelectedItem = null;

        protected FeatureBase(Module module)
        {
            Module = module;
            Items = new List<T>();
        }

        public List<T> Items { get; set; }
        public T SelectedItem { get; set; }

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

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager?.GetString("General"), "", false);
        }

        protected abstract void OnSettingsSaved();

        public virtual void LoadItems()
        {
            Items.Clear();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var collection = GetCollection(service, item);
            item.AppendTo(collection);
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        private ConfigurationElementCollection GetCollection(IConfigurationService service, T item)
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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            item.Element = collection.CreateElement();
            item.Apply();
            collection.AddAt(index, item.Element);
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void EditItem(T newItem)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
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
            Items.Remove(SelectedItem);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ConfigurationElementCollection collection = GetCollection(service, SelectedItem);
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
            int index = Items.IndexOf(SelectedItem);
            Items.Remove(SelectedItem);
            Items.Insert(index - 1, SelectedItem);

            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index - 1, element);

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void MoveDownItem()
        {
            int index = Items.IndexOf(SelectedItem);
            Items.Remove(SelectedItem);
            Items.Insert(index + 1, SelectedItem);

            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index + 1, element);

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public virtual void RevertItems()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            collection.Revert();

            service.ServerManager.CommitChanges();
            SelectedItem = null;
            LoadItems();
        }

        protected virtual ConfigurationElementCollection GetSecondaryCollection(IConfigurationService service)
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

        public void HandleMouseClick(ListView listView1, Action<T, string> updatePropertyAction)
        {
            _editBox = new TextBox
            {
                Visible = false
            };
            listView1.Controls.Add(_editBox);
            _listView = listView1;

            listView1.MouseClick += (s, e) =>
            {
                var info = listView1.HitTest(e.Location);
                if (info?.Item == null)
                {
                    return;
                }

                var clickedItem = info.Item;
                if (_lastSelectedItem == clickedItem)
                {
                    BeginEditAt(info);
                }

                _lastSelectedItem = clickedItem;
            };

            _editBox.Leave += (s, e) => CommitEdit(updatePropertyAction);
            _editBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    CommitEdit(updatePropertyAction);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    _editBox.Visible = false;
                }
            };
        }

        public void RenameInline(T item)
        {
            var index = Items.IndexOf(SelectedItem);
            if (index < 0)
            {
                return;
            }

            ListViewItem row = _listView.Items[index];
            _listView.Focus();
            row.Selected = true;
            row.Focused = true;

            Rectangle bounds = row.Bounds;
            Point clickPoint = new Point(bounds.Left + 2, bounds.Top + 2);
            var hitTest = _listView.HitTest(clickPoint);
            BeginEditAt(hitTest);
        }

        public void BeginEditAt(ListViewHitTestInfo hitTest)
        {
            if (hitTest.Item == null)
            {
                return;
            }

            if (hitTest.SubItem == null)
            {
                return;
            }

            var itemBounds = hitTest.Item.Bounds;
            var columnWidth = hitTest.Item.ListView.Columns[0].Width;

            var editBounds = new Rectangle(itemBounds.Left, itemBounds.Top, columnWidth, itemBounds.Height);

            _editBox.Bounds = editBounds;
            _editBox.Text = hitTest.Item.Text;
            _editBox.Tag = hitTest; // Store the item info in the tag
            _editBox.Visible = true;
            _editBox.Focus();
            _editBox.SelectAll();
        }

        private void CommitEdit(Action<T, string> updatePropertyAction)
        {
            if (_editBox.Visible && _editBox.Tag is ListViewHitTestInfo info)
            {
                info.Item.Text = _editBox.Text;
                updatePropertyAction?.Invoke(SelectedItem, _editBox.Text);
            }

            _editBox.Visible = false;
        }
        #endregion
    }
}
