using System;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Compression
{
    internal sealed class CompressionService : ModuleService
    {
        [ModuleServiceMethod]
        public CompressionSnapshot GetSettings()
        {
            var url = ManagementUnit.Configuration.GetSection("system.webServer/urlCompression");
            var result = new CompressionSnapshot { StaticEnabled = (bool)url["doStaticCompression"], DynamicEnabled = (bool)url["doDynamicCompression"], HasServerSettings = ManagementUnit.Scope == ManagementScope.Server };
            if (result.HasServerSettings)
            {
                var http = ManagementUnit.Configuration.GetSection("system.webServer/httpCompression");
                result.DoDiskSpaceLimiting = (bool)http["doDiskSpaceLimiting"];
                result.MaxDiskSpaceUsage = http["maxDiskSpaceUsage"].ToString();
                result.Directory = http["directory"].ToString();
                result.MinFileSizeForComp = http["minFileSizeForComp"].ToString();
            }

            return result;
        }

        [ModuleServiceMethod]
        public void Apply(CompressionSnapshot settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            var url = ManagementUnit.Configuration.GetSection("system.webServer/urlCompression");
            url["doStaticCompression"] = settings.StaticEnabled;
            url["doDynamicCompression"] = settings.DynamicEnabled;
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                if (!uint.TryParse(settings.MinFileSizeForComp, out var fileSize)) throw new ArgumentException("Invalid minimum file size.", nameof(settings));
                if (!uint.TryParse(settings.MaxDiskSpaceUsage, out var diskSpace) || diskSpace > int.MaxValue) throw new ArgumentException("Invalid maximum disk space usage.", nameof(settings));
                var http = ManagementUnit.Configuration.GetSection("system.webServer/httpCompression");
                http["doDiskSpaceLimiting"] = settings.DoDiskSpaceLimiting;
                http["maxDiskSpaceUsage"] = diskSpace;
                http["directory"] = settings.Directory;
                http["minFileSizeForComp"] = fileSize;
            }

            ManagementUnit.Update();
        }
    }
}