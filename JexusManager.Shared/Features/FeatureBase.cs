// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    public abstract class FeatureBase<T>
        where T : class, IItem<T>
    {
        protected FeatureBase(Module module)
        {
            this.Module = module;
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
                return this.SelectedItem != null && this.Items.IndexOf(this.SelectedItem) > 0
                       && this.Items.All(item => item.Element.IsLocked == "false" && item.Element.LockAttributes.Count == 0);
            }
        }
        public bool CanMoveDown
        {
            get
            {
                return this.SelectedItem != null && this.Items.IndexOf(this.SelectedItem) < this.Items.Count - 1
                       && this.Items.All(item => item.Element.IsLocked == "false" && item.Element.LockAttributes.Count == 0);
            }
        }

        protected abstract ConfigurationElementCollection GetCollection(IConfigurationService service);

        protected object GetService(Type type)
        {
            return (this.Module as IServiceProvider).GetService(type);
        }

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected abstract void OnSettingsSaved();

        public virtual void LoadItems()
        {
            this.Items.Clear();
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationElementCollection collection = this.GetCollection(service);
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
                            : new object[] { addElement, true });
                this.Items.Add(item);
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
                                : new object[] { addElement, false });
                    this.Items.Add(item);
                }
            }

            this.OnSettingsSaved();
        }

        public virtual void AddItem(T item)
        {
            this.Items.Add(item);
            this.SelectedItem = item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var collection = this.GetCollection(service, item);
            item.AppendTo(collection);
            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
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
            this.Items.Insert(index, item);
            this.SelectedItem = item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var collection = this.GetCollection(service);
            item.Element = collection.CreateElement();
            item.Apply();
            collection.AddAt(index, item.Element);
            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public virtual void EditItem(T newItem)
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            if (newItem.Flag != "Local")
            {
                ConfigurationElementCollection collection = this.GetCollection(service);
                collection.Remove(newItem.Element);
                newItem.AppendTo(collection);
                newItem.Flag = "Local";
            }
            else
            {
                newItem.Apply();
            }

            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public virtual void RemoveItem()
        {
            this.Items.Remove(this.SelectedItem);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationElementCollection collection = GetCollection(service, SelectedItem);
            collection.Remove(this.SelectedItem.Element);
            service.ServerManager.CommitChanges();

            this.SelectedItem = default(T);
            this.OnSettingsSaved();
        }

        public virtual void MoveUpItem()
        {
            int index = this.Items.IndexOf(this.SelectedItem);
            this.Items.Remove(this.SelectedItem);
            this.Items.Insert(index - 1, this.SelectedItem);

            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index - 1, element);

            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public virtual void MoveDownItem()
        {
            int index = this.Items.IndexOf(this.SelectedItem);
            this.Items.Remove(this.SelectedItem);
            this.Items.Insert(index + 1, this.SelectedItem);

            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            var element = collection[index];
            collection.Remove(element);
            collection.AddAt(index + 1, element);

            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public virtual void RevertItems()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var collection = GetCollection(service);
            collection.Revert();

            service.ServerManager.CommitChanges();
            this.SelectedItem = null;
            LoadItems();
        }

        protected virtual ConfigurationElementCollection GetSecondaryCollection(IConfigurationService service)
        {
            return null;
        }
    }
}
