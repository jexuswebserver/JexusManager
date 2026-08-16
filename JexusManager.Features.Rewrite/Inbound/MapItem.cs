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
        private readonly string _originalName;

        public string Name { get; internal set; }

        internal string OriginalKey { get; set; }

        public string Flag { get; set; }

        public string DefaultValue { get; set; }

        public MapSettingsUpdatedEventHandler MapSettingsUpdated { get; set; }

        public MapItem()
            : this(null)
        {
        }

        public MapItem(MapsFeature feature, string originalName = null)
            : base(feature?.Module)
        {
            _feature = feature;
            _originalName = originalName ?? string.Empty;
            this.Flag = "Local";
            this.Name = string.Empty;
            IgnoreCase = true;
            DefaultValue = string.Empty;
            this.Items = new List<MapRule>();
        }

        public void Apply()
        {
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
            throw new NotSupportedException("Rewrite map entries are accessed through the module service.");
        }

        protected override void OnSettingsSaved()
        {
            OnRewriteSettingsSaved();
        }

        public void Add()
        {
            using (var dialog = new AddMapDialog(Module, null, _feature))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                _feature.Proxy.AddMapRule(Name, newItem);
                Items.Add(newItem);
                SelectedItem = newItem;
            }

            OnRewriteSettingsSaved();
        }

        public void Edit()
        {
            using (var dialog = new AddMapDialog(Module, SelectedItem, _feature))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var original = SelectedItem;
                _feature.Proxy.UpdateMapRule(Name, original, newItem);
                var index = Items.IndexOf(original);
                if (index >= 0)
                {
                    Items[index] = newItem;
                    SelectedItem = newItem;
                }
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

            var original = new MapItem(_feature, _originalName) { Name = _originalName };
            _feature.Proxy.UpdateMap(original, this);
            OnRewriteSettingsSaved();
        }

        internal void Select()
        {
            _feature.SelectedItem = this;
        }

        internal void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            var item = SelectedItem;
            _feature.Proxy.RemoveMapRule(Name, item);
            Items.Remove(item);
            SelectedItem = null;
            OnRewriteSettingsSaved();
        }

        internal void MoveUp()
        {
            var item = SelectedItem;
            _feature.Proxy.MoveMapRuleUp(Name, item);
            var index = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(index - 1, item);
            OnRewriteSettingsSaved();
        }

        internal void MoveDown()
        {
            var item = SelectedItem;
            _feature.Proxy.MoveMapRuleDown(Name, item);
            var index = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(index + 1, item);
            OnRewriteSettingsSaved();
        }

        internal void Revert()
        {
            Items.Clear();
            Items.AddRange(_feature.Proxy.GetMapRules(Name));
            SelectedItem = null;
            OnRewriteSettingsSaved();
        }

        internal void LoadRules()
        {
            Items.Clear();
            Items.AddRange(_feature.Proxy.GetMapRules(Name));
            OnRewriteSettingsSaved();
        }
    }
}
