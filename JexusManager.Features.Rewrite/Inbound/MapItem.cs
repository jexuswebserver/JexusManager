// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using JexusManager.Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal class MapItem : FeatureBase<MapRule>, IItem<MapItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(MapItem other)
        {
            return other != null && other.Name == Name;
        }

        private readonly MapsFeature _feature;

        public string Name { get; internal set; }

        public string Flag { get; set; }

        public string DefaultValue { get; set; }

        public MapSettingsUpdatedEventHandler MapSettingsUpdated { get; set; }

        public MapItem(ConfigurationElement element, MapsFeature feature)
            : base(feature.Module)
        {
            this.Element = element;
            _feature = feature;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            this.Name = element == null ? string.Empty : (string)element["name"];
            IgnoreCase = element == null ? true : (bool)element["ignoreCase"];
            DefaultValue = element == null ? string.Empty : (string)element["defaultValue"];
            this.Items = new List<MapRule>();
            if (element != null)
            {
                var collection = element.GetCollection();
                foreach (ConfigurationElement rule in collection)
                {
                    this.Items.Add(new MapRule(rule, _feature));
                }
            }
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["defaultValue"] = DefaultValue;
            Element["ignoreCase"] = IgnoreCase;
        }

        internal protected void OnRewriteSettingsSaved()
        {
            MapSettingsUpdated?.Invoke();
            _feature.OnRewriteSettingsSaved();
        }

        public bool IgnoreCase { get; set; }

        public bool Equals(MapItem other)
        {
            return Match(other) && other.DefaultValue == DefaultValue && other.IgnoreCase == IgnoreCase;
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return Element.GetCollection();
        }

        protected override void OnSettingsSaved()
        {
            OnRewriteSettingsSaved();
        }

        public void AddRule()
        {
            using (var dialog = new AddMapDialog(Module, null, _feature))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                ConfigurationElementCollection rulesCollection = SelectedItem.Element.GetCollection();

                if (SelectedItem != newItem)
                {
                    Items.Add(newItem);
                    SelectedItem = newItem;
                }
                else if (newItem.Flag != "Local")
                {
                    rulesCollection.Remove(newItem.Element);
                    newItem.Flag = "Local";
                }

                newItem.AppendTo(rulesCollection);
                service.ServerManager.CommitChanges();
            }

            OnRewriteSettingsSaved();
        }

        public void EditRule()
        {
            using (var dialog = new AddMapDialog(Module, SelectedItem, _feature))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                ConfigurationElementCollection rulesCollection = SelectedItem.Element.GetCollection();

                if (SelectedItem != newItem)
                {
                    Items.Add(newItem);
                    SelectedItem = newItem;
                }
                else if (newItem.Flag != "Local")
                {
                    rulesCollection.Remove(newItem.Element);
                    newItem.Flag = "Local";
                }

                newItem.AppendTo(rulesCollection);
                service.ServerManager.CommitChanges();
            }

            OnRewriteSettingsSaved();
        }

        public void Set()
        {
            using (var dialog = new MapSettingsDialog(Module, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            OnRewriteSettingsSaved();
        }

        internal void Select()
        {
            _feature.SelectedItem = this;
        }

        internal void RemoveRule()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            Items.Remove(SelectedItem);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ConfigurationElementCollection collection = Element.GetCollection();
            collection.Remove(SelectedItem.Element);
            service.ServerManager.CommitChanges();

            SelectedItem = null;
            OnRewriteSettingsSaved();
        }
    }
}
