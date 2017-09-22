// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Compression
{
    using System;
    using System.Diagnostics;
    using System.Resources;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using System.Windows.Forms;

    internal class CompressionFeature
    {
        public CompressionFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var urlCompressionSection = service.GetSection("system.webServer/urlCompression");
            StaticEnabled = (bool)urlCompressionSection["doStaticCompression"];
            DynamicEnabled = (bool)urlCompressionSection["doDynamicCompression"];
            if (service.Server != null)
            {
                var httpCompressionSection = service.GetSection("system.webServer/httpCompression");
                DoDiskSpaceLimiting = (bool)httpCompressionSection["doDiskSpaceLimiting"];
                MaxDiskSpaceUsage = httpCompressionSection["maxDiskSpaceUsage"].ToString();
                Directory = httpCompressionSection["directory"].ToString();
                MinFileSizeForComp = httpCompressionSection["minFileSizeForComp"].ToString();
                DoFileSize = MinFileSizeForComp != "0";
            }

            OnCompressionSettingsSaved();
        }

        public string Directory { get; set; }

        protected void OnCompressionSettingsSaved()
        {
            CompressionSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210466");
            return false;
        }

        public CompressionSettingsSavedEventHandler CompressionSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }
        public string Name
        {
            get
            {
                return "Compression";
            }
        }

        public string MinFileSizeForComp { get; set; }
        public string MaxDiskSpaceUsage { get; set; }
        public bool DoDiskSpaceLimiting { get; set; }
        public bool DynamicEnabled { get; set; }
        public bool StaticEnabled { get; set; }

        public string FileSize
        {
            get { return DoFileSize ? MinFileSizeForComp : "0"; }
        }

        public bool DoFileSize { get; set; }

        public void CancelChanges()
        {
            Load();
        }

        public bool ApplyChanges()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var urlCompressionSection = service.GetSection("system.webServer/urlCompression");
            urlCompressionSection["doStaticCompression"] = StaticEnabled;
            urlCompressionSection["doDynamicCompression"] = DynamicEnabled;

            if (service.Server != null)
            {
                uint fileSize;
                if (!uint.TryParse(MinFileSizeForComp, out fileSize) || fileSize > 4294967295)
                {
                    var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                    dialog.ShowMessage($"'{MinFileSizeForComp}' is an invalid value for file size. The value must be between 0 and 4294967295.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                uint diskspace;
                if (!uint.TryParse(MaxDiskSpaceUsage, out diskspace) || diskspace > 2147483647)
                {
                    var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                    dialog.ShowMessage($"'{MaxDiskSpaceUsage}' is an invalid value for maximum disk space usage. The value must be between 0 and 2147483647.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var httpCompressionSection = service.GetSection("system.webServer/httpCompression");
                httpCompressionSection["doDiskSpaceLimiting"] = DoDiskSpaceLimiting;
                httpCompressionSection["maxDiskSpaceUsage"] = diskspace;
                httpCompressionSection["directory"] = Directory;
                httpCompressionSection["minFileSizeForComp"] = fileSize;
            }

            service.ServerManager.CommitChanges();
            return true;
        }
    }
}
