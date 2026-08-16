using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Caching
{
    internal sealed class CachingService : ModuleService
    {
        [ModuleServiceMethod]
        public CachingItem[] GetProfiles()
        {
            var result = new List<CachingItem>();
            foreach (ConfigurationElement element in ManagementUnit.Configuration.GetSection("system.webServer/caching").GetCollection("profiles"))
            {
                result.Add(new CachingItem { OriginalKey = (string)element["extension"], Extension = (string)element["extension"], Policy = (long)element["policy"], KernelCachePolicy = (long)element["kernelCachePolicy"], Duration = (System.TimeSpan)element["duration"], VaryByQueryString = (string)element["varyByQueryString"], VaryByHeaders = (string)element["varyByHeaders"], Flag = element.IsLocallyStored ? "Local" : "Inhertied" });
            }
            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(CachingItem item)
        {
            AddProfile(GetProfileCollection(), item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(CachingItem original, CachingItem item)
        {
            var profiles = GetProfileCollection();
            var existing = Find(profiles, original);
            if (existing != null)
            {
                if (existing.IsLocallyStored)
                {
                    ApplyProfile(existing, item);
                }
                else
                {
                    profiles.Remove(existing);
                    AddProfile(profiles, item);
                }
            }
            else
            {
                AddProfile(profiles, item);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(CachingItem item)
        {
            var profiles = GetProfileCollection();
            var existing = Find(profiles, item);
            if (existing == null) throw new System.InvalidOperationException("Cache profile was not found.");
            profiles.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public CachingSettings GetSettings()
        {
            var section = ManagementUnit.Configuration.GetSection("system.webServer/caching");
            return new CachingSettings { Enabled = (bool)section["enabled"], EnableKernelCache = (bool)section["enableKernelCache"], MaxResponseSize = (uint)section["maxResponseSize"], MaxCacheSize = (uint)section["maxCacheSize"] };
        }

        [ModuleServiceMethod]
        public void ApplySettings(CachingSettings settings)
        {
            if (settings == null) throw new System.ArgumentNullException(nameof(settings));
            var section = ManagementUnit.Configuration.GetSection("system.webServer/caching");
            section["enabled"] = settings.Enabled; section["enableKernelCache"] = settings.EnableKernelCache; section["maxResponseSize"] = settings.MaxResponseSize; section["maxCacheSize"] = settings.MaxCacheSize;
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection GetProfileCollection() => ManagementUnit.Configuration.GetSection("system.webServer/caching").GetCollection("profiles");
        private static ConfigurationElement Find(ConfigurationElementCollection profiles, CachingItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Extension : item.OriginalKey;
            foreach (ConfigurationElement element in profiles) if ((string)element["extension"] == key) return element;
            return null;
        }
        private static void AddProfile(ConfigurationElementCollection profiles, CachingItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Extension)) throw new System.ArgumentException("A cache profile extension is required.");
            var element = profiles.CreateElement();
            ApplyProfile(element, item);
            profiles.Add(element);
        }

        private static void ApplyProfile(ConfigurationElement element, CachingItem item)
        {
            element["extension"] = item.Extension; element["policy"] = item.Policy; element["kernelCachePolicy"] = item.KernelCachePolicy; element["duration"] = item.Duration; element["varyByQueryString"] = item.VaryByQueryString; element["varyByHeaders"] = item.VaryByHeaders;
        }
    }
}
