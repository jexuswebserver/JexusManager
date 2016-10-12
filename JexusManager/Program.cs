// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;

namespace JexusManager
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;
    using System.Windows.Forms;

    internal static class Program
    {
        internal static int RunAsAdministrator(string executablePath, string[] args)
        {

            var identity = WindowsIdentity.GetCurrent();
            if (new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator))
            {
                return 1;
            }
            const string key = "--SELFCALL";
            if (args != null && args.Any(i => i == key))
            {
                return -1;
            }
            else
            {
                //创建启动对象
                var startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = string.Join(" ", args) + " " + key,
                    Verb = "runas"
                };
                //设置运行文件

                //设置启动参数
                //设置启动动作,确保以管理员身份运行
                //如果不是管理员，则启动UAC
                Process.Start(startInfo);
                //退出
                return 0;
            }



        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // TODO: set encryption support
            // ProtectedConfigurationProvider.Provider = new WorkingEncryptionServiceProvider();
            var code = RunAsAdministrator(Application.ExecutablePath, args);
            if (code > 0)
                Application.Run(new MainForm());
            else if (code < 0)
                MessageBox.Show("Please using an account with role of Administrator login windows, and try again.");
        }
    }



}
