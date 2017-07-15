// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace JexusManager.Features.HttpApi
{
    internal class ReservedUrlsFeature : HttpApiFeature<ReservedUrlsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly ReservedUrlsFeature _owner;

            public FeatureTaskList(ReservedUrlsFeature owner)
            {
                _owner = owner;
            }

            private const string LocalhostIssuer = "CN=localhost";
            private readonly string _localMachineIssuer = string.Format("CN={0}", Environment.MachineName);

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("CreateSelf", "Add...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void CreateSelf()
            {
                _owner.Create();
            }

        }

        public ReservedUrlsFeature(Microsoft.Web.Management.Client.Module module)
            : base(module)
        {
        }

        private FeatureTaskList _taskList;

        public override TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public override void Load()
        {
            this.Items = new List<ReservedUrlsItem>();
            var httpNamespaceAcls = Microsoft.Web.Administration.NativeMethods.QueryHttpNamespaceAcls();
            foreach (var mapping in httpNamespaceAcls)
            {
                this.Items.Add(new ReservedUrlsItem(mapping.UrlPrefix, mapping.SecurityDescriptor, this));
            }

            this.OnHttpApiSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove reserved URL?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
            {
                return;
            }

            // remove reserved URL
            using (var process = new Process())
            {
                var start = process.StartInfo;
                start.Verb = "runas";
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /u:\"{SelectedItem.UrlPrefix}\" /d:\"{SelectedItem.SecurityDescriptor}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                    OnHttpApiSettingsSaved();
                }
            }
        }

        protected void OnHttpApiSettingsSaved()
        {
            HttpApiSettingsUpdate?.Invoke();
        }

        public override bool ShowHelp()
        {
            Process.Start("https://msdn.microsoft.com/en-us/library/windows/desktop/cc307243(v=vs.85).aspx");
            return false;
        }

        private void Create()
        {
            var dialog = new NewReservedUrlDialog(Module, this);
            dialog.ShowDialog();

            // add reserved URL
            using (var process = new Process())
            {
                var start = process.StartInfo;
                start.Verb = "runas";
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /u:\"{dialog.Item.UrlPrefix}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Items.Add(dialog.Item);
                    OnHttpApiSettingsSaved();
                }
            }
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public override string Name
        {
            get
            {
                return "Reserved URLs";
            }
        }
    }
}
