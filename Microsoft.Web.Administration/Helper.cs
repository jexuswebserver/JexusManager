// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.Web.Administration
{
    internal static class Helper
    {
        internal static readonly string RootPath = "/";

        public static string ExpandIisExpressEnvironmentVariables(this string path)
        {
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
