// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Org.BouncyCastle.Utilities.Encoders;

namespace JexusManager.Features.HttpApi
{
    internal class IpMappingFeature : HttpApiFeature<IpMappingItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly IpMappingFeature _owner;

            public FeatureTaskList(IpMappingFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (_owner.SelectedItem != null)
                {
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public IpMappingFeature(Microsoft.Web.Management.Client.Module module)
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
            Items = new List<IpMappingItem>();
            var ipMappings = Microsoft.Web.Administration.NativeMethods.QuerySslCertificateInfo();
            foreach (var mapping in ipMappings)
            {
                Items.Add(new IpMappingItem(mapping.IpPort.Address.ToString(), mapping.IpPort.Port.ToString(), mapping.AppId.ToString(), Hex.ToHexString(mapping.Hash), mapping.StoreName, this));
            }

            OnHttpApiSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove this IP mapping?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
            {
                return;
            }

            // remove IP mapping
            using (var process = new Process())
            {
                var start = process.StartInfo;
                start.Verb = "runas";
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /a:\"{SelectedItem.Address}\" /o:{SelectedItem.Port}\"";
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
            DialogHelper.ProcessStart("https://msdn.microsoft.com/en-us/library/windows/desktop/cc307243(v=vs.85).aspx");
            return false;
        }

        public bool AutomicRebindEnabled { get; set; }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public override string Name => "IP Mappings";
    }
}
