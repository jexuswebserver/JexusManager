using Microsoft.Web.Management.Client;

namespace JexusManager.Features.FastCgi
{
    internal sealed class FastCgiModuleProxy : ModuleServiceProxy
    {
        internal FastCgiItem[] GetApplications() => (FastCgiItem[])Invoke(nameof(GetApplications));

        internal void Add(FastCgiItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Update(FastCgiItem original, FastCgiItem item)
        {
            Invoke(nameof(Update), original, item);
        }

        internal void Remove(FastCgiItem item)
        {
            Invoke(nameof(Remove), item);
        }
    }
}
