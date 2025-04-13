// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using JexusManager;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace Microsoft.Web.Administration
{
    public sealed class IisExpressServerManager : ServerManager
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("IisExpressServerManager");

        public Version Version { get; }

        public string PrimaryExecutable { get; }

        public override bool SupportsSni => Version >= Version.Parse("8.0") && Environment.OSVersion.Version >= Version.Parse("6.2");

        public override bool SupportsWildcard => Version >= Version.Parse("10.0") && Environment.OSVersion.Version >= Version.Parse("10.0");

        public static bool ServerInstalled
        {
            get
            {
                return GetPrimaryExecutable() != null;
            }
        }

        public IisExpressServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        public IisExpressServerManager(bool readOnly, string applicationHostConfigurationPath)
            : base(readOnly, applicationHostConfigurationPath)
        {
            Mode = WorkingMode.IisExpress;
            PrimaryExecutable = GetPrimaryExecutable();
            Version = GetIisExpressVersion();
        }

        private static string GetPrimaryExecutable()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "IIS Express");
            if (!Directory.Exists(directory))
            {
                // IMPORTANT: for x86 IIS 7 Express
                directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express");
                if (!Directory.Exists(directory))
                {
                    return null;
                }
            }

            var fileName = Path.Combine(directory, "iisexpress.exe");
            return File.Exists(fileName) ? fileName : null;
        }

        private Version GetIisExpressVersion()
        {
            if (PrimaryExecutable != null && File.Exists(PrimaryExecutable))
            {
                if (Version.TryParse(FileVersionInfo.GetVersionInfo(PrimaryExecutable).ProductVersion, out Version result))
                {
                    return result;
                }
            }

            return Version.Parse("0.0.0.0");
        }

        internal override bool GetSiteState(Site site)
        {
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                            ? "runas"
                            : null,
                        UseShellExecute = true,
                        FileName = "cmd",
                        Arguments =
                            $"/c \"\"{CertificateInstallerLocator.FileName}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                process.Start();
                process.WaitForExit();

                return process.ExitCode > 0;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    _logger.LogWarning(ex, "Win32 error getting site state. Native error code: {Code}", ex.NativeErrorCode);
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult != NativeMethods.NoProcessAssociated)
                {
                    _logger.LogError(ex, "Error getting site state. HResult: {HResult}", ex.HResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting site state");
            }

            return false;
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
            return true;
        }

        internal override void Start(Site site)
        {
            StartInner(site, false);
        }

        private void StartInner(Site site, bool restart)
        {
            if (site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated)
            {
                throw new InvalidOperationException("This site requires elevation. Please run Jexus Manager as administrator");
            }

            Application application = site.Applications[0];
            var actualExecutable = application.GetActualExecutable();
            var pool = application.GetPool() ?? throw new InvalidOperationException($"The application pool {application.ApplicationPoolName} does not exist.");
            var x64Tool = CertificateInstallerLocator.AlternativeFileName;
            var tool = x64Tool != null && !pool.Enable32BitAppOnWin64 && pool.EnableEmulationOnWinArm64
                ? x64Tool
                : CertificateInstallerLocator.FileName;
            var temp = Path.GetTempFileName();
            using var process = new Process();
            var start = process.StartInfo;
            start.FileName = "cmd";
            var extra = restart ? "/r" : string.Empty;
            start.Arguments =
                $"/c \"\"{tool}\" /launcher:\"{actualExecutable}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id} /resultFile:\"{temp}\"\" {extra}";
            start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            AspNetCoreHelper.InjectEnvironmentVariables(site, start, actualExecutable);

            try
            {
                process.Start();
                process.WaitForExit();
                if (process.ExitCode > 0)
                {
                    site.State = ObjectState.Started;
                }
                else if (process.ExitCode == 0)
                {
                    throw new InvalidOperationException("The process has terminated");
                }

                pool.WorkerProcesses.Clear();
                var workerProcess = pool.WorkerProcesses.CreateElement();
                workerProcess["processId"] = process.ExitCode;
                workerProcess["appPoolName"] = pool.Name;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    throw new COMException(
                        $"cannot start site: {ex.Message}, {File.ReadAllText(temp)}");
                }

                throw new COMException(
                    $"site start cancelled: {ex.Message}, {File.ReadAllText(temp)}");
            }
            catch (Exception ex)
            {
                throw new COMException(
                    $"cannot start site: {ex.Message}, {File.ReadAllText(temp)}");
            }
            finally
            {
                site.State = process.ExitCode > 0 ? ObjectState.Started : ObjectState.Stopped;
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                }
            }
        }

        internal override void Stop(Site site)
        {
            try
            {
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                    ? "runas"
                    : null;
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{CertificateInstallerLocator.FileName}\" /k /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    site.State = ObjectState.Stopped;
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    _logger.LogError(ex, "Win32 error stopping site. Native error code: {Code}", ex.NativeErrorCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping site", ex);
            }
        }

        internal override void Restart(Site site)
        {
            StartInner(site, true);
        }

        internal override IEnumerable<string> GetSchemaFiles()
        {
            var extra = Path.Combine(Environment.CurrentDirectory, "schemas");
            var files = Directory.Exists(extra) ? Directory.GetFiles(extra) : Array.Empty<string>();

            // IMPORTANT: for x64 IIS 7 Express
            var directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "IIS Express",
                    "config",
                    "schema");
            if (Directory.Exists(directory))
            {
                return Directory.GetFiles(directory).Concat(files);
            }

            // IMPORTANT: for x86 IIS 7 Express
            var x86 = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "IIS Express",
                "config",
                "schema");
            return Directory.Exists(x86) ? Directory.GetFiles(x86) : base.GetSchemaFiles();
        }
    }
}
