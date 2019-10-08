// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using System.Collections.Generic;
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

            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /-password",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception(process.ExitCode.ToString());
                }
            }

            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = appcmd,
                        Arguments = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /password:{password}",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception(process.ExitCode.ToString());
                }
            }
        }
    }
}
