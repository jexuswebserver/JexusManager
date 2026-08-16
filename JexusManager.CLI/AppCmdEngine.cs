// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JexusManager.AppCmd
{
    /// <summary>
    /// A dedicated command line tool that clones the IIS appcmd behavior.
    /// In IIS mode all commands are forwarded to the real appcmd.exe and its exact
    /// output and exit code are relayed, so the behavior is identical. The
    /// /resultFile option allows the output to be captured from an elevated process
    /// (the real appcmd cannot redirect while elevated). In IIS Express mode the
    /// commands are implemented locally with the same output format and exit codes.
    /// Read operations (list) never require elevation.
    /// </summary>
    public static class AppCmdEngine
    {
        private const string DefaultIisConfig = @"C:\Windows\System32\inetsrv\config\applicationHost.config";

        public static int Run(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                WriteOutput(HelpTexts.Usage, string.Empty);
                return 87;
            }

            var mode = "iis";
            var config = string.Empty;
            var resultFile = string.Empty;
            var forwarded = new List<string>();
            foreach (var argument in args)
            {
                if (argument.StartsWith("/", StringComparison.Ordinal))
                {
                    var colon = argument.IndexOf(':');
                    var key = colon < 0 ? argument.Substring(1) : argument.Substring(1, colon - 1);
                    if (key == "mode")
                    {
                        mode = colon < 0 ? string.Empty : argument.Substring(colon + 1);
                        continue;
                    }

                    if (key == "config")
                    {
                        config = colon < 0 ? string.Empty : argument.Substring(colon + 1);
                        continue;
                    }

                    if (key == "resultFile")
                    {
                        resultFile = colon < 0 ? string.Empty : argument.Substring(colon + 1);
                        continue;
                    }
                }

                forwarded.Add(argument);
            }

            if (string.Equals(mode, "iisexpress", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(config))
                {
                    var express = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        "IISExpress",
                        "config",
                        "applicationhost.config");
                    config = File.Exists(express) ? express : DefaultIisConfig;
                }

                return ExecuteIisExpress(forwarded, config, resultFile);
            }

            return ExecuteIis(forwarded, resultFile);
        }

        /// <summary>
        /// IIS mode: forward the command to the real appcmd.exe and relay its exact
        /// output and exit code.
        /// </summary>
        private static int ExecuteIis(List<string> args, string resultFile)
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                WriteOutput("ERROR ( message:appcmd.exe was not found. Install IIS, or pass /mode:iisexpress. )\r\n", resultFile);
                return 1168;
            }

            var builder = new StringBuilder();
            foreach (var argument in args)
            {
                builder.Append(' ');
                if (argument.Contains(' '))
                {
                    builder.Append('"').Append(argument).Append('"');
                }
                else
                {
                    builder.Append(argument);
                }
            }

            var output = RunRedirected(appcmd, builder.ToString().TrimStart(), out var exitCode);
            WriteOutput(output, resultFile);
            return exitCode;
        }

        private static int ExecuteIisExpress(List<string> args, string config, string resultFile)
        {
            if (args.Count == 0)
            {
                WriteOutput(HelpTexts.Usage, resultFile);
                return 87;
            }

            if (args[0] == "/?")
            {
                WriteOutput(HelpTexts.Usage, resultFile);
                return 1;
            }

            if (args.Count == 1)
            {
                var normalized = NormalizeObject(args[0]);
                if (normalized != null)
                {
                    WriteOutput(CommandNotSupported(string.Empty, normalized), resultFile);
                }
                else
                {
                    WriteOutput(ObjectNotSupported(args[0]), resultFile);
                }

                return 87;
            }

            var verb = args[0];
            var objectName = args[1];
            var normalizedObject = NormalizeObject(objectName);
            if (objectName == "/?")
            {
                if (args.Count == 2)
                {
                    var helpObject = NormalizeObject(verb);
                    if (helpObject != null)
                    {
                        WriteOutput(GetObjectHelp(helpObject), resultFile);
                        return 1;
                    }

                    WriteOutput(ObjectNotSupported(verb), resultFile);
                    return 87;
                }

                return ShowVerbHelp(verb, normalizedObject, resultFile);
            }

            if (args.Count >= 3 && args[2] == "/?")
            {
                return ShowVerbHelp(verb, normalizedObject, resultFile);
            }

            if (normalizedObject == null)
            {
                WriteOutput(ObjectNotSupported(objectName), resultFile);
                return 87;
            }

            if (!IsKnownVerb(verb))
            {
                WriteOutput(CommandNotSupported(verb, normalizedObject), resultFile);
                return 87;
            }

            var name = string.Empty;
            var stateFilter = string.Empty;
            for (var index = 2; index < args.Count; index++)
            {
                var argument = args[index];
                if (argument.StartsWith("/", StringComparison.Ordinal))
                {
                    var colon = argument.IndexOf(':');
                    var key = colon < 0 ? argument.Substring(1) : argument.Substring(1, colon - 1);
                    var value = colon < 0 ? string.Empty : argument.Substring(colon + 1);
                    if (key == "site.name" || key == "app.name" || key == "vdir.name" || key == "apppool.name" || key == "name")
                    {
                        name = value;
                    }
                    else if (key == "state")
                    {
                        stateFilter = value;
                    }
                }
                else
                {
                    name = string.IsNullOrEmpty(name) ? argument : name + " " + argument;
                }
            }

            if (verb == "list")
            {
                var output = ListObjects(normalizedObject, name, stateFilter, config);
                var exitCode = string.IsNullOrEmpty(output) && !string.IsNullOrEmpty(name) ? 1 : 0;
                WriteOutput(output, resultFile);
                return exitCode;
            }

            if (normalizedObject == "APPPOOL" && (verb == "start" || verb == "stop" || verb == "recycle"))
            {
                WriteOutput("ERROR ( message:Application pools are not supported by IIS Express. )\r\n", resultFile);
                return 1168;
            }

            if (normalizedObject != "SITE" || (verb != "start" && verb != "stop"))
            {
                WriteOutput(CommandNotSupported(verb, normalizedObject), resultFile);
                return 87;
            }

            if (string.IsNullOrEmpty(name))
            {
                WriteOutput(MustSpecify(normalizedObject), resultFile);
                return 87;
            }

            var site = FindSite(config, name);
            if (site == null)
            {
                WriteOutput(CannotFind(normalizedObject, name), resultFile);
                return 1168;
            }

            var success = verb == "start" ? StartSite(config, site.Id) : StopSite(config, site.Id);
            if (!success)
            {
                WriteOutput("ERROR ( message:iisexpress.exe was not found. Install IIS Express to use this mode. )\r\n", resultFile);
                return 1168;
            }

            WriteOutput($"\"{site.Name}\" successfully {(verb == "start" ? "started." : "stopped")}\r\n", resultFile);
            return 0;
        }

        private static int ShowVerbHelp(string verb, string normalizedObject, string resultFile)
        {
            var help = GetVerbHelp(verb, normalizedObject);
            if (help != null)
            {
                WriteOutput(help, resultFile);
                return 1;
            }

            WriteOutput(CommandNotSupported(verb, normalizedObject ?? "OBJECT"), resultFile);
            return 87;
        }

        private static string ListObjects(string normalizedObject, string name, string stateFilter, string config)
        {
            var server = new Microsoft.Web.Administration.IisExpressServerManager(config);
            var builder = new StringBuilder();
            switch (normalizedObject)
            {
                case "SITE":
                    foreach (var site in server.Sites)
                    {
                        if (!string.IsNullOrEmpty(name) && !string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var state = GetIisExpressSiteState(config, site.Id) ? "Started" : "Stopped";
                        if (!string.IsNullOrEmpty(stateFilter) && !string.Equals(state, stateFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var bindings = string.Join(",", site.Bindings.Select(FormatBinding));
                        builder.AppendLine($"SITE \"{site.Name}\" (id:{site.Id},bindings:{bindings},state:{state})");
                    }

                    return builder.ToString();
                case "APP":
                    foreach (var site in server.Sites)
                    {
                        foreach (var application in site.Applications)
                        {
                            var fullPath = application.Path == "/" ? site.Name + "/" : site.Name + application.Path;
                            if (!string.IsNullOrEmpty(name) && !string.Equals(fullPath, name, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            builder.AppendLine($"APP \"{fullPath}\" (applicationPool:{application.ApplicationPoolName})");
                        }
                    }

                    return builder.ToString();
                case "VDIR":
                    foreach (var site in server.Sites)
                    {
                        foreach (var application in site.Applications)
                        {
                            foreach (var virtualDirectory in application.VirtualDirectories)
                            {
                                var fullPath = application.Path == "/" && virtualDirectory.Path == "/"
                                    ? site.Name + "/"
                                    : site.Name + application.Path + virtualDirectory.Path;
                                if (!string.IsNullOrEmpty(name) && !string.Equals(fullPath, name, StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }

                                builder.AppendLine($"VDIR \"{fullPath}\" (physicalPath:{virtualDirectory.PhysicalPath})");
                            }
                        }
                    }

                    return builder.ToString();
                case "APPPOOL":
                    foreach (var pool in server.ApplicationPools)
                    {
                        if (!string.IsNullOrEmpty(name) && !string.Equals(pool.Name, name, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var state = "Started";
                        if (!string.IsNullOrEmpty(stateFilter) && !string.Equals(state, stateFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var managedRuntimeVersion = pool.ManagedRuntimeVersion ?? string.Empty;
                        var managedPipelineMode = pool.ManagedPipelineMode == Microsoft.Web.Administration.ManagedPipelineMode.Integrated ? "Integrated" : "Classic";
                        builder.AppendLine($"APPPOOL \"{pool.Name}\" (MgdVersion:{managedRuntimeVersion},MgdMode:{managedPipelineMode},state:{state})");
                    }

                    return builder.ToString();
            }

            return string.Empty;
        }

        private static Microsoft.Web.Administration.Site FindSite(string config, string name)
        {
            var server = new Microsoft.Web.Administration.IisExpressServerManager(config);
            foreach (var site in server.Sites)
            {
                if (string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return site;
                }
            }

            return null;
        }

        private static bool StartSite(string config, long siteId)
        {
            var launcher = GetIisExpressExecutable();
            if (launcher == null)
            {
                return false;
            }

            var arguments = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
            var process = new Process
            {
                StartInfo =
                {
                    FileName = launcher,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                }
            };
            process.Start();
            return process.WaitForExit(5000);
        }

        private static bool StopSite(string config, long siteId)
        {
            var toKill = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
            var stopped = false;
            foreach (var item in Process.GetProcessesByName("iisexpress"))
            {
                if (item.GetCommandLine().TrimEnd().EndsWith(toKill, StringComparison.Ordinal))
                {
                    item.Kill();
                    item.WaitForExit();
                    stopped = true;
                }
            }

            return GetIisExpressExecutable() != null || stopped;
        }

        private static bool GetIisExpressSiteState(string config, long siteId)
        {
            var toQuery = $"/config:\"{config}\" /siteid:{siteId} /systray:false /trace:error";
            foreach (var item in Process.GetProcessesByName("iisexpress"))
            {
                if (item.GetCommandLine().TrimEnd().EndsWith(toQuery, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static string GetIisExpressExecutable()
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            var fileName = Path.Combine(directory, "iisexpress.exe");
            if (File.Exists(fileName))
            {
                return fileName;
            }

            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express");
            fileName = Path.Combine(directory, "iisexpress.exe");
            return File.Exists(fileName) ? fileName : null;
        }

        private static string FormatBinding(Microsoft.Web.Administration.Binding binding)
        {
            var information = binding.BindingInformation;
            if (information.StartsWith(":", StringComparison.Ordinal))
            {
                information = "*" + information;
            }

            return binding.Protocol + "/" + information;
        }

        private static string NormalizeObject(string objectName)
        {
            switch (objectName.ToUpperInvariant())
            {
                case "SITE":
                case "SITES":
                    return "SITE";
                case "APP":
                case "APPS":
                    return "APP";
                case "VDIR":
                case "VDIRS":
                    return "VDIR";
                case "APPPOOL":
                case "APPPOOLS":
                    return "APPPOOL";
                default:
                    return null;
            }
        }

        private static bool IsKnownVerb(string verb)
        {
            switch (verb.ToLowerInvariant())
            {
                case "list":
                case "start":
                case "stop":
                case "recycle":
                    return true;
                default:
                    return false;
            }
        }

        private static string GetObjectHelp(string normalizedObject)
        {
            switch (normalizedObject)
            {
                case "SITE":
                    return HelpTexts.Site;
                case "APP":
                    return HelpTexts.App;
                case "VDIR":
                    return HelpTexts.Vdir;
                case "APPPOOL":
                    return HelpTexts.AppPool;
                default:
                    return null;
            }
        }

        private static string GetVerbHelp(string verb, string normalizedObject)
        {
            var key = verb.ToUpperInvariant() + normalizedObject;
            switch (key)
            {
                case "LISTSITE":
                    return HelpTexts.ListSite;
                case "STARTSITE":
                    return HelpTexts.StartSite;
                case "STOPSITE":
                    return HelpTexts.StopSite;
                case "LISTAPP":
                    return HelpTexts.ListApp;
                case "LISTVDIR":
                    return HelpTexts.ListVdir;
                case "LISTAPPPOOL":
                    return HelpTexts.ListAppPool;
                case "STARTAPPPOOL":
                    return HelpTexts.StartAppPool;
                case "STOPAPPPOOL":
                    return HelpTexts.StopAppPool;
                case "RECYCLEAPPPOOL":
                    return HelpTexts.RecycleAppPool;
                default:
                    return null;
            }
        }

        private static string ObjectNotSupported(string objectName)
        {
            return $"Object '{objectName.ToUpperInvariant()}' is not supported.  Run 'appcmd.exe /?' to display supported objects.\r\r\n";
        }

        private static string CommandNotSupported(string verb, string normalizedObject)
        {
            return $"Command '{verb.ToUpperInvariant()}' is not supported on object '{normalizedObject}'. Run 'appcmd.exe {normalizedObject} /?'\r\r\nto display supported commands.\r\r\n";
        }

        private static string MustSpecify(string normalizedObject)
        {
            return $"ERROR ( message:Must specify the {normalizedObject} object with identifier. )\r\n";
        }

        private static string CannotFind(string normalizedObject, string name)
        {
            return $"ERROR ( message:Cannot find {normalizedObject} object with identifier \"{name}\". )\r\n";
        }

        private static string RunRedirected(string fileName, string arguments, out int exitCode)
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            exitCode = process.ExitCode;
            return output;
        }

        private static void WriteOutput(string output, string resultFile)
        {
            if (!string.IsNullOrEmpty(resultFile))
            {
                File.WriteAllText(resultFile, output);
            }

            Console.Write(output);
        }
    }
}
