using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.HttpErrors
{
    internal sealed class HttpErrorsService : ModuleService
    {
        private const string SectionPath = "system.webServer/httpErrors";

        [ModuleServiceMethod]
        public HttpErrorsSettings GetSettings()
        {
            var section = GetSection();
            return new HttpErrorsSettings
            {
                ErrorMode = (long)section["errorMode"],
                DefaultResponseMode = (long)section["defaultResponseMode"],
                DefaultPath = (string)section["defaultPath"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(HttpErrorsSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection();
            section["errorMode"] = settings.ErrorMode;
            section["defaultResponseMode"] = settings.DefaultResponseMode;
            section["defaultPath"] = settings.DefaultPath;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }
    }
}
