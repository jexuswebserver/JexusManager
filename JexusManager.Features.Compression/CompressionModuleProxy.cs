using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Compression
{
    internal sealed class CompressionModuleProxy : ModuleServiceProxy
    {
        internal CompressionSnapshot GetSettings() => (CompressionSnapshot)Invoke(nameof(GetSettings));
        internal void Apply(CompressionSnapshot settings) => Invoke(nameof(Apply), settings);
    }
}
