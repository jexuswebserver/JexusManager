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
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                Extension = VaryByQueryString = VaryByHeaders = string.Empty;
                return;
            }

            this.Extension = (string)element["extension"];
            this.Policy = (long)element["policy"];
            this.KernelCachePolicy = (long)element["kernelCachePolicy"];
            this.Duration = (TimeSpan)element["duration"];
            this.VaryByQueryString = (string)element["varyByQueryString"];
            this.VaryByHeaders = (string)element["varyByHeaders"];
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
            return this.Match(other);
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
            return other != null && other.Extension == this.Extension;
        }
    }
}
