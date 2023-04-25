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

using System.ComponentModel;
using System.IO;
using Rollbar;

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
            Items = new List<SniMappingItem>();
            var sniMappings = NativeMethods.QuerySslSniInfo();
            foreach (var mapping in sniMappings)
            {
                Items.Add(new SniMappingItem(mapping.Host, mapping.Port.ToString(), mapping.AppId.ToString(), Hex.ToHexString(mapping.Hash), mapping.StoreName, this));
            }

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

            try
            {
                // remove certificate and mapping
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{CertificateInstallerLocator.FileName}\" /x:\"{SelectedItem.Host}\" /o:{SelectedItem.Port}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                    this.OnHttpApiSettingsSaved();
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                var message = NativeMethods.KnownCases(ex.NativeErrorCode);
                if (string.IsNullOrEmpty(message))
                {
                    RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                    // throw;
                }
                else
                {
                    dialog.ShowError(ex, message, Name, false);
                }
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
            }
        }

        private void View()
        {
            X509Certificate2 cert = null;
            X509Store personal = new X509Store(SelectedItem.Store, StoreLocation.LocalMachine);
            try
            {
                personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var found = personal.Certificates.Find(X509FindType.FindByThumbprint, SelectedItem.Hash, false);
                if (found.Count > 0)
                {
                    cert = found[0];
                }

                personal.Close();
            }
            catch (CryptographicException ex)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                dialog.ShowError(ex, $"This mapping might point to an invalid certificate. Thumbprint {SelectedItem.Hash}, Store {SelectedItem.Store}.", Name, false);
                return;
            }

            if (cert != null)
            {
                DialogHelper.DisplayCertificate(cert, IntPtr.Zero);
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

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public bool AutomicRebindEnabled { get; set; }

        public override string Name => "SNI Mappings";
    }
}
