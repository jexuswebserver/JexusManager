using Microsoft.Web.Management.Client;

namespace JexusManager.Features.FastCgi
{
    internal sealed class FastCgiModuleProxy : ModuleServiceProxy
    {
        internal FastCgiItem[] GetApplications() => (FastCgiItem[])Invoke(nameof(GetApplications));
    }
}