// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace JexusManager.Features.HttpApi
{
    internal class IpMappingFeature : HttpApiFeature<IpMappingItem>
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly IpMappingFeature _owner;

            public FeatureTaskList(IpMappingFeature owner)
            {
                _owner = owner;
            }

            private const string LocalhostIssuer = "CN=localhost";
            private readonly string _localMachineIssuer = string.Format("CN={0}", Environment.MachineName);

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                //result.Add(new MethodTaskItem("Import", "Import...", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem("CreateRequest", "Create Certificate Request...", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem("Complete", "Complete Certificate Request...", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem("CreateDomain", "Create Domain Certificate...", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                //result.Add(new MethodTaskItem("CreateSelf", "Create Self-Signed Certificate...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    //result.Add(new MethodTaskItem("View", "View...", string.Empty).SetUsage());
                    //if (_owner.SelectedItem.Certificate.HasPrivateKey)
                    //{
                    //    try
                    //    {
                    //        var keyInfo = (RSACryptoServiceProvider)_owner.SelectedItem.Certificate.PrivateKey;
                    //        if (keyInfo.CspKeyContainerInfo.Exportable)
                    //        {
                    //            result.Add(new MethodTaskItem("Export", "Export...", string.Empty).SetUsage());
                    //            if (_owner.SelectedItem.Certificate.Issuer != LocalhostIssuer && _owner.SelectedItem.Certificate.Issuer != _localMachineIssuer)
                    //            {
                    //                result.Add(new MethodTaskItem("Renew", "Renew...", string.Empty).SetUsage());
                    //            }
                    //        }
                    //    }
                    //    catch (CryptographicException ex)
                    //    {
                    //        if (ex.HResult != -2146893802)
                    //        {
                    //            throw;
                    //        }
                    //    }
                    //}

                    //result.Add(new MethodTaskItem("Remove", "Remove", string.Empty, string.Empty, Resources.remove_16).SetUsage());
                }

                //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                //if (!this._owner.AutomicRebindEnabled)
                //{
                //    result.Add(new MethodTaskItem("Enable", "Enable Automatic Rebind of Renewed Certificate", string.Empty).SetUsage());
                //}

                //if (this._owner.AutomicRebindEnabled)
                //{
                //    result.Add(new MethodTaskItem("Disable", "Disable Automatic Rebind of Renewed Certificate", string.Empty).SetUsage());
                //}

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
            public void Remove()
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
            this.Items = new List<IpMappingItem>();
            var ipMappings = Microsoft.Web.Administration.NativeMethods.QuerySslCertificateInfo();
            foreach (var mapping in ipMappings)
            {
                this.Items.Add(new IpMappingItem(mapping.IpPort.Address.ToString(), mapping.IpPort.Port.ToString(), mapping.AppId.ToString(), Hex.ToHexString(mapping.Hash), mapping.StoreName, this));
            }

            this.OnHttpApiSettingsSaved();
        }

        public void Import()
        {
            //var dialog = new ImportCertificateDialog(Module);
            //dialog.ShowDialog();

            //Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            this.OnHttpApiSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove this certificate, and permanently remove it from the certificate store?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
            {
                return;
            }

            // remove certificate and mapping
            //using (var process = new Process())
            //{
            //    var start = process.StartInfo;
            //    start.Verb = "runas";
            //    start.FileName = "cmd";
            //    start.Arguments = string.Format(
            //        "/c \"\"{2}\" /h:\"{0}\" /s:{1}\"",
            //        SelectedItem.Certificate.Thumbprint,
            //        SelectedItem.Store == "Personal" ? "MY" : "WebHosting",
            //        Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe"));
            //    start.CreateNoWindow = true;
            //    start.WindowStyle = ProcessWindowStyle.Hidden;
            //    process.Start();
            //    process.WaitForExit();

            //    if (process.ExitCode == 0)
            //    {
            //        Items.Remove(SelectedItem);
            //        SelectedItem = null;
            //        this.OnHttpApiSettingsSaved();
            //    }
            //}
        }

        protected void OnHttpApiSettingsSaved()
        {
            this.HttpApiSettingsUpdated?.Invoke();
        }

        public override bool ShowHelp()
        {
            Process.Start("https://msdn.microsoft.com/en-us/library/windows/desktop/cc307243(v=vs.85).aspx");
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
            //var dialog = new SelfCertificateDialog(Module);
            //dialog.ShowDialog();

            //Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            this.OnHttpApiSettingsSaved();
        }

        private void CreateDomain()
        {
        }

        private void Complete()
        {
            //var dialog = new CompleteRequestDialog(Module);
            //dialog.ShowDialog();

            //Items.Add(new CertificatesItem(dialog.Item, dialog.Store, this));
            this.OnHttpApiSettingsSaved();
        }

        private void CreateRequest()
        {
            //var wizard = new CertificateRequestWizard(this.Module);
            //wizard.ShowDialog();
        }

        private void Export()
        {
            //var dialog = new ExportCertificateDialog(SelectedItem.Certificate, Module);
            //dialog.ShowDialog();
        }

        private void Renew()
        {
            //var wizard = new CertificateRenewWizard(SelectedItem.Certificate, Module);
            //wizard.ShowDialog();
        }

        private void View()
        {
        }

        public bool AutomicRebindEnabled { get; set; }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public HttpApiSettingsSavedEventHandler HttpApiSettingsUpdated { get; set; }

        public override string Name
        {
            get
            {
                return "IP Mappings";
            }
        }
    }
}
