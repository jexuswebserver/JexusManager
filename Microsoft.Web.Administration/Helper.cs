// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;

namespace Microsoft.Web.Administration
{
    internal static class Helper
    {
        internal static readonly string RootPath = "/";
        private static bool ProcessIs32Bit = IntPtr.Size == 4;

        public static readonly string FileNameMachineConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/machine.config"
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                ProcessIs32Bit ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "machine.config");

        public static readonly string FileNameWebConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/web.config"
            : Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                ProcessIs32Bit ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "web.config");

        public static string ExpandIisExpressEnvironmentVariables(this string path, string executable)
        {
            var binFolder = executable == null
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express")
                : Path.GetDirectoryName(executable);
            return Environment.ExpandEnvironmentVariables(path.Replace("%IIS_SITES_HOME%",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Web Sites"))
                .Replace("%IIS_USER_HOME%",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IISExpress"))
                .Replace("%IIS_BIN%", binFolder));
        }

        public static string GetActualExecutable(this Application application)
        {
            if (application.Server.Mode != WorkingMode.IisExpress)
            {
                return null;
            }

            var name = application.ApplicationPoolName;
            var pool = application.Server.ApplicationPools.FirstOrDefault(item => item.Name == name);
            var result = Path.Combine(
                Environment.GetFolderPath(
                    pool != null && pool.Enable32BitAppOnWin64
                        ? Environment.SpecialFolder.ProgramFilesX86
                        : Environment.SpecialFolder.ProgramFiles),
                "IIS Express",
                "iisexpress.exe");
            if (!File.Exists(result))
            {
                // fall back to 32 bit (IIS 7.5 Express)
                result = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express",
                    "iisexpress.exe");
            }

            return result;
        }

        public static bool IsRoot(this Application application)
        {
            return application.Path == RootPath;
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
