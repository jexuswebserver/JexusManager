using System;

namespace JexusManager.Features.Compression
{
    [Serializable]
    public sealed class CompressionSnapshot
    {
        public bool StaticEnabled { get; set; }
        public bool DynamicEnabled { get; set; }
        public bool HasServerSettings { get; set; }
        public bool DoDiskSpaceLimiting { get; set; }
        public string MaxDiskSpaceUsage { get; set; }
        public string Directory { get; set; }
        public string MinFileSizeForComp { get; set; }
    }
}
