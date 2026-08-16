// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using JexusManager.AppCmd;
using Xunit;

namespace Tests.AppCmd
{
    public sealed class AppCmdTestFixture
    {
        [Fact]
        public void NoArgsShowsUsage()
        {
            var output = Run(Array.Empty<string>(), out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal(HelpTexts.Usage, output);
        }

        [Fact]
        public void QuestionMarkShowsUsage()
        {
            var output = Run(new[] { "/?" }, out var exitCode);
            Assert.Equal(1, exitCode);
            Assert.Equal(HelpTexts.Usage, output);
        }

        [Theory]
        [InlineData(new[] { "site", "/?" }, HelpTexts.Site)]
        [InlineData(new[] { "app", "/?" }, HelpTexts.App)]
        [InlineData(new[] { "vdir", "/?" }, HelpTexts.Vdir)]
        [InlineData(new[] { "apppool", "/?" }, HelpTexts.AppPool)]
        public void ObjectHelpShown(string[] args, string help)
        {
            var output = Run(args, out var exitCode);
            Assert.Equal(1, exitCode);
            Assert.Equal(help, output);
        }

        [Theory]
        [InlineData(new[] { "list", "site", "/?" }, HelpTexts.ListSite)]
        [InlineData(new[] { "start", "site", "/?" }, HelpTexts.StartSite)]
        [InlineData(new[] { "stop", "site", "/?" }, HelpTexts.StopSite)]
        [InlineData(new[] { "list", "app", "/?" }, HelpTexts.ListApp)]
        [InlineData(new[] { "list", "vdir", "/?" }, HelpTexts.ListVdir)]
        [InlineData(new[] { "list", "apppool", "/?" }, HelpTexts.ListAppPool)]
        [InlineData(new[] { "start", "apppool", "/?" }, HelpTexts.StartAppPool)]
        [InlineData(new[] { "stop", "apppool", "/?" }, HelpTexts.StopAppPool)]
        [InlineData(new[] { "recycle", "apppool", "/?" }, HelpTexts.RecycleAppPool)]
        public void VerbHelpShown(string[] args, string help)
        {
            var output = Run(args, out var exitCode);
            Assert.Equal(1, exitCode);
            Assert.Equal(help, output);
        }

        [Fact]
        public void ListSites()
        {
            var output = Run(new[] { "list", "site", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("SITE \"WebSite1\" (id:1,bindings:http/*:8080:localhost,state:Stopped)\r\n", output);
            Assert.Contains("SITE \"GuessMeWeb\" (id:2,bindings:http/*:61902:localhost,https/*:44300:localhost,state:Stopped)\r\n", output);
        }

        [Fact]
        public void ListSiteByName()
        {
            var output = Run(new[] { "list", "site", "WebSite1", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("SITE \"WebSite1\" (id:1,", output);
            Assert.DoesNotContain("GuessMeWeb", output);
        }

        [Fact]
        public void ListSiteByProperty()
        {
            var output = Run(new[] { "list", "site", "/site.name:WebSite1", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("SITE \"WebSite1\" (id:1,", output);
            Assert.DoesNotContain("GuessMeWeb", output);
        }

        [Fact]
        public void ListSiteWithStateFilter()
        {
            var output = Run(new[] { "list", "site", "/state:Started", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Empty(output);
        }

        [Fact]
        public void ListMissingSiteSilentlyFails()
        {
            var output = Run(new[] { "list", "site", "Missing", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(1, exitCode);
            Assert.Empty(output);
        }

        [Fact]
        public void ListApps()
        {
            var output = Run(new[] { "list", "apps", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("APP \"WebSite1/\" (applicationPool:Clr4IntegratedAppPool)\r\n", output);
        }

        [Fact]
        public void ListAppPools()
        {
            var output = Run(new[] { "list", "apppool", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("APPPOOL \"Clr4IntegratedAppPool\" (MgdVersion:v4.0,MgdMode:Integrated,state:Started)\r\n", output);
            Assert.Contains("APPPOOL \"Clr4ClassicAppPool\" (MgdVersion:v4.0,MgdMode:Classic,state:Started)\r\n", output);
        }

        [Fact]
        public void ListVirtualDirectories()
        {
            var output = Run(new[] { "list", "vdirs", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(0, exitCode);
            Assert.Contains("VDIR \"WebSite1/\" (physicalPath:", output);
        }

        [Fact]
        public void StartMissingSiteFails()
        {
            var output = Run(new[] { "start", "site", "Missing", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(1168, exitCode);
            Assert.Equal("ERROR ( message:Cannot find SITE object with identifier \"Missing\". )\r\n", output);
        }

        [Fact]
        public void StopWithoutIdentifierFails()
        {
            var output = Run(new[] { "stop", "site", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("ERROR ( message:Must specify the SITE object with identifier. )\r\n", output);
        }

        [Fact]
        public void RestartNotSupported()
        {
            var output = Run(new[] { "restart", "site", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("Command 'RESTART' is not supported on object 'SITE'. Run 'appcmd.exe SITE /?'\r\r\nto display supported commands.\r\r\n", output);
        }

        [Fact]
        public void DeleteNotSupported()
        {
            var output = Run(new[] { "delete", "site", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.StartsWith("Command 'DELETE' is not supported on object 'SITE'.", output);
        }

        [Fact]
        public void RecycleSiteNotSupported()
        {
            var output = Run(new[] { "recycle", "site", "WebSite1", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.StartsWith("Command 'RECYCLE' is not supported on object 'SITE'.", output);
        }

        [Fact]
        public void AppPoolOperationsNotSupportedForIisExpress()
        {
            var output = Run(new[] { "start", "apppool", "Clr4IntegratedAppPool", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(1168, exitCode);
            Assert.Equal("ERROR ( message:Application pools are not supported by IIS Express. )\r\n", output);
        }

        [Fact]
        public void UnknownObjectFails()
        {
            var output = Run(new[] { "list", "foo", "/config:original.config", "/mode:iisexpress" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("Object 'FOO' is not supported.  Run 'appcmd.exe /?' to display supported objects.\r\r\n", output);
        }

        [Fact]
        public void MissingObjectFails()
        {
            var output = Run(new[] { "list" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("Object 'LIST' is not supported.  Run 'appcmd.exe /?' to display supported objects.\r\r\n", output);
        }

        [Fact]
        public void BareObjectFails()
        {
            var output = Run(new[] { "site" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("Command '' is not supported on object 'SITE'. Run 'appcmd.exe SITE /?'\r\r\nto display supported commands.\r\r\n", output);
        }

        [Fact]
        public void ListQuestionMarkFails()
        {
            var output = Run(new[] { "list", "/?" }, out var exitCode);
            Assert.Equal(87, exitCode);
            Assert.Equal("Object 'LIST' is not supported.  Run 'appcmd.exe /?' to display supported objects.\r\r\n", output);
        }

        [Fact]
        public void ResultFileIsWritten()
        {
            var resultFile = Path.GetTempFileName();
            try
            {
                var output = Run(new[] { "list", "site", "WebSite1", "/config:original.config", "/mode:iisexpress", $"/resultFile:{resultFile}" }, out var exitCode);
                Assert.Equal(0, exitCode);
                Assert.Equal(output, File.ReadAllText(resultFile));
            }
            finally
            {
                File.Delete(resultFile);
            }
        }

        [Fact]
        public void IisModeRelaysRealAppCmdOutput()
        {
            var appcmd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "inetsrv", "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                return;
            }

            var output = Run(new[] { "list", "site" }, out var exitCode);
            var expected = RunProcess(appcmd, "list site", out var expectedExitCode);
            Assert.Equal(expectedExitCode, exitCode);
            Assert.Equal(expected, output);
        }

        private static string Run(string[] args, out int exitCode)
        {
            using var writer = new StringWriter();
            var previous = Console.Out;
            Console.SetOut(writer);
            try
            {
                exitCode = AppCmdEngine.Run(args);
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(previous);
            }
        }

        private static string RunProcess(string fileName, string arguments, out int exitCode)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
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
    }
}
