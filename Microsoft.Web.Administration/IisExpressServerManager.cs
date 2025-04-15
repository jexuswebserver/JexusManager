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
            _logger.LogInformation("Getting state for site {SiteName} (ID: {SiteId})", site.Name, site.Id);
            
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
                
                _logger.LogInformation("Executing command to get site state: {Command} {Arguments}", 
                    process.StartInfo.FileName, process.StartInfo.Arguments);
                
                process.Start();
                _logger.LogInformation("State check process started with ID: {ProcessId}", process.Id);
                process.WaitForExit();
                _logger.LogInformation("State check process exited with code: {ExitCode}", process.ExitCode);

                bool isRunning = process.ExitCode > 0;
                _logger.LogInformation("Site {SiteName} state: {State}", site.Name, isRunning ? "Running" : "Stopped");
                return isRunning;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    _logger.LogWarning(ex, "Win32 error getting site state for {SiteName}. Native error code: {Code}", 
                        site.Name, ex.NativeErrorCode);
                }
                else
                {
                    _logger.LogInformation("User cancelled elevation prompt while checking site state for {SiteName}", site.Name);
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult != NativeMethods.NoProcessAssociated)
                {
                    _logger.LogError(ex, "Error getting site state for {SiteName}. HResult: {HResult}", site.Name, ex.HResult);
                }
                else
                {
                    _logger.LogInformation("No process associated with site {SiteName} (expected for stopped sites)", site.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting site state for {SiteName}", site.Name);
            }

            _logger.LogInformation("Site {SiteName} assumed to be stopped due to exception", site.Name);
            return false;
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
            return true;
        }

        internal override void Start(Site site)
        {
            _logger.LogInformation("Starting site {SiteName} (ID: {SiteId})", site.Name, site.Id);
            StartInner(site, false);
        }

        private void StartInner(Site site, bool restart)
        {
            _logger.LogInformation("{Action} site {SiteName} (ID: {SiteId})", restart ? "Restarting" : "Starting", site.Name, site.Id);
            
            if (site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated)
            {
                _logger.LogWarning("Site {SiteName} requires elevation but process is not elevated", site.Name);
                throw new InvalidOperationException("This site requires elevation. Please run Jexus Manager as administrator");
            }

            Application application = site.Applications[0];
            var actualExecutable = application.GetActualExecutable();
            _logger.LogInformation("Using executable: {Executable}", actualExecutable);
            
            var pool = application.GetPool();
            if (pool == null)
            {
                _logger.LogError("Application pool {PoolName} not found for site {SiteName}", application.ApplicationPoolName, site.Name);
                throw new InvalidOperationException($"The application pool {application.ApplicationPoolName} does not exist.");
            }
            
            var x64Tool = CertificateInstallerLocator.AlternativeFileName;
            var tool = x64Tool != null && !pool.Enable32BitAppOnWin64 && pool.EnableEmulationOnWinArm64
                ? x64Tool
                : CertificateInstallerLocator.FileName;
            _logger.LogInformation("Using tool: {Tool}", tool);
            
            var temp = Path.GetTempFileName();
            _logger.LogInformation("Using temp file for results: {TempFile}", temp);
            
            using var process = new Process();
            var start = process.StartInfo;
            start.FileName = "cmd";
            var extra = restart ? "/r" : string.Empty;
            var arguments = $"/c \"\"{tool}\" /launcher:\"{actualExecutable}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id} /resultFile:\"{temp}\"\" {extra}";
            start.Arguments = arguments;
            start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            
            _logger.LogInformation("Executing command: {FileName} {Arguments}", start.FileName, arguments);
            
            AspNetCoreHelper.InjectEnvironmentVariables(site, start, actualExecutable);

            try
            {
                process.Start();
                _logger.LogInformation("Process started with ID: {ProcessId}", process.Id);
                process.WaitForExit();
                _logger.LogInformation("Process exited with code: {ExitCode}", process.ExitCode);
                
                if (process.ExitCode > 0)
                {
                    site.State = ObjectState.Started;
                    _logger.LogInformation("Site {SiteName} started successfully (worker process ID: {ProcessId})", site.Name, process.ExitCode);
                }
                else if (process.ExitCode == 0)
                {
                    _logger.LogError("Process terminated unexpectedly for site {SiteName}", site.Name);
                    if (File.Exists(temp))
                    {
                        _logger.LogError("Result file contents: {Contents}", File.ReadAllText(temp));
                    }
                    throw new InvalidOperationException("The process has terminated");
                }

                pool.WorkerProcesses.Clear();
                var workerProcess = pool.WorkerProcesses.CreateElement();
                workerProcess["processId"] = process.ExitCode;
                workerProcess["appPoolName"] = pool.Name;
                _logger.LogInformation("Created worker process entry with ID: {ProcessId} for pool: {PoolName}", process.ExitCode, pool.Name);
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    string resultContent = File.Exists(temp) ? File.ReadAllText(temp) : "no result file available";
                    _logger.LogError(ex, "Win32 exception starting site {SiteName}: {Message}. Result file: {ResultContent}", 
                        site.Name, ex.Message, resultContent);
                    throw new COMException(
                        $"cannot start site: {ex.Message}, {resultContent}");
                }

                string cancelContent = File.Exists(temp) ? File.ReadAllText(temp) : "no result file available";
                _logger.LogWarning(ex, "Site start cancelled by user for {SiteName}: {Message}. Result file: {CancelContent}", 
                    site.Name, ex.Message, cancelContent);
                throw new COMException(
                    $"site start cancelled: {ex.Message}, {cancelContent}");
            }
            catch (Exception ex)
            {
                string exContent = File.Exists(temp) ? File.ReadAllText(temp) : "no result file available";
                _logger.LogError(ex, "Exception starting site {SiteName}: {Message}. Result file: {ExContent}", 
                    site.Name, ex.Message, exContent);
                throw new COMException(
                    $"cannot start site: {ex.Message}, {exContent}");
            }
            finally
            {
                site.State = process.ExitCode > 0 ? ObjectState.Started : ObjectState.Stopped;
                _logger.LogInformation("Set site {SiteName} state to {State}", site.Name, site.State);
                
                if (File.Exists(temp))
                {
                    try
                    {
                        File.Delete(temp);
                        _logger.LogInformation("Deleted temp file: {TempFile}", temp);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete temp file: {TempFile}", temp);
                    }
                }
            }
        }

        internal override void Stop(Site site)
        {
            _logger.LogInformation("Stopping site {SiteName} (ID: {SiteId})", site.Name, site.Id);
            
            try
            {
                using var process = new Process();
                var start = process.StartInfo;
                
                bool needsElevation = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated;
                start.Verb = needsElevation ? "runas" : null;
                
                if (needsElevation)
                {
                    _logger.LogInformation("Site {SiteName} requires elevation, launching with runas", site.Name);
                }
                
                start.UseShellExecute = true;
                start.FileName = "cmd";
                var arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /k /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"";
                start.Arguments = arguments;
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                
                _logger.LogInformation("Executing command to stop site: {FileName} {Arguments}", start.FileName, arguments);
                
                process.Start();
                _logger.LogInformation("Stop process started with ID: {ProcessId}", process.Id);
                process.WaitForExit();
                _logger.LogInformation("Stop process exited with code: {ExitCode}", process.ExitCode);

                if (process.ExitCode == 0)
                {
                    site.State = ObjectState.Stopped;
                    _logger.LogInformation("Site {SiteName} stopped successfully", site.Name);
                }
                else
                {
                    _logger.LogWarning("Stop operation for site {SiteName} returned non-zero exit code: {ExitCode}", 
                        site.Name, process.ExitCode);
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    _logger.LogError(ex, "Win32 error stopping site {SiteName}. Native error code: {Code}", site.Name, ex.NativeErrorCode);
                }
                else
                {
                    _logger.LogWarning(ex, "Stop operation was cancelled by user for site {SiteName}", site.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping site {SiteName}", site.Name);
            }
        }

        internal override void Restart(Site site)
        {
            _logger.LogInformation("Restarting site {SiteName} (ID: {SiteId})", site.Name, site.Id);
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
