using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.FastCgi
{
    internal sealed class FastCgiService : ModuleService
    {
        [ModuleServiceMethod]
        public FastCgiItem[] GetApplications()
        {
            var result = new List<FastCgiItem>();
            var collection = ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection("system.webServer/fastCgi").GetCollection();
            foreach (ConfigurationElement element in collection)
            {
                result.Add(new FastCgiItem(element));
            }

            return result.ToArray();
        }
    }
}