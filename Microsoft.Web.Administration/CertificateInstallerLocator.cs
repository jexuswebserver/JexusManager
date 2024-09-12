using System;
using System.IO;

namespace Microsoft.Web.Administration
{
    public static class CertificateInstallerLocator
    {
        public static string FileName
        {
            get { return FindMainHelper("certificateinstaller.exe"); }
        }

        public static string AlternativeFileName
        {
            get { return FindMainHelper("certificateinstaller.x64.exe"); }
        }

        private static string FindMainHelper(string executable)
        {
            var defaultPath = Path.Combine(Environment.CurrentDirectory, executable);
            if (File.Exists(defaultPath))
            {
                return defaultPath;
            }

            var debugBuild = Path.Combine(Environment.CurrentDirectory, $@"..\..\..\..\CertificateInstaller\bin\Debug\net8.0-windows\{executable}");
            if (File.Exists(debugBuild))
            {
                return debugBuild;
            }

            var releaseBuild = Path.Combine(Environment.CurrentDirectory, $@"..\..\..\..\CertificateInstaller\bin\Release\net8.0-windows\{executable}");
            if (File.Exists(releaseBuild))
            {
                return releaseBuild;
            }

            var debugBuildFx = Path.Combine(Environment.CurrentDirectory, $@"..\..\..\CertificateInstaller\bin\Debug\{executable}");
            if (File.Exists(debugBuildFx))
            {
                return debugBuildFx;
            }

            var releaseBuildFx = Path.Combine(Environment.CurrentDirectory, $@"..\..\..\CertificateInstaller\bin\Release\{executable}");
            if (File.Exists(releaseBuildFx))
            {
                return releaseBuildFx;
            }

            return null;
        }
    }
}
