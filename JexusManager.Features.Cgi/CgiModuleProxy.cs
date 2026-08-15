using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Cgi
{
    internal sealed class CgiModuleProxy : ModuleServiceProxy
    {
        internal CgiItem GetSettings() => (CgiItem)Invoke(nameof(GetSettings));
        internal void Apply(CgiItem settings) => Invoke(nameof(Apply), settings);
    }
}
