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
using Rollbar;

namespace JexusManager.Features.Certificates
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using Services;
    using Wizards.CertificateRenewWizard;
    using Wizards.CertificateRequestWizard;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class CertificatesFeature
    {
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
                    if (_owner.SelectedItem.Certificate.HasPrivateKey)
                    {
                        try
                        {
                            if (_owner.SelectedItem.Certificate.PrivateKey is RSACng cng)
                            {
                                if (cng.Key.ExportPolicy.HasFlag(CngExportPolicies.AllowExport))
                                {
                                    result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                                    if (_owner.SelectedItem.Certificate.Issuer != LocalhostIssuer && _owner.SelectedItem.Certificate.Issuer != _localMachineIssuer)
                                    {
                                        result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                                    }
                                }
                            }
                            else if (_owner.SelectedItem.Certificate.PrivateKey is RSACryptoServiceProvider keyInfo)
                            {
                                if (keyInfo.CspKeyContainerInfo.Exportable)
                                {
                                    result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                                    if (_owner.SelectedItem.Certificate.Issuer != LocalhostIssuer && _owner.SelectedItem.Certificate.Issuer != _localMachineIssuer)
                                    {
                                        result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                                    }
                                }
                            }
                        }
                        catch (CryptographicException ex)
                        {
                            if (ex.HResult != NativeMethods.BadKeySet)
                            {
                                // TODO: add CNG support.
                                // throw;
                            }
                        }
                    }

                    result.Add(RemoveTaskItem);
                    if (_owner.SelectedItem.Certificate.Issuer == LocalhostIssuer || _owner.SelectedItem.Certificate.Issuer == _localMachineIssuer)
                    {
                        result.Add(new MethodTaskItem("Trust", "Trust Self-Signed Certificate", string.Empty).SetUsage());
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

        public CertificatesFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

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
                        if (ex.HResult != NativeMethods.NonExistingStore)
                        {
                            throw;
                        }
                    }
                }
            }

            OnCertificatesSettingsSaved();
        }

        public List<CertificatesItem> Items { get; set; }

        public void Import()
        {
            var dialog = new ImportCertificateDialog(Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
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

            try
            {
                // remove certificate and mapping
                using (var process = new Process())
                {
                    var start = process.StartInfo;
                    start.Verb = "runas";
                    start.FileName = "cmd";
                    start.Arguments =
                        $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{SelectedItem.Certificate.Thumbprint}\" /s:{(SelectedItem.Store == "Personal" ? "MY" : "WebHosting")}\"";
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
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                {
                    RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> {{ "native", ex.NativeErrorCode } });
                    // throw;
                }
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
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
            var dialog = new SelfCertificateDialog(Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            OnCertificatesSettingsSaved();
        }

        private void CreateDomain()
        {
        }

        private void Complete()
        {
            var dialog = new CompleteRequestDialog(Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            OnCertificatesSettingsSaved();
        }

        private void CreateRequest()
        {
            var wizard = new CertificateRequestWizard(Module, this);
            wizard.ShowDialog();
        }

        private void Export()
        {
            var dialog = new ExportCertificateDialog(SelectedItem.Certificate, Module, this);
            dialog.ShowDialog();
        }

        private void Renew()
        {
            var wizard = new CertificateRenewWizard(SelectedItem.Certificate, Module, this);
            wizard.ShowDialog();
        }

        private void View()
        {
            var cert = SelectedItem.Certificate;
            DialogHelper.DisplayCertificate(cert, IntPtr.Zero);
        }

        private void Trust()
        {
            var cert = SelectedItem.Certificate;
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            if (store.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false).Count == 0)
            {
                try
                {
                    store.Add(cert);
                }
                catch (CryptographicException ex)
                {
                    if (ex.HResult != NativeMethods.UserCancelled)
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

        public bool AutomicRebindEnabled { get; set; }

        public CertificatesItem SelectedItem { get; internal set; }

        public CertificatesSettingsSavedEventHandler CertificatesSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }

        public string Name
        {
            get
            {
                return "Server Certificates";
            }
        }
    }
}
