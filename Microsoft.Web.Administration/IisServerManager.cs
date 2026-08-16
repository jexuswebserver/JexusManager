// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Server manager for IIS.
    /// </summary>
    public sealed class IisServerManager : ServerManager
    {
        public override bool SupportsSni => Environment.OSVersion.Version >= Version.Parse("6.2");
        public override bool SupportsWildcard => Environment.OSVersion.Version >= Version.Parse("10.0");

        public IisServerManager()
            : this(null, true)
        {
        }

        public IisServerManager(string hostName, bool local)
            : this(hostName)
        {
        }

        public IisServerManager(bool readOnly, string applicationHostConfigurationPath)
            : this("localhost", readOnly, applicationHostConfigurationPath)
        {
        }

        public IisServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        internal IisServerManager(string hostName, bool readOnly, string fileName)
            : base(hostName, readOnly, fileName)
        {
            Mode = WorkingMode.Iis;
        }

        internal override bool GetSiteState(Site site)
        {
            var output = RunAppCmd("list site /state:Started");
            return output != null && output.Contains($"SITE \"{site.Name}\"");
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
            var output = RunAppCmd("list apppool /state:Started");
            return output != null && output.Contains($"APPPOOL \"{pool.Name}\"");
        }

        internal override void Start(Site site)
        {
            RunAppCmd("start site \"" + site.Name + "\"");
        }

        internal override void Stop(Site site)
        {
            RunAppCmd("stop site \"" + site.Name + "\"");
        }

        internal override void Start(ApplicationPool pool)
        {
            RunAppCmd("start apppool \"" + pool.Name + "\"");
        }

        internal override void Stop(ApplicationPool pool)
        {
            RunAppCmd("stop apppool \"" + pool.Name + "\"");
        }

        internal override void Recycle(ApplicationPool pool)
        {
            RunAppCmd("recycle apppool \"" + pool.Name + "\"");
        }

        /// <summary>
        /// Runs an appcmd-style command. Read operations (list) never elevate.
        /// Write operations (start/stop/recycle) are elevated only when the current
        /// process is not running as administrator. The dedicated command line tool
        /// (<c>JexusManager.AppCmd.exe</c>) is used when present, because an elevated
        /// copy can still redirect its output through /resultFile (the real appcmd
        /// cannot), and it falls back to the real appcmd.exe.
        /// </summary>
        private static string RunAppCmd(string arguments)
        {
            var tool = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JexusManager.AppCmd.exe");
            if (File.Exists(tool))
            {
                return PublicNativeMethods.IsProcessElevated
                    ? RunRedirected(tool, arguments)
                    : arguments.StartsWith("list", StringComparison.OrdinalIgnoreCase)
                        ? RunRedirected(tool, arguments)
                        : RunElevated(tool, arguments);
            }

            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                return null;
            }

            return PublicNativeMethods.IsProcessElevated || arguments.StartsWith("list", StringComparison.OrdinalIgnoreCase)
                ? RunRedirected(appcmd, arguments)
                : RunElevated(appcmd, arguments);
        }

        private static string RunRedirected(string fileName, string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private static string RunElevated(string fileName, string arguments)
        {
            var resultFile = Path.GetTempFileName();
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = $"{arguments} /resultFile:\"{resultFile}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
                return File.Exists(resultFile) ? File.ReadAllText(resultFile) : null;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    throw;
                }

                return null;
            }
            finally
            {
                if (File.Exists(resultFile))
                {
                    File.Delete(resultFile);
                }
            }
        }

        internal override IEnumerable<string> GetSchemaFiles()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "inetsrv",
                "config",
                "schema");
            return Directory.Exists(directory) ? Directory.GetFiles(directory) : base.GetSchemaFiles();
        }
    }
}
