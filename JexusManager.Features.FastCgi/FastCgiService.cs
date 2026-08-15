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

        [ModuleServiceMethod]
        public void Add(FastCgiItem item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            var collection = ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection("system.webServer/fastCgi").GetCollection();
            var element = collection.CreateElement();
            element["fullPath"] = item.Path;
            element["arguments"] = item.Arguments;
            element["monitorChangesTo"] = item.MonitorChangesTo;
            element["stderrMode"] = item.ErrorMode;
            element["maxInstances"] = item.MaxInstances;
            element["idleTimeout"] = item.IdleTimeout;
            element["activityTimeout"] = item.ActivityTimeout;
            element["requestTimeout"] = item.RequestTimeout;
            element["instanceMaxRequests"] = item.InstanceMaxRequests;
            element["signalBeforeTerminateSeconds"] = item.SignalBeforeTerminateSeconds;
            element["protocol"] = item.AdvancedSettings.Protocol;
            element["queueLength"] = item.QueueLength;
            element["flushNamedPipe"] = item.AdvancedSettings.FlushNamedPipe;
            element["rapidFailsPerMinute"] = item.RapidFailsPerMinute;
            collection.Add(element);
            ManagementUnit.Update();
        }
    }
}
