// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using Rollbar;
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
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                return false;
            }

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appcmd,
                    Arguments = $"list site /state:Started",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Contains($"SITE \"{site.Name}\"");
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                return false;
            }

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = appcmd,
                    Arguments = $"list apppool /state:Started",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Contains($"APPPOOL \"{pool.Name}\"");
        }

        internal override void Start(Site site)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (File.Exists(appcmd))
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"start site \"{site.Name}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }

        internal override void Stop(Site site)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (File.Exists(appcmd))
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"stop site \"{site.Name}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }

        internal override void Start(ApplicationPool pool)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (File.Exists(appcmd))
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"start apppool \"{pool.Name}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }

        internal override void Stop(ApplicationPool pool)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (File.Exists(appcmd))
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"stop apppool \"{pool.Name}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
        }

        internal override void Recycle(ApplicationPool pool)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (File.Exists(appcmd))
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"recycle apppool \"{pool.Name}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
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

        internal override void SetPassword(VirtualDirectory virtualDirectory, string password)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                // IMPORTANT: fallback to default password setting. Should throw encryption exception.
                virtualDirectory.Password = password;
                return;
            }

            // IMPORTANT: save vdir to config file.
            {
                var command = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /-password";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:{resultFile} /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }

            {
                if (string.IsNullOrEmpty(password))
                {
                    return;
                }

                var command = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /password:{password}";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:{resultFile} /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }
        }

        internal override void SetPassword(ApplicationPoolProcessModel processModel, string password)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                // IMPORTANT: fallback to default password setting. Should throw encryption exception.
                processModel.Password = password;
                return;
            }

            // IMPORTANT: save vdir to config file.
            {
                var command = $"set apppool \"{processModel.ParentElement["name"]}\" /-processModel.password";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:{resultFile} /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }

            {
                if (string.IsNullOrEmpty(password))
                {
                    return;
                }

                var command = $"set apppool \"{processModel.ParentElement["name"]}\" /processModel.password:{password}";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:{resultFile} /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }
        }
    }
}
