using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Authorization
{
    internal sealed class AuthorizationModuleProxy : ModuleServiceProxy
    {
        internal AuthorizationRule[] GetRules() => (AuthorizationRule[])Invoke(nameof(GetRules));
        internal void Add(AuthorizationRule rule) => Invoke(nameof(Add), rule);
        internal void Update(AuthorizationRule original, AuthorizationRule rule) => Invoke(nameof(Update), original, rule);
        internal void Remove(AuthorizationRule rule) => Invoke(nameof(Remove), rule);
    }
}
