// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using JexusManager;
using System.Reflection;
using System.Collections;
using Microsoft.Web.Management.Client.Win32;
using Microsoft.Web.Management.Client;
using System.Collections.Generic;
using Org.BouncyCastle.Utilities.Encoders;
using System.Diagnostics;
using Microsoft.Web.Administration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using JexusManager.Services;

namespace JexusManager.Features.HttpApi
{
    internal class IpMappingFeature : HttpApiFeature<IpMappingItem>
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("IpMappingFeature");

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
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("View", "View Certificate...", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void View()
            {
                _owner.View();
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
            Items = new List<IpMappingItem>(((HttpApiModule)Module).Proxy.GetIpMappings());
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

            DeleteMapping();
        }

        private void DeleteMapping()
        {
            var item = SelectedItem;
            if (((HttpApiModule)Module).Proxy.DeleteIpMapping(item.Address, item.Port))
            {
                Items.Remove(item);
                SelectedItem = null;
                OnHttpApiSettingsSaved();
            }
        }

        private void View()
        {
            var cert = ((HttpApiModule)Module).Proxy.GetCertificate(SelectedItem.Hash, SelectedItem.Store);
            if (cert == null)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                dialog.ShowError(new CryptographicException(), $"This mapping might point to an invalid certificate. Thumbprint {SelectedItem.Hash}, Store {SelectedItem.Store}.", Name, false);
                return;
            }

            DialogHelper.DisplayCertificate(cert, IntPtr.Zero);
        }

        protected void OnHttpApiSettingsSaved()
        {
            HttpApiSettingsUpdate?.Invoke();
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("https://msdn.microsoft.com/library/windows/desktop/cc307243(v=vs.85).aspx");
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
