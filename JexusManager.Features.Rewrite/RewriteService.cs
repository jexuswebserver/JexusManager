// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using JexusManager.Features.Rewrite.Inbound;
using JexusManager.Features.Rewrite.Outbound;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Rewrite
{
    internal sealed class RewriteService : ModuleService
    {
        private const string RewriteSectionPath = "system.webServer/rewrite";
        private const string RulesSectionPath = "system.webServer/rewrite/rules";
        private const string OutboundRulesSectionPath = "system.webServer/rewrite/outboundRules";
        private const string MapsSectionPath = "system.webServer/rewrite/rewriteMaps";
        private const string ProvidersSectionPath = "system.webServer/rewrite/providers";
        private const string AllowedVariablesSectionPath = "system.webServer/rewrite/allowedServerVariables";

        [ModuleServiceMethod]
        public RewriteSettings GetSettings()
        {
            return new RewriteSettings
            {
                Enabled = (bool)GetSection(RewriteSectionPath)["enabled"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(RewriteSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            GetSection(RewriteSectionPath)["enabled"] = settings.Enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public InboundRule[] GetInboundRules()
        {
            var result = new List<InboundRule>();
            foreach (ConfigurationElement element in GetSection(RulesSectionPath).GetCollection())
            {
                result.Add(CreateInboundRule(element));
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddInboundRule(InboundRule item)
        {
            AddRule(GetSection(RulesSectionPath).GetCollection(), item, ApplyInboundRule);
        }

        [ModuleServiceMethod]
        public void UpdateInboundRule(InboundRule original, InboundRule item)
        {
            UpdateRule(GetSection(RulesSectionPath).GetCollection(), GetKey(original), item, ApplyInboundRule);
        }

        [ModuleServiceMethod]
        public void RemoveInboundRule(InboundRule item)
        {
            RemoveRule(GetSection(RulesSectionPath).GetCollection(), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void MoveInboundRuleUp(InboundRule item)
        {
            Move(GetSection(RulesSectionPath).GetCollection(), item?.Name, -1);
        }

        [ModuleServiceMethod]
        public void MoveInboundRuleDown(InboundRule item)
        {
            Move(GetSection(RulesSectionPath).GetCollection(), item?.Name, 1);
        }

        [ModuleServiceMethod]
        public void SetInboundRuleEnabled(InboundRule item, bool enabled)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection(RulesSectionPath).GetCollection();
            var existing = Find(collection, "name", item.Name);
            if (existing == null)
            {
                throw new InvalidOperationException("Inbound rule was not found.");
            }

            existing["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RevertInboundRules()
        {
            GetSection(RulesSectionPath).GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public OutboundRule[] GetOutboundRules()
        {
            var result = new List<OutboundRule>();
            foreach (ConfigurationElement element in GetSection(OutboundRulesSectionPath).GetCollection("rules"))
            {
                result.Add(CreateOutboundRule(element));
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddOutboundRule(OutboundRule item)
        {
            AddRule(GetSection(OutboundRulesSectionPath).GetCollection("rules"), item, ApplyOutboundRule);
        }

        [ModuleServiceMethod]
        public void UpdateOutboundRule(OutboundRule original, OutboundRule item)
        {
            UpdateRule(GetSection(OutboundRulesSectionPath).GetCollection("rules"), GetKey(original), item, ApplyOutboundRule);
        }

        [ModuleServiceMethod]
        public void RemoveOutboundRule(OutboundRule item)
        {
            RemoveRule(GetSection(OutboundRulesSectionPath).GetCollection("rules"), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void MoveOutboundRuleUp(OutboundRule item)
        {
            Move(GetSection(OutboundRulesSectionPath).GetCollection("rules"), item?.Name, -1);
        }

        [ModuleServiceMethod]
        public void MoveOutboundRuleDown(OutboundRule item)
        {
            Move(GetSection(OutboundRulesSectionPath).GetCollection("rules"), item?.Name, 1);
        }

        [ModuleServiceMethod]
        public void SetOutboundRuleEnabled(OutboundRule item, bool enabled)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection(OutboundRulesSectionPath).GetCollection("rules");
            var existing = Find(collection, "name", item.Name);
            if (existing == null)
            {
                throw new InvalidOperationException("Outbound rule was not found.");
            }

            existing["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RevertOutboundRules()
        {
            GetSection(OutboundRulesSectionPath).GetCollection("rules").Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public MapItem[] GetMaps()
        {
            var result = new List<MapItem>();
            foreach (ConfigurationElement element in GetSection(MapsSectionPath).GetCollection())
            {
                var item = new MapItem();
                item.OriginalKey = (string)element["name"];
                item.Name = (string)element["name"];
                item.DefaultValue = (string)element["defaultValue"];
                item.IgnoreCase = (bool)element["ignoreCase"];
                item.Flag = element.IsLocallyStored ? "Local" : "Inherited";
                foreach (ConfigurationElement rule in element.GetCollection())
                {
                    item.Items.Add(new MapRule
                    {
                        Original = (string)rule["key"],
                        New = (string)rule["value"],
                        Flag = rule.IsLocallyStored ? "Local" : "Inherited"
                    });
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddMap(MapItem item)
        {
            AddItem(GetSection(MapsSectionPath).GetCollection(), item?.Name, element => ApplyMap(element, item));
        }

        [ModuleServiceMethod]
        public void UpdateMap(MapItem original, MapItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection(MapsSectionPath).GetCollection();
            var existing = Find(collection, "name", string.IsNullOrEmpty(original.OriginalKey) ? original.Name : original.OriginalKey);
            if (existing == null)
            {
                throw new InvalidOperationException("Rewrite map was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyMap(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyMap(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RemoveMap(MapItem item)
        {
            RemoveRule(GetSection(MapsSectionPath).GetCollection(), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void RevertMaps()
        {
            GetSection(MapsSectionPath).GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public MapRule[] GetMapRules(string mapName)
        {
            var result = new List<MapRule>();
            var map = Find(GetSection(MapsSectionPath).GetCollection(), "name", mapName);
            if (map == null)
            {
                return result.ToArray();
            }

            foreach (ConfigurationElement rule in map.GetCollection())
            {
                result.Add(new MapRule
                {
                    Original = (string)rule["key"],
                    New = (string)rule["value"],
                    Flag = rule.IsLocallyStored ? "Local" : "Inherited"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddMapRule(string mapName, MapRule item)
        {
            AddRule(GetMapRulesCollection(mapName), item, (element, rule) =>
            {
                element["key"] = rule.Original;
                element["value"] = rule.New;
            });
        }

        [ModuleServiceMethod]
        public void UpdateMapRule(string mapName, MapRule original, MapRule item)
        {
            UpdateRule(GetMapRulesCollection(mapName), string.IsNullOrEmpty(original?.OriginalKey) ? original?.Original : original.OriginalKey, item, (element, rule) =>
            {
                element["key"] = rule.Original;
                element["value"] = rule.New;
            });
        }

        [ModuleServiceMethod]
        public void RemoveMapRule(string mapName, MapRule item)
        {
            RemoveRule(GetMapRulesCollection(mapName), item?.Original, "key");
        }

        [ModuleServiceMethod]
        public void MoveMapRuleUp(string mapName, MapRule item)
        {
            Move(GetMapRulesCollection(mapName), item?.Original, -1);
        }

        [ModuleServiceMethod]
        public void MoveMapRuleDown(string mapName, MapRule item)
        {
            Move(GetMapRulesCollection(mapName), item?.Original, 1);
        }

        [ModuleServiceMethod]
        public ProviderItem[] GetProviders()
        {
            var result = new List<ProviderItem>();
            foreach (ConfigurationElement element in GetSection(ProvidersSectionPath).GetCollection())
            {
                var item = new ProviderItem
                {
                    OriginalKey = (string)element["name"],
                    Name = (string)element["name"],
                    Type = (string)element["type"],
                    Flag = element.IsLocallyStored ? "Local" : "Inhertied"
                };
                var settings = element.GetChildElement("settings")?.GetCollection();
                if (settings != null)
                {
                    foreach (ConfigurationElement setting in settings)
                    {
                        item.Settings.Add(new SettingItem
                        {
                            Key = (string)setting["key"],
                            Value = (string)setting["value"]
                        });
                    }
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddProvider(ProviderItem item)
        {
            AddItem(GetSection(ProvidersSectionPath).GetCollection(), item?.Name, element => ApplyProvider(element, item));
        }

        [ModuleServiceMethod]
        public void UpdateProvider(ProviderItem original, ProviderItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection(ProvidersSectionPath).GetCollection();
            var existing = Find(collection, "name", string.IsNullOrEmpty(original.OriginalKey) ? original.Name : original.OriginalKey);
            if (existing == null)
            {
                throw new InvalidOperationException("Rewrite provider was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyProvider(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyProvider(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RemoveProvider(ProviderItem item)
        {
            RemoveRule(GetSection(ProvidersSectionPath).GetCollection(), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void RevertProviders()
        {
            GetSection(ProvidersSectionPath).GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void AddProviderSetting(string providerName, SettingItem item)
        {
            AddItem(GetProviderSettingsCollection(providerName), item?.Key, element =>
            {
                element["key"] = item.Key;
                element["value"] = item.Value;
            });
        }

        [ModuleServiceMethod]
        public void UpdateProviderSetting(string providerName, SettingItem original, SettingItem item)
        {
            UpdateRule(GetProviderSettingsCollection(providerName), string.IsNullOrEmpty(original?.OriginalKey) ? original?.Key : original.OriginalKey, item, (element, setting) =>
            {
                element["key"] = setting.Key;
                element["value"] = setting.Value;
            });
        }

        [ModuleServiceMethod]
        public void RemoveProviderSetting(string providerName, SettingItem item)
        {
            RemoveRule(GetProviderSettingsCollection(providerName), item?.Key, "key");
        }

        [ModuleServiceMethod]
        public AllowedVariableItem[] GetAllowedVariables()
        {
            var result = new List<AllowedVariableItem>();
            foreach (ConfigurationElement element in GetSection(AllowedVariablesSectionPath).GetCollection())
            {
                result.Add(new AllowedVariableItem
                {
                    Name = (string)element["name"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddAllowedVariable(AllowedVariableItem item)
        {
            AddItem(GetSection(AllowedVariablesSectionPath).GetCollection(), item?.Name, element => element["name"] = item.Name);
        }

        [ModuleServiceMethod]
        public void RemoveAllowedVariable(AllowedVariableItem item)
        {
            RemoveRule(GetSection(AllowedVariablesSectionPath).GetCollection(), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void RevertAllowedVariables()
        {
            GetSection(AllowedVariablesSectionPath).GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public CustomTagsItem[] GetCustomTags()
        {
            var result = new List<CustomTagsItem>();
            foreach (ConfigurationElement element in GetSection(OutboundRulesSectionPath).GetCollection("customTags"))
            {
                var item = new CustomTagsItem
                {
                    OriginalKey = (string)element["name"],
                    Name = (string)element["name"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                };
                foreach (ConfigurationElement child in element.GetCollection())
                {
                    item.Tags.Add(new CustomTagItem
                    {
                        Name = (string)child["name"],
                        Attribute = (string)child["attribute"]
                    });
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddCustomTag(CustomTagsItem item)
        {
            AddItem(GetSection(OutboundRulesSectionPath).GetCollection("customTags"), item?.Name, element => ApplyCustomTag(element, item));
        }

        [ModuleServiceMethod]
        public void UpdateCustomTag(CustomTagsItem original, CustomTagsItem item)
        {
            UpdateRule(GetSection(OutboundRulesSectionPath).GetCollection("customTags"), GetKey(original), item, ApplyCustomTag);
        }

        [ModuleServiceMethod]
        public void RemoveCustomTag(CustomTagsItem item)
        {
            RemoveRule(GetSection(OutboundRulesSectionPath).GetCollection("customTags"), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public PreConditionItem[] GetPreConditions()
        {
            var result = new List<PreConditionItem>();
            foreach (ConfigurationElement element in GetSection(OutboundRulesSectionPath).GetCollection("preConditions"))
            {
                var item = new PreConditionItem
                {
                    OriginalKey = (string)element["name"],
                    Name = (string)element["name"],
                    LogicalGrouping = (long)element["logicalGrouping"],
                    PatternSyntax = (long)element["patternSyntax"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                };
                foreach (ConfigurationElement child in element.GetCollection())
                {
                    item.Conditions.Add(CreateCondition(child));
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddPreCondition(PreConditionItem item)
        {
            AddItem(GetSection(OutboundRulesSectionPath).GetCollection("preConditions"), item?.Name, element => ApplyPreCondition(element, item));
        }

        [ModuleServiceMethod]
        public void UpdatePreCondition(PreConditionItem original, PreConditionItem item)
        {
            UpdateRule(GetSection(OutboundRulesSectionPath).GetCollection("preConditions"), GetKey(original), item, ApplyPreCondition);
        }

        [ModuleServiceMethod]
        public void RemovePreCondition(PreConditionItem item)
        {
            RemoveRule(GetSection(OutboundRulesSectionPath).GetCollection("preConditions"), item?.Name, "name");
        }

        [ModuleServiceMethod]
        public void RevertPreConditions()
        {
            GetSection(OutboundRulesSectionPath).GetCollection("preConditions").Revert();
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection(string sectionPath)
        {
            return ManagementUnit.Configuration.GetSection(sectionPath);
        }

        private ConfigurationElementCollection GetMapRulesCollection(string mapName)
        {
            var map = Find(GetSection(MapsSectionPath).GetCollection(), "name", mapName);
            if (map == null)
            {
                throw new InvalidOperationException("Rewrite map was not found.");
            }

            return map.GetCollection();
        }

        private ConfigurationElementCollection GetProviderSettingsCollection(string providerName)
        {
            var provider = Find(GetSection(ProvidersSectionPath).GetCollection(), "name", providerName);
            if (provider == null)
            {
                throw new InvalidOperationException("Rewrite provider was not found.");
            }

            return provider.GetChildElement("settings").GetCollection();
        }

        private static InboundRule CreateInboundRule(ConfigurationElement element)
        {
            var rule = new InboundRule
            {
                OriginalKey = (string)element["name"],
                Name = (string)element["name"],
                Enabled = (bool)element["enabled"],
                PatternSyntax = (long)element["patternSyntax"],
                StopProcessing = (bool)element["stopProcessing"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            };
            var match = element.ChildElements["match"];
            rule.PatternUrl = (string)match["url"];
            rule.Negate = (bool)match["negate"];
            rule.IgnoreCase = (bool)match["ignoreCase"];
            var action = element.ChildElements["action"];
            rule.Type = (long)action["type"];
            rule.ActionUrl = (string)action["url"];
            rule.AppendQueryString = (bool)action["appendQueryString"];
            rule.LogRewrittenUrl = (bool)action["logRewrittenUrl"];
            var redirect = (long)action["redirectType"];
            rule.RedirectType = redirect switch
            {
                301 => 0,
                302 => 1,
                303 => 2,
                307 => 3,
                _ => rule.RedirectType
            };
            rule.StatusCode = (uint)action["statusCode"];
            rule.SubStatusCode = (uint)action["subStatusCode"];
            rule.StatusReason = (string)action["statusReason"];
            rule.StatusDescription = (string)action["statusDescription"];
            var conditions = element.ChildElements["conditions"];
            rule.TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            rule.LogicalGrouping = (long)conditions["logicalGrouping"];
            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                rule.Conditions.Add(CreateCondition(condition));
            }

            foreach (ConfigurationElement variable in element.ChildElements["serverVariables"].GetCollection())
            {
                rule.ServerVariables.Add(new ServerVariableItem
                {
                    Name = (string)variable["name"],
                    Value = (string)variable["value"],
                    Replace = (bool)variable["replace"]
                });
            }

            return rule;
        }

        private static ConditionItem CreateCondition(ConfigurationElement element)
        {
            var condition = new ConditionItem
            {
                Input = (string)element["input"],
                Pattern = (string)element["pattern"],
                IgnoreCase = (bool)element["ignoreCase"]
            };
            var root = (long)element["matchType"];
            var negate = (bool)element["negate"];
            var value = root == 0 ? 2 : root - 1;
            condition.MatchType = (int)value * 2 + (negate ? 1 : 0);
            return condition;
        }

        private static OutboundRule CreateOutboundRule(ConfigurationElement element)
        {
            var rule = new OutboundRule
            {
                OriginalKey = (string)element["name"],
                Name = (string)element["name"],
                PreCondition = (string)element["preCondition"],
                Enabled = (bool)element["enabled"],
                Syntax = (long)element["patternSyntax"],
                Stopping = (bool)element["stopProcessing"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            };
            var match = element.ChildElements["match"];
            rule.Filter = (long)match["filterByTags"];
            rule.CustomTags = (string)match["customTags"];
            rule.ServerVariable = (string)match["serverVariable"];
            rule.Pattern = (string)match["pattern"];
            rule.IgnoreCase = (bool)match["ignoreCase"];
            rule.Negate = (bool)match["negate"];
            var conditions = element.ChildElements["conditions"];
            rule.TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            rule.LogicalGrouping = (long)conditions["logicalGrouping"];
            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                rule.Conditions.Add(CreateCondition(condition));
            }

            var action = element.ChildElements["action"];
            rule.Action = (long)action["type"];
            rule.Value = (string)action["value"];
            rule.Replace = (bool)action["replace"];
            return rule;
        }

        private static void ApplyCondition(ConfigurationElement element, ConditionItem condition)
        {
            element["input"] = condition.Input;
            element["pattern"] = condition.Pattern;
            element["ignoreCase"] = condition.IgnoreCase;
            element["negate"] = condition.MatchType % 2 == 1;
            var value = condition.MatchType / 2;
            element["matchType"] = value == 2 ? 0 : value + 1;
        }

        private static void ApplyInboundRule(ConfigurationElement element, InboundRule rule)
        {
            element["name"] = rule.Name;
            element["enabled"] = rule.Enabled;
            element["patternSyntax"] = rule.PatternSyntax;
            element["stopProcessing"] = rule.StopProcessing;
            var match = element.ChildElements["match"];
            match["url"] = rule.PatternUrl;
            match["negate"] = rule.Negate;
            match["ignoreCase"] = rule.IgnoreCase;
            var action = element.ChildElements["action"];
            action["type"] = rule.Type;
            action["url"] = rule.ActionUrl;
            action["appendQueryString"] = rule.AppendQueryString;
            action["logRewrittenUrl"] = rule.LogRewrittenUrl;
            action["redirectType"] = rule.RedirectType switch
            {
                0 => 301L,
                1 => 302L,
                2 => 303L,
                3 => 307L,
                _ => 301L
            };
            action["statusCode"] = rule.StatusCode;
            action["subStatusCode"] = rule.SubStatusCode;
            action["statusReason"] = rule.StatusReason;
            action["statusDescription"] = rule.StatusDescription;
            var conditions = element.ChildElements["conditions"];
            conditions["trackAllCaptures"] = rule.TrackAllCaptures;
            conditions["logicalGrouping"] = rule.LogicalGrouping;
            var conditionCollection = conditions.GetCollection();
            conditionCollection.Clear();
            foreach (var condition in rule.Conditions)
            {
                var child = conditionCollection.CreateElement();
                ApplyCondition(child, condition);
                conditionCollection.Add(child);
            }

            var variableCollection = element.ChildElements["serverVariables"].GetCollection();
            variableCollection.Clear();
            foreach (var variable in rule.ServerVariables)
            {
                var child = variableCollection.CreateElement();
                child["name"] = variable.Name;
                child["value"] = variable.Value;
                child["replace"] = variable.Replace;
                variableCollection.Add(child);
            }
        }

        private static void ApplyOutboundRule(ConfigurationElement element, OutboundRule rule)
        {
            element["name"] = rule.Name;
            element["preCondition"] = rule.PreCondition;
            element["enabled"] = rule.Enabled;
            element["patternSyntax"] = rule.Syntax;
            element["stopProcessing"] = rule.Stopping;
            var match = element.ChildElements["match"];
            match["filterByTags"] = rule.Filter;
            match["customTags"] = rule.CustomTags;
            match["serverVariable"] = rule.ServerVariable;
            match["pattern"] = rule.Pattern;
            match["ignoreCase"] = rule.IgnoreCase;
            match["negate"] = rule.Negate;
            var conditions = element.ChildElements["conditions"];
            conditions["trackAllCaptures"] = rule.TrackAllCaptures;
            conditions["logicalGrouping"] = rule.LogicalGrouping;
            var conditionCollection = conditions.GetCollection();
            conditionCollection.Clear();
            foreach (var condition in rule.Conditions)
            {
                var child = conditionCollection.CreateElement();
                ApplyCondition(child, condition);
                conditionCollection.Add(child);
            }

            var action = element.ChildElements["action"];
            action["type"] = rule.Action;
            action["value"] = rule.Value;
            action["replace"] = rule.Replace;
        }

        private static void ApplyMap(ConfigurationElement element, MapItem item)
        {
            element["name"] = item.Name;
            element["defaultValue"] = item.DefaultValue;
            element["ignoreCase"] = item.IgnoreCase;
            var rules = element.GetCollection();
            rules.Clear();
            foreach (var rule in item.Items)
            {
                var child = rules.CreateElement();
                child["key"] = rule.Original;
                child["value"] = rule.New;
                rules.Add(child);
            }
        }

        private static void ApplyProvider(ConfigurationElement element, ProviderItem item)
        {
            element["name"] = item.Name;
            element["type"] = item.Type;
            var settings = element.GetCollection("settings");
            settings.Clear();
            foreach (var setting in item.Settings)
            {
                var child = settings.CreateElement();
                child["key"] = setting.Key;
                child["value"] = setting.Value;
                settings.Add(child);
            }
        }

        private static void ApplyCustomTag(ConfigurationElement element, CustomTagsItem item)
        {
            element["name"] = item.Name;
            var tags = element.GetCollection();
            tags.Clear();
            foreach (var tag in item.Tags)
            {
                var child = tags.CreateElement();
                child["name"] = tag.Name;
                child["attribute"] = tag.Attribute;
                tags.Add(child);
            }
        }

        private static void ApplyPreCondition(ConfigurationElement element, PreConditionItem item)
        {
            element["name"] = item.Name;
            element["logicalGrouping"] = item.LogicalGrouping;
            element["patternSyntax"] = item.PatternSyntax;
            var conditions = element.GetCollection();
            conditions.Clear();
            foreach (var condition in item.Conditions)
            {
                var child = conditions.CreateElement();
                ApplyCondition(child, condition);
                conditions.Add(child);
            }
        }

        private void AddRule<T>(ConfigurationElementCollection collection, T item, Action<ConfigurationElement, T> apply)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var element = collection.CreateElement();
            apply(element, item);
            collection.Add(element);
            ManagementUnit.Update();
        }

        private static string GetKey<T>(T item) where T : class
        {
            var property = item?.GetType().GetProperty("OriginalKey");
            var key = property?.GetValue(item) as string;
            if (!string.IsNullOrEmpty(key))
            {
                return key;
            }

            var name = item?.GetType().GetProperty("Name");
            return name?.GetValue(item) as string;
        }

        private void UpdateRule<T>(ConfigurationElementCollection collection, string key, T item, Action<ConfigurationElement, T> apply)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var existing = Find(collection, "name", key);
            if (existing == null)
            {
                throw new InvalidOperationException("The entry was not found.");
            }

            if (existing.IsLocallyStored)
            {
                apply(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                apply(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        private void RemoveRule(ConfigurationElementCollection collection, string key, string attributeName)
        {
            var existing = Find(collection, attributeName, key);
            if (existing == null)
            {
                throw new InvalidOperationException("The entry was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        private void Move(ConfigurationElementCollection collection, string key, int delta)
        {
            var existing = Find(collection, "name", key);
            if (existing == null)
            {
                throw new InvalidOperationException("The entry was not found.");
            }

            var index = collection.IndexOf(existing);
            var target = index + delta;
            if (target < 0 || target >= collection.Count)
            {
                return;
            }

            collection.RemoveAt(index);
            collection.AddAt(target, existing);
            ManagementUnit.Update();
        }

        private void AddItem(ConfigurationElementCollection collection, string key, Action<ConfigurationElement> apply)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("A value is required.");
            }

            var element = collection.CreateElement();
            apply(element);
            collection.Add(element);
            ManagementUnit.Update();
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, string attributeName, string value)
        {
            if (value == null)
            {
                return null;
            }

            foreach (ConfigurationElement element in collection)
            {
                if ((string)element[attributeName] == value)
                {
                    return element;
                }
            }

            return null;
        }
    }
}
