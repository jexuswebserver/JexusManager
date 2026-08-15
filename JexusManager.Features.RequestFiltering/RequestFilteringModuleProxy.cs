// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.RequestFiltering
{
    internal sealed class RequestFilteringModuleProxy : ModuleServiceProxy
    {
        internal VerbsItem[] GetVerbs()
        {
            return (VerbsItem[])Invoke(nameof(GetVerbs));
        }

        internal void AddVerb(VerbsItem item)
        {
            Invoke(nameof(AddVerb), item);
        }

        internal void RemoveVerb(VerbsItem item)
        {
            Invoke(nameof(RemoveVerb), item);
        }

        internal UrlsItem[] GetUrls()
        {
            return (UrlsItem[])Invoke(nameof(GetUrls));
        }

        internal void AddUrl(UrlsItem item)
        {
            Invoke(nameof(AddUrl), item);
        }

        internal void RemoveUrl(UrlsItem item)
        {
            Invoke(nameof(RemoveUrl), item);
        }

        internal QueryStringsItem[] GetQueryStrings()
        {
            return (QueryStringsItem[])Invoke(nameof(GetQueryStrings));
        }

        internal void AddQueryString(QueryStringsItem item)
        {
            Invoke(nameof(AddQueryString), item);
        }

        internal void RemoveQueryString(QueryStringsItem item)
        {
            Invoke(nameof(RemoveQueryString), item);
        }

        internal HiddenSegmentsItem[] GetHiddenSegments()
        {
            return (HiddenSegmentsItem[])Invoke(nameof(GetHiddenSegments));
        }

        internal void AddHiddenSegment(HiddenSegmentsItem item)
        {
            Invoke(nameof(AddHiddenSegment), item);
        }

        internal void RemoveHiddenSegment(HiddenSegmentsItem item)
        {
            Invoke(nameof(RemoveHiddenSegment), item);
        }

        internal HeadersItem[] GetHeaders()
        {
            return (HeadersItem[])Invoke(nameof(GetHeaders));
        }

        internal void AddHeader(HeadersItem item)
        {
            Invoke(nameof(AddHeader), item);
        }

        internal void RemoveHeader(HeadersItem item)
        {
            Invoke(nameof(RemoveHeader), item);
        }

        internal FileExtensionsItem[] GetFileExtensions()
        {
            return (FileExtensionsItem[])Invoke(nameof(GetFileExtensions));
        }

        internal void AddFileExtension(FileExtensionsItem item)
        {
            Invoke(nameof(AddFileExtension), item);
        }

        internal void RemoveFileExtension(FileExtensionsItem item)
        {
            Invoke(nameof(RemoveFileExtension), item);
        }

        internal FilteringRulesItem[] GetFilteringRules()
        {
            return (FilteringRulesItem[])Invoke(nameof(GetFilteringRules));
        }

        internal void AddFilteringRule(FilteringRulesItem item)
        {
            Invoke(nameof(AddFilteringRule), item);
        }

        internal void UpdateFilteringRule(FilteringRulesItem original, FilteringRulesItem item)
        {
            Invoke(nameof(UpdateFilteringRule), original, item);
        }

        internal void RemoveFilteringRule(FilteringRulesItem item)
        {
            Invoke(nameof(RemoveFilteringRule), item);
        }

        internal RequestFilteringSettings GetSettings()
        {
            return (RequestFilteringSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(RequestFilteringSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }
    }
}
