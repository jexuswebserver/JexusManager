using Microsoft.Web.Management.Client;

namespace JexusManager.Features.ResponseHeaders
{
    internal sealed class ResponseHeadersModuleProxy : ModuleServiceProxy
    {
        internal ResponseHeadersItem[] GetItems() => (ResponseHeadersItem[])Invoke(nameof(GetItems));
        internal void Add(ResponseHeadersItem item) => Invoke(nameof(Add), item);
        internal void Update(ResponseHeadersItem oldItem, ResponseHeadersItem item) => Invoke(nameof(Update), oldItem, item);
        internal void Remove(ResponseHeadersItem item) => Invoke(nameof(Remove), item);
    }
}