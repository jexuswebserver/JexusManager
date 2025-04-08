// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;
    using JexusManager.Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddProviderDialog : DialogForm
    {
        private readonly ProvidersFeature _feature;

        public AddProviderDialog(Module module, ProvidersFeature feature, ProviderItem existing = null)
            : base(module)
        {
            InitializeComponent();
            _feature = feature;
            
            Text = existing == null ? "Add Provider" : "Edit Provider";
            if (existing != null)
            {
                txtName.Text = existing.Name;
                txtName.ReadOnly = true;
                
                // Find the matching managed type if possible
                for (int i = 0; i < cbManagedType.Items.Count; i++)
                {
                    if (cbManagedType.Items[i].ToString() == existing.Type)
                    {
                        cbManagedType.SelectedIndex = i;
                        break;
                    }
                }
                
                if (cbManagedType.SelectedIndex < 0)
                {
                    cbManagedType.SelectedIndex = 0;
                }
            }
            else
            {
                cbManagedType.SelectedIndex = 0;
            }
            
            ProviderItem = existing;
            
            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();
            
            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));
                
            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        ShowMessage("Provider name cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    
                    // Check for duplicate provider names
                    if (ProviderItem == null && _feature.Items.Exists(item => item.Name == txtName.Text))
                    {
                        ShowMessage("A provider with this name already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    
                    if (ProviderItem == null)
                    {
                        // Create a new provider
                        var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                        var section = service.GetSection("system.webServer/rewrite/providers");
                        var collection = section.GetCollection();
                        
                        var element = collection.CreateElement();
                        ProviderItem = new ProviderItem(element);
                        ProviderItem.Name = txtName.Text;
                        ProviderItem.Type = cbManagedType.Text;
                        
                        _feature.AddProvider(ProviderItem);
                    }
                    else
                    {
                        // Update existing provider
                        ProviderItem.Type = cbManagedType.Text;
                        ProviderItem.Apply();
                        
                        var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                        service.ServerManager.CommitChanges();
                    }
                    
                    DialogResult = DialogResult.OK;
                }));
                
            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    _feature.ShowHelp();
                }));
        }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProviderItem ProviderItem { get; private set; }
    }
}
