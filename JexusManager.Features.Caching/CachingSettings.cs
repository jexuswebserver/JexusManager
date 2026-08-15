using System;

namespace JexusManager.Features.Caching
{
    [Serializable]
    public sealed class CachingSettings
    {
        public bool Enabled { get; set; }
        public bool EnableKernelCache { get; set; }
        public uint MaxResponseSize { get; set; }
        public uint MaxCacheSize { get; set; }
    }
}