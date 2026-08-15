using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Caching
{
    internal sealed class CachingModuleProxy : ModuleServiceProxy
    {
        internal CachingItem[] GetProfiles() => (CachingItem[])Invoke(nameof(GetProfiles));
        internal void Add(CachingItem item) => Invoke(nameof(Add), item);
        internal void Update(CachingItem original, CachingItem item) => Invoke(nameof(Update), original, item);
        internal void Remove(CachingItem item) => Invoke(nameof(Remove), item);
        internal CachingSettings GetSettings() => (CachingSettings)Invoke(nameof(GetSettings));
        internal void ApplySettings(CachingSettings settings) => Invoke(nameof(ApplySettings), settings);
    }
}
