using System;
using System.Linq;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.HttpRedirect
{
    internal sealed class HttpRedirectService : ModuleService
    {
        private const string SectionPath = "system.webServer/httpRedirect";

        [ModuleServiceMethod]
        public HttpRedirectSnapshot GetSettings()
        {
            var section = GetSection();
            return new HttpRedirectSnapshot
            {
                Enabled = (bool)section["enabled"], Destination = (string)section["destination"],
                ExactDestination = (bool)section["exactDestination"], ChildOnly = (bool)section["childOnly"],
                ResponseStatus = (long)section["httpResponseStatus"],
                SupportedStatuses = section.Schema.AttributeSchemas["httpResponseStatus"].GetEnumValues().Select(item => item.Value).ToArray()
            };
        }

        [ModuleServiceMethod]
        public void Apply(HttpRedirectSnapshot settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var section = GetSection();
            section["enabled"] = settings.Enabled;
            section["destination"] = settings.Destination;
            section["exactDestination"] = settings.ExactDestination;
            section["childOnly"] = settings.ChildOnly;
            section["httpResponseStatus"] = settings.SupportedStatuses.Contains(settings.ResponseStatus) ? settings.ResponseStatus : 301;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection() => ManagementUnit.Configuration.GetSection(SectionPath);
    }
}