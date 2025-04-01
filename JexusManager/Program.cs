﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace JexusManager
{
    using JexusManager.Dialogs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Mono.Options;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var help = false;
            var jexus = false;
            OptionSet p =
                new OptionSet()
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

            Microsoft.Web.Administration.JexusServerManager.Enabled = jexus;

            // UI initialization
            ApplicationConfiguration.Initialize();
            var textBox = new RichTextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Font = new Font("Consolas", 9),
                BackColor = Color.Black,
                ForeColor = Color.LightGray
            };

            var provider = new TextBoxLoggerProvider(textBox);
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddProvider(provider);
            });

            ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
            LogHelper.Initialize(loggerFactory);

            Application.Run(new MainForm(extra, textBox));
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
    }
}
