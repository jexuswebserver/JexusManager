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

        public static Version GetIisExpressVersion()
        {
            var fileName =
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "IIS Express",
                    "iisexpress.exe");
            if (!File.Exists(fileName))
            {
                fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express",
                    "iisexpress.exe");
            }

            if (File.Exists(fileName))
            {
                if (Version.TryParse(FileVersionInfo.GetVersionInfo(fileName).ProductVersion, out Version result))
                {
                    return result;
                }
            }

            return Version.Parse("0.0.0.0");
        }
    }
}
