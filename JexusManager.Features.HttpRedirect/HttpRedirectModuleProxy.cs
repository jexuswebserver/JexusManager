using Microsoft.Web.Management.Client;

namespace JexusManager.Features.HttpRedirect
{
    internal sealed class HttpRedirectModuleProxy : ModuleServiceProxy
    {
        internal HttpRedirectSnapshot GetSettings() => (HttpRedirectSnapshot)Invoke(nameof(GetSettings));
        internal void Apply(HttpRedirectSnapshot settings) => Invoke(nameof(Apply), settings);
    }
}