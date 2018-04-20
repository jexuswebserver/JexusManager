// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;

    using Microsoft.Web.Administration;

    internal class CachingItem : IItem<CachingItem>
    {
        public CachingItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                Extension = VaryByQueryString = VaryByHeaders = string.Empty;
                return;
            }

            Extension = (string)element["extension"];
            Policy = (long)element["policy"];
            KernelCachePolicy = (long)element["kernelCachePolicy"];
            Duration = (TimeSpan)element["duration"];
            VaryByQueryString = (string)element["varyByQueryString"];
            VaryByHeaders = (string)element["varyByHeaders"];
        }

        public string VaryByHeaders { get; set; }

        public string VaryByQueryString { get; set; }

        public TimeSpan Duration { get; set; }

        public long KernelCachePolicy { get; set; }

        public long Policy { get; set; }

        public string Extension { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool Equals(CachingItem other)
        {
            // all properties
            return Match(other);
        }

        public void Apply()
        {
            Element["extension"] = Extension;
            Element["policy"] = Policy;
            Element["kernelCachePolicy"] = KernelCachePolicy;
            Element["duration"] = Duration;
            Element["varyByQueryString"] = VaryByQueryString;
            Element["varyByHeaders"] = VaryByHeaders;
        }

        public bool Match(CachingItem other)
        {
            // match combined keys.
            return other != null && other.Extension == Extension;
        }
    }
}
