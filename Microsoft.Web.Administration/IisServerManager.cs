// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
#if !__MonoCS__
    using System.Management.Automation;
#endif
    /// <summary>
    /// Server manager for IIS.
    /// </summary>
    public sealed class IisServerManager : ServerManager
    {
        public override bool SupportsSni => Environment.OSVersion.Version >= Version.Parse("6.2");

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
#if !__MonoCS__
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                var path = Environment.ExpandEnvironmentVariables(
                    "%windir%\\system32\\inetsrv\\Microsoft.Web.Administration.dll");
                    // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                    // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                PowerShellInstance.AddScript($"param($param1) [Reflection.Assembly]::LoadFrom('{path}'); Get-IISsite -Name \"$param1\"");

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                PowerShellInstance.AddParameter("param1", site.Name);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // check the other output streams (for example, the error stream)
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // do something with the items found.
                    return false;
                }

                if (PSOutput.Count < 2)
                {
                    // TODO: newly created sites go here. Why?
                    return false;
                }

                dynamic site1 = PSOutput[1];
                return site1.State?.ToString() == "Started";
            }
#else
                return false;
#endif
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
#if !__MonoCS__
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                // use "AddScript" to add the contents of a script file to the end of the execution pipeline.
                // use "AddCommand" to add individual commands/cmdlets to the end of the execution pipeline.
                PowerShellInstance.AddScript("param($param1) [Reflection.Assembly]::LoadFrom('C:\\Windows\\system32\\inetsrv\\Microsoft.Web.Administration.dll'); Get-IISAppPool -Name \"$param1\"");

                // use "AddParameter" to add a single parameter to the last command/script on the pipeline.
                PowerShellInstance.AddParameter("param1", pool.Name);

                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();

                // check the other output streams (for example, the error stream)
                if (PowerShellInstance.Streams.Error.Count > 0)
                {
                    // error records were written to the error stream.
                    // do something with the items found.
                    return false;
                }

                if (PSOutput.Count < 2)
                {
                    return false;
                }

                dynamic site = PSOutput[1];
                return site.State?.ToString() == "Started";
            }
#else
            return false;
#endif
        }

        internal override void Start(Site site)
        {
        }

        internal override void Stop(Site site)
        {
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
