// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Features.Rewrite.Inbound;
using JexusManager.Features.Rewrite.Outbound;
using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Rewrite
{
    internal sealed class RewriteModuleProxy : ModuleServiceProxy
    {
        internal RewriteSettings GetSettings()
        {
            return (RewriteSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(RewriteSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }

        internal InboundRule[] GetInboundRules()
        {
            return (InboundRule[])Invoke(nameof(GetInboundRules));
        }

        internal void AddInboundRule(InboundRule item)
        {
            Invoke(nameof(AddInboundRule), item);
        }

        internal void UpdateInboundRule(InboundRule original, InboundRule item)
        {
            Invoke(nameof(UpdateInboundRule), original, item);
        }

        internal void RemoveInboundRule(InboundRule item)
        {
            Invoke(nameof(RemoveInboundRule), item);
        }

        internal void MoveInboundRuleUp(InboundRule item)
        {
            Invoke(nameof(MoveInboundRuleUp), item);
        }

        internal void MoveInboundRuleDown(InboundRule item)
        {
            Invoke(nameof(MoveInboundRuleDown), item);
        }

        internal void SetInboundRuleEnabled(InboundRule item, bool enabled)
        {
            Invoke(nameof(SetInboundRuleEnabled), item, enabled);
        }

        internal void RevertInboundRules()
        {
            Invoke(nameof(RevertInboundRules));
        }

        internal OutboundRule[] GetOutboundRules()
        {
            return (OutboundRule[])Invoke(nameof(GetOutboundRules));
        }

        internal void AddOutboundRule(OutboundRule item)
        {
            Invoke(nameof(AddOutboundRule), item);
        }

        internal void UpdateOutboundRule(OutboundRule original, OutboundRule item)
        {
            Invoke(nameof(UpdateOutboundRule), original, item);
        }

        internal void RemoveOutboundRule(OutboundRule item)
        {
            Invoke(nameof(RemoveOutboundRule), item);
        }

        internal void MoveOutboundRuleUp(OutboundRule item)
        {
            Invoke(nameof(MoveOutboundRuleUp), item);
        }

        internal void MoveOutboundRuleDown(OutboundRule item)
        {
            Invoke(nameof(MoveOutboundRuleDown), item);
        }

        internal void SetOutboundRuleEnabled(OutboundRule item, bool enabled)
        {
            Invoke(nameof(SetOutboundRuleEnabled), item, enabled);
        }

        internal void RevertOutboundRules()
        {
            Invoke(nameof(RevertOutboundRules));
        }

        internal MapItem[] GetMaps()
        {
            return (MapItem[])Invoke(nameof(GetMaps));
        }

        internal void AddMap(MapItem item)
        {
            Invoke(nameof(AddMap), item);
        }

        internal void UpdateMap(MapItem original, MapItem item)
        {
            Invoke(nameof(UpdateMap), original, item);
        }

        internal void RemoveMap(MapItem item)
        {
            Invoke(nameof(RemoveMap), item);
        }

        internal void RevertMaps()
        {
            Invoke(nameof(RevertMaps));
        }

        internal MapRule[] GetMapRules(string mapName)
        {
            return (MapRule[])Invoke(nameof(GetMapRules), mapName);
        }

        internal void AddMapRule(string mapName, MapRule item)
        {
            Invoke(nameof(AddMapRule), mapName, item);
        }

        internal void UpdateMapRule(string mapName, MapRule original, MapRule item)
        {
            Invoke(nameof(UpdateMapRule), mapName, original, item);
        }

        internal void RemoveMapRule(string mapName, MapRule item)
        {
            Invoke(nameof(RemoveMapRule), mapName, item);
        }

        internal void MoveMapRuleUp(string mapName, MapRule item)
        {
            Invoke(nameof(MoveMapRuleUp), mapName, item);
        }

        internal void MoveMapRuleDown(string mapName, MapRule item)
        {
            Invoke(nameof(MoveMapRuleDown), mapName, item);
        }

        internal ProviderItem[] GetProviders()
        {
            return (ProviderItem[])Invoke(nameof(GetProviders));
        }

        internal void AddProvider(ProviderItem item)
        {
            Invoke(nameof(AddProvider), item);
        }

        internal void UpdateProvider(ProviderItem original, ProviderItem item)
        {
            Invoke(nameof(UpdateProvider), original, item);
        }

        internal void RemoveProvider(ProviderItem item)
        {
            Invoke(nameof(RemoveProvider), item);
        }

        internal void RevertProviders()
        {
            Invoke(nameof(RevertProviders));
        }

        internal void AddProviderSetting(string providerName, SettingItem item)
        {
            Invoke(nameof(AddProviderSetting), providerName, item);
        }

        internal void UpdateProviderSetting(string providerName, SettingItem original, SettingItem item)
        {
            Invoke(nameof(UpdateProviderSetting), providerName, original, item);
        }

        internal void RemoveProviderSetting(string providerName, SettingItem item)
        {
            Invoke(nameof(RemoveProviderSetting), providerName, item);
        }

        internal AllowedVariableItem[] GetAllowedVariables()
        {
            return (AllowedVariableItem[])Invoke(nameof(GetAllowedVariables));
        }

        internal void AddAllowedVariable(AllowedVariableItem item)
        {
            Invoke(nameof(AddAllowedVariable), item);
        }

        internal void RemoveAllowedVariable(AllowedVariableItem item)
        {
            Invoke(nameof(RemoveAllowedVariable), item);
        }

        internal void RevertAllowedVariables()
        {
            Invoke(nameof(RevertAllowedVariables));
        }

        internal CustomTagsItem[] GetCustomTags()
        {
            return (CustomTagsItem[])Invoke(nameof(GetCustomTags));
        }

        internal void AddCustomTag(CustomTagsItem item)
        {
            Invoke(nameof(AddCustomTag), item);
        }

        internal void UpdateCustomTag(CustomTagsItem original, CustomTagsItem item)
        {
            Invoke(nameof(UpdateCustomTag), original, item);
        }

        internal void RemoveCustomTag(CustomTagsItem item)
        {
            Invoke(nameof(RemoveCustomTag), item);
        }

        internal PreConditionItem[] GetPreConditions()
        {
            return (PreConditionItem[])Invoke(nameof(GetPreConditions));
        }

        internal void AddPreCondition(PreConditionItem item)
        {
            Invoke(nameof(AddPreCondition), item);
        }

        internal void UpdatePreCondition(PreConditionItem original, PreConditionItem item)
        {
            Invoke(nameof(UpdatePreCondition), original, item);
        }

        internal void RemovePreCondition(PreConditionItem item)
        {
            Invoke(nameof(RemovePreCondition), item);
        }

        internal void RevertPreConditions()
        {
            Invoke(nameof(RevertPreConditions));
        }
    }
}
