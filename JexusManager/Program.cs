// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using RollbarDotNet;

namespace JexusManager
{
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

                SetupRollbar();
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
                            Rollbar.PersonData(() => new Person("0")
                            {
                                UserName = "anonymous",
                                Email = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                            });

            Application.ThreadException += (sender, args) =>
            {
                Rollbar.Report(args.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Rollbar.Report(args.ExceptionObject as Exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Rollbar.Report(args.Exception);
            };
        }
    }
}
