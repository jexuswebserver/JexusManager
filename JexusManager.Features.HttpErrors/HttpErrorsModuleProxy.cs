using Microsoft.Web.Management.Client;

namespace JexusManager.Features.HttpErrors
{
    internal sealed class HttpErrorsModuleProxy : ModuleServiceProxy
    {
        internal HttpErrorsSettings GetSettings()
        {
            return (HttpErrorsSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(HttpErrorsSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }
    }
}
