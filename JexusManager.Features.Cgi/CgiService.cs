using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Cgi
{
    internal sealed class CgiService : ModuleService
    {
        private const string SectionPath = "system.webServer/cgi";

        [ModuleServiceMethod]
        public CgiItem GetSettings()
        {
            var section = GetSection();
            return new CgiItem { CreateCgiWithNewConsole = (bool)section["createCGIWithNewConsole"], CreateProcessAsUser = (bool)section["createProcessAsUser"], Timeout = (TimeSpan)section["timeout"] };
        }

        [ModuleServiceMethod]
        public void Apply(CgiItem settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var section = GetSection();
            section["createCGIWithNewConsole"] = settings.CreateCgiWithNewConsole;
            section["createProcessAsUser"] = settings.CreateProcessAsUser;
            section["timeout"] = settings.Timeout;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection() => ManagementUnit.Configuration.GetSection(SectionPath);
    }
}
