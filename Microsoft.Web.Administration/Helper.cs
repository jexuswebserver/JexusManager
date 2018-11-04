// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Web.Administration
{
    internal static class Helper
    {
        internal static readonly string RootPath = "/";

        public static readonly string FileNameMachineConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/machine.config"
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                IntPtr.Size == 2 ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "machine.config");

        public static readonly string FileNameWebConfig = IsRunningOnMono()
            ? "/Library/Frameworks/Mono.framework/Versions/Current/etc/mono/4.5/web.config"
            : Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Microsoft.NET",
                IntPtr.Size == 2 ? "Framework" : "Framework64",
                "v4.0.30319",
                "CONFIG",
                "web.config");

        public static string ExpandIisExpressEnvironmentVariables(this string path)
        {
            // TODO: IIS_BIN should check pool bitness.
            return Environment.ExpandEnvironmentVariables(path.Replace("%IIS_SITES_HOME%",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Web Sites"))
                .Replace("%IIS_USER_HOME%",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IISExpress"))
                .Replace("%IIS_BIN%",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express"))
                );
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
