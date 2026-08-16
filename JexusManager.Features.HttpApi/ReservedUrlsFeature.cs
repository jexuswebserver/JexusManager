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
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using JexusManager;
using System.Reflection;
using System.Diagnostics;

namespace JexusManager.Features.HttpApi
{
    public class ReservedUrlsFeature : HttpApiFeature<ReservedUrlsItem>
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("ReservedUrlsFeature");

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
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
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
            Items = new List<ReservedUrlsItem>(((HttpApiModule)Module).Proxy.GetReservedUrls());
            OnHttpApiSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove this URL reservation?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
            {
                return;
            }

            DeleteReservedUrl();
        }

        private void DeleteReservedUrl()
        {
            var item = SelectedItem;
            if (((HttpApiModule)Module).Proxy.DeleteReservedUrl(item.UrlPrefix, item.SecurityDescriptor))
            {
                Items.Remove(item);
                SelectedItem = null;
                OnHttpApiSettingsSaved();
            }
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

        private void Create()
        {
            using var dialog = new NewReservedUrlDialog(Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var message = ((HttpApiModule)Module).Proxy.AddReservedUrl(dialog.Item.UrlPrefix);
            if (string.IsNullOrEmpty(message))
            {
                Items.Add(dialog.Item);
                OnHttpApiSettingsSaved();
            }
            else
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                service.ShowMessage($"Invalid URL prefix input is detected. {message}", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        public override string Name => "URL Reservations";
    }
}
