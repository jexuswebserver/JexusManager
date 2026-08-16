// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using JexusManager;

namespace JexusManager.Features.HttpApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;
    using Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Org.BouncyCastle.Utilities.Encoders;

    using Module = Microsoft.Web.Management.Client.Module;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class SniMappingFeature : HttpApiFeature<SniMappingItem>
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("SniMappingFeature");

        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly SniMappingFeature _owner;

            public FeatureTaskList(SniMappingFeature owner)
            {
                _owner = owner;
            }

            private const string LocalhostIssuer = "CN=localhost";
            private readonly string _localMachineIssuer = $"CN={Environment.MachineName}";

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

        public SniMappingFeature(Module module)
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
            Items = new List<SniMappingItem>(((HttpApiModule)Module).Proxy.GetSniMappings());
            OnHttpApiSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove this SNI mapping?", "Confirm Remove",
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
            if (((HttpApiModule)Module).Proxy.DeleteSniMapping(item.Host, item.Port))
            {
                Items.Remove(item);
                SelectedItem = null;
                this.OnHttpApiSettingsSaved();
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

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public bool AutomicRebindEnabled { get; set; }

        public override string Name => "SNI Mappings";
    }
}
