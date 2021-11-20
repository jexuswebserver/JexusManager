// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using Rollbar;

namespace JexusManager
{
    using JexusManager.Dialogs;
    using Mono.Options;
    using Rollbar.DTOs;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var suppress = false;
            var help = false;
            var jexus = false;
            OptionSet p =
                new OptionSet()
                    .Add("s|suppress", "Suppress Rollbar reports", delegate (string v) { if (v != null) suppress = true; })
                    .Add("h|help|?", "Display help", delegate (string v) { if (v != null) help = true; })
                    .Add("j|jexus", "Enable Jexus web server support", delegate(string v) { if (v != null) jexus = true; });

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException)
            {
                ShowHelp(p);
                return;
            }

            if (help)
            {
                ShowHelp(p);
                return;
            }

            if (!suppress)
            {
                var key = Registry.CurrentUser.CreateSubKey(@"Software\LeXtudio\JexusManager");
                if (key != null)
                {
                    var asked = (string)key.GetValue("RollbarAsked", "false");
                    if (!string.Equals(asked, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (MessageBox.Show(
                                "This application uses Rollbar to collect crash information. Click Yes to continue and allow Rollbar to work. Click No to exit.",
                                "Information",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information) != DialogResult.Yes)
                        {
                            return;
                        }
                    
                        key.SetValue("RollbarAsked", "true");
                    }
                }
#if !DEBUG
                SetupRollbar();
#endif
            }

            Microsoft.Web.Administration.JexusServerManager.Enabled = jexus;

            ApplicationConfiguration.Initialize();
            
            // TODO: set encryption support
            // ProtectedConfigurationProvider.Provider = new WorkingEncryptionServiceProvider();

            Application.Run(new MainForm(extra));
        }

        private static void ShowHelp(OptionSet optionSet)
        {
            var textWriter = new StringWriter();
            textWriter.WriteLine("Jexus Manager is available at https://www.jexusmanager.com");
            textWriter.WriteLine("JexusManager.exe [Options] [file name] [file name]");
            textWriter.WriteLine("Options:");
            optionSet.WriteOptionDescriptions(textWriter);

            MessageBox.Show(textWriter.ToString(), "Jexus Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void SetupRollbar()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var userName = $"{version} on {GetWindowsVersion()} with {Get45PlusFromRegistry()}";
            RollbarLocator.RollbarInstance.Configure(
                new RollbarConfig("5b11a2cb773f42d8afb4265951208c24")
                {
                    Environment = "production",
                    Transform = payload =>
                    {
                        payload.Data.Person = new Person(version)
                        {
                            UserName = userName
                        };
                    }
                });
            var path = Assembly.GetExecutingAssembly().Location;
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var inProgramFiles = path.StartsWith(programFiles, StringComparison.OrdinalIgnoreCase)
                || path.StartsWith(programFilesX86, StringComparison.OrdinalIgnoreCase);
            RollbarLocator.RollbarInstance.Info($"Jexus Manager started from program files: {inProgramFiles}");
            
            Application.ThreadException += (sender, args) =>
            {
                RollbarLocator.RollbarInstance.Error(args.Exception);
                ExceptionDialog.Report(userName, args.Exception.ToString());
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                RollbarLocator.RollbarInstance.Error(args.ExceptionObject as System.Exception);
                ExceptionDialog.Report(userName, args.ExceptionObject.ToString());
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                RollbarLocator.RollbarInstance.Error(args.Exception);
                ExceptionDialog.Report(userName, args.Exception.ToString());
            };
        }

        private static string GetWindowsVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";

            using RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(subkey);
            return ndpKey != null ? $"{ndpKey.GetValue("ProductName")} ({ndpKey.GetValue("ReleaseId", "unknown")})" : "Unknown Windows release";
        }
        
        // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#net_d
        private static string Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey);
            if (ndpKey != null && ndpKey.GetValue("Release") != null)
            {
                return ".NET Framework Version: " + CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
            }
            else
            {
                return ".NET Framework Version 4.5 or later is not detected.";
            }
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 528040)
                return "4.8 or later";
            if (releaseKey >= 461808)
                return "4.7.2";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }
    }
}
