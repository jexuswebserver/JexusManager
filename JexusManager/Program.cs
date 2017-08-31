// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using RollbarDotNet;

namespace JexusManager
{
    using JexusManager.Dialogs;
    using System;
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
            foreach (var arg in args)
            {
                if (arg == "-s")
                {
                    suppress = true;
                }
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // TODO: set encryption support
            // ProtectedConfigurationProvider.Provider = new WorkingEncryptionServiceProvider();

            Application.Run(new MainForm());
        }
        
        private static void SetupRollbar()
        {
            Rollbar.Init(new RollbarConfig
            {
                AccessToken = "5525758f15504199b7125d35d2058cfe",
                Environment = "production"
            });
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var userName = $"{version} on {GetWindowsVersion()} with {Get45PlusFromRegistry()}";
            Rollbar.PersonData(() => new Person(version)
            {
                UserName = userName
            });
            Rollbar.Report($"Jexus Manager started", ErrorLevel.Info);
            
            Application.ThreadException += (sender, args) =>
            {
                Rollbar.Report(args.Exception);
                ExceptionDialog.Report(userName, args.Exception.ToString());
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Rollbar.Report(args.ExceptionObject as Exception);
                ExceptionDialog.Report(userName, args.ExceptionObject.ToString());
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Rollbar.Report(args.Exception);
                ExceptionDialog.Report(userName, args.Exception.ToString());
            };
        }

        private static string GetWindowsVersion()
        {
            const string subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(subkey))
            {
                return ndpKey != null ? $"{ndpKey.GetValue("ProductName")} ({ndpKey.GetValue("ReleaseId", "unknown")})" : "Unknown Windows release";
            }
        }
        
        // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#net_d
        private static string Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null) {
                    return ".NET Framework Version: " + CheckFor45PlusVersion((int) ndpKey.GetValue("Release"));
                }
                else {
                    return ".NET Framework Version 4.5 or later is not detected.";
                } 
            }
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 460798)
                return "4.7 or later";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254) {
                return "4.6.1";
            }
            if (releaseKey >= 393295) {
                return "4.6";
            }
            if ((releaseKey >= 379893)) {
                return "4.5.2";
            }
            if ((releaseKey >= 378675)) {
                return "4.5.1";
            }
            if ((releaseKey >= 378389)) {
                return "4.5";
            }
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }
    }
}
