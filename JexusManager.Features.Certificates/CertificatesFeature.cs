﻿// Copyright (c) Lex Li. All rights reserved.
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
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using JexusManager;
using System.Reflection;
using System.Collections;
using Microsoft.Web.Management.Client.Win32;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Management.Client;
using System.Resources;
using System.Collections.Generic;
using JexusManager.Services;
using Microsoft.Web.Administration;
using System.Diagnostics;
using JexusManager.Features.Certificates.Wizards.CertificateRequestWizard;
using JexusManager.Features.Certificates.Wizards.CertificateRenewWizard;

namespace JexusManager.Features.Certificates
{
    public class CertificatesFeature : FeatureBase<CertificatesItem>
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("CertificatesFeature");

        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly CertificatesFeature _owner;

            public FeatureTaskList(CertificatesFeature owner)
            {
                _owner = owner;
            }

            private const string LocalhostIssuer = "CN=localhost";
            private readonly string _localMachineIssuer = $"CN={Environment.MachineName}";

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Import", "Import...", string.Empty).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("CreateRequest", "Create Certificate Request...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("Complete", "Complete Certificate Request...", string.Empty).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("CreateDomain", "Create Domain Certificate...", string.Empty).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("CreateSelf", "Create Self-Signed Certificate...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("View", "View...", string.Empty).SetUsage());
                    X509Certificate2 certificate = _owner.SelectedItem.Item;
                    if (certificate.HasPrivateKey)
                    {
                        try
                        {
                            ECDsaCng eCdsa = certificate.GetECDsaPrivateKey() as ECDsaCng;
                            if (eCdsa != null)
                            {
                                if (eCdsa.Key.ExportPolicy.HasFlag(CngExportPolicies.AllowExport))
                                {
                                    result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                                    if (certificate.Issuer != LocalhostIssuer && certificate.Issuer != _localMachineIssuer)
                                    {
                                        // result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                                    }
                                }
                            }
                            else if (certificate.GetRSAPrivateKey() is RSACng cng)
                            {
                                if (cng.Key.ExportPolicy.HasFlag(CngExportPolicies.AllowExport))
                                {
                                    result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                                    if (certificate.Issuer != LocalhostIssuer && certificate.Issuer != _localMachineIssuer)
                                    {
                                        // result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                                    }
                                }
                            }
                            else if (certificate.GetRSAPrivateKey() is RSACryptoServiceProvider keyInfo)
                            {
                                if (keyInfo.CspKeyContainerInfo.Exportable)
                                {
                                    result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                                    if (certificate.Issuer != LocalhostIssuer && certificate.Issuer != _localMachineIssuer)
                                    {
                                        // result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                                    }
                                }
                            }
                        }
                        catch (CryptographicException ex)
                        {
                            if (ex.HResult != Microsoft.Web.Administration.NativeMethods.BadKeySet)
                            {
                                // TODO: add CNG support.
                                _logger.LogError(ex, "Cryptographic error in certificates feature. HResult: {HResult}", ex.HResult);
                            }
                        }
                    }

                    result.Add(RemoveTaskItem);
                    if (certificate.Issuer == LocalhostIssuer || certificate.Issuer == _localMachineIssuer || certificate.Issuer == certificate.Subject)
                    {
                        X509Chain chain = new X509Chain();
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                        chain.Build(certificate);
                        if (chain.ChainStatus.Length > 0) // not trusted yet
                        {
                            result.Add(new MethodTaskItem("Trust", "Trust Self-Signed Certificate", string.Empty).SetUsage());
                        }
                    }
                }

                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                if (!_owner.AutomicRebindEnabled)
                {
                    result.Add(new MethodTaskItem("Enable", "Enable Automatic Rebind of Renewed Certificate", string.Empty).SetUsage());
                }

                if (_owner.AutomicRebindEnabled)
                {
                    result.Add(new MethodTaskItem("Disable", "Disable Automatic Rebind of Renewed Certificate", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Import()
            {
                _owner.Import();
            }

            [Obfuscation(Exclude = true)]
            public void View()
            {
                _owner.View();
            }

            [Obfuscation(Exclude = true)]
            public void Export()
            {
                _owner.Export();
            }

            [Obfuscation(Exclude = true)]
            public void Renew()
            {
                _owner.Renew();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void CreateRequest()
            {
                _owner.CreateRequest();
            }

            [Obfuscation(Exclude = true)]
            public void Complete()
            {
                _owner.Complete();
            }

            [Obfuscation(Exclude = true)]
            public void CreateDomain()
            {
                _owner.CreateDomain();
            }

            [Obfuscation(Exclude = true)]
            public void CreateSelf()
            {
                _owner.CreateSelf();
            }

            [Obfuscation(Exclude = true)]
            public void Enable()
            {
                _owner.Enable();
            }

            [Obfuscation(Exclude = true)]
            public void Disable()
            {
                _owner.Disable();
            }

            [Obfuscation(Exclude = true)]
            public void Trust()
            {
                _owner.Trust();
            }
        }

        public CertificatesFeature(Microsoft.Web.Management.Client.Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            Items = new List<CertificatesItem>();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            if (service.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)service.Server;
                var certificate = AsyncHelper.RunSync(() => server.GetCertificateAsync());
                if (certificate != null)
                {
                    Items.Add(new CertificatesItem(certificate, "Jexus", this));
                }
            }
            else
            {
                X509Store personal = new X509Store("MY", StoreLocation.LocalMachine);
                personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                foreach (var certificate in personal.Certificates)
                {
                    Items.Add(new CertificatesItem(certificate, "Personal", this));
                }

                personal.Close();

                if (Environment.OSVersion.Version >= Version.Parse("6.2"))
                {
                    // IMPORTANT: WebHosting store is available since Windows 8.
                    X509Store hosting = new X509Store("WebHosting", StoreLocation.LocalMachine);
                    try
                    {
                        hosting.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                        foreach (var certificate in hosting.Certificates)
                        {
                            Items.Add(new CertificatesItem(certificate, "WebHosting", this));
                        }

                        hosting.Close();
                    }
                    catch (CryptographicException ex)
                    {
                        if (ex.HResult != Microsoft.Web.Administration.NativeMethods.NonExistingStore)
                        {
                            _logger.LogError(ex, "CryptographicException {HResult} from CertificatesFeature.", ex.HResult);
                            throw;
                        }
                    }
                }
            }

            OnCertificatesSettingsSaved();
        }

        public void Import()
        {
            using (var dialog = new ImportCertificateDialog(Module, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            }
            OnCertificatesSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove this certificate, and permanently remove it from the certificate store?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
            {
                return;
            }

            DeleteCertificate();
        }

        private void DeleteCertificate()
        {
            try
            {
                // remove certificate and mapping
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{SelectedItem.Item.Thumbprint}\" /s:{(SelectedItem.Store == "Personal" ? "MY" : "WebHosting")}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                    OnCertificatesSettingsSaved();
                }
            }
            catch (Win32Exception ex)
            {
                var message = Microsoft.Web.Administration.NativeMethods.KnownCases(ex.NativeErrorCode);
                if (string.IsNullOrEmpty(message))
                {                    
                    _logger.LogError(ex, "Win32 error deleting certificate. Native error code: {Code}", ex.NativeErrorCode);
                }
                else
                {
                    var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                    dialog.ShowError(ex, message, Name, false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting certificate");
            }
        }

        protected void OnCertificatesSettingsSaved()
        {
            CertificatesSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210528");
            return false;
        }

        private void Disable()
        {
        }

        private void Enable()
        {
        }

        private void CreateSelf()
        {
            using (var dialog = new SelfCertificateDialog(Module, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            }
            OnCertificatesSettingsSaved();
        }

        private void CreateDomain()
        {
        }

        private void Complete()
        {
            using (var dialog = new CompleteRequestDialog(Module, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            }
            OnCertificatesSettingsSaved();
        }

        private void CreateRequest()
        {
            using var wizard = new CertificateRequestWizard(Module, this);
            wizard.ShowDialog();
        }

        private void Export()
        {
            using var dialog = new ExportCertificateDialog(SelectedItem.Item, Module, this);
            dialog.ShowDialog();
        }

        private void Renew()
        {
            using var wizard = new CertificateRenewWizard(SelectedItem.Item, Module, this);
            wizard.ShowDialog();
        }

        internal void View()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(CertificatesItem item)
        {
            DialogHelper.DisplayCertificate(item.Item, IntPtr.Zero);
        }

        public override void InitializeGrouping(ToolStripComboBox cbGroup)
        {
            cbGroup.Items.AddRange(["No Grouping", "Expiration Date", "Issued By", "Certificate Store"]);
        }

        public override string GetGroupKey(ListViewItem item, string selectedGroup)
        {
            switch (selectedGroup)
            {
                case "Expiration Date":
                    return item.SubItems[3].Text;
                case "Issued By":
                    return item.SubItems[2].Text;
                case "Certificate Store":
                    return item.SubItems[5].Text;
                default:
                    return "Unknown";
            }
        }

        private void Trust()
        {
            var cert = SelectedItem.Item;
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            if (store.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false).Count == 0)
            {
                try
                {
                    store.Add(cert);
                    var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                    dialog.ShowMessage("This certificate is now trusted by the logon user.", Name);
                }
                catch (CryptographicException ex)
                {
                    if (ex.HResult != Microsoft.Web.Administration.NativeMethods.UserCancelled)
                    {
                        var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                        dialog.ShowMessage($"An unexpected error happened. HResult is {ex.HResult}. Contact your system administrator.", Name,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // add operation cancelled.
                }
            }

            store.Close();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            throw new NotImplementedException();
        }

        protected override void OnSettingsSaved()
        {
        }

        public bool AutomicRebindEnabled { get; set; }

        public CertificatesSettingsSavedEventHandler CertificatesSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get
            {
                return "Server Certificates";
            }
        }
    }
}
